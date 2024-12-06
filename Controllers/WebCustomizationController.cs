using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Dapper;
using KalakariWeb.Data;
using KalakariWeb.Models;

namespace KalakariWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebCustomizationController : ControllerBase
    {
        private readonly DatabaseConnection _dbConnection;

        public WebCustomizationController(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // Insert new customization record
        [HttpPost("Insert")]
        public async Task<IActionResult> InsertCustomization([FromBody] WebCustomization customizationDto)
        {
            if (customizationDto == null || string.IsNullOrWhiteSpace(customizationDto.UserEmail))
            {
                return BadRequest("Invalid input data.");
            }

            // Updated SQL query to include new columns
            var insertQuery = @"
        INSERT INTO Tbl_Web_Customization 
        (UserEmail, Header, Topic, Author, Image, Content, Footer, InsertedOn, ModifiedOn, IsActive) 
        VALUES (@UserEmail, @Header, @Topic, @Author, @Image, @Content, @Footer, GETDATE(), GETDATE(), @IsActive);
    ";

            var result = await _dbConnection.Connection.ExecuteAsync(insertQuery, new
            {
                customizationDto.UserEmail,
                customizationDto.Header,
                customizationDto.Topic,
                customizationDto.Author,
                customizationDto.Image,
                customizationDto.Content,
                customizationDto.Footer,
                IsActive = customizationDto.IsActive ?? true
            });

            return Ok(new { Message = "Customization record inserted successfully.", AffectedRows = result });
        }

        // Update an existing customization record
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateCustomization([FromBody] WebCustomization customizationDto)
        {
            if (customizationDto == null || customizationDto.Id == 0)
            {
                return BadRequest("Invalid input data.");
            }

            var updateQuery = @"
                UPDATE Tbl_Web_Customization
                SET 
                    Header = @Header,
                    Topic = @Topic,
                    Footer = @Footer,
                    ModifiedOn = GETDATE(),
                    IsActive = @IsActive
                WHERE 
                    Id = @Id;
            ";

            var result = await _dbConnection.Connection.ExecuteAsync(updateQuery, new
            {
                customizationDto.Id,
                customizationDto.Header,
                customizationDto.Topic,
                customizationDto.Footer,
                IsActive = customizationDto.IsActive
            });

            return result > 0
                ? Ok(new { Message = "Customization record updated successfully.", AffectedRows = result })
                : NotFound(new { Message = "Record not found or no changes made." });
        }

        // Optional: Fetch all customizations for a user
        [HttpGet("GetByUserEmail")]
        public async Task<IActionResult> GetByUserEmail(string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
            {
                return BadRequest("User email is required.");
            }

            var selectQuery = @"
                SELECT * FROM Tbl_Web_Customization WHERE UserEmail = @UserEmail AND IsActive = 1 ORDER BY 1 DESC;
            ";

            var records = await _dbConnection.Connection.QueryAsync<WebCustomization>(selectQuery, new { UserEmail = userEmail });

            return Ok(records);
        }
    }
}
