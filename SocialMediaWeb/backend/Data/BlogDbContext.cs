using Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Backend.Data
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {

        }
        public DbSet<User>Users { get; set; }
        public DbSet<Category>Categories { get; set; }
        public DbSet<Comment>Comments { get; set; }
        public DbSet<Follow>Follows { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Reaction>Reactions { get; set; }
        public DbSet<Report> Reports { get; set; }
    }
}
