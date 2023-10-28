using AutoMapper;
using Core.Dtos.Subscriptions;
using Core.Entities.Subscriptions;
using Core.Requests.Subscriptions;
using Core.Responses.Subscriptions;

namespace Core.Profiles.Subscriptions
{
    public class SubscriptionProfile : Profile
    {
        public SubscriptionProfile()
        {
            CreateMap<SubscriptionDto, Subscription>();
            CreateMap<Subscription, SubscriptionDto>();
            CreateMap<SubscriptionRequest, SubscriptionDto>();
            CreateMap<SubscriptionDto, SubscriptionResponse>();
        }
    }
}
