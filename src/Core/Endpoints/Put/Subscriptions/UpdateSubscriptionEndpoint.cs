using Core.Commands.Subscriptions;
using Core.Requests.Subscriptions;
using Core.Responses.Subscriptions;
using Core.Utilities;
using FastEndpoints;
using MediatR;

namespace Core.Endpoints.Put.Subscriptions
{
    public class UpdateSubscriptionEndpoint : Endpoint<SubscriptionRequest, SubscriptionResponse>
    {
        private readonly IMediator _mediator;
        public UpdateSubscriptionEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override void Configure()
        {
            Put("/api/subscriptions/update");
            AllowAnonymous();
        }

        public override async Task HandleAsync(SubscriptionRequest req, CancellationToken ct)
        {
            try
            {
                var response = await _mediator.Send(new UpdateSubscriptionCommand(req));
                await SendAsync(response, StatusCodes.Status200OK, ct);
            }
            catch (Exception)
            {
                await CustomRequestResponses.BadRequestAsync(HttpContext, "Could not update subscription");
            }
        }
    }
}
