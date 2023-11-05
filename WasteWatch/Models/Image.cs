using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WasteWatch.Models
{
    public class Image
    {
        public int Id { get; set; }
        [NotMapped]
        public string? ApiBase64Data { get; set; }
        public byte[]? BinaryData { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public ICollection<Category> Categories { get; set; }

    }
}
