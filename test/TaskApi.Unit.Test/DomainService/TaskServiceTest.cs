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
    }
}
