

using Chat.Dal.Repos;
using Chat.Dal.Repos.Interfaces;

namespace Chat.Dal.Tests.IntegrationTests;

public class PostTests: BaseTest, IClassFixture<EnsureChatDatabaseTestFixture> {
    private readonly IPostRepo _postRepo;
    public PostTests(ITestOutputHelper outputHelper) : base(
        outputHelper) {
        _postRepo = new PostRepo(Context);
    }

    public override void Dispose() {
        base.Dispose();
    }
}
