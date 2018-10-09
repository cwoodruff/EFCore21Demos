using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Demos
{
    public class Program
    {
        private static void Main()
        {
            using (var db = new BloggingContext())
            {
                var blogs = db.Blogs.ToList();

                foreach (var blog in blogs)
                {
                    Console.WriteLine(blog.Url);
                }
            }

            Console.ReadLine();
        }
        
        private static void SetupDatabase()
        {
            using (var db = new BloggingContext())
            {

            }
        }
    }

    public class BloggingContext : DbContext
    {
        public BloggingContext()
        {
        }

        public BloggingContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>(
                entity =>
                {
                    entity.HasIndex(e => e.BlogId);

                    entity.Property(e => e.Url)
                        .IsRequired()
                        .HasMaxLength(255);

                    //entity.HasMany<Post>();
                }
            );
            
            modelBuilder.Entity<Post>(
                entity =>
                {
                    entity.HasIndex(e => e.PostId);

                    entity.Property(e => e.Title)
                        .IsRequired()
                        .HasMaxLength(255);
                    
                    entity.Property(e => e.Content)
                        .IsRequired();

                    //entity.HasOne(e => e.Blog);
                }
            );
            
            modelBuilder.Entity<Blog>().HasData(new Blog {BlogId = 1, Url = "http://sample.com"});
            
            modelBuilder.Entity<Post>().HasData(
                new {BlogId = 1, PostId = 1, Title = "First post", Content = "Test 1"},
                new {BlogId = 1, PostId = 2, Title = "Second post", Content = "Test 2"});
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                        @"Server=(localdb)\mssqllocaldb;Database=Demo.DataSeeding;Trusted_Connection=True;ConnectRetryCount=0")
                    .UseLoggerFactory(new LoggerFactory().AddConsole((s, l) => l == LogLevel.Information && !s.EndsWith("Connection")));
            }
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }

        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
