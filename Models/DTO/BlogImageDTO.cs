﻿namespace CodePulse.API.Models.DTO
{
    public class BlogImageDTO
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
