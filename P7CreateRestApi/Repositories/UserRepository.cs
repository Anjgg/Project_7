using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;

namespace P7CreateRestApi.Repositories
{
    public class UserRepository : Repository<User>
    {
        private readonly LocalDbContext _context;

        public UserRepository(LocalDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<User?> FindUser(LoginDto loginDto)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.UserName == loginDto.Username && user.Password == loginDto.Password);
        }
    }
}