﻿using Microsoft.EntityFrameworkCore;
using TatBlog.Core.Entities;
using TatBlog.Data.Mappings;

namespace TatBlog.Data.Contexts
{
    public class BlogDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Subscriber> Subscribers { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<PostTag> PostTags { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=LAPTOP-N4TOHTRH\SQLEXPRESS;Database=TatBlog;User ID=sa;Password=minhbao8102;TrustServerCertificate=True;");
        //}

        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoryMap).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostMap).Assembly);
        }
    }
}
