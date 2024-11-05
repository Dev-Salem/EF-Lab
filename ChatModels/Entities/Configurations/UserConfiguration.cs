using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatModels.Entities.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User> {
    public void Configure(EntityTypeBuilder<User> builder) {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.HasMany(x => x.Posts)
            .WithOne(x => x.UserNavigation).OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(x => x.UserId);
        builder.HasMany(x=>x.Comments).WithOne(x=>x.UserNavigation)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}