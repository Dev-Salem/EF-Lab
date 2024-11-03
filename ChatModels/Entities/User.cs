using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChatModels.Entities.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ChatModels.Entities;
[EntityTypeConfiguration(typeof(UserConfiguration))]
[Table("User")]
public partial class User : BaseEntity {

    public string Name { get; set; } = "Name";
    public  List<Post> Posts { get; set; } = [];

    public List<Comment> Comments { get; set; } = [];
    public override string ToString() {
        return $"(Name: {Name}, Id: {Id}, Comments: {Comments.Count}, Posts: {Posts.Count}), ";
    }
}
