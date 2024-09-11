using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace BlazorApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        [HttpPost("Upload")]
        public async Task<IActionResult> Upload()
        {
            var file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/images/recipes_photo", file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Return the relative URL
                var relativeUrl = $"/images/recipes_photo/{file.FileName}";
                return Ok(new { Url = relativeUrl });
            }
            return BadRequest("File upload failed");
        }
    }
}








// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using System.IO;
// using System.Threading.Tasks;
//
// namespace BlazorApp.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class UploadController : ControllerBase
//     {
//         [HttpPost("Upload")]
//         public async Task<IActionResult> Upload()
//         {
//             var file = Request.Form.Files[0];
//             if (file.Length > 0)
//             {
//                 var filePath = Path.Combine("wwwroot/images/recipes_photo", file.FileName);
//
//                 using (var stream = new FileStream(filePath, FileMode.Create))
//                 {
//                     await file.CopyToAsync(stream);
//                 }
//
//                 // Return the relative URL
//                 var relativeUrl = $"/images/recipes_photo/{file.FileName}";
//                 return Ok(new { Url = relativeUrl });
//             }
//             return BadRequest("File upload failed");
//         }
//     }
// }