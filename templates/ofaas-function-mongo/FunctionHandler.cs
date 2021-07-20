using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Oni.Serverless.Function
{
    public class FunctionHandler : ControllerBase
    {
        [HttpPost("/")]
        public async Task<IActionResult> RunAsync([FromServices] IMongoClient client)
        {
            IMongoDatabase db = client.GetDatabase(
                Environment.GetEnvironmentVariable("database-name"));
            
            BsonDocument document = new BsonDocument();
            document.Add("fullName", "John Doe");
            document.Add("age", 50);
            document.Add("memberSince", DateTime.UtcNow);

            IMongoCollection<BsonDocument> collection
                = db.GetCollection<BsonDocument>("oni-collection-name");

            await collection.InsertOneAsync(document);

            return Ok($"Hello from serverless function using the .net 5.0 runtime. Today is {DateTime.Now}");
        }
    }
}
