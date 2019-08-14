using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using datingapp.API.Data;
using datingapp.API.Dtos;
using datingapp.API.Helpers;
using datingapp.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace datingapp.API.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]


    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public UsersController(IDatingRepository repo, IMapper mapper, DataContext context)
        {
            _context = context;
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        [ServiceFilter(typeof(LogUserActivity))]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();

            var usersForReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            return Ok(usersForReturn);
        }


        [HttpGet("{id}", Name = "GetUser")]
        [ServiceFilter(typeof(LogUserActivity))]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);

            var userForReturn = _mapper.Map<UserForDetailDto>(user);

            return Ok(userForReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {

            // verify if the user trying to update is the user of the id itself
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            var user = await _repo.GetUser(id);

            _repo.delete<User>(user);

            _context.SaveChanges();

            return NoContent();


        }





        [HttpGet("{id}/getLikees")]
        [ServiceFilter(typeof(LogUserActivity))]
        public async Task<IActionResult> getLikees(int id)
        {

            var likees = await _repo.GetLikees(id);

            var userForReturn = _mapper.Map<IEnumerable<UserForListDto>>(likees);

            return Ok(userForReturn);

        }
        [HttpGet("{id}/getLikers")]
        [ServiceFilter(typeof(LogUserActivity))]
        public async Task<IActionResult> getLikers(int id)
        {

            var likers = await _repo.GetLikers(id);

            var userForReturn = _mapper.Map<IEnumerable<UserForListDto>>(likers);

            return Ok(userForReturn);

        }


        [HttpPut("{id}")]
        [ServiceFilter(typeof(LogUserActivity))]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {

            // verify if the user trying to update is the user of the id itself
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var userFromRepo = await _repo.GetUser(id);

            _mapper.Map(userForUpdateDto, userFromRepo);

            if (await _repo.SaveAll())
            {
                return NoContent();
            }

            throw new Exception($"Updating user {id} failed");
        }

        [HttpPost("{id}/like/{recipientId}")]
        [ServiceFilter(typeof(LogUserActivity))]

        public async Task<IActionResult> LikeUser(int id, int recipientId)
        {
            // verify if the user trying to update is the user of the id itself
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var like = await _repo.GetLike(id, recipientId);

            if (like != null)
            {
                return BadRequest("You have lready liked this user");
            }

            if (await _repo.GetUser(recipientId) == null)
            {
                return NotFound();
            }

            like = new Like
            {
                LikerId = id,
                LikeeId = recipientId
            };

            _repo.add<Like>(like);

            if (await _repo.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Cannot like user");


        }


    }
}