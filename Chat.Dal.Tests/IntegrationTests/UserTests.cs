using Chat.Dal.Repos;
using Chat.Dal.Repos.Interfaces;
using Chat.Dal.Tests.Base;

namespace Chat.Dal.Tests.IntegrationTests;

[Collection("Integration Tests")]
public class UserTests :BaseTest, IClassFixture<EnsureChatDatabaseTestFixture> {
    private readonly IUserRepo _userRepo;
    public UserTests(ITestOutputHelper outputHelper) : base(
        outputHelper) {
        _userRepo = new UserRepo(Context);
    }

    [Fact]
    public void ShouldGetAllUsers() {
        var qr = Context.Users.AsQueryable().ToQueryString();
        OutputHelper.WriteLine(qr);
        var users = Context.Users.ToList();
        Assert.Equal(3, users.Count);
    }

}
