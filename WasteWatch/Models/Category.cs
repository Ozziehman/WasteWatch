﻿namespace WasteWatch.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public ICollection<ImageProcessed> ProcessedImages { get; set; }
    }
}
