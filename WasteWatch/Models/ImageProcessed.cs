using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WasteWatch.Models
{
    public class ImageProcessed : Image
    {
        public string Boxes { get; set; }
        public string BoxesYOLO { get; set; }
        public IdentityUser ProcessedBy { get; set; }
    }
}
