using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResultPattern.Entites;

namespace ResultPattern.Data.Configurations;

internal sealed class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");

        builder.HasKey(x => x.Id);

        builder.Property(p => p.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Content)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasMany(p => p.Comments)
            .WithOne(c => c.Post)
            .HasForeignKey(p => p.PostId)
            .HasConstraintName("FK_Post_Comments");
    }
}