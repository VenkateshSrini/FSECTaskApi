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
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Threading;

namespace TaskApi.Unit.Test.Repository
{
   public class TaskRepoTest
    {
        [Theory]
        [InlineData(2,-1,-1,"","",0)]
        [InlineData(-1, 1, -1, "", "", 0)]
        [InlineData(-1, -1, 1, "", "", 0)]
        [InlineData(-1, -1, -1, "", "", 1)]
        [InlineData(-1, -1, 1, "5/3/2019", "6/3/2019", 0)]
        [InlineData(1, 1, 1, "5/3/2019", "6/3/2019", 0)]
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

        [Theory]
        [InlineData(2, -1, -1, "", "", 0)]
        [InlineData(-1, 1, -1, "", "", 0)]
        [InlineData(-1, -1, 1, "", "", 0)]
        [InlineData(-1, -1, 1, "5/3/2019", "6/3/2019", 0)]
        [InlineData(1, 1, 1, "5/3/2019", "6/3/2019", 0)]
        public void GetTaskForAnyCriteriaTest(int taskId, int parentTaskId, int priority,
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
            var searchMessage = new SearchMsg
            {
                FromDate = (!string.IsNullOrWhiteSpace(start)) ? DateTime.Parse(start) : DateTime.MinValue,
                ToDate = (!string.IsNullOrWhiteSpace(end)) ? DateTime.Parse(end) : DateTime.MinValue,
                ParentTaskId = parentTaskId,
                Priority = priority,
                TaskId = taskId
            };

            //common setup ends
            var repoLogger = new LoggerFactory().CreateLogger<TaskRepo>();
            var taskRepo = new TaskRepo(mockTaskContext.Object, repoLogger);
            var result = taskRepo.GetTaskForAnyCriteria(searchMessage);
            Assert.Single(result);


        }
        
        [Theory(Skip ="cannot moq extension method first or default")]
        [InlineData(2, -1, -1, "", "", 0, "TestTask")]
        public async Task AddTask(int taskId, int parentTaskId, int priority,
            string start, string end, int status, string taskDetails)
        {
            var finalRepo = new List<Tasks>();
            var finalParentRepo = new List<ParentTask>();
            var taskList = new List<Tasks>{
                (new Tasks
                        {
                            EndDate = (!string.IsNullOrWhiteSpace(end)) ? DateTime.Parse(end) : DateTime.MinValue,
                            StartDate = (!string.IsNullOrWhiteSpace(start)) ? DateTime.Parse(start) : DateTime.MinValue,
                            ParentTask = (parentTaskId > -1) ? new ParentTask { Parent_Task = parentTaskId } : null,
                            Priortiy = priority,
                            Status = status,
                            TaskId = taskId,
                            TaskDeatails = "Test task",
                        })
            }.AsQueryable();
            var parTaskList = new List<ParentTask> { new ParentTask {
              
                Parent_Task=taskId
            } }.AsQueryable<ParentTask>();

            
            var mockDbsetTask = new Mock<DbSet<Tasks>>();
            var mockDbsetParTask = new Mock<DbSet<ParentTask>>();
            mockDbsetTask.As<IQueryable<Tasks>>().Setup(m => m.Provider).Returns(taskList.Provider);
            mockDbsetTask.As<IQueryable<Tasks>>().Setup(m => m.Expression).Returns(taskList.Expression);
            mockDbsetTask.As<IQueryable<Tasks>>().Setup(m => m.ElementType).Returns(taskList.ElementType);
            mockDbsetTask.As<IQueryable<Tasks>>().Setup(m => m.GetEnumerator()).Returns(taskList.GetEnumerator());
            mockDbsetTask.Setup(ctx => ctx.Add(It.IsAny<Tasks>())).Callback<Tasks>((s) => finalRepo.Add(s));
            mockDbsetParTask.As<IQueryable<ParentTask>>().Setup(m => m.Provider).Returns(parTaskList.Provider);
            mockDbsetParTask.As<IQueryable<ParentTask>>().Setup(m => m.Expression).Returns(parTaskList.Expression);
            mockDbsetParTask.As<IQueryable<ParentTask>>().Setup(m => m.ElementType).Returns(parTaskList.ElementType);
            mockDbsetParTask.As<IQueryable<ParentTask>>().Setup(m => m.GetEnumerator()).Returns(parTaskList.GetEnumerator());
            mockDbsetParTask.Setup(ctx => ctx.Add(It.IsAny<ParentTask>())).Callback<ParentTask>((s) => 
                                                                            finalParentRepo.Add(s));
            mockDbsetParTask.Setup(ctx => ctx.FirstOrDefault(It.IsAny<Expression<Func<ParentTask, bool>>>()))
                                  .Returns(new ParentTask
                                                              {
                                                                  Parent_Task = taskId
                                                              }
                                                            );
            DbContextOptions<TaskContext> contextOption = new DbContextOptions<TaskContext>();
            var mockTaskContext = new Mock<TaskContext>(contextOption);
            mockTaskContext.SetupGet(ctx => ctx.Tasks).Returns(mockDbsetTask.Object);
            mockTaskContext.SetupGet(ctx => ctx.ParentTasks).Returns(mockDbsetParTask.Object);
            mockTaskContext.SetupSet(ctx => ctx.Tasks = mockDbsetTask.Object);
            mockTaskContext.SetupSet(ctx => ctx.ParentTasks = mockDbsetParTask.Object);
            mockTaskContext.Setup(ctx => ctx.SaveChanges())
                           .Returns(1);
            var repoLogger = new LoggerFactory().CreateLogger<TaskRepo>();
            var taskRepo = new TaskRepo(mockTaskContext.Object, repoLogger);
            var result = await taskRepo.AddTask(new Tasks
            {
                EndDate = (!string.IsNullOrWhiteSpace(end)) ? DateTime.Parse(end) : DateTime.MinValue,
                StartDate = (!string.IsNullOrWhiteSpace(start)) ? DateTime.Parse(start) : DateTime.MinValue,
                ParentTask = (parentTaskId > -1) ? new ParentTask { Parent_Task = parentTaskId } : null,
                Priortiy = priority,
                Status = status,
                TaskId = taskId,
                TaskDeatails = "Test task",
            });
            Assert.True(result);
        }
    }
}
