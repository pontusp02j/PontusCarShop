using FastEndpoints;
using System.Net;
using MediatR;
using Core.Commands;
using Core.Requests.Cars;
using Core.Responses.Cars;
using Core.Commands.Cars;

namespace Core.Endpoints.Post.Cars
{
    public class CreateCarUserRelationEndpoint : Endpoint<CreateCarUserRelationRequest>
    {
        private readonly IMediator _mediator;

        public CreateCarUserRelationEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override void Configure()
        {
            Post("/api/cars/createCarUserRelation");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateCarUserRelationRequest req, CancellationToken ct)
        {
            try
            {
                await _mediator.Send(new CreateCarUserRelationCommand(req));

                await SendAsync(StatusCodes.Status200OK);
            }
            catch (Exception)
            {
                await BadRequestAsync("Something went wrong when creating car user relation");
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
