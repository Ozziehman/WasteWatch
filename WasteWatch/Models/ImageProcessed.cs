namespace WasteWatch.Models
{
    public class ImageProcessed
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; }
        public string Boxes { get; set; }
        public string BoxesYOLO { get; set; }
    }
}
