using System.Collections.Generic;
using System.Linq;
using WasteWatch.Models;
using WasteWatch.Data;

namespace WasteWatch.DataAccessLayer
{
    public class ImageRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ImageRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Image> GetAllImages()
        {
            return _dbContext.Images.ToList();
        }

        public Image GetImageById(int id)
        {
            return _dbContext.Images.FirstOrDefault(image => image.Id == id);
        }

        public void AddImage(Image image)
        {
            _dbContext.Images.Add(image);
            _dbContext.SaveChanges();
        }

        public void UpdateImage(Image image)
        {
            _dbContext.Images.Update(image);
            _dbContext.SaveChanges();
        }

        public void DeleteImage(int id)
        {
            var image = _dbContext.Images.FirstOrDefault(i => i.Id == id);
            if (image != null)
            {
                _dbContext.Images.Remove(image);
                _dbContext.SaveChanges();
            }
        }
    }
}
