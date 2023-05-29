using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TV_DASH_API.Models
{
    public class ImageDbContext : DbContext
    {
        public ImageDbContext(DbContextOptions<ImageDbContext> options) : base(options)
        {

        }
        public DbSet<ImageModel> TVDash_Images { get; set; }
    }
}
