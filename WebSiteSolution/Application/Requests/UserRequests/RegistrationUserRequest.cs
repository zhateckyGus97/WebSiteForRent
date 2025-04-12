using Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Requests.UserRequests
{
    public class RegistrationUserRequest
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Passport { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
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
            //RuleFor(x => x.Role)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .MaximumLength(ValidationConstants.MaxRoleLength).WithMessage("{PropertyName} must be at most 15 characters long.");
            RuleFor(x => x.Passport)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Matches(ValidationConstants.PassportPattern).WithMessage("{PropertyName} must be as xxxx-xxxxxx format.");
            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8); // добавить условий
        }
    }
}
