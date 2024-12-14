using API.Data;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(DataContext context) : BaseAPIController
    {
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region ActionResults
        [HttpGet] // api/user
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var userList = await context.Users.ToListAsync();

            return userList;
        }

        [Authorize]
        [HttpGet("{id:int}")] // api/user/3
        public async Task<ActionResult<User>> GetUsers(int id)
        {
            var user = await context.Users.FindAsync(id);

            return user == null ? NotFound() : user;
        }
        #endregion

        #region Methods
        #endregion
    }
}
