﻿namespace SearchWebApp.Models
{
    public record BookModel
    {
        public int Id
        {
            get; set;
        }

        public string Author
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public string Description
        {
            get; set;
        }
    }
}
