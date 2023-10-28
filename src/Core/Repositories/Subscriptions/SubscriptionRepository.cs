using AutoMapper;
using Core.Dtos.Subscriptions;
using Core.Dtos.Users;
using Core.Entities.Subscriptions;
using Core.Entities.Users;
using Core.Responses.Subscriptions;
using Shop.Core.Services;

namespace Core.Repositories.Subscriptions
{
    public interface ISubscriptionRepository
    {
        Task<IEnumerable<SubscriptionDto>> GetAllSubscriptionsAsync();
        Task<SubscriptionDto> CreateSubscription(SubscriptionDto dto);
        Task<SubscriptionDto> UpdateSubscriptionAsync(SubscriptionDto subscriptionDto);
    }
    public class SubscriptionRepository : RepositoryBase<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(CarShopDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

        public async Task<IEnumerable<SubscriptionDto>> GetAllSubscriptionsAsync()
        {
            return (await FindAllAsync()).Select(_ => _mapper.Map<SubscriptionDto>(_));
        }

        public async Task<SubscriptionDto> CreateSubscription(SubscriptionDto dto)
        {
            var entity = Create(_mapper.Map<Subscription>(dto));
            await SaveChangesAsync();
            return _mapper.Map<SubscriptionDto>(entity);
        }

        public async Task<SubscriptionDto> UpdateSubscriptionAsync(SubscriptionDto subscriptionDto)
        {
            try
            {
                await UpdateByIdAsync(subscriptionDto.Id, _mapper.Map<Subscription>(subscriptionDto));
                await SaveChangesAsync();
                return subscriptionDto;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
