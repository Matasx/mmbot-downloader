using System.Net;

namespace Downloader.Core.Core
{
    public class TransientErrorRetryHttpClientHandler : HttpClientHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.ConnectionClose = true;

            for (var i = 0; i < 3; i++)
            {
                var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
                var valid = !IsTransientHttpStatusCode(response);

                if (valid || cancellationToken.IsCancellationRequested)
                {
                    return response;
                }
                response.Dispose();
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken).ConfigureAwait(false);
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        private static bool IsTransientHttpStatusCode(HttpResponseMessage response)
        {
            return (int)response.StatusCode >= 500 || response.StatusCode == HttpStatusCode.RequestTimeout || response.StatusCode == HttpStatusCode.TooManyRequests;
        }
    }
}
