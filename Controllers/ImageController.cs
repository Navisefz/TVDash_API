using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using TV_DASH_API.Models;

namespace ImageUpload.Controllers
{//route
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ImageDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ImageController(ImageDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

       /* [HttpGet]
        public ActionResult<IEnumerable<ImageModel>> GetImages(int floor)
        {
            string folderName = $"Floor_{floor}_Images";
            string folderPath = Path.Combine(_hostEnvironment.ContentRootPath, folderName);

            if (!Directory.Exists(folderPath))
            {
                return NotFound();
            }

            var imagePaths = Directory.GetFiles(folderPath);
            var imageModels = imagePaths.Select(imagePath => new ImageModel()
            {
                ImageName = Path.GetFileName(imagePath),
                ImageSrc = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/{folderName}/{Path.GetFileName(imagePath)}"
            });

            return Ok(imageModels);
        }
       */
       //get method
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageModel>>> GetImages(int floor)
        {
            string root = "Images";
           string folderName = $"Floor_{floor}_Images";
            return await _context.TVDash_Images
                .Where(x => x.Floor == floor)
                .Select(x => new ImageModel()
                {
                   ImageID = x.ImageID,
                   Floor=x.Floor,
                   ImageName = x.ImageName,
                 //  ImageSrc = String.Format("{0}://{1}{2}/$"Floor_{ floor }_Images"/{3}", Request.Scheme, Request.Host, Request.PathBase, x.ImageName)
                    ImageSrc = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/{root}/{folderName}/{x.ImageName}"
                })
                .ToListAsync();
        }
       

        [HttpGet("{id}")]
        public async Task<ActionResult<ImageModel>> GetImageModel(int id)
        {
            var imageModel = await _context.TVDash_Images.FindAsync(id);

            if (imageModel == null)
            {
                return NotFound();
            }

            return imageModel;
        }


        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImageModel(int id, [FromForm] ImageModel imageModel, int floor )
        {
            if (id != imageModel.ImageID)
            {
                return BadRequest();
            }

            if (imageModel.ImageFile != null)
            {
                DeleteImage(imageModel.ImageName,id);
                imageModel.ImageName = await SaveImage(imageModel.ImageFile, floor);
            }

            _context.Entry(imageModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<IActionResult> UploadImages(List<IFormFile> files , int floor)
        {
            foreach (var file in files)
            {
                var imageName = await SaveImage(file, floor) ;
                var imageModel = new ImageModel { ImageName = imageName, Floor = floor };
                
                _context.TVDash_Images.Add(imageModel);
                
            }

            await _context.SaveChangesAsync();

            return Ok("Success");
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult<ImageModel>> DeleteImageModel(int id)
        {
            var imageModel = await _context.TVDash_Images.FindAsync(id);
            if (imageModel == null)
            {
                return NotFound();
            }
            DeleteImage(imageModel.ImageName,id);
            _context.TVDash_Images.Remove(imageModel);
            await _context.SaveChangesAsync();
            //return imageModel;
            return this.Content("application/json");
        }

       


        private bool ImageModelExists(int id)
        {
            return _context.TVDash_Images.Any(e => e.ImageID == id);
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile, int floorNumber)
        { 
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(20).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("-yy-MM-dd-hhmms") + Path.GetExtension(imageFile.FileName);
            string root= "Images";
            string folderName = $"Floor_{floorNumber}_Images";
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, root, folderName, imageName);

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return imageName;
        }


        [NonAction]
        public void DeleteImage(string imageName, int floorNumber)
        {
            string root = "Images";
            string folderName = $"Floor_{floorNumber}_Images";
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, root, folderName, imageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }

    
    }
}
