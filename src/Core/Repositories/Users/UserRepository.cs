using AutoMapper;
using Core.Domain.Enums.Users;
using Core.Dtos.Users;
using Core.Entities.Cars;
using Core.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Shop.Core.Services;

namespace Core.Repositories.Users
{
    public interface IUserRepository
    {
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto?> GetUserByUsernameAsync(string username);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> AddUserAsync(UserDto user);
        Task<UserDto> UpdateUserAsync(UserDto user);
        Task AddCarUserRelationAsync(int userId, int carId);
        Task DeleteUserByIdAsync(int id);
        Task<int> SaveUserChangesAsync();
    }

    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(CarShopDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _dbContext.Users.Include(_ => _.Role).Include(_ => _.Subscription).Include(_ => _.ViewedCars).FirstOrDefaultAsync(_ => _.Id == id);

            if (user == null)
            {
                return null;
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            var user = await _dbContext.Users.Include(_ => _.Role).Include(_ => _.Subscription).Include(_ => _.ViewedCars).SingleOrDefaultAsync(_ => _.UserName == username);

            if(user == null)
            { 
                return null;
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return (await _dbContext.Users.Include(_ => _.Role).Include(_ => _.Subscription).Include(_ => _.ViewedCars).ToListAsync()).Select(_ => _mapper.Map<UserDto>(_));
        }

        public async Task<UserDto> AddUserAsync(UserDto user)
        {
            user.UserRoleId = (await _dbContext.UserRoles.FirstOrDefaultAsync(_ => _.PermissionLevel == UserPermissionLevel.User))?.Id;
            var entity = Create(_mapper.Map<User>(user));
            await SaveChangesAsync();
            user.Id = entity.Id;
            return user;
        }

        public async Task<UserDto> UpdateUserAsync(UserDto user)
        {
            try
            {
                await UpdateByIdAsync(user.Id, _mapper.Map<User>(user));
                await SaveChangesAsync();
                var updatedUser = await _dbContext.Users.Include(_ => _.Role).Include(_ => _.Subscription).Include(_ => _.ViewedCars).FirstOrDefaultAsync(_ => _.Id == user.Id);
                return _mapper.Map<UserDto>(updatedUser);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddCarUserRelationAsync(int userId, int carId){
            var carEntity = await _dbContext.Cars.FindAsync(carId);
            var existingUser = await _dbContext.Users.Include(_ => _.ViewedCars).FirstOrDefaultAsync(_ => _.Id == userId);

            if(existingUser == null || carEntity == null || existingUser.ViewedCars.Any(_ => _.Id == carEntity.Id)){
                return;
            }

            existingUser.ViewedCars.Add(carEntity);
            await SaveChangesAsync();
        }

        public async Task DeleteUserByIdAsync(int id)
        {
            var user = await FindByIdAsync(id);
            if (user != null)
            {
                Delete(user);
                await SaveChangesAsync();
            }else
            {
                throw new Exception("No user found to delete");
            }
        }

        public async Task<int> SaveUserChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
