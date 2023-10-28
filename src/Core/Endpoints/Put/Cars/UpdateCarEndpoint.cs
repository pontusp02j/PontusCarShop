using Core.Commands;
using Core.Commands.Cars;
using Core.Requests.Cars;
using Core.Responses.Cars;
using FastEndpoints;
using MediatR;
using System.Net;

namespace Core.Endpoints.Put.Cars
{
    public class UpdateCarEndpoint : Endpoint<CreateOrUpdateCarRequest, CarResponse>
    {
        private readonly IMediator _mediator;

        public UpdateCarEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override void Configure()
        {
            Put("/api/cars/update");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateOrUpdateCarRequest req, CancellationToken ct)
        {
            try
            {
                var response = await _mediator.Send(new UpdateCarCommand(req));
                await SendAsync(response, StatusCodes.Status200OK, ct);
            }
            catch (Exception)
            {
                await BadRequestAsync("Something went wrong when updating car");
            }
        }
        private async Task BadRequestAsync(string message)
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(message)
            };

            await HttpContext.Response.WriteAsync(await response.Content.ReadAsStringAsync());
        }
    }
}
