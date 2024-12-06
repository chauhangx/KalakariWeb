namespace KalakariWeb.Services
{
    // Services/WebsiteService.cs
    using Dapper;
    using KalakariWeb.Data;
    using KalakariWeb.Models;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    public class WebsiteService
    {
        private readonly DatabaseConnection _databaseConnection;

        public WebsiteService(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public async Task<List<Website>> GetAllWebsitesAsync()
        {
            using (IDbConnection dbConnection = _databaseConnection.Connection)
            {
                string sql = "SELECT * FROM Websites";
                return (await dbConnection.QueryAsync<Website>(sql)).AsList();
            }
        }

        public async Task<Website> GetWebsiteByIdAsync(int id)
        {
            using (IDbConnection dbConnection = _databaseConnection.Connection)
            {
                string sql = "SELECT * FROM Websites WHERE Id = @Id";
                return await dbConnection.QueryFirstOrDefaultAsync<Website>(sql, new { Id = id });
            }
        }

        public async Task CreateWebsiteAsync(Website website)
        {
            using (IDbConnection dbConnection = _databaseConnection.Connection)
            {
                string sql = "INSERT INTO Websites (WebsiteName, WebsiteType, ContactNumber, Email, Interests, BlogTitle, BlogContent) VALUES (@WebsiteName, @WebsiteType, @ContactNumber, @Email, @Interests, @BlogTitle, @BlogContent)";
                await dbConnection.ExecuteAsync(sql, website);
            }
        }

        public async Task UpdateWebsiteAsync(Website website)
        {
            using (IDbConnection dbConnection = _databaseConnection.Connection)
            {
                string sql = "UPDATE Websites SET WebsiteName = @WebsiteName, WebsiteType = @WebsiteType, ContactNumber = @ContactNumber, Email = @Email, Interests = @Interests, BlogTitle = @BlogTitle, BlogContent = @BlogContent WHERE Id = @Id";
                await dbConnection.ExecuteAsync(sql, website);
            }
        }

        public async Task DeleteWebsiteAsync(int id)
        {
            using (IDbConnection dbConnection = _databaseConnection.Connection)
            {
                string sql = "DELETE FROM Websites WHERE Id = @Id";
                await dbConnection.ExecuteAsync(sql, new { Id = id });
            }
        }
    }

}
