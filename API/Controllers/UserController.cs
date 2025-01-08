using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Models;
using API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserRepository userRepository, IMapper mapper) : BaseAPIController
    {
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region ActionResults
        [HttpGet] // api/user
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
        {
            var userList = await userRepository.GetMembersAsync();
            return Ok(userList);
        }

        [HttpGet("{id:int}")] // api/user/3
        public async Task<ActionResult<MemberDTO>> GetUser(int id)
        {
            var user = await userRepository.GetMemberByIDAsync(id);

            return user == null ? NotFound() : user;
        }

        [HttpGet("{username}")] // api/user/3
        public async Task<ActionResult<MemberDTO>> GetUser(string username)
        {
            var user = await userRepository.GetMemberByUserNameAsync(username);

            return user == null ? NotFound() : user;
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO)
        {
            string? username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (username is null) return BadRequest("No username found in token");

            var user = await userRepository.GetUserByUserNameAsync(username);

            if (username is null) return BadRequest("No username found in token");

            mapper.Map(memberUpdateDTO, user);

            if (await userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update the user");
        }
        #endregion

        #region Methods
        #endregion
    }
}
