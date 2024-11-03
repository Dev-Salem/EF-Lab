
using Chat.Dal.Initialization;

namespace Chat.Dal.Tests.Base;

public abstract class BaseTest : IDisposable{
    protected readonly ApplicationDbContext Context;
    protected readonly ITestOutputHelper OutputHelper;
    public virtual void Dispose()
    {
        Context.Dispose();
    }

    protected BaseTest(ITestOutputHelper outputHelper) {
         Context = TestHelper.GetConnect();
         OutputHelper = outputHelper;
    }
    
    protected void ExecuteInATransaction(Action actionToExecute)
    {
        var strategy = Context.Database.CreateExecutionStrategy();
        strategy.Execute(() =>
        {
            using var trans = Context.Database.BeginTransaction();
            actionToExecute();
            trans.Rollback();
        });
    }
    
    protected void ExecuteInASharedTransaction(Action<IDbContextTransaction> actionToExecute)
    {
        var strategy = Context.Database.CreateExecutionStrategy();
        strategy.Execute(() =>
        {
            using IDbContextTransaction trans =
                Context.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            actionToExecute(trans);
            trans.Rollback();
        });
    }
}

public sealed class EnsureChatDatabaseTestFixture : IDisposable
{
    public EnsureChatDatabaseTestFixture() {
        var context = TestHelper.GetConnect();
        SampleDataInitialization.ClearData(context);
        context.Dispose();
    }
    public void Dispose()
    {}
}