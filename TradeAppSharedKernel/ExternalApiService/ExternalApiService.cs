using RestSharp;
using System.Text.Json;
using TradeAppSharedKernel.ExternalApiService;

namespace TradeAppSharedKernel.ExternalAPI
{
    public class ExternalApiService : IExternalApiService
    {
        private readonly HttpClient _httpClient;
        private readonly RestClient _client;

        public ExternalApiService(HttpClient httpClient, RestClient client)
        {

            _httpClient = httpClient;
            _client = client;
        }

        public async Task<List<StockListResponse>> GetStocksAsync(string token)
        {
            var request = new RestRequest("/api/Stock/GetStockList", Method.Get);
            request.AddHeader("Authorization", $"Bearer {token}");

            var response = await _client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                Console.WriteLine($"[ERROR] Stock API failed: {response.StatusCode} - {response.Content}");
                return new List<StockListResponse>();
            }

            var data = JsonSerializer.Deserialize<List<StockListResponse>>(response.Content!,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return data ?? new List<StockListResponse>();
        }

    }


}
