using Application.Requests.UserReauests;
using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Requests.DealRequests
{
    public class CreateDealRequest
    {
        public int UserId { get; set; }
        public int ApartmentId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public double TotalPrice { get; set; }
    }

    public class CreateDealRequestValidaor : AbstractValidator<CreateDealRequest>
    {
        public CreateDealRequestValidaor()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThan(0).WithMessage("{PropertyName} has been more than 0");
            RuleFor(x => x.ApartmentId)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThan(0).WithMessage("{PropertyName} has been more than 0");
            RuleFor(x => x.CheckInDate)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThan(DateTime.MinValue).LessThan(DateTime.MaxValue).WithMessage("Enter the correct date");
            RuleFor(x => x.CheckOutDate)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThan(x => x.CheckInDate).LessThan(DateTime.MaxValue).WithMessage("Enter the correct date");
            RuleFor(x => x.TotalPrice)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThan(0).WithMessage("{PropertyName} has been more than 0");
            /*RuleFor(x => x.CreatedAt)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThan(DateTime.MinValue).LessThan(DateTime.Now).WithMessage("Enter the correct date");*/
        }
    }
}
