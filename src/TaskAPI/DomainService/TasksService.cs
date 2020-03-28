using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TaskAPI.DomainModel;
using TaskAPI.Messages;
using TaskAPI.Repository;

namespace TaskAPI.DomainService
{
    public class TasksService : ITaskService
    {
        IMapper mapper;
        ITaskRepo taskRepo;
        ILogger<TasksService> logger;
        public TasksService(IMapper mapper, ITaskRepo taskRepo, ILogger<TasksService> logger)
        {
            this.mapper = mapper;
            this.taskRepo = taskRepo;
            this.logger = logger;
        }
        public async Task<bool> AddTask(TaskAdd taskAdd)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(taskAdd);
            var validationResults = new List<ValidationResult>();
            if (Validator.TryValidateObject(taskAdd, validationContext, validationResults))
            {
                var tasks = mapper.Map<Tasks>(taskAdd);
                
                var result = await taskRepo.AddTask(tasks);
                taskAdd.TaskId = tasks.TaskId;
                return result;
            }
            return false;
        }

        public async Task<List<ParentTaskMsg>> GetAllParentTask()
        {
            var parentTask = await taskRepo.GetAllParentTasks();
            var parentMsgTasks = mapper.Map<List<ParentTaskMsg>>(parentTask);
            return parentMsgTasks;
        }

        public List<TaskListing> GetTaskMatchAll(SearchMsg searchMsg)
        {
            var tasks= taskRepo.GetTaskForAllCriteria(searchMsg);
            var taskListings = mapper.Map<List<TaskListing>>(tasks);
            return taskListings;
        }

        public List<TaskListing> GetTaskMatchAny(SearchMsg searchMsg)
        {
            var tasks = taskRepo.GetTaskForAnyCriteria(searchMsg);
            var taskListings = mapper.Map<List<TaskListing>>(tasks);
            return taskListings;
        }

        public async Task<bool> ModifyTask(TaskMod taskMod)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(taskMod);
            var validationResults = new List<ValidationResult>();
            if (Validator.TryValidateObject(taskMod, validationContext, validationResults))
            {
                var tasks = mapper.Map<Tasks>(taskMod);
                return await taskRepo.EditTask(tasks);
            }
            return false;
        }
        public async Task<bool> EndTask(int taskId)
        {
            var searchMsg = new SearchMsg { TaskId = taskId };
            var tasks = taskRepo.GetTaskForAnyCriteria(searchMsg)?.FirstOrDefault();
            if (tasks == default)
                throw new ApplicationException("Task Not found");
            tasks.Status = -1;

            return await taskRepo.EditTask(tasks);
        }
    }
}
