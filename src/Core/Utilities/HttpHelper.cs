namespace Core.Utilities
{
    public static class HttpHelper
    {
        public static string GetBaseUrl(HttpContext context)
        {
            var request = context.Request;

            var host = request.Host.ToUriComponent();

            var pathBase = request.PathBase.ToUriComponent();

            var scheme = request.Scheme;

            return $"{scheme}://{host}{pathBase}";
        }
    }
}
