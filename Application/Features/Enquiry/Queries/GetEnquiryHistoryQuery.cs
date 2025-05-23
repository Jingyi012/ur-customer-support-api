using Application.DTOs.Enquiry;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Enquiry.Queries
{
    public class GetEnquiryHistoryQuery : IRequest<Response<List<EnquiryHistoryResponseDto>>>
    {
        public int EnquiryId { get; set; }
    }

    public class GetEnquiryHistoryQueryHandler : IRequestHandler<GetEnquiryHistoryQuery, Response<List<EnquiryHistoryResponseDto>>>
    {
        private readonly IEnquiryHistoryRepositoryAsync _enquiryHistoryRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public GetEnquiryHistoryQueryHandler(IEnquiryHistoryRepositoryAsync enquiryHistoryRepository, IMapper mapper, IUserService userService)
        {
            _enquiryHistoryRepository = enquiryHistoryRepository;
            _mapper = mapper;
            _userService = userService;
        }
        public async Task<Response<List<EnquiryHistoryResponseDto>>> Handle(GetEnquiryHistoryQuery query, CancellationToken cancellationToken)
        {
            var enquiryHistory = await _enquiryHistoryRepository.GetEnquiryHistoryAsync(query.EnquiryId);
            if (enquiryHistory == null) throw new ApiException($"Enquiry history Not Found.");

            var mappedEnquiryHistory = _mapper.Map<List<EnquiryHistoryResponseDto>>(enquiryHistory);

            var userIds = mappedEnquiryHistory
                .SelectMany(e => new[] { e.LastModifiedBy, e.AssignedTo })
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct()
                .ToList();

            var users = await _userService.GetUsersByIdsAsync(userIds);
            var userDict = users.ToDictionary(u => u.Id, u => u.UserName);

            foreach (var item in mappedEnquiryHistory)
            {
                if (!string.IsNullOrWhiteSpace(item.LastModifiedBy) && userDict.TryGetValue(item.LastModifiedBy, out var modifiedByUsername))
                {
                    item.LastModifiedBy = modifiedByUsername;
                }

                if (!string.IsNullOrWhiteSpace(item.AssignedTo) && userDict.TryGetValue(item.AssignedTo, out var assignedUsername))
                {
                    item.AssignedTo = assignedUsername;
                }
                else
                {
                    item.AssignedTo = null;
                }
            }

            return new Response<List<EnquiryHistoryResponseDto>>(mappedEnquiryHistory);
        }
    }
}
