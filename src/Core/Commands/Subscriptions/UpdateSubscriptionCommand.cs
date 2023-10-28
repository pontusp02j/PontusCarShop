using Core.Requests.Subscriptions;
using Core.Responses.Subscriptions;
using MediatR;

namespace Core.Commands.Subscriptions
{
    public record UpdateSubscriptionCommand(SubscriptionRequest SubscriptionRequest) : IRequest<SubscriptionResponse>;
}
