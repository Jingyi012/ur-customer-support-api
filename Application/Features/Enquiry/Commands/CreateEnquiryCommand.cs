using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Enquiry.Commands
{
    public partial class CreateEnquiryCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public EnquiryType Type { get; set; }
        public string Message { get; set; }
    }
    public class CreateEnquiryCommandValidator : AbstractValidator<CreateEnquiryCommand>
    {
        public CreateEnquiryCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.CompanyName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.Phone)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.Type)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.Message)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }

    public class CreateEnquiryCommandHandler : IRequestHandler<CreateEnquiryCommand, Response<int>>
    {
        private readonly IEnquiryRepositoryAsync _enquiryRepository;
        private readonly IMapper _mapper;

        public CreateEnquiryCommandHandler(IEnquiryRepositoryAsync enquiryRepository, IMapper mapper)
        {
            _enquiryRepository = enquiryRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateEnquiryCommand request, CancellationToken cancellationToken)
        {
            var enquiry = _mapper.Map<Domain.Entities.Enquiry>(request);
            await _enquiryRepository.AddAsync(enquiry);
            return new Response<int>(enquiry.Id);
        }
    }
}
