
namespace Chat.Dal.Repos;

public class PostRepo: BaseRepo<Post>, IPostRepo {
    public PostRepo(ApplicationDbContext context) : base(context) { }
    public PostRepo(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    internal IIncludableQueryable<Post, User?> BuildBaseQuery() =>
        Table
            .Include(c => c.Comments)
            .Include(u => u.UserNavigation);

    public override IEnumerable<Post> GetAll() {
        return BuildBaseQuery();
    }

    public override IEnumerable<Post> GetAllIgnoreQueryFilter() {
        return BuildBaseQuery().IgnoreQueryFilters();
    }

    public override Post? Find(int? id) =>BuildBaseQuery().FirstOrDefault(x => x.Id == id);
}