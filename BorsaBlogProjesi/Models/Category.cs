﻿using System.Collections.Generic;

namespace BorsaBlogProjesi.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<BlogPost> BlogPosts { get; set; }

    }
}
