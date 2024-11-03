
namespace Chat.Dal.Repos;

public class UserRepo : BaseRepo<User>, IUserRepo{
    public UserRepo(ApplicationDbContext context) : base(context) { }
    public UserRepo(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    internal IIncludableQueryable<User, List<Post>> BuildBaseQuery() =>
        Table
            .Include(c => c.Comments)
            .Include(u => u.Posts);

    public override IEnumerable<User> GetAll() {
        return BuildBaseQuery();
    }

    public override IEnumerable<User> GetAllIgnoreQueryFilter() {
        return BuildBaseQuery().IgnoreQueryFilters();
    }

    public override User? Find(int? id) =>BuildBaseQuery().FirstOrDefault(x => x.Id == id);
}