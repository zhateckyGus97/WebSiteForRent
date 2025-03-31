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
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(100).WithMessage("{PropertyName} has 100 maxlength");
            RuleFor(x => x.Email).NotEmpty().MaximumLength(50).WithMessage("{PropertyName} has 50 maxlength");
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(20).WithMessage("{PropertyName} has 20 maxlength");
            RuleFor(x => x.Role).NotEmpty().MaximumLength(15).WithMessage("{PropertyName} has 15 maxlength");
            RuleFor(x => x.Passport).NotEmpty().MaximumLength(11).WithMessage("{PropertyName} has xxxx-xxxxxx format");
            RuleFor(x => x.DateOfBirth).NotEmpty();
        }
    }
}
