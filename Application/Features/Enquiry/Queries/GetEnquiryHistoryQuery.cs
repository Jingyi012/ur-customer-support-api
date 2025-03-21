using Application.DTOs.Enquiry;
using Application.Exceptions;
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
        public GetEnquiryHistoryQueryHandler(IEnquiryHistoryRepositoryAsync enquiryHistoryRepository, IMapper mapper)
        {
            _enquiryHistoryRepository = enquiryHistoryRepository;
            _mapper = mapper;
        }
        public async Task<Response<List<EnquiryHistoryResponseDto>>> Handle(GetEnquiryHistoryQuery query, CancellationToken cancellationToken)
        {
            var enquiryHistory = await _enquiryHistoryRepository.GetEnquiryHistoryAsync(query.EnquiryId);
            if (enquiryHistory == null) throw new ApiException($"Enquiry history Not Found.");

            var mappedEnquiryHistory = _mapper.Map<List<EnquiryHistoryResponseDto>>(enquiryHistory);
            return new Response<List<EnquiryHistoryResponseDto>>(mappedEnquiryHistory);
        }
    }
}
