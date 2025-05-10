using FluentValidation;

namespace Application.Requests.UserRequests
{
    public class RegistrationUserRequest
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Passport { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string? AttachmentUrl { get; set; }
        public string? Password { get; set; }
    }

    public class RegistrationUserRequestValidator : AbstractValidator<RegistrationUserRequest>
    {
        public RegistrationUserRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(ValidationConstants.MaxFullNameLength).WithMessage("{PropertyName} must be at most 100 characters long.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(ValidationConstants.MaxEmailLength).WithMessage("{PropertyName} must be at most 50 characters long.")
                .EmailAddress().WithMessage("Enter the correct email.");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(ValidationConstants.MaxPhoneNumberLength).WithMessage("{PropertyName} must be at most 20 characters long.");
            RuleFor(x => x.Passport)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Matches(ValidationConstants.PassportPattern).WithMessage("{PropertyName} must be as xxxx-xxxxxx format.");
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
