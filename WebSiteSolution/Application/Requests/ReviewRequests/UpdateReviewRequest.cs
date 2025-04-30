using FluentValidation;

namespace Application.Requests.ReviewRequests
{
    public class UpdateReviewRequest
    {
        public int Id { get; set; }
        public int ApartmentId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }

    public class UpdateReviewRequestValidator : AbstractValidator<UpdateReviewRequest>
    {
        public UpdateReviewRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("Id has been more then 0");
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
                .MaximumLength(ValidationConstants.MaxCommentLength).WithMessage($"{0} must be at most {ValidationConstants.MaxCommentLength} characters long.");
        }
    }
}
