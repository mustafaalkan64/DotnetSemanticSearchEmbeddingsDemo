namespace SemanticProductSearchExampleApi.Services
{
    using Qdrant.Client;
    using Qdrant.Client.Grpc;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class QdrantClientService
    {
        private readonly QdrantGrpcClient _client;
        private const string CollectionName = "documents";
        private const ulong VectorSize = 4; // Örnek vektör boyutu

        private readonly QdrantClient _qdrantClient;

        public QdrantClientService(string host = "localhost", int port = 6334) // Qdrant gRPC portu genellikle 6334'tür
        {
            _qdrantClient = new QdrantClient(host, port);

        }

        //public async Task CreateCollectionAsync(string collectionName, ulong vectorSize)
        //{
        //    await _qdrantClient.CreateCollectionAsync(

        //        collectionName: CollectionName,
        //        vectorsConfig: new VectorParams
        //        {
        //            Size = vectorSize,
        //            Distance = Distance.Cosine
        //        });

        //    System.Console.WriteLine($"Koleksiyon '{collectionName}' oluşturuldu.");
        //}

        private async Task CreateCollectionAsync(string collectionName)
        {
            // Koleksiyonun zaten var olup olmadığını kontrol edebilirsiniz.
            var collections = await _qdrantClient.ListCollectionsAsync();
            if (collections.Contains(collectionName))
            {
                Console.WriteLine($"Koleksiyon '{collectionName}' zaten mevcut. Siliniyor ve yeniden oluşturuluyor...");
                await _qdrantClient.DeleteCollectionAsync(collectionName);
                await Task.Delay(100); // Küçük bir bekleme süresi
            }

            else
            {

                await _qdrantClient.CreateCollectionAsync(

                   collectionName: CollectionName,
                   vectorsConfig: new VectorParams
                   {
                       Size = VectorSize,
                       Distance = Distance.Cosine
                   });
            }
        }

        public async Task AddDocumentAsync(ulong id, string text, float[] embedding)
        {
            await CreateCollectionAsync(CollectionName);
            await _qdrantClient.UpsertAsync(CollectionName, new[]
            {
                new PointStruct
                {
                    Id = id,
                    Vectors = embedding,
                    Payload = { ["text"] = text }
                }
            });
        }

        //public async Task AddDocumentAsync(string id, string text, float[] embedding)
        //{
        //    var points = new List<PointStruct>();

        //    // Örnek 1: Basit Metin ve Kategori ile PointStruct
        //    // Vektörün boyutu burada 4 olarak varsayılmıştır.
        //    points.Add(new PointStruct
        //    {
        //        // ID olarak doğrudan ulong kullanma
        //        Id = 1,
        //        Vectors = new[] { 0.1f, 0.2f, 0.3f, 0.4f }, // 4 boyutlu vektör
        //        Payload =
        //        {
        //            // String değer ekleme
        //            { "title", Value.ForString("Qdrant ile Doküman Yönetimi") },
        //            // String değer ekleme
        //            { "category", Value.ForString("Yazılım Geliştirme") },
        //            // Boolean değer ekleme
        //            { "is_published", Value.ForBool(true) },
        //            // Sayısal değer (int) ekleme
        //            { "view_count", Value.ForNumber(1250) }
        //        }
        //    });

        //    // Örnek 2: GUID (UUID) ile PointStruct ve Farklı Veri Tipleri
        //    points.Add(new PointStruct
        //    {
        //        // ID olarak GUID kullanma (UUID tipi)
        //        Id = Guid.NewGuid().ToString(), // Qdrant'ta string UUID olarak temsil edilir.
        //        Vectors = new[] { 0.5f, 0.6f, 0.7f, 0.8f },
        //        Payload =
        //        {
        //            { "description", Value.ForString("Vektör veritabanı teknolojileri.") },
        //            { "tags", Value.ForList( // Liste (Array) değeri ekleme
        //                Value.ForString("AI"),
        //                Value.ForString("Machine Learning"),
        //                Value.ForString("Database")
        //            )},
        //            { "author_id", Value.ForNumber(456) },
        //            { "rating", Value.ForNumber(4.7f) } // Float değer ekleme
        //        }
        //    });

        //    // Örnek 3: Sadece Vektör ve ID ile PointStruct (Payload olmadan)
        //    // Payload alanı isteğe bağlıdır.
        //    points.Add(new PointStruct
        //    {
        //        Id = 3,
        //        Vectors = { 0.9f, 0.8f, 0.7f, 0.6f }
        //        // Payload boş bırakılabilir.
        //    });
        //}

        public async Task<List<string>> SearchAsync(float[] queryVector)
        {
            var result = await _qdrantClient.SearchAsync(CollectionName, queryVector, limit: 5, offset: 0);
            return result.Select(r => r.Payload["text"].StringValue).ToList();
        }
    }
}
