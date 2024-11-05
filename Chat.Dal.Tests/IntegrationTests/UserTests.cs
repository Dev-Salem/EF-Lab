using Chat.Dal.Exceptions;
using Chat.Dal.Repos;
using Chat.Dal.Repos.Interfaces;
using Chat.Dal.Tests.Base;
using ChatModels.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Chat.Dal.Tests.IntegrationTests;

[Collection("Integration Tests")]
public class UserTests : BaseTest, IClassFixture<EnsureChatDatabaseTestFixture> {
    private readonly IUserRepo _userRepo;

    public UserTests(ITestOutputHelper outputHelper) : base(
        outputHelper) {
        _userRepo = new UserRepo(Context);
    }

    [Fact]
    public void ShouldGetAllUsers() {
        var query = Context.Users;
        var qs = query.ToQueryString();
        OutputHelper.WriteLine(qs);
        var users = query.ToList();
        Assert.Equal(3, users.Count);
    }

    [Fact]
    public void ShouldGetUsersWithLessThan50Characters() {
        var query = Context.Users.Where(u => u.Name.Length <= 5);
        var qs = query.ToQueryString();
        OutputHelper.WriteLine(qs);
        var users = query.ToList();
        Assert.Equal(2, users.Count);
        foreach (var user in users) {
            Assert.InRange(user.Name.Length, 1, 5);
        }
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 1)]
    public void ShouldGetUserById(int id, int expectedCount) {
        var query = Context.Users.Where(u => u.Id == id);
        var qs = query.ToQueryString();
        OutputHelper.WriteLine(qs);
        var users = query.ToList();
        Assert.Equal(users.Count, expectedCount);
        Assert.Single(users);
        Assert.Empty(users.Single().Posts);
        Assert.Empty(users.Single().Comments);
    }

    [Fact]
    public void ShouldGetUsersByUsingRepo() {
        var query = ((UserRepo)_userRepo).GetAll().AsQueryable();
        var qs = query.ToQueryString();
        OutputHelper.WriteLine(qs);
        var users = query.ToList();
        Assert.Equal(3, users.Count);
        foreach (var user in users) {
            Assert.NotEmpty(user.Posts);
            Assert.NotEmpty(user.Comments);
        }
    }

    [Fact]
    public void ShouldGetUserWithFirstCharMAndLastCharT() {
        var query = Context.Users
            .Where(u => u.Name.StartsWith("M") && u.Name.EndsWith("t"));
        var qs = query.ToQueryString();
        OutputHelper.WriteLine(qs);
        var users = query.ToList();
        Assert.Single(users);
    }


    [Fact]
    public void FirstShouldThrowExceptionIfNonMatch() {
        Assert.Throws<InvalidOperationException>(() => Context.Users.First(u => u.Id >3));
    }

    [Fact]
    public void SingleOrDefaultShouldReturnNullIfNonMatch() {
        var query = Context.Users.SingleOrDefault(u => u.Id>3);
        Assert.Null(query);
    }

    [Fact]
    public void ShouldGetAllUsersWithPosts() {
        var query = Context.Users
            .Include(u => u.Posts);
        var qs = query.ToQueryString();
        OutputHelper.WriteLine(qs);
        var users = query.ToList();
        Assert.Equal(3, users.Count);
        foreach (var user in users) {
            Assert.Single(user.Posts);
        }
    }
    
    
    [Fact]
    public void ShouldGetAllUsersWithComments() {
        var query = Context.Users
            .Include(u => u.Comments);
        var qs = query.ToQueryString();
        OutputHelper.WriteLine(qs);
        var users = query.ToList();
        Assert.Equal(3, users.Count);
        foreach (var user in users) {
            Assert.Single(user.Comments);
        }
    }
        
    [Fact]
    public void ShouldGetAllUsersWithPostsAndComments() {
        var query = Context.Users
            .Include(u=>u.Posts)
            .Include(u => u.Comments);
        var qs = query.ToQueryString();
        OutputHelper.WriteLine(qs);
        var users = query.ToList();
        Assert.Equal(3, users.Count);
        users.ForEach(u => {
            Assert.Single(u.Posts);
            Assert.Single(u.Comments);
        });
    }
    
    [Fact]
    public void ShouldGetCollectionRelatedInformationExplicitly() {
        var user = Context.Users.First();
        var query = Context
            .Entry(user).Collection(u => u.Posts).Query();
        var qs = query.ToQueryString();
        OutputHelper.WriteLine(qs);
        query.Load();
        OutputHelper.WriteLine(user.ToString());
        Assert.NotNull(user.Posts);
    }

    [Fact]
    public void ShouldGetUsersUsingRawSql() {
        
        var query = Context
            .Users.FromSqlRaw($"SELECT u.id, u.\"Name\" FROM \"User\" AS u");
        var qs = query.ToQueryString();
        OutputHelper.WriteLine(qs);
        var users = query.ToList();
        Assert.Equal(3, users.Count);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 1)]
    public void ShouldGetTheCountOfUsersById(int id, int expectedCount) {
        var count = Context.Users.Count(u => u.Id == id);
        Assert.Equal(expectedCount, count);
    }
    
    [Fact]
    public void ShouldGetTheAverageOfAllPosts() {
        var average = Context.Users.Average(u => u.Posts.Count);
        Assert.Equal(1, average);
    }
    
    [Fact]
    public void ShouldAddUser() {
        ExecuteInATransaction(RunTheTest);
        void RunTheTest() {
            var user = new User
            {
                Name = "Maya",
                Posts = [],
                Comments = []
            };
            var count = Context.Users.Count();
            Context.Users.Add(user);
            Context.SaveChanges();
            OutputHelper.WriteLine($"Add: {user}");
            var newCount = Context.Users.Count();
            Assert.Equal(count+1, newCount);
        }
    }
    
    [Fact]
    public void ShouldAddUserUsingAttach() {
        ExecuteInATransaction(RunTheTest);
        void RunTheTest() {
            var user = new User
            {
                Name = "Maya",
                Posts = [],
                Comments = []
            };
            var count = Context.Users.Count();
            Context.Users.Attach(user);
            Assert.Equal(EntityState.Added, Context.Entry(user).State);
            Context.SaveChanges();
            OutputHelper.WriteLine($"Add: {user}");
            var newCount = Context.Users.Count();
            Assert.Equal(count+1, newCount);
        }
    }
    
    [Fact]
    public void ShouldAddMultipleUsers() {
        ExecuteInATransaction(RunTheTest);
        return;

        void RunTheTest() {
            List<User> users = [
               new() {Name = "Michael", Posts = [], Comments = []},
               new() {Name = "Barack", Posts = [], Comments = []},
               new() {Name = "Destiny", Posts = [], Comments = []},
            ];
            var count = Context.Users.Count();
            Context.Users.AddRange(users);
            Context.SaveChanges();
            var newCount = Context.Users.Count();
            Assert.Equal(count+3, newCount);
        }
    }
    
    [Fact]
    public void ShouldAddAnObjectGraph() {
        ExecuteInATransaction(RunTheTest);
        void RunTheTest() {
            var user = new User
            {
                Name = "Maya",
                Posts = [
                new Post { Content = "My Content 4", }
                ],
                Comments = []
            };

            Context.Users.Add(user);
            Context.SaveChanges();
            var lastUser = Context.Users.Single(u => u.Id == 4);
            var count = lastUser.Posts.Count();
            Assert.Equal(1, count);
        }
    }
    [Fact]
    public void ShouldUpdateAUser() {
        ExecuteInATransaction(RunTheTest);
        return;
        void RunTheTest() {
            var user = Context.Users.First();
            Assert.Equal("Matt", user.Name);
            user.Name = "Rat";
            Context.SaveChanges();
            // var context2 = TestHelper.GetSecondConnection(Context, trans);
            Assert.Equal("Rat", user.Name);
        }
    }

    [Fact]
    public void ShouldUpdateUntrackedUser() {
        ExecuteInATransaction(RunTheTest);
        return;

        void RunTheTest() {
            var user = Context.Users.AsNoTracking().First(u=>u.Id ==1);
            Assert.Equal("Matt", user.Name);
            var modifiedUser = (User)user.Clone();
            modifiedUser.Name = "Rat";
            Context.Users.Update(modifiedUser);
            var user2 = Context.Users.First(u => u.Id ==1);
            Assert.Equal("Rat", user2.Name);
        }
    }

    // [Fact]
    // public void ShouldThrowConcurrencyException() {
    //     ExecuteInATransaction(RunTheTest);
    //     return;
    //
    //     void RunTheTest() {
    //         var user = Context.Users.First(u=>u.Id ==1);
    //         Context.Users.FromSqlInterpolated(
    //             $"Update \"User\" set name='Rat' where id = {user.Id}");
    //         Context.SaveChanges();
    //         var user2 = Context.Users.First(u=>u.Id ==1);
    //         OutputHelper.WriteLine($"Update: {user2.Name}");
    //         // var ex = Assert.Throws<CustomConcurrencyException>(
    //         //     () => Context.SaveChanges());
    //         // var entry = ((DbUpdateConcurrencyException) ex.InnerException)?.Entries[0];
    //         // PropertyValues originalProps = entry.OriginalValues;
    //         // PropertyValues currentProps = entry.CurrentValues;
    //         // PropertyValues databaseProps = entry.GetDatabaseValues();
    //         // OutputHelper
    //         //     .WriteLine($"Original Props: {originalProps}, currentProps = {currentProps}, databaseProps: {databaseProps}");
    //     }
    // }

    [Fact]
    public void ShouldDeleteAUser() {
        ExecuteInATransaction(RunTheTest);
        return;
        void RunTheTest() {
            var userCount = Context.Users.Count();
            var user = Context.Users.First(u=>u.Id ==1);
            Context.Users.Remove(user);
            Context.SaveChanges();
            var newUserCount = Context.Users.Count();
            Assert.Equal(userCount-1, newUserCount);
            Assert.Equal( EntityState.Detached,Context.Entry(user).State);
        }
    }
    [Fact]
    public void ShouldDeleteNonTrackingUser() {
        ExecuteInATransaction(RunTheTest);
        return;
        void RunTheTest() {
            var userCount = Context.Users.Count();
            var user = Context.Users.AsNoTracking().First(u=>u.Id ==1);
            Context.Entry(user).State = EntityState.Deleted;
            Context.SaveChanges();
            var newUserCount = Context.Users.Count();
            Assert.Equal(userCount-1, newUserCount);
            Assert.Equal( EntityState.Detached,Context.Entry(user).State);
        }
    }
}
