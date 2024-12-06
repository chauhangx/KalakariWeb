namespace KalakariWeb.Controllers
{
    // Controllers/SignupController.cs
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Dapper;
    using System.Security.Cryptography;
    using System.Text;
    using KalakariWeb.Data;
    using KalakariWeb.Models;

    [Route("api/[controller]")]
    [ApiController]
    public class SignupController : ControllerBase
    {
        private readonly DatabaseConnection _dbConnection;

        public SignupController(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpDto userDto)
        {
            //userDto.Username
            if (userDto == null  || string.IsNullOrWhiteSpace(userDto.Email) || string.IsNullOrWhiteSpace(userDto.Password))
            {
                return BadRequest("Invalid input data.");
            }

            // Hash the password
            var passwordHash = HashPassword(userDto.Password);

            // Check if user already exists
            var existingUser = await _dbConnection.Connection.QuerySingleOrDefaultAsync<User>("SELECT * FROM Users WHERE Email = @Email", new { userDto.Email });
            if (existingUser != null)
            {
                return Conflict(new { Message= "User with this username or email already exists." });
            }

            // Insert new user into the database
            var insertQuery = "INSERT INTO Users (UserId, Email, PasswordHash) VALUES (NEWID(), @Email, @PasswordHash)";
            await _dbConnection.Connection.ExecuteAsync(insertQuery, new {  userDto.Email, PasswordHash = passwordHash });

            return Ok("User account created successfully.");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
        {
            if (userDto == null || string.IsNullOrWhiteSpace(userDto.Email) || string.IsNullOrWhiteSpace(userDto.Password))
            {
                return BadRequest("Invalid input data.");
            }

            // Retrieve the user from the database
            var existingUser = await _dbConnection.Connection.QuerySingleOrDefaultAsync<UserDB>("SELECT * FROM Users WHERE Email = @Email", new { userDto.Email });

            if (existingUser == null)
            {
                return Unauthorized(new { Message = "Invalid email or password." });
            }

            // Verify the password
            var inputPasswordHash = HashPassword(userDto.Password);
            if (existingUser.PasswordHash != inputPasswordHash)
            {
                return Unauthorized(new { Message = "Invalid email or password." });
            }

            // (Optional) Generate a JWT or session token here if required
            // var token = GenerateJwtToken(existingUser);

            return Ok(new { Message = "Login successful." });
        }



    }
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
       
    }
    public class UserDB: User
    {
        public string PasswordHash { get; internal set; }
    }
   
    // DTO class
    public class UserSignUpDto: User
    {
        
    }
    public class UserLoginDto : User
    {

    }

}
