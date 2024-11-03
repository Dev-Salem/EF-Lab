using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatModels.Entities.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment> {
    public void Configure(EntityTypeBuilder<Comment> builder) {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Content).HasMaxLength(255);
        builder.HasOne(x => x.UserNavigation).WithMany(x => x.Comments).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x=>x.PostNavigation).WithMany(x => x.Comments).OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(x => x.PostNavigation).IsRequired(true);
        builder.Navigation(x => x.UserNavigation).IsRequired(true);
    }
}