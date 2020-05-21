using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Life.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id {get; set;}
        public string name {get; set;}
        public string email {get; set;}
        public string oauthSubject {get; set;}
        public string oauthIssuer {get; set;}
        public string Password { get; set; }
        public string Username { get; set; }
    }

    public class UserView
    {
        public string tokenId {get; set;}
    }
}