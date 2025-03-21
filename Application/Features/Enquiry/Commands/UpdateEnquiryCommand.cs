using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Enquiry.Commands
{
    public class UpdateEnquiryCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public EnquiryStatus Status { get; set; }
        public string? AssignedTo { get; set; }
        public string? Remarks { get; set; } = string.Empty;
    }
    public class UpdateEnquiryCommandHandler : IRequestHandler<UpdateEnquiryCommand, Response<int>>
    {
        private readonly IEnquiryRepositoryAsync _enquiryRepository;
        private readonly IMapper _mapper;

        public UpdateEnquiryCommandHandler(IEnquiryRepositoryAsync enquiryRepository, IMapper mapper)
        {
            _enquiryRepository = enquiryRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(UpdateEnquiryCommand request, CancellationToken cancellationToken)
        {
            var enquiry = await _enquiryRepository.GetByIdAsync(request.Id);

            if (enquiry == null)
            {
                throw new ApiException($"Enquiry Not Found.");
            }
            else
            {
                _mapper.Map(request, enquiry);
                enquiry.UpdateHistory();

                await _enquiryRepository.UpdateAsync(enquiry);
                return new Response<int>(enquiry.Id);
            }
        }
    }
}
