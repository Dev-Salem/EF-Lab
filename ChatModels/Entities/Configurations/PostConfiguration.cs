using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatModels.Entities.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post> {
    public void Configure(EntityTypeBuilder<Post> builder) {

        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.Comments).WithOne(x => x.PostNavigation)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.UserNavigation).WithMany(x => x.Posts)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Property(x => x.Content).HasMaxLength(500).IsRequired();
    }
}