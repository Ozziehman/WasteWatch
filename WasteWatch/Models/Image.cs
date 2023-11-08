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
        public bool IsProcessed { get; set; }
    }
}
