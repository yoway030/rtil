using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace rtil.Database
{
    public class ReviewDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string CollectionName { get; set; } = null!;
    }

    public class ReviewDatabaseCollection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("ReviewId")]
        public string ReviewId { get; set; } = null!;

        [BsonElement("ReviewAuthor")]
        public string? ReviewAuthor { get; set; }
        
        [BsonElement("ReviewTitle")]
        public string? ReviewTitle { get; set; }

        [BsonElement("ReviewUrl")]
        public string? ReviewUrl { get; set; }

        [BsonElement("TeamsMessageId")]
        public string? TeamsMessageId { get; set; }
    }

    public class ResponseReviewContentsParse
    {
        public string ReviewId { get; set; } = null!;
        public string? ReviewAuthor { get; set; }
        public string? ReviewTitle { get; set; }
        public string? ReviewUrl { get; set; }
        public string? ReviewUpdate { get; set; }
        public string? TeamsMessageId { get; set; }
        public string? TeamsMentionBody { get; set; }
        public string? TeamsMentionAt { get; set; }
    }

    public class ReviewDatabaseService
    {
        private readonly IMongoCollection<ReviewDatabaseCollection> _reviewCollection;

        public ReviewDatabaseService(IOptions<ReviewDatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _reviewCollection = mongoDatabase.GetCollection<ReviewDatabaseCollection>(
                databaseSettings.Value.CollectionName);
        }

        public async Task<List<ReviewDatabaseCollection>> GetAsync() =>
            await _reviewCollection.Find(_ => true).ToListAsync();

        public async Task<ReviewDatabaseCollection?> GetAsync(string reviewId) =>
            await _reviewCollection.Find(x => x.ReviewId == reviewId).FirstOrDefaultAsync();

        public async Task CreateAsync(ReviewDatabaseCollection newReview) =>
            await _reviewCollection.InsertOneAsync(newReview);

        public async Task UpdateAsync(string reviewId, ReviewDatabaseCollection updatedReview) =>
            await _reviewCollection.ReplaceOneAsync(x => x.ReviewId == reviewId, updatedReview);

        public async Task RemoveAsync(string reviewId) =>
            await _reviewCollection.DeleteOneAsync(x => x.ReviewId == reviewId);
    }
}