using ChatModels.Entities;
using ChatModels.Entities.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ChatModels.ViewModels;

[EntityTypeConfiguration(typeof(CommentViewModelConfiguration))]
[Keyless]
public class CommentViewModel : INonPersisted {
    public int Id { get; set; }
    public int PostId { get; set; }
    public required Post PostNavigation { get; set; }
    public required IEnumerable<Comment> Comments { get; set; } = [];

    public string CommentSection {
        get {
            var postContent = PostNavigation.Content;
            var postUser = PostNavigation.UserNavigation.Name;
            var comments = Comments.Aggregate("", (current, comment) => current + ($"({comment.UserNavigation.Name}: {comment.Content})" + "\n"));
            return $"============={postUser}: {postContent}============\n{comments}";
        }
    }

    public override string ToString() =>CommentSection;
    
}