using FluentValidation;

namespace Application.Requests.UserRequests
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Passport { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? AttachmentUrl { get; set; }
        public string? Password { get; set; }
    }

    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(ValidationConstants.MaxFullNameLength).WithMessage($"{0} must be at most {ValidationConstants.MaxFullNameLength} characters long.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(ValidationConstants.MaxEmailLength).WithMessage($"{0} must be at most {ValidationConstants.MaxEmailLength} characters long.")
                .EmailAddress().WithMessage("Enter the correct email.");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(ValidationConstants.MaxPhoneNumberLength).WithMessage($"{0} must be at most {ValidationConstants.MaxPhoneNumberLength} characters long.");
            RuleFor(x => x.Passport)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Matches("^\\d{4}-\\d{6}$").WithMessage("{PropertyName} must be as xxxx-xxxxxx format.");
            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(ValidationConstants.MinPasswordLength).WithMessage($"Password must be at least {ValidationConstants.MinPasswordLength} characters long")
                .MaximumLength(ValidationConstants.MaxPasswordLength).WithMessage($"Password cannot exceed {ValidationConstants.MaxPasswordLength} characters")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character")
                .Must(x => !x.Contains(" ")).WithMessage("Password cannot contain whitespace");
        }
    }
}
