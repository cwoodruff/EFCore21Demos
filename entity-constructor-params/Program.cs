using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Demos
{
    class Program
    {
        private static void Main()
        {
            using (var db = new BloggingContext())
            {
                // Query with a DbFunction
                var blogs = db.Blogs;

                foreach (var blog in blogs)
                {
                    Console.WriteLine(blog.Url);
                }
            }

            Console.ReadLine();
        }

        public class BloggingContext : DbContext
        {
            public DbSet<Blog> Blogs { get; set; }
            public DbSet<Post> Posts { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder
                    .UseSqlServer(
                        @"Server=(localdb)\mssqllocaldb;Database=Demo.EnityConstructorParams;Trusted_Connection=True;ConnectRetryCount=0");
            }
        }

        public class Blog
        {
            public Blog(int blogId, string url)
            {
                BlogId = blogId;
                Url = url;
            }

            public int BlogId { get; set; }
            public string Url { get; set; }

            public List<Post> Posts { get; set; } = new List<Post>();
        }

        public class Post
        {
            public Post(int postId, string title, string content, int blogId, Blog blog)
            {
                PostId = postId;
                Title = title;
                Content = content;
                BlogId = blogId;
                Blog = blog;
            }

            public int PostId { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }

            public int BlogId { get; set; }
            public Blog Blog { get; set; }
        }
    }
}
