using Application.DTO;

namespace Application.Interfaces
{
    public interface IApartmentService
    {
        public Task<int> Add(ApartmentDTO Apartment);
        public Task<bool> Delete(int id);
        public Task<ApartmentDTO> GetById(int id);
        public Task<IEnumerable<ApartmentDTO>> GetAll();
        public Task<bool> Update(ApartmentDTO Apartment);
    }
}
