using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using TaskAPI.DomainModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Logging;
using TaskAPI.Repository;
using TaskAPI.Messages;

namespace TaskApi.Unit.Test.Repository
{
   public class TaskRepoTest
    {
        [Theory]
        [InlineData(2,-1,-1,"","",0)]
        public void GetTaskForAllCriteriaTest(int taskId, int parentTaskId, int priority, 
            string start, string end, int status)
        {
            //common setup
            var mockDbsetTask = new Mock<DbSet<Tasks>>();
            var mockDbsetParTask = new Mock<DbSet<ParentTask>>();
            var taskList = new List<Tasks> {
                (new Tasks{
                    EndDate=(!string.IsNullOrWhiteSpace(end))?DateTime.Parse(end):DateTime.MinValue,
                    StartDate=(!string.IsNullOrWhiteSpace(start))?DateTime.Parse(start):DateTime.MinValue,
                    ParentTaskId=parentTaskId,
                    Priortiy=priority,
                    Status=status,
                    TaskId=taskId,
                    TaskDeatails = "Test task",
                    ParentTask = new ParentTask{Parent_ID=1, Parent_Task=taskId}
                })
            }.AsQueryable();
            var parTaskList = new List<ParentTask> { new ParentTask {
                Parent_ID = 1,
                Parent_Task=taskId
            } }.AsQueryable<ParentTask>();
            mockDbsetTask.As<IQueryable<Tasks>>().Setup(m => m.Provider).Returns(taskList.Provider);
            mockDbsetTask.As<IQueryable<Tasks>>().Setup(m => m.Expression).Returns(taskList.Expression);
            mockDbsetTask.As<IQueryable<Tasks>>().Setup(m => m.ElementType).Returns(taskList.ElementType);
            mockDbsetTask.As<IQueryable<Tasks>>().Setup(m => m.GetEnumerator()).Returns(taskList.GetEnumerator());

            mockDbsetParTask.As<IQueryable<ParentTask>>().Setup(m => m.Provider).Returns(parTaskList.Provider);
            mockDbsetParTask.As<IQueryable<ParentTask>>().Setup(m => m.Expression).Returns(parTaskList.Expression);
            mockDbsetParTask.As<IQueryable<ParentTask>>().Setup(m => m.ElementType).Returns(parTaskList.ElementType);
            mockDbsetParTask.As<IQueryable<ParentTask>>().Setup(m => m.GetEnumerator()).Returns(parTaskList.GetEnumerator());
            DbContextOptions<TaskContext> contextOption = new DbContextOptions<TaskContext>();
            var mockTaskContext = new Mock<TaskContext>(contextOption);
            mockTaskContext.SetupGet(ctx => ctx.Tasks).Returns(mockDbsetTask.Object);
            mockTaskContext.SetupGet(ctx => ctx.ParentTasks).Returns(mockDbsetParTask.Object);
            mockTaskContext.SetupSet(ctx => ctx.Tasks = mockDbsetTask.Object);

            mockTaskContext.SetupSet(ctx => ctx.ParentTasks = mockDbsetParTask.Object);
            var searchMessage = new SearchMsg { 
                FromDate = (!string.IsNullOrWhiteSpace(start)) ? DateTime.Parse(start) : DateTime.MinValue,
                ToDate= (!string.IsNullOrWhiteSpace(end)) ? DateTime.Parse(end) : DateTime.MinValue,
                ParentTaskId = parentTaskId,
                Priority = priority,
                TaskId = taskId
            };
            
            //common setup ends
            var repoLogger = new LoggerFactory().CreateLogger<TaskRepo>();
            var taskRepo = new TaskRepo(mockTaskContext.Object, repoLogger);
            var result =  taskRepo.GetTaskForAllCriteria(searchMessage);
            Assert.Single(result);


        }
    }
}
