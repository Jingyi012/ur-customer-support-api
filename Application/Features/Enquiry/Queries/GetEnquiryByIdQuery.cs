using Application.DTOs.Enquiry;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Enquiry.Queries
{
    public class GetEnquiryByIdQuery : IRequest<Response<EnquiryResponseDto>>
    {
        public int Id { get; set; }
    }

    public class GetEnquiryByIdQueryHandler : IRequestHandler<GetEnquiryByIdQuery, Response<EnquiryResponseDto>>
    {
        private readonly IEnquiryRepositoryAsync _enquiryRepository;
        private readonly IEnquiryHistoryRepositoryAsync _enquiryHistoryRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public GetEnquiryByIdQueryHandler(IEnquiryRepositoryAsync enquiryRepository, IEnquiryHistoryRepositoryAsync enquiryHistoryRepositoryAsync, IMapper mapper, IUserService userService)
        {
            _enquiryRepository = enquiryRepository;
            _enquiryHistoryRepository = enquiryHistoryRepositoryAsync;
            _mapper = mapper;
            _userService = userService;
        }
        public async Task<Response<EnquiryResponseDto>> Handle(GetEnquiryByIdQuery query, CancellationToken cancellationToken)
        {
            var enquiry = await _enquiryRepository.GetByIdAsync(query.Id);
            if (enquiry == null)
                throw new ApiException("Enquiry Not Found.");

            var mappedEnquiry = _mapper.Map<EnquiryResponseDto>(enquiry);

            var userIds = new[] { mappedEnquiry.AssignedTo, mappedEnquiry.LastModifiedBy }
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct()
                .ToList();

            var users = await _userService.GetUsersByIdsAsync(userIds);
            var userDictionary = users.ToDictionary(u => u.Id, u => u.UserName);

            mappedEnquiry.AssignedTo = GetUsernameOrNull(mappedEnquiry.AssignedTo, userDictionary);
            mappedEnquiry.LastModifiedBy = GetUsernameOrNull(mappedEnquiry.LastModifiedBy, userDictionary);

            return new Response<EnquiryResponseDto>(mappedEnquiry);
        }

        private static string? GetUsernameOrNull(string? userId, Dictionary<string, string> userDict)
        {
            return !string.IsNullOrWhiteSpace(userId) && userDict.TryGetValue(userId, out var username)
                ? username
                : null;
        }
    }
}
