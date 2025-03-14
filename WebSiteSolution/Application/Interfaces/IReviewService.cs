using Application.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IReviewService
    {
        public Task<int> Add(ReviewDTO review);
        public Task<bool> Delete(int id);
        public Task<ReviewDTO> GetById(int id);
        public Task<IEnumerable<ReviewDTO>> GetAll();
        public Task<bool> Update(ReviewDTO review);
    }
}
