using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Xunit;
using TaskAPI.Messages;
using AutoMapper;
using Moq;
using TaskAPI.DomainModel;
using TaskAPI.DomainService;
using Microsoft.Extensions.Logging;
using TaskAPI.Repository;

namespace TaskApi.Unit.Test.DomainService
{
    public class TaskServiceTest
    {
        ILogger<TasksService> logger;
        public TaskServiceTest()
        {
            var loggerFactory = new LoggerFactory();
            logger = loggerFactory.CreateLogger<TasksService>();
        }
        [Theory]
        [InlineData("Task1",1, "3/24/2020", "3/25/2020")]
        public async Task AddTaskTest(string taskDesc, int priority, string startDatestr, string endDatestr)
        {
            DateTime startDate = DateTime.Parse(startDatestr);
            DateTime endDate = DateTime.Parse(endDatestr);
            var taskAdd = new TaskAdd
            {
                TaskDescription = taskDesc,
                Priority = priority,
                StartDate = startDate,
                EndDate = endDate


            };
            var resulTasks = new Tasks
            {
                EndDate = endDate,
                Priortiy = priority,
                TaskDeatails = taskDesc,
                Status = 1,
                StartDate = startDate,
                TaskId = 1
            };
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(map => map.Map<Tasks>(It.IsAny<TaskAdd>())).Returns(resulTasks);
            var mockTaskRepo = new Mock<ITaskRepo>();
            mockTaskRepo.Setup(tskService => tskService.AddTask(It.IsAny<Tasks>()))
                .Returns(Task.FromResult<bool>(true));
            var taskService = new TasksService(mockMapper.Object, mockTaskRepo.Object, logger);
            var result = await taskService.AddTask(taskAdd);
            Assert.True(result);
        }
        [Theory]
        [InlineData("Task1", 1, "3/22/2020", "3/25/2020")]
        public async Task ModTest(string taskDescription, int priority, string startDateStr, string endDateStr)
        {
            DateTime startDate = DateTime.Parse(startDateStr);
            DateTime endDate = DateTime.Parse(endDateStr);
            var taskMod = new TaskMod
            {
                EndDate = endDate,
                StartDate = startDate,
                Priority = priority,
                TaskDescription = taskDescription
            };
            var resulTasks = new Tasks
            {
                EndDate = endDate,
                Priortiy = priority,
                TaskDeatails = taskDescription,
                Status = 1,
                StartDate = startDate,
                TaskId = 1
            };
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(map => map.Map<Tasks>(It.IsAny<TaskMod>())).Returns(resulTasks);
            var mockTaskRepo = new Mock<ITaskRepo>();
            mockTaskRepo.Setup(tskService => tskService.EditTask(It.IsAny<Tasks>()))
                .Returns(Task.FromResult<bool>(true));
            var taskService = new TasksService(mockMapper.Object, mockTaskRepo.Object, logger);
            var result = await taskService.ModifyTask(taskMod);
            Assert.True(result);
        }
        [Fact]
        public async Task GetAllParentTaskTest()
        {
            var parentTasks = new List<ParentTask> { new ParentTask { 
                ParentTaskDescription = "Task0",
                Parent_ID=1,
                Parent_Task=1,
                
            } };
            var parentTaskMsgs = new List<ParentTaskMsg> { new ParentTaskMsg {
                Parent_ID =1,
                ParentTask_ID=1,
                Parent_Task_Description="Task0"
            } };
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(map => map.Map<List<ParentTaskMsg>>(It.IsAny<List<ParentTask>>()))
                      .Returns(parentTaskMsgs);
            var mockTaskRepo = new Mock<ITaskRepo>();
            mockTaskRepo.Setup(repo => repo.GetAllParentTasks()).Returns(Task.FromResult(parentTasks));
            var taskService = new TasksService(mockMapper.Object, mockTaskRepo.Object, logger);
            var result = await taskService.GetAllParentTask();
            Assert.Single(result);
        }
        [Theory]
        [InlineData(-1,-1,1,"3/24/2020", "3/25/2020")]
        public void TestGetAllTaskTest(int taskId, int parentTaskId, int priority, string strStartDate, string strEndDate)
        {
            var startDate = DateTime.Parse(strStartDate);
            var endDate = DateTime.Parse(strEndDate);
            var tasks = new List<Tasks> { new Tasks { 
                EndDate = endDate,
                ParentTaskId=parentTaskId,
                Priortiy=priority,
                StartDate=startDate,
                Status=1,
                TaskDeatails="Task1",
                TaskId=taskId,
                ParentTask = new ParentTask
                {
                    ParentTaskDescription="ParentTask",
                    Parent_ID=1,
                    Parent_Task=1,
                    
                }
            } };
            var taskListings = new List<TaskListing> { new TaskListing {
                EndDate=endDate,
                ParentDescription="ParentTask",
                ParentTaskId=1,
                Priority=priority,
                StartDate=startDate,
                TaskDescription="Task1",
                TaskId = taskId
            } };
            var searchMsg = new SearchMsg
            {
                TaskId = taskId,
                FromDate = startDate,
                ToDate = endDate,
                ParentTaskId = parentTaskId,
                Priority = priority

            };
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(map => map.Map<List<TaskListing>>(It.IsAny<List<Tasks>>()))
                      .Returns(taskListings);
            var mockTaskRepo = new Mock<ITaskRepo>();
            mockTaskRepo.Setup(repo => repo.GetTaskForAllCriteria(It.IsAny<SearchMsg>())).Returns(tasks);
            var taskService = new TasksService(mockMapper.Object, mockTaskRepo.Object, logger);
            var result = taskService.GetTaskMatchAll(searchMsg);
            Assert.Single(result);

        }
        [Theory]
        [InlineData(-1, -1, 1, "3/24/2020", "3/25/2020")]
        public void GetAnyTaskTest(int taskId, int parentTaskId, int priority, string strStartDate, string strEndDate)
        {
            var startDate = DateTime.Parse(strStartDate);
            var endDate = DateTime.Parse(strEndDate);
            var tasks = new List<Tasks> { new Tasks {
                EndDate = endDate,
                ParentTaskId=parentTaskId,
                Priortiy=priority,
                StartDate=startDate,
                Status=1,
                TaskDeatails="Task1",
                TaskId=taskId,
                ParentTask = new ParentTask
                {
                    ParentTaskDescription="ParentTask",
                    Parent_ID=1,
                    Parent_Task=1,

                }
            } };
            var taskListings = new List<TaskListing> { new TaskListing {
                EndDate=endDate,
                ParentDescription="ParentTask",
                ParentTaskId=1,
                Priority=priority,
                StartDate=startDate,
                TaskDescription="Task1",
                TaskId = taskId
            } };
            var searchMsg = new SearchMsg
            {
                TaskId = taskId,
                FromDate = startDate,
                ToDate = endDate,
                ParentTaskId = parentTaskId,
                Priority = priority

            };
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(map => map.Map<List<TaskListing>>(It.IsAny<List<Tasks>>()))
                      .Returns(taskListings);
            var mockTaskRepo = new Mock<ITaskRepo>();
            mockTaskRepo.Setup(repo => repo.GetTaskForAllCriteria(It.IsAny<SearchMsg>())).Returns(tasks);
            var taskService = new TasksService(mockMapper.Object, mockTaskRepo.Object, logger);
            var result = taskService.GetTaskMatchAny(searchMsg);
            Assert.Single(result);

        }
        [Theory]
        [InlineData(1, 1, 1, "3/24/2020", "3/25/2020")]
        public async Task EndTaskTest(int taskId, int parentTaskId, int priority, string strStartDate, string strEndDate)
        {
            var startDate = DateTime.Parse(strStartDate);
            var endDate = DateTime.Parse(strEndDate);
            var tasks = new List<Tasks> { new Tasks {
                EndDate = endDate,
                ParentTaskId=parentTaskId,
                Priortiy=priority,
                StartDate=startDate,
                Status=1,
                TaskDeatails="Task1",
                TaskId=taskId,
                ParentTask = new ParentTask
                {
                    ParentTaskDescription="ParentTask",
                    Parent_ID=1,
                    Parent_Task=1,

                }
            } };
            var mockMapper = new Mock<IMapper>();
            var mockTaskRepo = new Mock<ITaskRepo>();
            mockTaskRepo.Setup(repo => repo.GetTaskForAnyCriteria(It.IsAny<SearchMsg>())).Returns(tasks);
            mockTaskRepo.Setup(repo => repo.EditTask(It.IsAny<Tasks>())).Returns(Task.FromResult(true));
            var taskService = new TasksService(mockMapper.Object, mockTaskRepo.Object, logger);
            var result = await taskService.EndTask(taskId);
            Assert.True(result);
        }
    }
}
