using System;
using System.Collections.Generic;
using System.Text;
using TaskAPI.Messages;
using Xunit;
namespace TaskApi.Unit.Test.Messages
{
    public class ParentTaskMsgTest
    {
        [Fact]
        public void GetterSetterTest()
        {
            var parentMsg = new ParentTaskMsg
            {
                ParentTask_ID = 1,
                Parent_ID = 22,
                Parent_Task_Description = "task"
            };
            Assert.Equal(1, parentMsg.ParentTask_ID);
            Assert.Equal(22, parentMsg.Parent_ID);
            Assert.Equal("task", parentMsg.Parent_Task_Description);
        }
    }
}

