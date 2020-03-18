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

            return taskContext.Tasks.Where(criteriaPredicate.Compile()).ToList();

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

            return taskContext.Tasks.Where(criteriaPredicate.Compile()).ToList();
        }
    }
}
