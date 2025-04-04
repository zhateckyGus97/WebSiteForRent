using Application.Requests.UserReauests;
using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Requests.ReviewRequests
{
    public class CreateReviewRequest
    {
        public int ApartmentId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }

    public class CreateReviewRequestValidaor : AbstractValidator<CreateReviewRequest>
    {
        public CreateReviewRequestValidaor()
        {
            RuleFor(x => x.ApartmentId)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThan(0).WithMessage("{PropertyName} has been more than 0");
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThan(0).WithMessage("{PropertyName} has been more than 0");
            RuleFor(x => x.Rating)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThan(0).LessThan(6).WithMessage("{PropertyName} has been from 1 to 5");
            RuleFor(x => x.Comment)
                .MaximumLength(300).WithMessage("{PropertyName} has 300 maxlength ");
            /*RuleFor(x => x.CreatedAt)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThan(DateTime.MinValue).LessThan(DateTime.Now).WithMessage("Enter the correct date");*/
        }
    }
}
