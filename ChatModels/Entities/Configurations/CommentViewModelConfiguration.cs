using ChatModels.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatModels.Entities.Configurations;

public class CommentViewModelConfiguration : IEntityTypeConfiguration<CommentViewModel>{
    public void Configure(EntityTypeBuilder<CommentViewModel> builder) {
    }
}