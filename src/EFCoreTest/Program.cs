using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFCoreTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //PopulateDatabase();
            var results = GetMatchingPostsByTitle("First post");
            Console.WriteLine($"Returned {results.Count()} results.");
            foreach(var result in results)
            {
                Console.WriteLine($"Returned post title: {result.Title}, postID: {result.BlogId}");
            }

            Console.ReadKey();
        }

        private static void PopulateDatabase()
        {
            var firstPost = new Post { Content = "This is another content", CreateDate = DateTime.Now, Title = "Second post" };
            var secondPost = new Post { Content = "This is another content", CreateDate = DateTime.Now, Title = "Second post" };
            var blog = new Blog { Url = "https://cmatskas.com" };
            blog.Posts.Add(firstPost);
            blog.Posts.Add(secondPost);

            using (var context = new BlogDbContext())
            {
                context.Database.EnsureCreated();
                context.Blogs.Add(blog);
                context.SaveChanges();
            }
        }

        private static IEnumerable<Post> GetMatchingPostsByTitle(string searchTerm)
        {
            var posts = new List<Post>();
            using (var context = new BlogDbContext())
            {
                posts = context.Posts
                    .FromSql("SELECT * FROM dbo.GetMatchingPostByTitle({0})", searchTerm)
                    .Where(p => p.BlogId == 1)
                    .OrderByDescending(p => p.CreateDate)
                    .ToList();
            }

            return posts;
        }
    }
}
