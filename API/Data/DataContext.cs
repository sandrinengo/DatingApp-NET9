using System;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
	public DbSet<User> Users { get; set; }
	public DbSet<Photo> Photo { get; set; }
	public DbSet<UserLike> Likes { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.Entity<UserLike>()
		.HasKey(key => new { key.SourceUserID, key.TargetUserID });

		builder.Entity<UserLike>()
		.HasOne(source => source.SourceUser)
		.WithMany(like => like.LikedUsers)
		.HasForeignKey(s => s.SourceUserID)
		.OnDelete(DeleteBehavior.Cascade);

		builder.Entity<UserLike>()
		.HasOne(source => source.TargetUser)
		.WithMany(like => like.LikedByUsers)
		.HasForeignKey(s => s.TargetUserID)
		.OnDelete(DeleteBehavior.Cascade);// Change to DeleteBehavior.NoAction because if we use Cascade SQL server doesn't like it. Cascade is okay with SQLLite
	}
}
