using Core.Responses.Subscriptions;
using MediatR;

namespace Core.Queries.Subscriptions
{
    public record GetAllSubscriptionsQuery() : IRequest<IEnumerable<SubscriptionResponse>>;
}
