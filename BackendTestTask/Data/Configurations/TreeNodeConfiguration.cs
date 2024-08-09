using BackendTestTask.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendTestTask.Data.Configurations;

public class TreeNodeConfiguration : IEntityTypeConfiguration<TreeNode>
{
    public void Configure(EntityTypeBuilder<TreeNode> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(e => e.Name).IsRequired().HasMaxLength(256);

        builder.HasOne(e => e.ParentNode)
            .WithMany(e => e.Children)
            .HasPrincipalKey(e => e.Id)
            .HasForeignKey(e => e.ParentNodeId);

        builder.HasIndex(e => new { e.TreeId, e.Id }).IsUnique();
    }
}
