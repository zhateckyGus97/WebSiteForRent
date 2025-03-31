using Application.DTO;
using Application.Requests.ApartmentRequests;

namespace Application.Interfaces
{
    public interface IApartmentService
    {
        public Task<int> Add(CreateApartmentRequest Apartment);
        public Task<bool> Delete(int id);
        public Task<CreateApartmentRequest> GetById(int id);
        public Task<IEnumerable<CreateApartmentRequest>> GetAll();
        public Task<bool> Update(UpdateApartmentRequest Apartment);
    }
}
