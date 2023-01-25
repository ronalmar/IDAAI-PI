using IDAAI_APP.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace IDAAI_APP.Middleware
{
    public class TokenQueryParameterMiddleware
    {
        private readonly RequestDelegate _next;
        private OperacionesController operaciones = new();

        public TokenQueryParameterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                ////if has request header, do nothing
                //if (httpContext.Request.Headers.TryGetValue("Authorization", out var authHeader) && authHeader.Any() &&
                //    authHeader[0].StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
                //{
                //    return;
                //}

                //var endpoint = httpContext.GetEndpoint();
                //var attribute = endpoint.Metadata.OfType<JwtParameterAttribute>().FirstOrDefault();
                //if (attribute != null && httpContext.Request.Query.TryGetValue(attribute.Parameter, out var param))
                //{
                //    var token = param.FirstOrDefault();
                //    if (!string.IsNullOrWhiteSpace(token))
                //    {
                //        httpContext.Request.Headers.Add("Authorization", $"Bearer {token}");
                //    }
                //}

                //if has request header, do nothing
                //if (httpContext.Request.Headers.TryGetValue("Authorization", out var authHeader) && authHeader.Any() &&
                //    authHeader[0].StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
                //{
                //    return;
                //}

                //if (httpContext.User.Claims.ToList().Count > 0)
                //{
                //    var claims = httpContext.User.Claims.ToList();
                //    var token = claims[1].Value;
                //    httpContext.Request.Headers.Add("Authorization", $"Bearer {token}");
                //}               
                await operaciones.RenovarToken();
            }
            finally
            {
                // Call the next middleware delegate in the pipeline 
                await _next.Invoke(httpContext);
            }
        }
    }
}
