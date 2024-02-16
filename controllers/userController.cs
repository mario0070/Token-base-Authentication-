
using Users.models;
using Users.service;

namespace Users.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private MongoDBService _mongoDBService;
        public UsersController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }


        [HttpGet]
        public async Task<List<UsersModel>> GetUsersAsync()
        {              
            return await _mongoDBService.GetAllUser();
        }

        [HttpPost]
        public async Task<string> PostAsync([FromBody] UsersModel user)
        {
            UsersModel users = new UsersModel{
                name = user.name,
                email = user.email,
                password = PasswordHasher.HashPassword(user.password),
                phone = user.phone,
            };
            return await _mongoDBService.CreatUser(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsersModel>> GetByIdAsync(string id)
        {
            var find = await _mongoDBService.GetUserByID(id);
            if(find == null){
                return NotFound($"The id {id} does not exist");
            }
            return Ok(find);
        }

        [HttpDelete("{id}")]
        public async Task<string> DeleteUserAsync(string id)
        {
            return await _mongoDBService.DeleteUser(id);
        }

        [HttpPut("{id}")]
        public async Task<string> UpdateUserAsync([FromBody] UsersModel users, string id)
        {
            return await _mongoDBService.UpdateUser(id, users);
        }

        [HttpPost("login")]
        [Authorize]
        public async Task<string> LoginAsync([FromBody] UsersModel users)
        {
            UsersModel user = new UsersModel{
                email = users.email,
            };
            
            return await _mongoDBService.Login(user, users.password);
        }

        [HttpGet("create-jwt")]
        public async Task<ActionResult<string>> CreateToken([FromBody] UsersModel users)
        {
            var jwt = JwtService.GenerateToken("139393939939393939300", "jamiu@gmail.com");
            return $"{jwt}";
        }

         [HttpGet("validate-jwt")]
            public async Task<ActionResult<string>> ValidateToken([FromBody] UsersModel users)
            {
                var jwt = JwtService.ValidateToken("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMzkzOTM5Mzk5MzkzOTM5MzkzMDAiLCJlbWFpbCI6ImphbWl1QGdtYWlsLmNvbSIsImp0aSI6ImEyNjFhODQ2LWRlMWMtNDE2Mi04ZTE1LWYzZWYwZWZhN2IwYSIsIm5iZiI6MTcwODEwMzgzMiwiZXhwIjoxNzA4MTA3NDMyLCJpYXQiOjE3MDgxMDM4MzJ9.BIw_DER6ttfBZqN3QRFi6sAt9uQsANoqEuwu_r0g23w");
                return $"{jwt}";
            }
        
    }
}