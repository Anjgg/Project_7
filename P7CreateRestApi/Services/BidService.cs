using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using P7CreateRestApi.Repositories;

namespace P7CreateRestApi.Services
{
    public interface IBidService
    {
        Task<BidDto> CreateAsync(BidDto bidModel);
        Task<bool> DeleteAsync(int id);
        Task<List<BidDto>> GetAllAsync();
        Task<BidDto?> GetByIdAsync(int id);
        Task<BidDto?> UpdateAsync(int id, BidDto bid);
    }

    public class BidService : IBidService
    {
        private readonly IRepository<Bid> _repository;

        public BidService(IRepository<Bid> repository)
        {
            _repository = repository;
        }

        public async Task<BidDto> CreateAsync(BidDto bidModel)
        {
            var bid = new Bid
            {
                Account = bidModel.Account,
                Type = bidModel.Type,
                BidQuantity = bidModel.BidQuantity
            };

            var created = await _repository.AddAsync(bid);

            return new BidDto
            {
                Id = created.Id,
                Account = created.Account,
                Type = created.Type,
                BidQuantity = created.BidQuantity
            };

        }

        public async Task<BidDto?> GetByIdAsync(int id)
        {
            var bid = await _repository.GetByIdAsync(id);
            if (bid == null)
            {
                return null;
            }
            return new BidDto
            {
                Id = bid.Id,
                Account = bid.Account,
                Type = bid.Type,
                BidQuantity = bid.BidQuantity
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingBid = await _repository.GetByIdAsync(id);
            if (existingBid != null)
            {
                _repository.Remove(existingBid);
                await _repository.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<BidDto?> UpdateAsync(int id, BidDto bidDto)
        {
            var existingBid = await _repository.GetByIdAsync(id);
            if (existingBid == null)
            {
                return null;
            }

            existingBid.Id = id; 
            existingBid.Account = bidDto.Account;
            existingBid.Type = bidDto.Type;
            existingBid.BidQuantity = bidDto.BidQuantity;

            var updated = await _repository.UpdateAsync(existingBid);

            return new BidDto
            {
                Id = updated.Id,
                Account = updated.Account,
                Type = updated.Type,
                BidQuantity = updated.BidQuantity
            };
        }

        public async Task<List<BidDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync()
                .ContinueWith(task => task.Result
                    .Select(bid => new BidDto
                    {
                        Id = bid.Id,
                        Account = bid.Account,
                        Type = bid.Type,
                        BidQuantity = bid.BidQuantity
                    })
                    .ToList());
        }
    }
}
