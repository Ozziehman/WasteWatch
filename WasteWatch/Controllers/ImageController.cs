using Microsoft.AspNetCore.Mvc;
using WasteWatch.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WasteWatch.Controllers
{
	public class ImageController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

        [HttpPost]
        public IActionResult UploadImage(IFormFile file)
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


                return View("ImageDisplay", imageModel);
            }

            return View("Index");
        }
    }
}
