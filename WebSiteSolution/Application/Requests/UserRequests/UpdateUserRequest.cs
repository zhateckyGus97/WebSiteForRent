using Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string AttachmentUrl { get; set; }
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
                .MaximumLength(100).WithMessage("{PropertyName} must be at most 100 characters long.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(50).WithMessage("{PropertyName} must be at most 50 characters long.")
                .EmailAddress().WithMessage("Enter the correct email.");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(20).WithMessage("{PropertyName} must be at most 20 characters long.");
            //RuleFor(x => x.Role)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .MaximumLength(15).WithMessage("{PropertyName} must be at most 15 characters long.");
            RuleFor(x => x.Passport)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Matches("^\\d{4}-\\d{6}$").WithMessage("{PropertyName} must be as xxxx-xxxxxx format.");
            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8); // добавить условий
        }
    }
}
