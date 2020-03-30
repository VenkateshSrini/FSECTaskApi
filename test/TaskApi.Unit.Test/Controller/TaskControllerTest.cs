using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using TaskAPI.Controllers;
using Xunit;
using System;
using TaskAPI.Messages;
using TaskAPI.DomainService;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace TaskApi.Unit.Test.Controller
{
    public class TaskControllerTest
    {
        private readonly ILogger<TaskController> logger;
        public TaskControllerTest()
        {
            var loggerFactory = new LoggerFactory();
            logger = loggerFactory.CreateLogger<TaskController>();
        }
        [Theory]
        [InlineData("Task1", 1, "3/24/2020", "3/25/2020")]
        public async Task PostTest(string taskDesc, int priority, string startDatestr, string endDatestr)
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
            var mockService = new Mock<ITaskService>();
            mockService.Setup(service => service.AddTask(It.IsAny<TaskAdd>())).Returns(Task.FromResult(true));
            var controller = new TaskController(mockService.Object, logger);
            var result = (await controller.Post(taskAdd)).Result as CreatedResult;
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
        }
        [Theory]
        [InlineData("Task1", 1, "3/24/2020", "3/25/2020")]
        public async Task PutTest(string taskDescription, int priority, string startDateStr, string endDateStr)
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
            var mockService = new Mock<ITaskService>();
            mockService.Setup(service => service.ModifyTask(It.IsAny<TaskMod>()))
                .Returns(Task.FromResult(true));
            var controller = new TaskController(mockService.Object, logger);
            var result = (await controller.Put(taskMod)).Result as AcceptedResult;
            Assert.NotNull(result);
            Assert.Equal(202, result.StatusCode);
        }
        [Theory]
        [InlineData(1)]
        public async Task EndTask(int taskId)
        {
            var mockService = new Mock<ITaskService>();
            mockService.Setup(service => service.EndTask(It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            var controller = new TaskController(mockService.Object, logger);
            var result = (await controller.EndTask(taskId)).Result as AcceptedResult;
            Assert.NotNull(result);
            Assert.Equal(202, result.StatusCode);

        }
        [Theory]
        [InlineData(-1, -1, 1, "3/24/2020", "3/25/2020")]
        public void GetTaskAnyCriteria(int taskId, int parentTaskId, int priority, string strStartDate, string strEndDate)
        {
            var startDate = DateTime.Parse(strStartDate);
            var endDate = DateTime.Parse(strEndDate);
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
            var mockService = new Mock<ITaskService>();
            mockService.Setup(service => service.GetTaskMatchAny(It.IsAny<SearchMsg>()))
                       .Returns(taskListings);
            var controller = new TaskController(mockService.Object, logger);
            var actionResult = controller.GetTaskAnyCriteria(taskId,parentTaskId,priority,strEndDate,strEndDate).Result as OkObjectResult;

            Assert.NotNull(actionResult);
            Assert.Equal(200, actionResult.StatusCode);
            var results = actionResult.Value as List<TaskListing>;
            Assert.NotNull(results);
            Assert.Single(results);
            
        }

        [Theory]
        [InlineData(-1, -1, 1, "3/24/2020", "3/25/2020")]
        public void GetTaskAllCriteria(int taskId, int parentTaskId, int priority, string strStartDate, string strEndDate)
        {
            var startDate = DateTime.Parse(strStartDate);
            var endDate = DateTime.Parse(strEndDate);
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
            var mockService = new Mock<ITaskService>();
            mockService.Setup(service => service.GetTaskMatchAll(It.IsAny<SearchMsg>()))
                       .Returns(taskListings);
            var controller = new TaskController(mockService.Object, logger);
            var actionResult = controller.GetTaskAllCriteria(taskId,parentTaskId, priority, strStartDate, strEndDate).Result as OkObjectResult;

            Assert.NotNull(actionResult);
            Assert.Equal(200, actionResult.StatusCode);
            var results = actionResult.Value as List<TaskListing>;
            Assert.NotNull(results);
            Assert.Single(results);

        }
        [Fact]
        public async Task GetAllParentTest()
        {
            var parentTaskMsgs = new List<ParentTaskMsg> { new ParentTaskMsg {
                Parent_ID =1,
                ParentTask_ID=1,
                Parent_Task_Description="Task0"
            } };
            var mockService = new Mock<ITaskService>();
            mockService.Setup(service => service.GetAllParentTask())
                        .Returns(Task.FromResult(parentTaskMsgs));
            var controller = new TaskController(mockService.Object, logger);
            var actionResult = (await controller.GetAllParent()).Result as OkObjectResult;
            var results = actionResult?.Value as List<ParentTaskMsg>;
            Assert.NotNull(actionResult);
            Assert.NotNull(results);
            Assert.Equal(200, actionResult.StatusCode);
            Assert.Single(results);
        }
    }
}
