using FastEndpoints;
using System.Net;
using MediatR;
using Core.Queries;
using Core.Requests.Cars;
using Core.Responses.Cars;
using Core.Queries.Cars;

namespace Core.Endpoints.Get.Cars
{
    public class GetCarsEndpoint : Endpoint<GetCarsRequest, List<CarResponse>>
    {
        private readonly IMediator _mediator;

        public GetCarsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override void Configure()
        {
            Get("/api/cars/search/{id?}/{description?}/{type?}/{status?}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetCarsRequest req, CancellationToken ct)
        {
            try
            {
                var response = await _mediator.Send(new GetAllCarsQuery(req));

                await SendAsync(response, StatusCodes.Status200OK, ct);
            }
            catch (Exception)
            {
                await BadRequestAsync("Something went wrong when getting cars");
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
