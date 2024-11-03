namespace Chat.Dal.Repos;

public class CommentRepo: BaseRepo<Comment>, ICommentRepo {
    public CommentRepo(ApplicationDbContext context) : base(context) { }
    public CommentRepo(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    internal IIncludableQueryable<Comment, User?> BuildBaseQuery() =>
        Table
            .Include(c => c.PostNavigation)
            .Include(u => u.UserNavigation);

    public override IEnumerable<Comment> GetAll() {
        return BuildBaseQuery();
    }

    public override IEnumerable<Comment> GetAllIgnoreQueryFilter() {
        return BuildBaseQuery().IgnoreQueryFilters();
    }

    public override Comment? Find(int? id) =>BuildBaseQuery().FirstOrDefault(x => x.Id == id);

}