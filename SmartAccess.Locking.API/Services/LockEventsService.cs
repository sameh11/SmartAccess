using SmartAccess.Locking.API.Models.Request;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace SmartAccess.Locking.API.Services
{
    public class LockEventsService : ILockEventsService
    {
        private readonly IHttpClientFactory _lockEventHttpClient;
        private readonly ILogger<LockEventsService> _logger = null;

        public LockEventsService() { }
        public LockEventsService(IHttpClientFactory lockEventHttpClient, ILogger<LockEventsService> logger)
        {
            _lockEventHttpClient = lockEventHttpClient;
            _logger = logger;
        }

        public async Task LogLockRequest(LockEventRequest lockevent)
        {
            var todoItemJson = new StringContent(JsonSerializer.Serialize(lockevent), Encoding.UTF8, Application.Json);
            var httpClient = _lockEventHttpClient.CreateClient("LockEventsClient");
            var httpResponseMessage = await httpClient.PostAsync("lockevents", todoItemJson);

            _logger.LogInformation($"Sending event to LockEventApi");


            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<LockEventRequest>(contentStream);
            }
        }
    }
}
