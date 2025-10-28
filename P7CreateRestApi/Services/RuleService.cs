using AutoMapper;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Repositories;

namespace P7CreateRestApi.Services
{
    public interface IRuleService
    {
        Task<List<RuleDto>> GetAllAsync();
        Task<RuleDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(RuleDto ruleDto);
        Task<RuleDto?> UpdateAsync(int id, RuleDto ruleDto);
        Task<bool> DeleteAsync(int id);
    }

    public class RuleService : IRuleService
    {
        private readonly IRepository<Rule> _repository;
        private readonly IMapper _mapper;

        public RuleService(IRepository<Rule> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<RuleDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync()
                                    .ContinueWith(task => task.Result
                                    .Select(rule => _mapper.Map<RuleDto>(rule))
                                    .ToList());
        }

        public async Task<RuleDto?> GetByIdAsync(int id)
        {
            var rule = await _repository.GetByIdAsync(id);
            if (rule == null)
                return null;
            else
                return _mapper.Map<RuleDto>(rule);
        }

        public async Task<int> CreateAsync(RuleDto ruleDto)
        {
            var rule = _mapper.Map<Rule>(ruleDto);
            var created = await _repository.AddAsync(rule);
            return created.Id;
        }

        public async Task<RuleDto?> UpdateAsync(int id, RuleDto ruleDto)
        {
            var existingRule = await _repository.GetByIdAsync(id);
            if (existingRule == null)
            {
                return null;
            }
            ruleDto.Id = id;
            _mapper.Map(ruleDto, existingRule);
            var updated = await _repository.UpdateAsync(existingRule);
            return _mapper.Map<RuleDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingRule = await _repository.GetByIdAsync(id);
            if (existingRule != null)
            {
                _repository.Remove(existingRule);
                await _repository.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
