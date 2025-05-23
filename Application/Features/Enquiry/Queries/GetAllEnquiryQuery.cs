using Application.DTOs.Enquiry;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Enquiry.Queries
{
    public class GetAllEnquiryQuery : IRequest<PagedResponse<List<EnquiryResponseDto>>>
    {
        public int? PageNumber { get; set; } = 1;
        public int? PageSize { get; set; } = 20;
        public string? Name { get; set; }
        public string? CompanyName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public EnquiryType? Type { get; set; }
        public EnquiryStatus? Status { get; set; }
        public string? AssignedTo { get; set; }
    }

    public class AdminGetAllEnquiryQueryHandler : IRequestHandler<GetAllEnquiryQuery, PagedResponse<List<EnquiryResponseDto>>>
    {
        private readonly IEnquiryRepositoryAsync _enquiryRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public AdminGetAllEnquiryQueryHandler(IEnquiryRepositoryAsync enquiryRepository, IMapper mapper, IUserService userService)
        {
            _enquiryRepository = enquiryRepository;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<PagedResponse<List<EnquiryResponseDto>>> Handle(GetAllEnquiryQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllEnquiryParameter>(request);
            var enquiry = await _enquiryRepository.GetAllEnquiry(validFilter);
            var mappedEnquiry = _mapper.Map<List<EnquiryResponseDto>>(enquiry.Data);

            var assignedUserIds = mappedEnquiry
                .Where(x => !string.IsNullOrWhiteSpace(x.AssignedTo))
                .Select(x => x.AssignedTo)
                .Distinct()
                .ToList();

            var users = await _userService.GetUsersByIdsAsync(assignedUserIds);
            var userDict = users.ToDictionary(u => u.Id, u => u.UserName);

            foreach (var item in mappedEnquiry)
            {
                if (!string.IsNullOrWhiteSpace(item.AssignedTo) && userDict.TryGetValue(item.AssignedTo, out var userName))
                {
                    item.AssignedTo = userName;
                }
                else
                {
                    item.AssignedTo = null;
                }
            }

            var enquiryResponse = new PagedResponse<List<EnquiryResponseDto>>(
                mappedEnquiry,
                enquiry.PageNumber,
                enquiry.PageSize,
                enquiry.TotalCount
            );

            return enquiryResponse;
        }
    }
}
