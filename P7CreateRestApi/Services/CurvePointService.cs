using AutoMapper;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Repositories;

namespace P7CreateRestApi.Services
{
    public interface ICurvePointService
    {
        Task<List<CurvePointDto>> GetAllAsync();
        Task<CurvePointDto?> GetByIdAsync(int id);
        Task<CurvePointDto> CreateAsync(CurvePointDto curvePointDto);
        Task<CurvePointDto?> UpdateAsync(int id, CurvePointDto curvePointDto);
        Task<bool> DeleteAsync(int id);
    }


    public class CurvePointService : ICurvePointService
    {
        private readonly IRepository<CurvePoint> _repository;
        private readonly IMapper _mapper;

        public CurvePointService(IRepository<CurvePoint> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<CurvePointDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync()
                                    .ContinueWith(task => task.Result
                                    .Select(curvePoint => _mapper.Map<CurvePointDto>(curvePoint))
                                    .ToList());
        }

        public async Task<CurvePointDto?> GetByIdAsync(int id)
        {
            var curvePoint = await _repository.GetByIdAsync(id);

            if (curvePoint == null)
                return null;
            else
                return _mapper.Map<CurvePointDto>(curvePoint);
        }

        public async Task<CurvePointDto> CreateAsync(CurvePointDto curvePointDto)
        {
            var curvePoint = _mapper.Map<CurvePoint>(curvePointDto);

            var created = await _repository.AddAsync(curvePoint);

            return _mapper.Map<CurvePointDto>(curvePoint);
        }

        public async Task<CurvePointDto?> UpdateAsync(int id, CurvePointDto curvePointDto)
        {
            var existingCurvePoint = await _repository.GetByIdAsync(id);
            if (existingCurvePoint == null)
            {
                return null;
            }

            curvePointDto.Id = id; // Ensure the ID is set correctly

            _mapper.Map(curvePointDto, existingCurvePoint);

            var updated = await _repository.UpdateAsync(existingCurvePoint);

            return _mapper.Map<CurvePointDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingCurvePoint = await _repository.GetByIdAsync(id);
            if (existingCurvePoint != null)
            {
                _repository.Remove(existingCurvePoint);
                await _repository.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
