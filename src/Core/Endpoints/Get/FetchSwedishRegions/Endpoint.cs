using FastEndpoints;
using System.Net;
using Shop.Core.Endpoints.Get.FetchSwedishRegions.Responses;

using Newtonsoft.Json;

namespace Shop.Endpoints.Get.FetchSwedishRegions
{
    public class FetchSwedishRegionsEndpoint : EndpointWithoutRequest<Response>
    {
        public FetchSwedishRegionsEndpoint() { }
        public override void Configure()
        {
            Get("/api/swedishregions");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                await FetchSwedishRegions(
                    "https://api.scb.se/OV0104/v1/doris/en/ssd/START/BE/BE0101/BE0101A/BefolkningNy"
                );
            }
            catch (Exception e)
            {
                await BadRequestAsync("Something went wrong when fetching out swedish region");
            }
        }

        private async Task FetchSwedishRegions(string url)
        {
            try
            {
                Response deserializedResultTest;

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Accept", "application/json");

                    HttpResponseMessage result = await client.GetAsync(url);
                    result.EnsureSuccessStatusCode();

                    string content = await result.Content.ReadAsStringAsync();

                    deserializedResultTest = JsonConvert.DeserializeObject<Response>(content)
                        ?? throw new Exception("Failed to fetch an object");
                }

                var firstVariable = deserializedResultTest?.variables.FirstOrDefault();
                var valueTexts = firstVariable?.valueTexts ?? new List<string>();

                var response = new Response
                {
                    variables = new List<VariableValueTexts> { new VariableValueTexts { valueTexts = valueTexts } }
                };

                await SendAsync(response);

            }

            catch (WebException ex)
            {
                throw new Exception("Failed to deserialize the response.", ex);
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
