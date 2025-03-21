using Application.DTOs.Enquiry;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
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
        public AdminGetAllEnquiryQueryHandler(IEnquiryRepositoryAsync enquiryRepository, IMapper mapper)
        {
            _enquiryRepository = enquiryRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<List<EnquiryResponseDto>>> Handle(GetAllEnquiryQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllEnquiryParameter>(request);
            var enquiry = await _enquiryRepository.GetAllEnquiry(validFilter);
            var mappedEnquiry = _mapper.Map<List<EnquiryResponseDto>>(enquiry.Data);

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
