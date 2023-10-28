using Core.Queries.Subscriptions;
using Core.Responses.Subscriptions;
using Core.Utilities;
using FastEndpoints;
using MediatR;

namespace Core.Endpoints.Get.Subscriptions
{
    public class GetAllSubscriptionsEndpoint : EndpointWithoutRequest<IEnumerable<SubscriptionResponse>>
    {
        private readonly IMediator _mediator;
        public GetAllSubscriptionsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override void Configure()
        {
            Get("/api/subscriptions");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var subscriptions = await _mediator.Send(new GetAllSubscriptionsQuery());
                await SendAsync(subscriptions, StatusCodes.Status200OK, ct);
            }
            catch (Exception)
            {
                await CustomRequestResponses.BadRequestAsync(HttpContext, "Something went wrong when getting subscriptions");
            }
        }
    }
}
