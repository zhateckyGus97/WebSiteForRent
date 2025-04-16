using Application.Requests.ApartmentRequests;
using Application.Responses;

namespace Application.Interfaces
{
    public interface IApartmentService
    {
        public Task<int> Add(CreateApartmentRequest Apartment);
        public Task<bool> Delete(int id);
        public Task<ApartmentResponse> GetById(int id);
        public Task<IEnumerable<ApartmentResponse>> GetAll();
        public Task<bool> Update(UpdateApartmentRequest Apartment);
    }
}
