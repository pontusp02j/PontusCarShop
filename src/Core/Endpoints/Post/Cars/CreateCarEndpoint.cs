using FastEndpoints;
using System.Net;
using MediatR;
using Core.Commands;
using Core.Requests.Cars;
using Core.Responses.Cars;
using Core.Commands.Cars;

namespace Core.Endpoints.Post.Cars
{
    public class CreateCarEndpoint : Endpoint<CreateOrUpdateCarRequest, CarResponse>
    {
        private readonly IMediator _mediator;

        public CreateCarEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override void Configure()
        {
            Post("/api/cars/create");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateOrUpdateCarRequest req, CancellationToken ct)
        {
            try
            {
                var response = await _mediator.Send(new CreateCarCommand(req));

                await SendAsync(response, StatusCodes.Status201Created, ct);
            }
            catch (Exception)
            {
                await BadRequestAsync("Something went wrong when creating car");
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
