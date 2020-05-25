﻿using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Life.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id {get; set;}
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }

    public class UserView
    {
        public string tokenId {get; set;}
    }
}