using Microsoft.AspNetCore.Mvc;
using WasteWatch.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WasteWatch.Controllers
{
	public class ImageController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}


        [HttpPost]
        public IActionResult UploadImages(List<IFormFile> files, [FromServices] IHttpContextAccessor httpContextAccessor)
        {
            var imageModels = new List<ImageModel>();

            foreach (var file in files)
            {
                if (file != null && file.Length > 0)
                {
                    var imageModel = new ImageModel
                    {
                        ImageName = file.FileName,
                        ImageData = new byte[file.Length]
                    };

                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo(stream);
                        imageModel.ImageData = stream.ToArray();
                    }

                    imageModels.Add(imageModel);
                }
            }

            // Convert the list of ImageModel objects to JSON
            var jsonImageModels = JsonConvert.SerializeObject(imageModels);

            // Get the current session
            var session = httpContextAccessor.HttpContext.Session;

            // Store the JSON representation in session
            session.SetString("ImageModels", jsonImageModels);

            return View("ImageDisplay");
        }
    }
}
