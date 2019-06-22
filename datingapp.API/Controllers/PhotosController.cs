using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using datingapp.API.Data;
using datingapp.API.Dtos;
using datingapp.API.Helpers;
using datingapp.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace datingapp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userid}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotosController(IDatingRepository repo, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _repo = repo;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);

        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id) {
            var photoFromRepo = await _repo.GetPhoto(id);
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }


        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm]PhotoForCreationDto photoForCreationDto){
             // verify if the user trying to update is the user of the id itself
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                return Unauthorized();
            }

            var userFromRepo = await _repo.GetUser(userId);

            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult();

            if(file.Length>0) {
                using (var stream = file.OpenReadStream()) {
                    var uploadParams = new ImageUploadParams() {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }; 
            }

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);   

            if(!userFromRepo.Photos.Any(u => u.IsMain)) {
                photo.IsMain = true;
            }

            userFromRepo.Photos.Add(photo);

            if(await _repo.SaveAll()) {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new {id = photo.Id}, photoToReturn);
            }
            

            return BadRequest("could not add the photo");

            
            


        }

        [HttpPost("{id}/setMain")]

        public async Task<IActionResult> SetMainPhoto(int userId, int id) {
              // verify if the user trying to update is the user of the id itself
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                return Unauthorized();
            }

            var user = await _repo.GetUser(userId);

            if(!user.Photos.Any(p => p.Id == id)) {
                return Unauthorized();
            }

            var photoFromRepo = await _repo.GetPhoto(id);

            if(photoFromRepo.IsMain) {
                return BadRequest("This is already a main photo");
            }

            var currentMainPhoto = await _repo.GetMainPhotoForUser(userId);
            currentMainPhoto.IsMain = false;

            photoFromRepo.IsMain = true;

            if(await _repo.SaveAll()) {
                return NoContent();
            }

            return BadRequest("Could not set photo to main");

        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeletePhoto(int userId, int id) {

            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                return Unauthorized();
            }

            var user = await _repo.GetUser(userId);

            if(!user.Photos.Any(p => p.Id == id)) {
                return Unauthorized();
            }

            var photoFromRepo = await _repo.GetPhoto(id);

            if(photoFromRepo.IsMain) {
                return BadRequest("Cannot delete the main photo");
            }

            if(photoFromRepo.PublicId != null) {
                // delete photo from cloudinary 
                var deleteParams = new DeletionParams(photoFromRepo.PublicId);

                var result = _cloudinary.Destroy(deleteParams);

                // delete photo from database
                if(result.Result == "ok") {
                    _repo.delete(photoFromRepo);
                }
            }

            if(photoFromRepo.PublicId == null) {
                 _repo.delete(photoFromRepo);
            }

            if (await _repo.SaveAll()) {
                return Ok();
            }

            return BadRequest("Failed to delete the photo");

        }



        



    }
}