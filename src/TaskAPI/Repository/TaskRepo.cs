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
            if (searchMsg.TaskId > 0)
                criteriaPredicate = criteriaPredicate.And(tsk => tsk.TaskId == searchMsg.TaskId);
            if (searchMsg.ParentTaskId > 0)
            {
                var parentTask = taskContext.ParentTasks.AsNoTracking().FirstOrDefault(
                   partask => partask.Parent_Task == searchMsg.ParentTaskId);
                var parentId = (parentTask != default) ? parentTask.Parent_ID : 0;

                criteriaPredicate = criteriaPredicate.And(tsk => tsk.ParentTaskId == parentId);
            }

            if (searchMsg.PriorityFrom > 0)
                criteriaPredicate = criteriaPredicate.Or(tsk => tsk.Priortiy >= searchMsg.PriorityFrom);
            if (searchMsg.PriorityTo > 0)
                criteriaPredicate = criteriaPredicate.Or(tsk => tsk.Priortiy <= searchMsg.PriorityTo);
            if (searchMsg.FromDate > DateTime.MinValue)
                criteriaPredicate = criteriaPredicate.And(tsk => tsk.StartDate == searchMsg.FromDate);
            if (searchMsg.ToDate > DateTime.MinValue)
                criteriaPredicate = criteriaPredicate.And(tsk => tsk.EndDate == searchMsg.ToDate);
            if (!string.IsNullOrWhiteSpace(searchMsg.TaskDescription))
                criteriaPredicate = criteriaPredicate.And(tsk =>
                tsk.TaskDeatails.CompareTo(searchMsg.TaskDescription) == 0);

            var anyTaskQuery = from taskEntity in taskContext.Tasks.Where(criteriaPredicate.Compile())
                               select taskEntity;

            var tasks = anyTaskQuery.ToList();
            tasks.ForEach(task =>
            {
                if (task.ParentTaskId > 0)
                {
                    task.ParentTask = taskContext.ParentTasks.Find(task.ParentTaskId);

                }
            });

            return tasks;

        }

        public List<Tasks> GetTaskForAnyCriteria(SearchMsg searchMsg)
        {
            var criteriaPredicate = PredicateBuilder.New<Tasks>(false);
            if (searchMsg.TaskId > 0)
                criteriaPredicate = criteriaPredicate.Or(tsk => tsk.TaskId == searchMsg.TaskId);
            if (!string.IsNullOrWhiteSpace(searchMsg.TaskDescription))
                criteriaPredicate = criteriaPredicate.Or(tsk =>
                tsk.TaskDeatails.CompareTo(searchMsg.TaskDescription) == 0);
            if (searchMsg.ParentTaskId > 0)
            {
                var parentTask = taskContext.ParentTasks.AsNoTracking().FirstOrDefault(
                    partask => partask.Parent_Task == searchMsg.ParentTaskId);
                var parentId = (parentTask != default) ? parentTask.Parent_ID : 0;

                criteriaPredicate = criteriaPredicate.Or(tsk => tsk.ParentTaskId== parentId);
            }
                
            if (searchMsg.PriorityFrom > 0)
                criteriaPredicate = criteriaPredicate.Or(tsk => tsk.Priortiy >= searchMsg.PriorityFrom);
            if (searchMsg.PriorityTo > 0)
                criteriaPredicate = criteriaPredicate.Or(tsk => tsk.Priortiy <= searchMsg.PriorityTo);
            if (searchMsg.FromDate > DateTime.MinValue)
                criteriaPredicate = criteriaPredicate.Or(tsk => tsk.StartDate == searchMsg.FromDate);
            if (searchMsg.ToDate > DateTime.MinValue)
                criteriaPredicate = criteriaPredicate.Or(tsk => tsk.EndDate == searchMsg.ToDate);
            var anyTaskQuery = from taskEntity in taskContext.Tasks.Where(criteriaPredicate.Compile())
                               select taskEntity ;
            
            var tasks = anyTaskQuery.ToList();
            tasks.ForEach(task =>
            {
                if (task.ParentTaskId > 0)
                {
                    task.ParentTask = taskContext.ParentTasks.Find(task.ParentTaskId);

                }
            });

           return tasks;
            // return anyTaskQuery.ToList();


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
            if ((tasks.ParentTask != default) && (tasks.ParentTask.Parent_Task == 0))
                tasks.ParentTask = null;

            var oldTaskQuery = from taskEntity in taskContext.Tasks.Where(tsk=>tsk.TaskId==tasks.TaskId)
                               from parTaskEntity in taskContext.ParentTasks.Where(partask =>
                               partask.Parent_ID == taskEntity.ParentTaskId).DefaultIfEmpty()
                               select new { taskEntity, parTaskEntity } ;
            var oldTaskValueObj = await oldTaskQuery.AsNoTracking().FirstOrDefaultAsync();
            var oldTask = oldTaskValueObj.taskEntity;
            if(oldTaskValueObj.parTaskEntity!=default)
            {
                oldTask.ParentTask = new ParentTask { 
                Parent_ID = oldTaskValueObj.parTaskEntity.Parent_ID,
                ParentTaskDescription = oldTaskValueObj.parTaskEntity.ParentTaskDescription,
                Parent_Task = oldTaskValueObj.parTaskEntity.Parent_Task
                };
            }
            

            if (oldTask == default)
                throw new ApplicationException("Task not found");
            if (((oldTask.ParentTask!=null) && (oldTask.ParentTask.Parent_ID!=tasks.ParentTaskId))||
                ((oldTask.ParentTask==default) && (tasks.ParentTask!=default)&& (tasks.ParentTask.Parent_Task>0)))
                _ = await manageParentTask(tasks);
            
            
            taskContext.Update<Tasks>(tasks);
            var rowsAffected = await taskContext.SaveChangesAsync();

            bool combinedResult =  (rowsAffected>0) ?true:false;
            bool parentUpdateResult = await UpdateParentTakDetails(tasks);
            if ((combinedResult) && (parentUpdateResult))
                return true;
            else
                return false;


        }
        private async Task<bool> UpdateParentTakDetails(Tasks task)
        {
            var parentTask = await taskContext.ParentTasks.FirstOrDefaultAsync(parTsk =>
                                                            parTsk.Parent_Task == task.ParentTaskId);
            if ((parentTask!= default) && 
                (parentTask.ParentTaskDescription.CompareTo(task.TaskDeatails)!=0))
            {
                parentTask.ParentTaskDescription = task.TaskDeatails;
                taskContext.Update(parentTask);
                var recordsAffected = await taskContext.SaveChangesAsync();
                return (recordsAffected > 0) ? true : false;
            }
            return true;

        }
        private void assignProperties(Tasks oldTask, Tasks newTasks)
        {
            oldTask.ParentTaskId = newTasks.ParentTaskId;
            oldTask.TaskId = newTasks.TaskId;
            oldTask.Status = newTasks.Status;
            oldTask.Priortiy = newTasks.Priortiy;
            oldTask.TaskDeatails = newTasks.TaskDeatails;
            oldTask.StartDate = newTasks.StartDate;
            oldTask.EndDate = newTasks.EndDate;
           

        }
        private async Task<Tasks> manageParentTask(Tasks task)
        {

            if ((task.ParentTask != null) && (task.ParentTask.Parent_Task > 0))
            {
                ParentTask parentTask = await taskContext.ParentTasks.FirstOrDefaultAsync(parTsk =>
                                                            parTsk.Parent_Task == task.ParentTaskId);
                if (parentTask == default)
                {
                    var parTaskFromTaskEntity = taskContext.Tasks.AsNoTracking()
                        .FirstOrDefault(tsk => tsk.TaskId == task.ParentTaskId);
                    parentTask = new ParentTask { Parent_Task = parTaskFromTaskEntity.TaskId, 
                    ParentTaskDescription = parTaskFromTaskEntity.TaskDeatails
                    };
                    taskContext.ParentTasks.Add(parentTask);
                    await taskContext.SaveChangesAsync();

                }
                else
                {
                    taskContext.Update(parentTask);
                    await taskContext.SaveChangesAsync();

                }
                
                task.ParentTaskId = parentTask.Parent_ID;
                task.ParentTask = parentTask;
            }
            else 
                task.ParentTask = null;

            return task;
        }

        public async Task<List<ParentTask>> GetAllParentTasks()
        {
            return await taskContext.ParentTasks.ToListAsync();
        }

        public List<Tasks> GetAllTasks()
        {
            var taskQuery = from taskEntity in taskContext.Tasks.Where(tsk=>tsk.Status>=0)
                           from parTaskEntity in taskContext.ParentTasks.Where(partask =>
                           partask.Parent_ID == taskEntity.ParentTaskId).DefaultIfEmpty()
                           select new { taskEntity, parTaskEntity };
            var taskValueObj = taskQuery.ToList();
            var tasks = taskValueObj.Select(valueObj =>
            {
                if (valueObj.parTaskEntity != null)
                {
                    valueObj.taskEntity.ParentTask = valueObj.parTaskEntity;
                }
                return valueObj.taskEntity;
            }).ToList();
            return tasks;
        }
    }
}
