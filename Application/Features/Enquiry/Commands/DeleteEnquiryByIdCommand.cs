using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Enquiry.Commands
{
    public class DeleteEnquiryByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteEnquiryByIdCommandHandler : IRequestHandler<DeleteEnquiryByIdCommand, Response<int>>
    {
        private readonly IEnquiryRepositoryAsync _enquiryRepository;

        public DeleteEnquiryByIdCommandHandler(IEnquiryRepositoryAsync enquiryRepository)
        {
            _enquiryRepository = enquiryRepository;
        }

        public async Task<Response<int>> Handle(DeleteEnquiryByIdCommand request, CancellationToken cancellationToken)
        {
            var enquiry = await _enquiryRepository.GetByIdAsync(request.Id);
            if (enquiry == null) throw new ApiException($"Enquiry Not Found.");
            await _enquiryRepository.DeleteAsync(enquiry);
            return new Response<int>(enquiry.Id);
        }
    }
}
