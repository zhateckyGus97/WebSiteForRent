using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Requests.DealRequests
{
    public class UpdateDealRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ApartmentId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public double TotalPrice { get; set; }
    }

    public class UpdateDealRequestValidator : AbstractValidator<UpdateDealRequest>
    {
        public UpdateDealRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
            RuleFor(x => x.ApartmentId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
            RuleFor(x => x.CheckInDate)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .ExclusiveBetween(DateTime.Now, DateTime.MaxValue).WithMessage("Enter the correct date.");
            RuleFor(x => x.CheckOutDate)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .ExclusiveBetween(DateTime.Now, DateTime.MaxValue).WithMessage("Enter the correct date.");
            RuleFor(x => x.TotalPrice)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
        }
    }
}