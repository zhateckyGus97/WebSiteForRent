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
            RuleFor(x => x.UserId).NotEmpty().NotNull();
            RuleFor(x => x.Title).MaximumLength(100).NotEmpty().WithMessage("{PropertyName} has max 100 length");
            RuleFor(x => x.Description).MaximumLength(500).NotEmpty().WithMessage("{PropertyName} has max 500 length");
            RuleFor(x => x.Address).MaximumLength(150).NotEmpty().WithMessage("{PropertyName} has max 100 length");
            RuleFor(x => x.PricePerDay).GreaterThan(0).NotEmpty().WithMessage("{PropertyName} has been more than 0");
            RuleFor(x => x.NumOfFloor).GreaterThan(0).NotEmpty().WithMessage("{PropertyName} has been more than 0");
            RuleFor(x => x.Square).GreaterThan(0).NotEmpty().WithMessage("{PropertyName} has been more than 0");
            RuleFor(x => x.Capacity).GreaterThan(0).NotEmpty().WithMessage("{PropertyName} has been more than 0");
        }
    }
}
