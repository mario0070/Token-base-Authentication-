using MongoDB.Bson;
using MongoDB.Driver;
using Users.models;
using Microsoft.Extensions.Options;

namespace Users.service
{
    public class MongoDBService{
        private readonly IMongoCollection<UsersModel> _UserCollection = null!;

        public MongoDBService(IOptions<MongoDBSettings>  mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DBName);
            _UserCollection = database.GetCollection<UsersModel>(mongoDBSettings.Value.CollectionName);
        }     

        public async Task<List<UsersModel>> GetAllUser(){
           return await _UserCollection.Find(_ => true).ToListAsync();
        }  

        public async Task<string> CreatUser(UsersModel user){
            var find =  _UserCollection.Find(u => u.email == user.email).FirstOrDefault();
            if(find != null){
                return "User Already Existed";
            }
            await _UserCollection.InsertOneAsync(user);
            return "Registration was successful";
        }   

        public async Task<List<UsersModel>> GetUserByID(string id){
            FilterDefinition<UsersModel> filter = Builders<UsersModel>.Filter.Eq("Id", id);
            return await _UserCollection.Find(filter).ToListAsync();
        }  

        public async Task<string> DeleteUser(string id){
            FilterDefinition<UsersModel> filter = Builders<UsersModel>.Filter.Eq("Id", id);
            await _UserCollection.DeleteOneAsync(filter);
            return "User is deleted";
        }  

        public async Task<string> UpdateUser(string id, UsersModel users){
            FilterDefinition<UsersModel> filter = Builders<UsersModel>.Filter.Eq("Id", id);
            UpdateDefinition<UsersModel> update = Builders<UsersModel>.Update.Set("name", users.name);
            await _UserCollection.UpdateOneAsync(filter, update);
            return "User is Updated";
        }  

        public async Task<string> Login(UsersModel user, string password){
            var find =  _UserCollection.Find(u => u.email == user.email).FirstOrDefault();
            if(find == null){
                return "User does not exist";
            }
            var hash = PasswordHasher.VerifyPassword(password, find.password);
            if(!hash){
                return "Passord is incorrect";
            }
            return $"User {find.name} is login successfully";
        }  
        
    }
}