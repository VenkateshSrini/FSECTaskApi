using System;
using System.Collections.Generic;
using System.Text;
using TaskAPI.Messages;
using Xunit;
namespace TaskApi.Unit.Test.Messages
{
    public class TaskListingTest
    {
        [Fact]
        public void GetterSetterTest()
        {
            var taskListing = new TaskListing {
                EndDate = DateTime.Today.AddDays(1),
                StartDate = DateTime.Today,
                ParentTaskId = 1,
                Priority = 1,
                TaskDescription = "task1",
                TaskId = 2,
                ParentDescription="task0",
                Status=1
            };
            Assert.Equal(DateTime.Today.AddDays(1), taskListing.EndDate);
            Assert.Equal(DateTime.Today, taskListing.StartDate);
            Assert.Equal(1, taskListing.ParentTaskId);
            Assert.Equal(1, taskListing.Priority);
            Assert.Equal("task1", taskListing.TaskDescription);
            Assert.Equal(2, taskListing.TaskId);
            Assert.Equal(1, taskListing.Status);
            Assert.Equal("task0", taskListing.ParentDescription);
        }
    }
}
