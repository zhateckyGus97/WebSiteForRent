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
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ApartmentId { get; set; }
        public User Deal_user { get; set; }
        public Apartment Deal_apartment { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public double TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateDealRequestValidaor : AbstractValidator<CreateDealRequest>
    {
        public CreateDealRequestValidaor()
        {
            RuleFor(x => x.UserId).NotEmpty().NotNull().GreaterThan(0).WithMessage("{PropertyName} has been more than 0");
            RuleFor(x => x.ApartmentId).NotEmpty().NotNull().GreaterThan(0).WithMessage("{PropertyName} has been more than 0");
            RuleFor(x => x.CheckInDate).NotEmpty().GreaterThan(DateTime.MinValue).LessThan(DateTime.MaxValue);
            RuleFor(x => x.CheckOutDate).NotEmpty().GreaterThan(x => x.CheckInDate).LessThan(DateTime.MaxValue);
            RuleFor(x => x.TotalPrice).NotEmpty().GreaterThan(0).WithMessage("{PropertyName} has been more than 0");
            RuleFor(x => x.CreatedAt).NotEmpty().GreaterThan(DateTime.MinValue).LessThan(DateTime.Now);
            RuleFor(x => x.CreatedAt).GreaterThan(x => x.CreatedAt).LessThan(DateTime.MaxValue);
        }
    }
}
