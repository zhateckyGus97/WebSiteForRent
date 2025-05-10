using FluentValidation;

namespace Application.Requests.ApartmentRequests
{
    public class CreateApartmentRequest
    {
        public int Owner_id { get; set; }
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
            RuleFor(x => x.Owner_id)
            .NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(ValidationConstants.MaxTitleLength).WithMessage("{PropertyName} must be at most 100 characters long.");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(ValidationConstants.MaxDescriptionLength).WithMessage("{PropertyName} must be at most 1000 characters long.");
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(ValidationConstants.MaxAddressLength).WithMessage("{PropertyName} must be at most 150 characters long.");
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