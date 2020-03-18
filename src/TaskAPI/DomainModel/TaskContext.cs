using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskAPI.DomainModel
{
    public class TaskContext:DbContext
    {
        public DbSet<ParentTask> ParentTasks { get; set; }
        public TaskContext(DbContextOptions<TaskContext> contextOptions)
           : base(contextOptions)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParentTask>()
                        .Property(p => p.Parent_ID).ValueGeneratedOnAdd();
            modelBuilder.Entity<Task>()
                        .Property(p => p.TaskId).ValueGeneratedOnAdd();
        }
    }
}
