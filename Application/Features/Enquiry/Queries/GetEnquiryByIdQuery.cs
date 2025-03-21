using Application.DTOs.Enquiry;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Enquiry.Queries
{
    public class GetEnquiryByIdQuery : IRequest<Response<EnquiryWithHistoryResponseDto>>
    {
        public int Id { get; set; }
    }

    public class GetEnquiryByIdQueryHandler : IRequestHandler<GetEnquiryByIdQuery, Response<EnquiryWithHistoryResponseDto>>
    {
        private readonly IEnquiryRepositoryAsync _enquiryRepository;
        private readonly IEnquiryHistoryRepositoryAsync _enquiryHistoryRepository;
        private readonly IMapper _mapper;

        public GetEnquiryByIdQueryHandler(IEnquiryRepositoryAsync enquiryRepository, IEnquiryHistoryRepositoryAsync enquiryHistoryRepositoryAsync, IMapper mapper)
        {
            _enquiryRepository = enquiryRepository;
            _enquiryHistoryRepository = enquiryHistoryRepositoryAsync;
            _mapper = mapper;
        }
        public async Task<Response<EnquiryWithHistoryResponseDto>> Handle(GetEnquiryByIdQuery query, CancellationToken cancellationToken)
        {
            var enquiry = await _enquiryRepository.GetByIdAsync(query.Id);
            if (enquiry == null) throw new ApiException($"Enquiry Not Found.");

            var mappedEnquiry = _mapper.Map<EnquiryWithHistoryResponseDto>(enquiry);

            var enquiryHistory = await _enquiryHistoryRepository.GetEnquiryHistoryAsync(query.Id);
            mappedEnquiry.History = _mapper.Map<List<EnquiryHistoryResponseDto>>(enquiryHistory);

            return new Response<EnquiryWithHistoryResponseDto>(mappedEnquiry);
        }
    }
}
