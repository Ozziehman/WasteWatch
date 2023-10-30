using Microsoft.AspNetCore.Mvc;
using WasteWatch.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WasteWatch.Data;

namespace WasteWatch.Controllers
{
	public class ImageController : Controller
	{
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ImageController> _logger;

        public ImageController(ApplicationDbContext context, ILogger<ImageController> logger)
        {
            _context = context;
            _logger = logger;
        }
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
            session.SetString("CurrentIndex", "0");

            return View("ImageDisplay");
        }

        //Next image
        public IActionResult NextImagePage([FromServices] IHttpContextAccessor httpContextAccessor)
        {

            // Get the current session
            var session = httpContextAccessor.HttpContext.Session;

            int currentIndex = Int32.Parse(session.GetString("CurrentIndex"));
            int nextIndex = currentIndex + 1;
            session.SetString("CurrentIndex", nextIndex.ToString());

            return View("ImageDisplay");
        }


        //STOP marking boxes
        public IActionResult StopImagePage([FromServices] IHttpContextAccessor httpContextAccessor)
        {
            // Get the current session
            var session = httpContextAccessor.HttpContext.Session;
            session.Clear();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult UploadDataToDb(string boxes)
        {
            //Get correct picture with the Imagemodels forms essionstorage and the current index of the list of images that is stored there
            //Raw Data of image could not be passed through AJAX because of size limitations so it gets it straight through the server side sessionstorage
            int currentIndex = Int32.Parse(HttpContext.Session.GetString("CurrentIndex"));
            string jsonImageModels = HttpContext.Session.GetString("ImageModels");
            List<ImageModel> imageModels = JsonConvert.DeserializeObject<List<ImageModel>>(jsonImageModels);
           
            byte[] rawImageDataByte = imageModels[currentIndex].ImageData;

            Image image = new Image
            {
                ImageData = rawImageDataByte,
                Boxes = boxes
            };

            _context.Images.Add(image);
            int result = _context.SaveChanges();


            var testimage = _context.Images.Find(3);
            var testbase64 = Convert.ToBase64String(testimage.ImageData);

            if (result > 0)
            {
                return Json(new { success = true, responseText = "Succesfully uploaded to db" });   
            }
            return Json(new { success = false, responseText = "Failed to upload to db" });


        }
    }
}
