using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChatModels.Entities.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ChatModels.Entities;

[EntityTypeConfiguration(typeof(PostConfiguration))]
[Table("Post")]
public partial class Post :BaseEntity {

    [Column("content")] public string Content { get; set; } = "Content..";
        
    public int UserId { get; set; }

    public virtual User UserNavigation { get; set; } = null!;

    public List<Comment> Comments { get; set; } = [];
    public override string ToString() {
        return $"(ID: {UserId}, Content: {Content}, User: {UserId}, Post: {Id})";
    }
}
