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

    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]

    public class MessageController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        public MessageController(IDatingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id) {
             // verify if the user trying to update is the user of the id itself
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                return Unauthorized();
            }

            var messageFromRepo = await _repo.GetMessage(id);

            if(messageFromRepo == null) {
                return NotFound();
            }

            return Ok(messageFromRepo);

            

        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId, [FromQuery]MessageParams messageParams) {
             // verify if the user trying to update is the user of the id itself
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                return Unauthorized();
            }

            messageParams.UserId = userId;

            var messagesFromRepo = await _repo.GetMessagesForUser(messageParams);

            var messages = _mapper.Map<IEnumerable<MessageForReturn>>(messagesFromRepo);

            return Ok(messages);
        }


        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int userId, int recipientId) {
             // verify if the user trying to update is the user of the id itself
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                return Unauthorized();
            }
            var messageFromRepo = await _repo.GetMessageThread(userId, recipientId);
            var messageThread = _mapper.Map<IEnumerable<MessageForReturn>>(messageFromRepo);
            return Ok(messageThread);
        }


        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageForCreationDto messageForCreationDto) 
        {

            var sender = await _repo.GetUser(userId);
            // verify if the user trying to update is the user of the id itself
            if(sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                return Unauthorized();
            }

            messageForCreationDto.SenderId = userId;

            var recipient = await _repo.GetUser(messageForCreationDto.RecipientId);

            if(recipient == null) {
                return BadRequest("Could not find the user");
            }

            var message = _mapper.Map<Message>(messageForCreationDto);

            _repo.add(message);

            

            if(await _repo.SaveAll()) {
                var messageForReturn = _mapper.Map<MessageForReturn>(message);
                return CreatedAtRoute("GetMessage", new {id = message.Id}, messageForReturn);
            }

            throw new Exception("Creating message failed");
            

        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id, int userId) {
             // verify if the user trying to update is the user of the id itself
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                return Unauthorized();
            }

            var messageFromRepo = await _repo.GetMessage(id);

            if(messageFromRepo.SenderId == userId) {
                messageFromRepo.SenderDeleted = true;
            }

            if(messageFromRepo.RecipientId == userId) {
                messageFromRepo.RecipientDeleted = true;
            }

            if(messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted) {
                _repo.delete(messageFromRepo);
            }

            if(await _repo.SaveAll()) {
                return NoContent();
            }

            throw new Exception("Error deleting the message");

        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageRead(int userId, int id) {
             // verify if the user trying to update is the user of the id itself
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                return Unauthorized();
            }

            var message = await _repo.GetMessage(id);

            message.IsRead = true;
            message.DateRead = DateTime.Now;

            await _repo.SaveAll();

            return NoContent();
        }


    }
}