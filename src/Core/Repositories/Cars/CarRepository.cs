using AutoMapper;
using Core.Entities.Cars;
using Microsoft.EntityFrameworkCore;
using Shop.Core.Services;

namespace Core.Repositories.Cars
{
    public interface ICarRepository
    {
        Task<List<CarDto>> GetAllCarsAsync();
        Task<CarDto> GetCarByIdAsync(int id);
        Task<CarDto> CreateAndSaveCarAsync(CarDto cdto);
        Task UpdateAndSaveCarAsync(CarDto updatedCar);
        Task SaveChangesAsync();

    }
    public class CarRepository : RepositoryBase<Car>, ICarRepository
    {
        public CarRepository(CarShopDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

        public async Task<List<CarDto>> GetAllCarsAsync()
        {
            var cars = await FindAllAsync();
            return cars.Select(_ => _mapper.Map<CarDto>(_)).ToList();
        }

        public async Task<CarDto> GetCarByIdAsync(int id)
        {
            var entity = await FindByIdAsync(id);
            return _mapper.Map<CarDto>(entity);
        }

        public async Task<CarDto> CreateAndSaveCarAsync(CarDto cdto)
        {
            var entity = Create(_mapper.Map<Car>(cdto));
            await SaveChangesAsync();
            cdto.Id = entity.Id;
            return cdto;
        }
        public async Task UpdateAndSaveCarAsync(CarDto updatedCar)
        {
            try
            {
                await UpdateByIdAsync(updatedCar.Id, _mapper.Map<Car>(updatedCar));
                await SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
