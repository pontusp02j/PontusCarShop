using Core.Requests.Subscriptions;
using Core.Responses.Subscriptions;
using MediatR;

namespace Core.Commands.Subscriptions
{
    public record CreateSubscriptionCommand(SubscriptionRequest SubscriptionRequest) : IRequest<SubscriptionResponse>;
}
