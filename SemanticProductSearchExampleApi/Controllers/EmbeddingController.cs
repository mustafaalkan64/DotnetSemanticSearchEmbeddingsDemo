using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SemanticProductSearchExampleApi.Models;
using SemanticProductSearchExampleApi.Services;
using System.Text.Json;

namespace SemanticProductSearchExampleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly EmbeddingService _embeddingService;
        private readonly QdrantClientService _qdrant;

        public ProductsController(EmbeddingService embeddingService, QdrantClientService qdrant)
        {
            _embeddingService = embeddingService;
            _qdrant = qdrant;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            var embedding = await _embeddingService.GetEmbeddingAsync($"{product.Name} {product.Description} {product.Category}");
            await _qdrant.AddDocumentAsync(product.Id, JsonSerializer.Serialize(product), embedding);
            return Ok(product);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            var embedding = await _embeddingService.GetEmbeddingAsync(q);
            var results = await _qdrant.SearchAsync(embedding);
            var products = results.Select(json => JsonSerializer.Deserialize<Product>(json)).ToList();
            return Ok(products);
        }
    }
}
