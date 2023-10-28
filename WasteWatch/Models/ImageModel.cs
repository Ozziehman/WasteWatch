using Newtonsoft.Json;

namespace WasteWatch.Models
{
    public class ImageModel
    {
        public string ImageName { get; set; }

        [JsonProperty("ImageData")] // Specify the JSON property name
        public string ImageDataBase64 { get; set; }

        [JsonIgnore] // Ignore this property during serialization
        public byte[] ImageData
        {
            get
            {
                return Convert.FromBase64String(ImageDataBase64);
            }
            set
            {
                ImageDataBase64 = Convert.ToBase64String(value);
            }
        }
    }
}
