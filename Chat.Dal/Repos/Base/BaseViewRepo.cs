namespace Chat.Dal.Repos.Base;

public interface IBaseViewRepo<T> : IDisposable where T : class {
     ApplicationDbContext Context { get; }
     IEnumerable<T> ExecuteSqlString(string sql);
     IEnumerable<T> GetAll();
     IEnumerable<T> GetAllIgnoreQueryFilter();
    
}

public abstract class BaseViewRepo<T> : IBaseViewRepo<T> where T : class, new() {
     public ApplicationDbContext Context { get; }
     public virtual IEnumerable<T> ExecuteSqlString(string sql) => Table.FromSqlRaw(sql);

     public virtual IEnumerable<T> GetAll() => Table.AsQueryable();

     public virtual IEnumerable<T> GetAllIgnoreQueryFilter()=> Table.IgnoreQueryFilters().AsQueryable();

     public DbSet<T> Table { get; }
     public BaseViewRepo(ApplicationDbContext context) {
          Context = context;
          Table = Context.Set<T>();
     }

     protected BaseViewRepo(DbContextOptions<ApplicationDbContext> options):
 this(new ApplicationDbContext(options)) {
        
     }
     
     private void Dispose(bool disposing) {
          if (disposing) { Context.Dispose(); }
     }

     public void Dispose() {
          Dispose(true);
          GC.SuppressFinalize(this);
     }

     ~BaseViewRepo() {
          Dispose(false);
     }
}
