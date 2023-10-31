using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using WasteWatch.Models;

namespace WasteWatch.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{

		public DbSet<Image> Images { get; set; }
		public DbSet<Category> Categories { get; set; }
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configuration for IdentityUserLogin entity because it errors otherwise
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(u => u.UserId);


            SeedCategories(modelBuilder);
        }

        private void SeedCategories(ModelBuilder modelBuilder)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFilePath = Path.Combine(baseDirectory, "App_Data", "Categories.json");
            List<string> categoryNames = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(jsonFilePath));

            for (int i = 0; i < categoryNames.Count; i++)
            {
                modelBuilder.Entity<Category>().HasData(
                    new Category
                    {
                        Id = i + 1,
                        CategoryName = categoryNames[i]
                    }
                );
            }
        }

    }
}