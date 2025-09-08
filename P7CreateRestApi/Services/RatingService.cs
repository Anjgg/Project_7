using AutoMapper;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Repositories;

namespace P7CreateRestApi.Services
{
    public interface IRatingService
    {
        Task<List<RatingDto>> GetAllAsync();
        Task<RatingDto?> GetByIdAsync(int id);
        Task<RatingDto> CreateAsync(RatingDto curvePointDto);
        Task<RatingDto?> UpdateAsync(int id, RatingDto curvePointDto);
        Task<bool> DeleteAsync(int id);
    }


    public class RatingService : IRatingService
    {
        private readonly IRepository<Rating> _repository;
        private readonly IMapper _mapper;

        public RatingService(IRepository<Rating> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<RatingDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync()
                                    .ContinueWith(task => task.Result
                                    .Select(rating => _mapper.Map<RatingDto>(rating))
                                    .ToList());
        }

        public async Task<RatingDto?> GetByIdAsync(int id)
        {
            var rating = await _repository.GetByIdAsync(id);
            
            if (rating == null)
                return null;
            else
                return _mapper.Map<RatingDto>(rating);
        }

        public async Task<RatingDto> CreateAsync(RatingDto ratingDto)
        {
            var rating = _mapper.Map<Rating>(ratingDto);

            var created = await _repository.AddAsync(rating);

            return _mapper.Map<RatingDto>(created);
        }

        public async Task<RatingDto?> UpdateAsync(int id, RatingDto ratingDto)
        {
            var existingRating = await _repository.GetByIdAsync(id);
            if (existingRating == null)
            {
                return null;
            }

            ratingDto.Id = id; // Ensure the ID is set correctly

            _mapper.Map(ratingDto, existingRating);

            var updated = await _repository.UpdateAsync(existingRating);

            return _mapper.Map<RatingDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingRating = await _repository.GetByIdAsync(id);
            if (existingRating != null)
            {
                _repository.Remove(existingRating);
                await _repository.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
