using AutoMapper;
using Core.Queries.Subscriptions;
using Core.Repositories.Subscriptions;
using Core.Responses;
using Core.Responses.Subscriptions;
using MediatR;
using System.Threading.Tasks;

namespace Core.RequestHandlers.Subscriptions
{
    public class GetAllSubscriptionsRequestHandler : IRequestHandler<GetAllSubscriptionsQuery, IEnumerable<SubscriptionResponse>>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IMapper _mapper;

        public GetAllSubscriptionsRequestHandler(ISubscriptionRepository subscriptionRepository, IMapper mapper)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubscriptionResponse>> Handle(GetAllSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return (await _subscriptionRepository.GetAllSubscriptionsAsync()).Select(_ => _mapper.Map<SubscriptionResponse>(_));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
