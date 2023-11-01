﻿using Microsoft.AspNetCore.Mvc;
using WasteWatch.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WasteWatch.Data;
using System;
using Microsoft.Extensions.Logging;
using System.IO;
using System.IO.Compression;
using static System.Collections.Specialized.BitVector32;


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

        public IActionResult LoadImage(string Id, [FromServices] IHttpContextAccessor httpContextAccessor)
        {
            if (Id != null)
            {
                if (int.TryParse(Id, out int IdInt))
                {
                    var session = httpContextAccessor.HttpContext.Session;
                    session.SetString("CurrentLoadedImage", Id);
                    var image = _context.ImagesProcessed.Find(IdInt);
                    if (image != null)
                    {
                        var jsonImage = JsonConvert.SerializeObject(image);

                        session.SetString("Image", jsonImage);
                        session.SetString("CurrentIndex", "0");
                        ViewData["Categories"] = _context.Categories.ToList();
                        return View("LoadedImage");
                    }
                    else
                    {
                        Console.WriteLine("Image not found");
                        return View("Index");
                    }
                }
                else
                {
                    Console.WriteLine("Id is not a valid integer");
                    return View("Index");
                }
            }
            else
            {
                Console.WriteLine("Id is null");
                return View("Index");
            }
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

        public IActionResult UploadDataToDb(string boxes, [FromServices] IHttpContextAccessor httpContextAccessor)
        {
            byte[] rawImageDataByte;
            List<ImageModel> imageModels = new List<ImageModel>();
            //Get correct picture with the Imagemodels forms essionstorage and the current index of the list of images that is stored there
            //Raw Data of image could not be passed through AJAX because of size limitations so it gets it straight through the server side sessionstorage

            ImageProcessed currentImage = null;
            var session = httpContextAccessor.HttpContext.Session;

            
  
            int currentIndex = Int32.Parse(HttpContext.Session.GetString("CurrentIndex"));
            string jsonImageModels = HttpContext.Session.GetString("ImageModels");
            string imageJson = HttpContext.Session.GetString("Image");

            //if you have loaded multiple images from the device
            if (jsonImageModels != null) 
            {
                //Add all image models to the imageModels list
                imageModels = JsonConvert.DeserializeObject<List<ImageModel>>(jsonImageModels);
                //get the raw imageData in byte form
                rawImageDataByte = imageModels[currentIndex].ImageData;

                //___________________________________________ Data is now stored in rawImageDataByte now to add the rest...
                List<BoxModel> boxModels = JsonConvert.DeserializeObject<List<BoxModel>>(boxes);

                //convert to yolo format
                string yoloFormat = ConvertToYoloFormat(boxModels, 500, 500);
                Console.WriteLine(yoloFormat);

                _logger.LogInformation(boxes);

                //make new image object and put the data from imageModel into image
                ImageProcessed imageProcessed = new ImageProcessed()
                {
                    ImageData = rawImageDataByte,
                    Boxes = boxes,
                    BoxesYOLO = yoloFormat
                };

                _context.ImagesProcessed.Add(imageProcessed);
                int result = _context.SaveChanges();


                //var testimage = _context.Images.Find(3);
                //var testbase64 = Convert.ToBase64String(testimage.ImageData);

                if (result > 0)
                {
                    return Json(new { success = true, responseText = "Succesfully uploaded to db" });
                }
                return Json(new { success = false, responseText = "Failed to upload to db" });
            }

            //If you have loaded 1 image form the db
            else if (imageJson != null)
            {

                //check if image got loaded correctly
                if (session.GetString("CurrentLoadedImage") != null)
                {
                    //get currentImage 'from database with proper Id
                    currentImage = _context.ImagesProcessed.Find(Int32.Parse(session.GetString("CurrentLoadedImage")));
                }

                //add the image Model to the list
                imageModels.Add(JsonConvert.DeserializeObject<ImageModel>(imageJson));
                //Get the rawImageDataByte
                rawImageDataByte = imageModels[currentIndex].ImageData;

                //___________________________________________ Data is now stored in rawImageDataByte now to add the rest...
                List<BoxModel> boxModels = JsonConvert.DeserializeObject<List<BoxModel>>(boxes);

                //convert to yolo format
                string yoloFormat = ConvertToYoloFormat(boxModels, 500, 500);
                Console.WriteLine(yoloFormat);



                _logger.LogInformation(boxes);

                currentImage.Boxes = boxes;
                currentImage.BoxesYOLO = yoloFormat;
                _context.ImagesProcessed.Update(currentImage);

                int result = _context.SaveChanges();


                //var testimage = _context.Images.Find(3);
                //var testbase64 = Convert.ToBase64String(testimage.ImageData);

                if (result > 0)
                {
                    return Json(new { success = true, responseText = "Succesfully uploaded to db" });
                }
                return Json(new { success = false, responseText = "Failed to upload to db" });
            }
            else
            {
                Console.WriteLine("No image found");
                return View("Index");
            }
           


        }

        public IActionResult GetImagesAndBoxesYOLOFromDb()
        {
            var images = _context.ImagesProcessed.ToList();
            var yoloData = _context.ImagesProcessed
                .Select(image => new { Id = image.Id, BoxesYOLO = image.BoxesYOLO })
                .ToList();

            // Create a memory stream to store the zip file
            using (var zipStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    foreach (var image in images)
                    {
                        // Create an entry in the "Images" folder for each image
                        var entry = archive.CreateEntry($"Images/{image.Id}.jpg");
                        using (var entryStream = entry.Open())
                        using (var imageStream = new MemoryStream(image.ImageData))
                        {
                            imageStream.CopyTo(entryStream);
                        }
                    }

                    foreach (var data in yoloData)
                    {
                        // Create an entry in the "YOLOData" folder for each YOLO data file
                        var entry = archive.CreateEntry($"YOLOData/{data.Id}_BoxesYOLO.txt");
                        using (var entryStream = entry.Open())
                        using (var writer = new StreamWriter(entryStream))
                        {
                            writer.Write(data.BoxesYOLO);
                        }
                    }
                }

                // Set the content type and headers for the response
                Response.Headers.Add("Content-Disposition", "attachment; filename=images_and_yolo_data.zip");
                Response.ContentType = "application/zip";

                // Write the zip data to the response
                return File(zipStream.ToArray(), "application/zip");
            }
        }


    }
}
