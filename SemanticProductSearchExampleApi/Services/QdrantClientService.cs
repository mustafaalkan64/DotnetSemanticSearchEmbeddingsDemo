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

        public async Task<List<string>> SearchAsync(float[] queryVector)
        {
            var result = await _qdrantClient.SearchAsync(CollectionName, queryVector, limit: 5, offset: 0);
            return result.Select(r => r.Payload["text"].StringValue).ToList();
        }
    }
}
