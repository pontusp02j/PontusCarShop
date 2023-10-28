using AutoMapper;
using Core.Commands.Subscriptions;
using Core.Dtos.Subscriptions;
using Core.Repositories.Subscriptions;
using Core.Responses.Subscriptions;
using MediatR;

namespace Core.RequestHandlers.Subscriptions
{
    public class CreateSubscriptionRequestHandler : IRequestHandler<CreateSubscriptionCommand, SubscriptionResponse>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IMapper _mapper;
        public CreateSubscriptionRequestHandler(ISubscriptionRepository subscriptionRepository, IMapper mapper)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
        }
        public async Task<SubscriptionResponse> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var addedSubscription = await _subscriptionRepository.CreateSubscription(_mapper.Map<SubscriptionDto>(request.SubscriptionRequest));
                return _mapper.Map<SubscriptionResponse>(addedSubscription);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
