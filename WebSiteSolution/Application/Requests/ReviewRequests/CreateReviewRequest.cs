using FluentValidation;

namespace Application.Requests.ReviewRequests
{
    public class CreateReviewRequest
    {
        public int ApartmentId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }

    public class CreateReviewRequestValidator : AbstractValidator<CreateReviewRequest>
    {
        public CreateReviewRequestValidator()
        {
            RuleFor(x => x.ApartmentId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
            RuleFor(x => x.Rating)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .ExclusiveBetween(0, 6).WithMessage("{PropertyName} must be from 1 to 5.");
            RuleFor(x => x.Comment)
                .MaximumLength(300).WithMessage("{PropertyName} must be at most 300 characters long.");
        }
    }
}
