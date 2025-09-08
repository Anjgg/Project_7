using AutoMapper;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Repositories;

namespace P7CreateRestApi.Services
{
    public interface ITradeService
    {
        Task<List<TradeDto>> GetAllAsync();
        Task<TradeDto?> GetByIdAsync(int id);
        Task<TradeDto> CreateAsync(TradeDto tradeDto);
        Task<TradeDto?> UpdateAsync(int id, TradeDto tradeDto);
        Task<bool> DeleteAsync(int id);
    }

    public class TradeService : ITradeService
    {
        private readonly IRepository<Trade> _repository;
        private readonly IMapper _mapper;

        public TradeService(IRepository<Trade> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<TradeDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync()
                                    .ContinueWith(task => task.Result
                                    .Select(trade => _mapper.Map<TradeDto>(trade))
                                    .ToList());
        }

        public async Task<TradeDto?> GetByIdAsync(int id)
        {
            var trade = await _repository.GetByIdAsync(id);
            if (trade == null)
                return null;
            else
                return _mapper.Map<TradeDto>(trade);
        }

        public async Task<TradeDto> CreateAsync(TradeDto tradeDto)
        {
            var trade = _mapper.Map<Trade>(tradeDto);
            var created = await _repository.AddAsync(trade);
            return _mapper.Map<TradeDto>(created);
        }

        public async Task<TradeDto?> UpdateAsync(int id, TradeDto tradeDto)
        {
            var existingTrade = await _repository.GetByIdAsync(id);
            if (existingTrade == null)
            {
                return null;
            }
            tradeDto.Id = id;
            _mapper.Map(tradeDto, existingTrade);
            var updated = await _repository.UpdateAsync(existingTrade);
            return _mapper.Map<TradeDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingTrade = await _repository.GetByIdAsync(id);
            if (existingTrade != null)
            {
                _repository.Remove(existingTrade);
                await _repository.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
