using System.Net.Http.Headers;


public class AuthenticationMessageHandler(IHttpContextAccessor _httpContextAccessor) : DelegatingHandler
{
    
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = _httpContextAccessor?.HttpContext?.Request.Cookies["access_token"];
        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }
}