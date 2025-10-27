using AutoMapper;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Repositories;

namespace P7CreateRestApi.Services
{
    public interface IBidService
    {
        Task<BidDto?> CreateAsync(BidDto bidModel);
        Task<bool> DeleteAsync(int id);
        Task<List<BidDto>> GetAllAsync();
        Task<BidDto?> GetByIdAsync(int id);
        Task<BidDto?> UpdateAsync(int id, BidDto bid);
    }

    public class BidService : IBidService
    {
        private readonly IRepository<Bid> _repository;
        private readonly IMapper _mapper;

        public BidService(IRepository<Bid> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<BidDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync()
                                    .ContinueWith(task => task.Result
                                    .Select(bid => _mapper.Map<BidDto>(bid))
                                    .ToList());
        }

        public async Task<BidDto?> GetByIdAsync(int id)
        {
            var bid = await _repository.GetByIdAsync(id);

            if (bid == null)
                return null;
            else
                return _mapper.Map<BidDto>(bid);
        }

        public async Task<BidDto?> CreateAsync(BidDto bidDto)
        {
            var bid = _mapper.Map<Bid>(bidDto);

            var created = await _repository.AddAsync(bid);

            return _mapper.Map<BidDto>(created);
        }

        public async Task<BidDto?> UpdateAsync(int id, BidDto bidDto)
        {
            var existingBid = await _repository.GetByIdAsync(id);
            if (existingBid == null)
            {
                return null;
            }

            bidDto.Id = id; // Ensure the ID is set correctly

            _mapper.Map(bidDto, existingBid);

            var updated = await _repository.UpdateAsync(existingBid);

            return _mapper.Map<BidDto>(updated);
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
    }
}
