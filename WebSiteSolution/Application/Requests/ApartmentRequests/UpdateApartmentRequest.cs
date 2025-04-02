using Application.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Requests.ApartmentRequests
{
    public class UpdateApartmentRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserDTO Apartment_user { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public double PricePerDay { get; set; }
        public int NumOfFloor { get; set; }
        public double Square { get; set; }
        public int Capacity { get; set; }
    }

    public class UpdateApartmentRequestValidaor : AbstractValidator<UpdateApartmentRequest>
    {
        public UpdateApartmentRequestValidaor()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThan(0).WithMessage("Id has been more than 0");
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(100).WithMessage("{PropertyName} has max 100 length");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(500).WithMessage("{PropertyName} has max 500 length");
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(150).WithMessage("{PropertyName} has max 100 length");
            RuleFor(x => x.PricePerDay)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThan(0).WithMessage("{PropertyName} has been more than 0");
            RuleFor(x => x.NumOfFloor)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThan(0).WithMessage("{PropertyName} has been more than 0");
            RuleFor(x => x.Square)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThan(0).WithMessage("{PropertyName} has been more than 0");
            RuleFor(x => x.Capacity)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThan(0).WithMessage("{PropertyName} has been more than 0");
        }
    }
}
