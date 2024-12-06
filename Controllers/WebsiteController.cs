namespace KalakariWeb.Controllers
{
    using KalakariWeb.Models;
    using KalakariWeb.Services;
    // Controllers/WebsiteController.cs
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class WebsiteController : ControllerBase
    {
        private readonly WebsiteService _websiteService;

        public WebsiteController(WebsiteService websiteService)
        {
            _websiteService = websiteService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Website>>> GetAllWebsites()
        {
            List<Website> dummy = new List<Website>()
            {
                new Website(){WebsiteName="test.com"}
            };
           // var websites = await _websiteService.GetAllWebsitesAsync();
            return Ok(dummy);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Website>> GetWebsite(int id)
        {
            var website = await _websiteService.GetWebsiteByIdAsync(id);
            if (website == null)
            {
                return NotFound();
            }
            return Ok(website);
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateWebsite([FromBody] Website website)
        {
            if (website == null || string.IsNullOrWhiteSpace(website.WebsiteName) || string.IsNullOrWhiteSpace(website.ContactNumber) || string.IsNullOrWhiteSpace(website.Email))
            {
                return BadRequest("Invalid input data.");
            }

            // Ensure that either interests or product categories are provided based on the type of website
            if (website.WebsiteType == "Blog" && string.IsNullOrWhiteSpace(website.Interests))
            {
                return BadRequest("Interests are required for a Blog.");
            }

            if (website.WebsiteType == "Online Store" && string.IsNullOrWhiteSpace(website.ProductCategories))
            {
                return BadRequest("Product categories are required for an Online Store.");
            }

            await _websiteService.CreateWebsiteAsync(website);
            return Ok(); // Return 200 OK
        }



        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateWebsite(int id, [FromBody] Website website)
        {
            //if (id != website.Id)
            //{
            //    return BadRequest();
            //}
            await _websiteService.UpdateWebsiteAsync(website);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWebsite(int id)
        {
            await _websiteService.DeleteWebsiteAsync(id);
            return NoContent();
        }
    }

}
