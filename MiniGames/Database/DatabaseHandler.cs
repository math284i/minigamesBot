using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace MiniGames.Database
{
    public class DatabaseHandler : DbContext
    {
        public virtual DbSet<DatabaseData> UsersById { get; set; }

        public DatabaseHandler()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource =  "users.db"
            };
            
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);
            optionsBuilder.UseSqlite(connection);
        }
    }
}