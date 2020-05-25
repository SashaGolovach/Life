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

        public User Get(string username) =>
            _usersDatabase.Find<User>(user => user.Username == username).FirstOrDefault();

        public User Create(User book)
        {
            _usersDatabase.InsertOne(book);
            return book;
        }

        public void Update(string id, User bookIn) =>
            _usersDatabase.ReplaceOne(book => book.Id == id, bookIn);

        public void Remove(User bookIn) =>
            _usersDatabase.DeleteOne(book => book.Id == bookIn.Id);

        public void Remove(string id) =>
            _usersDatabase.DeleteOne(book => book.Id == id);
    }
}