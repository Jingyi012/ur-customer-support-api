using Application.DTOs.Enquiry;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;
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

            foreach(var item in mappedEnquiryHistory)
            {
                var user = await _userService.GetUserByIdAsync(item.LastModifiedBy);
                if(user != null)
                {
                    item.LastModifiedBy = user.Data.UserName;
                }
            }

            return new Response<List<EnquiryHistoryResponseDto>>(mappedEnquiryHistory);
        }
    }
}
