﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TV_DASH_API.Models
{
    public class ImageModel
    {
        [Key]
        public int ImageID { get; set; }

        // public string Name { get; set; }
        // [Column(TypeName = "nvarchar(50)")]
        //public string Description { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string ImageName { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        [NotMapped]
        public string ImageSrc { get; set; }
        public int Floor { get; set; }

    }
}
