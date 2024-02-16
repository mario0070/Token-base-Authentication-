using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Users.models
{
    public class UsersModel{

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        // public ObjectId _id { get; set; }
        public string? name { get; set; }
        public string email { get; set; } = null! ;
        public string password { get; set; } = null! ;
        public int phone { get; set; }
    }
}