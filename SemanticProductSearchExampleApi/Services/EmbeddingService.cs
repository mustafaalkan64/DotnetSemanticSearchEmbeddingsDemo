namespace SemanticProductSearchExampleApi.Services
{
    using System.Net.Http.Json;

    public class EmbeddingService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public EmbeddingService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
        }

        public async Task<float[]> GetEmbeddingAsync(string input)
        {
            var configValue = _configuration["Ollama:BaseUrl"];
            var envValue = Environment.GetEnvironmentVariable("OLLAMA_API_URL");

            var ollamaUrl = !string.IsNullOrEmpty(envValue) ? envValue : configValue;
            // if you use your local, change ollama to localhost
            var response = await _httpClient.PostAsJsonAsync($"{ollamaUrl}/api/embeddings", new
            {
                model = "nomic-embed-text",
                prompt = input
            });

            var json = await response.Content.ReadFromJsonAsync<EmbeddingResponse>();
            return json?.Embedding ?? Array.Empty<float>();
        }

        public class EmbeddingResponse
        {
            public float[] Embedding { get; set; } = [];
        }
    }
}
