using System;
using System.Collections.Generic;
using System.Text;
using TaskAPI.Messages;
using Xunit;
namespace TaskApi.Unit.Test.Messages
{
   public class SearchMsgTest
    {
        [Fact]
        public void GetterSetter()
        {
            SearchMsg searchMsg = new SearchMsg
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today.AddDays(1),
                ParentTaskId = 1,
                PriorityFrom = 1,
                TaskId = 1
            };
            Assert.Equal(DateTime.Today, searchMsg.FromDate);
            Assert.Equal(DateTime.Today.AddDays(1), searchMsg.ToDate);
            Assert.Equal(1, searchMsg.ParentTaskId);
            Assert.Equal(1, searchMsg.PriorityFrom);
            Assert.Equal(1, searchMsg.TaskId);
        }
    }
}
