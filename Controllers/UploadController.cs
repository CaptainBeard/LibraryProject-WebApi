using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Net.Http.Headers;
using System.Drawing;

namespace library_project.Controllers
{
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Upload()
        {
            try
            { 
                var file = Request.Form.Files[0];
                Console.WriteLine(file);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string format = fileName.Substring(fileName.Length -4);
                    string newName = fileName.Substring(0, fileName.Length -4);
                    string base_filepath = "user_avatars/images/";
                    var fullPath = Path.Combine(base_filepath, fileName);
                    var dbPath = Path.Combine(base_filepath, fileName);
                    // Checks if image is less than 5 MB and it's .png or .jpg
                    if (file.Length > 5000000)
                    {
                        Console.WriteLine("Image is too big. Max size is 5 MB.");
                        return BadRequest();
                    }
                    if (format != ".png" && format != ".jpg")
                    {
                        Console.WriteLine("You can upload only .png or .jpg images.");
                        return BadRequest();
                    }
                    // Creates a bitmap from the filestream, resizes it and saves it to the server
                    using (Image image = Image.FromStream(file.OpenReadStream()))
                    {
                        Bitmap uploadImg = new Bitmap(image, 250, 250);
                        uploadImg.Save(base_filepath + newName + ".png", System.Drawing.Imaging.ImageFormat.Png);
                    }
                    return Ok(new { dbPath });

                    
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}