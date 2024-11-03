using ChatModels.Entities.Configurations;

namespace Chat.Dal.EfStructres;

public partial class ApplicationDbContext : DbContext {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options) {
        SavingChanges += (sender, args) =>
        {
            string cs = ((ApplicationDbContext)sender).Database?.GetConnectionString();
            Console.WriteLine($"Saving changes for {cs}");
        };
        SavedChanges += (sender, args) =>
        {
            string cs = ((ApplicationDbContext)sender).Database!.GetConnectionString();
            Console.WriteLine($"Saved {args!.EntitiesSavedCount} changes for {cs}");
        };
        SaveChangesFailed += (sender, args) =>
        {
            Console.WriteLine($"An exception occurred! {args.Exception.Message} entities");
        };
        
    }
    public virtual DbSet<Post> Posts { get; set; }
    
    public virtual DbSet<User> Users { get; set; }
    public DbSet<Comment> Comments { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      
        
        new UserConfiguration().Configure(modelBuilder.Entity<User>());
        new CommentConfiguration().Configure(modelBuilder.Entity<Comment>());
        new PostConfiguration().Configure(modelBuilder.Entity<Post>());
    }

    public override int SaveChanges() {
        try
        {
            return base.SaveChanges();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new CustomConcurrencyException("A concurrency error happened.", ex);
        }
        catch (RetryLimitExceededException ex)
        {

            throw new CustomRetryLimitExceededException("There is a problem with SQL Server.", ex);
        }
        catch (DbUpdateException ex)
        {
            throw new CustomDbUpdateException("An error occurred updating the database", ex);
        }
        catch (Exception ex)
        {
            throw new CustomException("An error occurred updating the database", ex);
        }
    }

}
