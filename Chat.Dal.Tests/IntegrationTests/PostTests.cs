

using Chat.Dal.Repos;
using Chat.Dal.Repos.Interfaces;

namespace Chat.Dal.Tests.IntegrationTests;

public class PostTests: BaseTest, IClassFixture<EnsureChatDatabaseTestFixture> {
    private readonly IPostRepo _postRepo;
    public PostTests(ITestOutputHelper outputHelper) : base(
        outputHelper) {
        _postRepo = new PostRepo(Context);
    }

    [Fact]
    public void ShouldGetAllPosts() {
        var query = Context.Posts.AsQueryable();
        var qs = query.ToQueryString();
        OutputHelper.WriteLine(qs);
        var posts = query.ToList();
        Assert.Equal(3, posts.Count);
    }

    [Fact]
    public void ShouldGetPostContent() {
        var query = Context.Posts.AsQueryable();
        var qs = query.ToQueryString();
        OutputHelper.WriteLine(qs);
        var posts = query.ToList();
        foreach (var post in posts) {
            Assert.Contains( "Content",post.Content);
        }
    }
    
    
    [Theory]
    [InlineData(1, 1)]
    [InlineData(2,1)]
    [InlineData(3,1)]
    public void ShouldGetPostById(int id, int expectedCount) {
        var query = Context.Posts.Where(p => p.Id == id);
        var qs = query.ToQueryString();
        OutputHelper.WriteLine(qs);
        var posts = query.ToList();
        Assert.Equal(posts.Count, expectedCount);
    }
    
    
    [Fact]
    public void ShouldGetPostsByUsingRepo() {
        var query = ((PostRepo)_postRepo).GetAll().AsQueryable();
        var qs = query.ToQueryString();
        OutputHelper.WriteLine(qs);
        var posts = query.ToList();
        Assert.Equal(3, posts.Count);
        foreach (var post in posts) {
            Assert.NotEmpty(post.Comments);
        }
    }
}
