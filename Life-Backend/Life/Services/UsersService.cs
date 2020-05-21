using System.Collections.Generic;
using Life.Models;
using MongoDB.Driver;
using MongoDatabaseSettings = Life.Models.MongoDatabaseSettings;

namespace Life.Services
{
    public class UsersService : IUsersService
    {
        private readonly IMongoCollection<User> _usersDatabase;

        public UsersService(MongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _usersDatabase = database.GetCollection<User>("users");
        }

        public List<User> Get() =>
            _usersDatabase.Find(book => true).ToList();

        public User Get(string id) =>
            _usersDatabase.Find<User>(book => book.id == id).FirstOrDefault();

        public User Create(User book)
        {
            _usersDatabase.InsertOne(book);
            return book;
        }

        public void Update(string id, User bookIn) =>
            _usersDatabase.ReplaceOne(book => book.id == id, bookIn);

        public void Remove(User bookIn) =>
            _usersDatabase.DeleteOne(book => book.id == bookIn.id);

        public void Remove(string id) =>
            _usersDatabase.DeleteOne(book => book.id == id);
    }
}