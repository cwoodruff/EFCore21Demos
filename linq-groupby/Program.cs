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
            SetupDatabase();

            using (var db = new BloggingContext())
            {
                var service = new BlogService(db);
                var posts = db.Posts.GroupBy(o => new { o.CategoryId }).ToList();

                foreach (var post in posts)
                {
                    Console.WriteLine(post.Title);
                }
            }

            Console.ReadLine();
        }

        private static void SetupDatabase()
        {
            using (var db = new BloggingContext())
            {
                if (db.Database.EnsureCreated())
                {
                    db.Blogs.Add(new Blog { Url = "http://sample.com/blogs/Dev" });

                    db.Categories.Add(new Category {CategoryId = 1, Name = "Article"});
                    db.Categories.Add(new Category {CategoryId = 2, Name = "Video"});
                    db.Categories.Add(new Category {CategoryId = 3, Name = "Audio"});

                    db.Posts.Add(new Post {Title = "Article 1", CategoryId = 1});
                    db.Posts.Add(new Post {Title = "Video 1", CategoryId = 2});
                    db.Posts.Add(new Post {Title = "Article 2", CategoryId = 1});
                    db.Posts.Add(new Post {Title = "Article 3", CategoryId = 1});
                    db.Posts.Add(new Post {Title = "Video 2", CategoryId = 2});
                    db.Posts.Add(new Post {Title = "Audio 1", CategoryId = 3});

                    db.SaveChanges();
                }
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

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                        @"Server=(localdb)\mssqllocaldb;Database=Demo.Like;Trusted_Connection=True;ConnectRetryCount=0")
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
        
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }

    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }
}
