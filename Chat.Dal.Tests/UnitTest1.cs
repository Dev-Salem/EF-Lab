namespace Chat.Dal.Tests;

public class UnitTest1 {
    [Fact]
    public void Test1() {
        Assert.Equal(3+5,8);
    }

    [Theory]
    [InlineData(1, 3, 4)]
    public void TheoryTest(int a, int b, int sum) {
        Assert.Equal(sum, a+b);
    }
}

public static class TestHelper {
    public static ApplicationDbContext GetConnect() {
        var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionBuilder.UseNpgsql(ApplicationDbContextFactory.GetConnectionString());
        return new ApplicationDbContext(optionBuilder.Options);
    }

    public static ApplicationDbContext GetSecondConnection(
        ApplicationDbContext oldContext,
        IDbContextTransaction trans) {
        var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionBuilder.UseNpgsql(oldContext.Database.GetConnectionString());
        var context = new ApplicationDbContext(optionBuilder.Options);
        context.Database.UseTransaction(trans.GetDbTransaction());
        return context;
    }
}