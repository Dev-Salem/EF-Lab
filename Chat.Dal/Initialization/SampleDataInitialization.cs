namespace Chat.Dal.Initialization;

public static class SampleDataInitialization {
    internal static void DropAndCreateDatabase(ApplicationDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.Migrate();
    }

    public static readonly List<User> Users =
    [
        new User
        {
            Name = "Matt",
            Posts = [],
            Comments = []
        },

        new User
        {
            Name = "Faris",
            Posts = [],
            Comments = []
        },

        new User
        {
            Name = "Sabine",
            Posts = [],
            Comments = []
        },

    ];

    public static readonly List<Post> Posts =
    [
        new Post
        {
            Content = "Content 1",
            UserId = Users[0].Id,
            UserNavigation = Users[0],
            Comments = []
        },

        new Post
        {
            Content = "Content 2",
            UserId = Users[1].Id,
            UserNavigation = Users[1],
            Comments = []
        },

        new Post
        {
            Content = "Content 3",
            UserId = Users[2].Id,
            UserNavigation = Users[2],
            Comments = []
        }

    ];

    public static readonly List<Comment> Comments =[
        new Comment
        {
            Content = "Comment 1",
            UserId = Users[0].Id,
            UserNavigation = Users[0],
            PostId = Posts[0].Id,
            PostNavigation = Posts[0],
        },
        new Comment
        {
            Content = "Comment 2",
            UserId = Users[1].Id,
            UserNavigation = Users[1],
            PostId = Posts[1].Id,
            PostNavigation = Posts[1],
        },
        new Comment
        {
            Content = "Comment 3",
            UserId = Users[2].Id,
            UserNavigation = Users[2],
            PostId = Posts[2].Id,
            PostNavigation = Posts[2],
        },
    ];
    public static void ClearData(ApplicationDbContext context) {
         using var transaction =  context.Database.BeginTransaction();
        try
        {
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE \"Comment\", \"Post\", \"User\" RESTART IDENTITY CASCADE;");
            context.Users.AddRange(Users);
            context.Posts.AddRange(Posts);
            context.Comments.AddRange(Comments);
            context.SaveChanges();
           transaction.Commit();
          
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception("Database reset and seeding failed", ex);
        }
    }
    }

    
/*

   var serviceCollection = new ServiceCollection();
   serviceCollection
       .AddDbContext<ApplicationDbContext>(
           options=> options.UseNpgsql(ApplicationDbContextFactory.GetConnectionString()));
   var serviceProvider = serviceCollection.BuildServiceProvider();
   var designTimeModel = serviceProvider.GetService<IModel>();

   foreach (var entityName in entites) {
       var entity = context.Model.FindEntityType(entityName);
       var tableName = entity?.GetTableName();
       var schemaName = entity?.GetSchema();
       if (!string.IsNullOrWhiteSpace(schemaName) && !string.IsNullOrWhiteSpace(tableName))
       {
           // Delete all rows in the specified table
           context.Database.ExecuteSqlRaw($"DELETE FROM \"{schemaName}\".\"{tableName}\";");

           // Truncate the table and restart identity column
           context.Database.ExecuteSqlRaw($"TRUNCATE TABLE \"{schemaName}\".\"{tableName}\" RESTART IDENTITY CASCADE;");
       }
       // else
       // {
       //     throw new ArgumentException("Schema name and table name must not be null or empty.");
       // }
   }
*/