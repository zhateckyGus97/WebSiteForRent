using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Requests.UserReauests
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string Passport { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class UpdateUserRequestValidaor : AbstractValidator<CreateUserRequest>
    {
        public UpdateUserRequestValidaor()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThan(0).WithMessage("Id has been more then 0");
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(100).WithMessage("{PropertyName} has 100 maxlength");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(50).WithMessage("{PropertyName} has 50 maxlength")
                .EmailAddress().WithMessage("Enter the correct email");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(20).WithMessage("{PropertyName} has 20 maxlength");
            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(15).WithMessage("{PropertyName} has 15 maxlength");
            RuleFor(x => x.Passport)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(11).WithMessage("{PropertyName} has xxxx-xxxxxx format");
            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
