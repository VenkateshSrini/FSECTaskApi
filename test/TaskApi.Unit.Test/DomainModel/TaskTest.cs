using System;
using System.Collections.Generic;
using System.Text;
using TaskAPI.DomainModel;
using Xunit;
namespace TaskApi.Unit.Test.DomainModel
{
    public class TaskTest
    {
        [Fact]
        public void GetSetTest()
        {
            var task = new Tasks
            {
                EndDate = DateTime.Today.AddDays(1),
                StartDate = DateTime.Today,
                ParentTask = new ParentTask { Parent_ID = 1, Parent_Task = 1 },
                ParentTaskId = 1,
                TaskDeatails = "Task",
                TaskId = 1,
                Priortiy=1,
                Status=1
                
            };
            Assert.Equal(DateTime.Today.AddDays(1), task.EndDate);
            Assert.Equal(DateTime.Today, task.StartDate);
            Assert.Equal(1, task.ParentTask.Parent_ID);
            Assert.Equal(1, task.ParentTask.Parent_Task);
            Assert.Equal(1, task.Status);
            Assert.Equal(1, task.Priortiy);
            Assert.Equal(1, task.ParentTaskId);
            Assert.Equal("Task", task.TaskDeatails);
            Assert.Equal(1, task.TaskId);
        }
    }
}
