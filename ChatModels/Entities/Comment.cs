using System.ComponentModel.DataAnnotations.Schema;
using ChatModels.Entities.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ChatModels.Entities;

[EntityTypeConfiguration(typeof(CommentConfiguration))]
[Table("Comment")]
public class Comment : BaseEntity {
    public string Content { get; set; } = "Content..";
    public int UserId { get; set; }
    public User? UserNavigation { get; set; }
    public int PostId { get; set; }
    public Post? PostNavigation { get; set; }

    public override string ToString() {
        return $"(ID: {Id}, Content: {Content}, User: {UserId}, Post: {PostId})";
    }
}   