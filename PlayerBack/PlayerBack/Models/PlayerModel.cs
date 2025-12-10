using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace PlayerBack.Models
{
    [BsonIgnoreExtraElements]
    public class PlayerModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ShortName { get; set; }
        public string Sex { get; set; }
        public string Picture { get; set; }
        public CountryModel Country { get; set; }
        public DataModel Data { get; set; }
    }
}
