namespace SemanticProductSearchExampleApi.Services
{
    using System.Net.Http.Json;

    public class EmbeddingService
    {
        private readonly HttpClient _httpClient;

        public EmbeddingService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<float[]> GetEmbeddingAsync(string input)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:11434/api/embeddings", new
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
