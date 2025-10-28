using Microsoft.AspNetCore.Identity;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;

namespace P7CreateRestApi.Services
{
    public interface IUserService
    {
        Task<(bool, List<string>?)> CreateAsync(CreateUserDto userDto);
        Task<(bool, List<string>?)> DeleteAsync(string id);
        List<UserDto>? GetAllUsers();
        Task<UserDto?> GetByIdAsync(string id);
        Task<(bool, List<string>?)> UpdateAsync(string id, UpdateUserDto user);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<User> userManager, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<(bool, List<string>?)> CreateAsync(CreateUserDto userDto)
        {
            var user = new User
            {
                UserName = userDto.email,
                Email = userDto.email,
            };

            var createdUser = await _userManager.CreateAsync(user, userDto.password);
            if (!createdUser.Succeeded)
            {
                var errors = createdUser.Errors.Select(e => e.Description).ToList();
                return (false, errors);
            }
            var role = string.IsNullOrEmpty(userDto.role) ? "DefaultUser" : userDto.role;
            if (!await _userManager.IsInRoleAsync(user, role))
                await _userManager.AddToRoleAsync(user, role);

            return (true, null);
        }

        public async Task<(bool, List<string>?)> DeleteAsync(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser != null)
            {
                var deletedTask = await _userManager.DeleteAsync(existingUser);
                if (!deletedTask.Succeeded)
                {
                    var errors = deletedTask.Errors.Select(e => e.Description).ToList();
                    return (false, errors);
                }
                return (true, null);
            }
            return (false, new List<string>() { "User not found" });
        }

        public List<UserDto>? GetAllUsers()
        {
            return _userManager.Users
                .Select(user => new UserDto(
                    user.Id.ToString(),
                    user.Email ?? string.Empty,
                    _userManager.GetRolesAsync(user).Result.ToList()
                )).ToList();

        }

        public async Task<UserDto?> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return null;
            else
                return new UserDto(id, user.Email ?? string.Empty, await _userManager.GetRolesAsync(user));

        }

        public async Task<(bool, List<string>?)> UpdateAsync(string id, UpdateUserDto updateUserDto)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null)
            {
                return (false, new List<string> { "User not found" });
            }
            if (!string.IsNullOrEmpty(updateUserDto.email))
            {
                existingUser.Email = updateUserDto.email;
                existingUser.UserName = updateUserDto.email;
            }

            if (!string.IsNullOrEmpty(updateUserDto.password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                var passwordResult = await _userManager.ResetPasswordAsync(existingUser, token, updateUserDto.password);
                if (!passwordResult.Succeeded)
                {
                    var errors = passwordResult.Errors.Select(e => e.Description).ToList();
                    return (false, errors);
                }
            }
            var currentRoles = await _userManager.GetRolesAsync(existingUser);
            if (!string.IsNullOrEmpty(updateUserDto.role) && !currentRoles.Contains(updateUserDto.role))
            {
                if (currentRoles.Count > 0)
                {
                    await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);
                }
                await _userManager.AddToRoleAsync(existingUser, updateUserDto.role);
            }
            return (true, null);
        }
    }
}
