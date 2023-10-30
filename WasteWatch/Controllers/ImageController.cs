using Microsoft.AspNetCore.Mvc;
using WasteWatch.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WasteWatch.Data;
using System;
using Microsoft.Extensions.Logging;
using System.IO;
using System.IO.Compression;


namespace WasteWatch.Controllers
{
	public class ImageController : Controller
	{
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ImageController> _logger;

        static string ConvertToYoloFormat(List<BoxModel> boxModels, int imageWidth, int imageHeight)
        {
            string yoloFormat = "";
            foreach (var box in boxModels)
            {
                // Convert coordinates to YOLO format
                double startX = double.Parse(box.StartX);
                double startY = double.Parse(box.StartY);
                double endX = double.Parse(box.EndX);
                double endY = double.Parse(box.EndY);

                double centerX = (startX + endX) / 2;
                double centerY = (startY + endY) / 2;
                double width = Math.Abs(endX - startX);
                double height = Math.Abs(endY - startY);

                double x = centerX / imageWidth;
                double y = centerY / imageHeight;
                double w = width / imageWidth;
                double h = height / imageHeight;

                // Append the YOLO formatted string
                yoloFormat += $"{box.Name} {x} {y} {w} {h}\n";
            }

            return yoloFormat;
        }

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

            ViewData["Categories"] = _context.Categories.ToList();

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

            ViewData["Categories"] = _context.Categories.ToList();

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
            List<BoxModel> boxModels = JsonConvert.DeserializeObject<List<BoxModel>>(boxes);

            string yoloFormat = ConvertToYoloFormat(boxModels, 500, 500);
            Console.WriteLine(yoloFormat);

            byte[] rawImageDataByte = imageModels[currentIndex].ImageData;

            _logger.LogInformation(boxes);

            Image image = new Image
            {
                ImageData = rawImageDataByte,
                Boxes = boxes,
                BoxesYOLO = yoloFormat
    };

            _context.Images.Add(image);
            int result = _context.SaveChanges();


            //var testimage = _context.Images.Find(3);
            //var testbase64 = Convert.ToBase64String(testimage.ImageData);

            if (result > 0)
            {
                return Json(new { success = true, responseText = "Succesfully uploaded to db" });   
            }
            return Json(new { success = false, responseText = "Failed to upload to db" });


        }

        public IActionResult GetImagesFromDb()
        {
            var images = _context.Images.ToList();
            if (images.Count == 0)
            {
                // Handle the case where there are no images to download.
                return NotFound();
            }

            // Create a memory stream to store the zip file
            using (var zipStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    foreach (var image in images)
                    {
                        // Create a memory stream for each image
                        using (var imageStream = new MemoryStream(image.ImageData))
                        {
                            // Create an entry in the zip archive for each image
                            var entry = archive.CreateEntry($"{image.Id}.jpg"); // Use the appropriate file extension
                            using (var entryStream = entry.Open())
                            {
                                imageStream.CopyTo(entryStream);
                            }
                        }
                    }
                }

                // Set the content type and headers for the response
                Response.Headers.Add("Content-Disposition", "attachment; filename=images.zip");
                Response.ContentType = "application/zip";

                // Write the zip data to the response
                return File(zipStream.ToArray(), "application/zip");
            }
        }

        public IActionResult GetBoxesYOLOFromDb()
        {
            var images = _context.Images
                .Select(image => new { Id = image.Id, BoxesYOLO = image.BoxesYOLO })
                .ToList();

            // Create a memory stream to store the zip file
            using (var zipStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    foreach (var image in images)
                    {
                        // Create an entry for the text data
                        using (var textEntryStream = archive.CreateEntry($"{image.Id}_BoxesYOLO.txt").Open())
                        using (var writer = new StreamWriter(textEntryStream))
                        {
                            writer.Write(image.BoxesYOLO);
                        }
                    }
                }

                // Set the content type and headers for the response
                Response.Headers.Add("Content-Disposition", "attachment; filename=boxes_yolo_data.zip");
                Response.ContentType = "application/zip";

                // Write the zip data to the response
                return File(zipStream.ToArray(), "application/zip");
            }
        }
    }
}
