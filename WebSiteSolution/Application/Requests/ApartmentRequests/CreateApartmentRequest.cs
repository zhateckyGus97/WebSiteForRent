using FluentValidation;

namespace Application.Requests.ApartmentRequests
{
    public class CreateApartmentRequest
    {
        public int OwnerId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Address { get; set; } = null!;
        public double PricePerDay { get; set; }
        public int NumOfFloor { get; set; }
        public double Square { get; set; }
        public int Capacity { get; set; }
        public List<int>? ApartmentAttacmentsId { get; set; }
    }

    public class CreateApartmentRequestValidator : AbstractValidator<CreateApartmentRequest>
    {
        public CreateApartmentRequestValidator()
        {
            RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(ValidationConstants.MaxTitleLength).WithMessage($"{0} must be at most {ValidationConstants.MaxTitleLength} characters long.");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(ValidationConstants.MaxDescriptionLength).WithMessage($"{0} must be at most {ValidationConstants.MaxDescriptionLength} characters long.");
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(ValidationConstants.MaxAddressLength).WithMessage($"{0} must be at most {ValidationConstants.MaxAddressLength} characters long.");
            RuleFor(x => x.PricePerDay)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
            RuleFor(x => x.NumOfFloor)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
            RuleFor(x => x.Square)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
            RuleFor(x => x.Capacity)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
        }
    }
}