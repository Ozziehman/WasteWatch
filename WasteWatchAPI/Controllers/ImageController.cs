using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WasteWatch.Models;
using WasteWatch.DataAccessLayer;
using WasteWatch.Data;
using Microsoft.IdentityModel.Tokens;

namespace WasteWatch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ImageRepository _imageRepository;

        public ImageController(ApplicationDbContext dbContext)
        {
            _imageRepository = new ImageRepository(dbContext);
        }

        [HttpGet]
        public IActionResult GetAllImages()
        {
            var images = _imageRepository.GetAllImages();
            return Ok(images);
        }

        [HttpGet("{id}")]
        public IActionResult GetImageById(int id)
        {
            var image = _imageRepository.GetImageById(id);
            if (image == null)
            {
                return NotFound();
            }
            return Ok(image);
        }

        [HttpPost]
        public IActionResult AddImage(Image image)
        {
            // Convert the base64 string to a byte array
            if (!string.IsNullOrEmpty(image.ApiBase64Data))
            {
                try
                {
                    byte[] binaryData = Convert.FromBase64String(image.ApiBase64Data);
                    image.BinaryData = binaryData;

                    // Add the Image to the repository
                    _imageRepository.AddImage(image);
                    return CreatedAtAction(nameof(GetImageById), new { id = image.Id }, image);
                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                    return BadRequest("Invalid base64 format: " + ex.Message);
                }
            }
            else
            {
                // Handle the case where the ApiBase64Data field is empty or null
                return BadRequest("Base64 data is required.");
            }
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteImage(int id)
        {
            _imageRepository.DeleteImage(id);
            return NoContent();
        }
    }
}
