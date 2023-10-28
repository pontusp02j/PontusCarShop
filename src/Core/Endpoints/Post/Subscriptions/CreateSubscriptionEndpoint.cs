using Core.Commands.Subscriptions;
using Core.Requests.Subscriptions;
using Core.Responses.Subscriptions;
using Core.Utilities;
using FastEndpoints;
using MediatR;

namespace Core.Endpoints.Post.Subscriptions
{
    public class CreateSubscriptionEndpoint : Endpoint<SubscriptionRequest, SubscriptionResponse>
    {
        private readonly IMediator _mediator;
        public CreateSubscriptionEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override void Configure()
        {
            Post("/api/subscriptions/create");
            AllowAnonymous();
        }

        public override async Task HandleAsync(SubscriptionRequest req, CancellationToken ct)
        {
            try
            {
                var response = await _mediator.Send(new CreateSubscriptionCommand(req));
                await SendAsync(response, StatusCodes.Status201Created, ct);
            }
            catch (Exception)
            {
                await CustomRequestResponses.BadRequestAsync(HttpContext, "Could not create subscription");
            }
        }
    }
}
