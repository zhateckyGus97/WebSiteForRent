using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Requests.UserRequests
{
    public class CreateUserRequest
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Passport { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
    }

    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
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
                .MaximumLength(ValidationConstants.MaxPhoneNumberLength).WithMessage("{PropertyName} must be at most 50 characters long.");
            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(ValidationConstants.MaxRoleLength).WithMessage("{PropertyName} must be at most 30 characters long.");
            RuleFor(x => x.Passport)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Matches(ValidationConstants.PassportPattern).WithMessage("{PropertyName} must be as xxxx-xxxxxx format.");
            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
