using System;
using System.Collections.Generic;
using System.Text;
using TaskAPI.DomainModel;
using Xunit;
namespace TaskApi.Unit.Test.DomainModel
{
   public class ParentTaskTest
    {
        [Fact]
        public void GetSetTest()
        {
            ParentTask parentTask = new ParentTask
            {
                Parent_ID=1,
                Parent_Task=1,
                Tasks = new List<Tasks>
                {
                    new Tasks{ TaskId=1}
                }
            };
            Assert.Equal(1, parentTask.Parent_Task);
            Assert.Equal(1, parentTask.Parent_ID);
            Assert.Single(parentTask.Tasks);
        }
    }
}
