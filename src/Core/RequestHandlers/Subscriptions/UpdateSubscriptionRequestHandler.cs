using AutoMapper;
using Core.Commands.Subscriptions;
using Core.Dtos.Subscriptions;
using Core.Repositories.Subscriptions;
using Core.Responses.Subscriptions;
using MediatR;

namespace Core.RequestHandlers.Subscriptions
{
    public class UpdateSubscriptionRequestHandler : IRequestHandler<UpdateSubscriptionCommand, SubscriptionResponse>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IMapper _mapper;
        public UpdateSubscriptionRequestHandler(ISubscriptionRepository subscriptionRepository, IMapper mapper)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
        }
        public async Task<SubscriptionResponse> Handle(UpdateSubscriptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var editedSubscription = await _subscriptionRepository.UpdateSubscriptionAsync(_mapper.Map<SubscriptionDto>(request.SubscriptionRequest));
                return _mapper.Map<SubscriptionResponse>(editedSubscription);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
