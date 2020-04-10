using System;
using System.Collections.Generic;
using System.Text;
using TaskAPI.Messages;
using Xunit;
namespace TaskApi.Unit.Test.Messages
{
    public class TaskAddTest
    {
        [Fact]
        public void GetterSetterTest()
        {
            var taskAdd = new TaskAdd
            {
                EndDate= DateTime.Today.AddDays(1),
                StartDate = DateTime.Today,
                ParentTaskId=1,
                Priority=1,
                TaskDescription="task1",
                TaskId=0
            };
            Assert.Equal(DateTime.Today.AddDays(1), taskAdd.EndDate);
            Assert.Equal(DateTime.Today, taskAdd.StartDate);
            Assert.Equal(1, taskAdd.ParentTaskId);
            Assert.Equal(1, taskAdd.Priority);
            Assert.Equal("task1", taskAdd.TaskDescription);
            Assert.Equal(0, taskAdd.TaskId);
        }
    }
}
