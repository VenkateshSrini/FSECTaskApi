using System;
using System.Collections.Generic;
using System.Text;
using TaskAPI.Messages;
using Xunit;
namespace TaskApi.Unit.Test.Messages
{
    public class TaskModTest
    {
        [Fact]
        public void GetterSetterTest()
        {
            var taskMod = new TaskMod
            {
                EndDate = DateTime.Today.AddDays(1),
                StartDate = DateTime.Today,
                ParentTaskId = 1,
                Priority = 1,
                TaskDescription = "task1",
                TaskId = 0
            };
            Assert.Equal(DateTime.Today.AddDays(1), taskMod.EndDate);
            Assert.Equal(DateTime.Today, taskMod.StartDate);
            Assert.Equal(1, taskMod.ParentTaskId);
            Assert.Equal(1, taskMod.Priority);
            Assert.Equal("task1", taskMod.TaskDescription);
            Assert.Equal(0, taskMod.TaskId);
        }
    }
}
