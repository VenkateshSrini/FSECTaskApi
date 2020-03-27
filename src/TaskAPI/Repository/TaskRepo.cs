using LinqKit;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskAPI.DomainModel;
using TaskAPI.Messages;

namespace TaskAPI.Repository
{
    public class TaskRepo:ITaskRepo
    {
        private TaskContext taskContext;
        private readonly ILogger<TaskRepo> logger;
        public TaskRepo(TaskContext taskContext, ILogger<TaskRepo> logger)
        {
            this.taskContext = taskContext;
            this.logger = logger; 
        }

        public List<Tasks> GetTaskForAllCriteria(SearchMsg searchMsg)
        {
            var criteriaPredicate = PredicateBuilder.New<Tasks>(true);
            if (searchMsg.TaskId > -1)
                criteriaPredicate = criteriaPredicate.And(tsk => tsk.TaskId == searchMsg.TaskId);
            if (searchMsg.ParentTaskId > -1)
                criteriaPredicate = criteriaPredicate.And(tsk => tsk.ParentTaskId == searchMsg.ParentTaskId);
            if (searchMsg.Priority > -1)
                criteriaPredicate = criteriaPredicate.And(tsk => tsk.Priortiy == searchMsg.Priority);
            if (searchMsg.FromDate > DateTime.MinValue)
                criteriaPredicate = criteriaPredicate.And(tsk => tsk.StartDate == searchMsg.FromDate);
            if (searchMsg.ToDate > DateTime.MinValue)
                criteriaPredicate = criteriaPredicate.And(tsk => tsk.EndDate == searchMsg.ToDate);

            return taskContext.Tasks.Include(tsk=>tsk.ParentTask)
                    .Where(criteriaPredicate.Compile()).ToList();

        }

        public List<Tasks> GetTaskForAnyCriteria(SearchMsg searchMsg)
        {
            var criteriaPredicate = PredicateBuilder.New<Tasks>(false);
            if (searchMsg.TaskId > -1)
                criteriaPredicate = criteriaPredicate.Or(tsk => tsk.TaskId == searchMsg.TaskId);
            if (searchMsg.ParentTaskId > -1)
                criteriaPredicate = criteriaPredicate.Or(tsk => tsk.ParentTaskId == searchMsg.ParentTaskId);
            if (searchMsg.Priority > -1)
                criteriaPredicate = criteriaPredicate.Or(tsk => tsk.Priortiy == searchMsg.Priority);
            if (searchMsg.FromDate > DateTime.MinValue)
                criteriaPredicate = criteriaPredicate.Or(tsk => tsk.StartDate == searchMsg.FromDate);
            if (searchMsg.ToDate > DateTime.MinValue)
                criteriaPredicate = criteriaPredicate.Or(tsk => tsk.EndDate == searchMsg.ToDate);

            return taskContext.Tasks.Include(tsk=>tsk.ParentTask)
                    .Where(criteriaPredicate.Compile()).ToList();
        }
        public async Task<bool> AddTask(Tasks tasks)
        {
            _ = await manageParentTask(tasks);
             taskContext.Tasks.Add(tasks);
            var rowsAffected = await taskContext.SaveChangesAsync();
            return (rowsAffected > 0) ? true : false;

        }
        public async Task<bool> EditTask(Tasks tasks)
        {
            var oldTask = await taskContext.Tasks.Include(tsk => tsk.ParentTask)
                                                .FirstOrDefaultAsync(tsk => (tsk.TaskId == tasks.TaskId));
            if (oldTask == default)
                throw new ApplicationException("Task not found");
            if ((oldTask.ParentTask!=null) && (oldTask.ParentTask.Parent_Task==tasks.TaskId))
                _ = await manageParentTask(tasks);
            taskContext.Update<Tasks>(tasks);
            var rowsAffected = await taskContext.SaveChangesAsync();
            return (rowsAffected>0) ?true:false;


        }
        private async Task<Tasks> manageParentTask(Tasks task)
        {
          
            if ((task.ParentTask!=null) && (task.ParentTask.Parent_Task > 0))
            {
                ParentTask parentTask = await taskContext.ParentTasks.FirstOrDefaultAsync(parTsk =>
                                                            parTsk.Parent_Task == task.ParentTaskId);
                if (parentTask==default)
                {
                    parentTask = new ParentTask { Parent_Task = task.ParentTaskId, ParentTaskDescription=task.TaskDeatails };
                     taskContext.ParentTasks.Add(parentTask);
                    await taskContext.SaveChangesAsync();

                }
                task.ParentTaskId = parentTask.Parent_ID;
                task.ParentTask = parentTask;
            }
            return task;
        }

        public async Task<List<ParentTask>> GetAllParentTasks()
        {
            return await taskContext.ParentTasks.ToListAsync();
        }
    }
}
