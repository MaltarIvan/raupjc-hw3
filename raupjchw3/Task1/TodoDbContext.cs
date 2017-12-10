using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace Task1
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(String connectionString) : base(connectionString)
        {
        }

        public IDbSet<TodoItem> TodoItems { get; set; }
        public IDbSet<TodoItemLabel> TodoItemLabels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoItem>().HasKey(t => t.Id);
            modelBuilder.Entity<TodoItem>().Property(t => t.Text).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(t => t.IsCompleted).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(t => t.UserId).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(t => t.DateCreated).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(t => t.DateCompleted).IsOptional();
            modelBuilder.Entity<TodoItem>().Property(t => t.DateDue).IsOptional();
            modelBuilder.Entity<TodoItem>().HasMany(t => t.Labels).WithMany(l => l.LabelTodoItems);

            modelBuilder.Entity<TodoItemLabel>().HasKey(l => l.Id);
            modelBuilder.Entity<TodoItemLabel>().Property(l => l.Value).IsRequired();
            modelBuilder.Entity<TodoItemLabel>().HasMany(l => l.LabelTodoItems).WithMany(t => t.Labels);
        }
    }
}
