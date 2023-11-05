using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WasteWatch.Models
{
    public class ImageProcessed
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; }
        public string Boxes { get; set; }
        public string BoxesYOLO { get; set; }
        public ICollection<Category> Categories { get; set; }
        public int OriginalImageId { get; set; }
        [ForeignKey("OriginalImageId")]
        public Image OriginalImage { get; set; }
        public IdentityUser ProcessedBy { get; set; }
    }
}
