﻿namespace Dydaktycznie.Models
{
    public class Presentation
    {
        public int PresentationID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public int SlidesCount { get; set; }
        public int ViewCount { get; set; }
        public int CategoryID { get; set; }
        public Category? Category { get; set; }  
        public Status status { get; set; }
        public string? FileName { get; set; } // Nowa właściwość dla nazwy pliku


    }
    public enum Status
    {
        draft,
        published,
        hidden,
    }
   
    
}
