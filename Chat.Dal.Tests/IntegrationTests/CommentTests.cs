using Chat.Dal.Repos;
using Chat.Dal.Repos.Interfaces;

namespace Chat.Dal.Tests.IntegrationTests;

public class CommentTests : BaseTest, IClassFixture<EnsureChatDatabaseTestFixture> {
    private readonly ICommentRepo _commentRepo;
    public CommentTests(ITestOutputHelper outputHelper) : base(
        outputHelper) {
        _commentRepo = new CommentRepo(Context);
    }


    }
