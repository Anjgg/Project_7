using AutoMapper;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Repositories;

namespace P7CreateRestApi.Services
{
    public interface IUserService
    {
        Task<UserDto> CreateAsync(UserDto userDto);
        Task<bool> DeleteAsync(int id);
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto?> UpdateAsync(int id, UserDto user);
    }

    public class UserService : IUserService
    {
        private readonly UserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(UserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserDto> CreateAsync(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            var created = await _repository.AddAsync(user);

            return _mapper.Map<UserDto>(created);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingUser = await _repository.GetByIdAsync(id);
            if (existingUser != null)
            {
                _repository.Remove(existingUser);
                await _repository.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync()
                                    .ContinueWith(task => task.Result
                                    .Select(user => _mapper.Map<UserDto>(user))
                                    .ToList());
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);

            if (user == null)
                return null;
            else
                return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> UpdateAsync(int id, UserDto userDto)
        {
            var existingUser = await _repository.GetByIdAsync(id);
            if (existingUser == null)
            {
                return null;
            }

            userDto.Id = id; // Ensure the ID is set correctly

            _mapper.Map(userDto, existingUser);

            var updated = await _repository.UpdateAsync(existingUser);

            return _mapper.Map<UserDto>(updated);
        }
    }
}
