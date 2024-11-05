using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;

namespace Chat.Dal.EfStructres;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext> {
    public ApplicationDbContext CreateDbContext(string[] args) {
        var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionBuilder.UseNpgsql(GetConnectionString());
        return new ApplicationDbContext(optionBuilder.Options);
    }
    
    public static string GetConnectionString() {
    NpgsqlConnectionStringBuilder connectionBuilder =
        new()
        {
            Host = "localhost",
            Username = "devsalem",
            Password = "1234",
            Database = "cSharp",
            IncludeErrorDetail = true,
        };
    var connection = connectionBuilder.ConnectionString;
    return connection;
}
}