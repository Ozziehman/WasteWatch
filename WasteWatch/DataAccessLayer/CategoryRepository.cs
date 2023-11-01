using System.Collections.Generic;
using System.Linq;
using WasteWatch.Data;
using WasteWatch.Models;

namespace WasteWatch.DataAccessLayer
{
    public class CategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddCategory(Category category)
        {
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();
        }

        public Category GetCategoryById(int id)
        {
            return _dbContext.Categories.FirstOrDefault(c => c.Id == id);
        }

        public List<Category> GetAllCategories()
        {
            return _dbContext.Categories.ToList();
        }

        public void UpdateCategory(Category category)
        {
            _dbContext.Categories.Update(category);
            _dbContext.SaveChanges();
        }

        public void DeleteCategory(int id)
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _dbContext.Categories.Remove(category);
                _dbContext.SaveChanges();
            }
        }
    }
}
