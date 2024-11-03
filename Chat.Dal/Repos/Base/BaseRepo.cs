namespace Chat.Dal.Repos.Base;

public interface IBaseRepo<T> where T : class, new() {
    T? Find(int? id);
    T? FindAsNoTracking(int id);
    T? FindIgnoreQueryFilters(int id);
    void ExecuteParameterizedQuery(string sql, object[] sqlParametersObjects);
    int Add(T entity, bool persist = true);
    int AddRange(IEnumerable<T> entities, bool persist = true);
    int Update(T entity, bool persist = true);
    
    int UpdateRange(IEnumerable<T> entities, bool persist = true);
    int Delete(int id, byte[] timeStamp, bool persist = true);
    int Delete(T entity, bool persist = true);
    int DeleteRange(IEnumerable<T> entities, bool persist = true);
    int SaveChanges();
}

public abstract class BaseRepo<T> : BaseViewRepo<T>, IBaseRepo<T>
    where T : BaseEntity, new() {
    protected BaseRepo(ApplicationDbContext context) : base(context) { }
    protected BaseRepo(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public virtual T? Find(int? id) => Table.Find(id);

    public T? FindAsNoTracking(int id) => Table.AsNoTrackingWithIdentityResolution()
        .FirstOrDefault(x => x.Id == id);

    public T? FindIgnoreQueryFilters(int id) =>
        Table.IgnoreQueryFilters().FirstOrDefault(x => x.Id == id);

    public void ExecuteParameterizedQuery(string sql, object[] sqlParametersObjects) => Context.Database.ExecuteSqlRaw(sql, sqlParametersObjects);

    public int Add(T entity, bool persist = true) {
        Table.Add(entity);
       return persist? SaveChanges():
        0;
    }

    public int AddRange(IEnumerable<T> entities, bool persist = true) {
       Table.AddRange(entities);
       return persist? SaveChanges(): 0;
    }

    public int Update(T entity, bool persist = true) {
       Table.Update(entity);
       return persist? SaveChanges(): 0;
    }

    public int UpdateRange(IEnumerable<T> entities, bool persist = true) {
       Table.UpdateRange(entities);
       return persist? SaveChanges(): 0;
    }

    public int Delete(int id, byte[] timeStamp, bool persist = true) {

        return persist? SaveChanges(): 0;
    }

    public int Delete(T entity, bool persist = true) {
        Table.Remove(entity);
        return persist? SaveChanges(): 0;
    }

    public int DeleteRange(IEnumerable<T> entities, bool persist = true) {
        Table.RemoveRange(entities);
        return persist? SaveChanges(): 0;
    }

    public int SaveChanges() {
        try {
         return   Context.SaveChanges();
        } catch (CustomException e) {
            Console.WriteLine(e);
            throw;
        } catch (Exception e) {
            throw new CustomException("An error occured updating database", e);
        }

    }
}

