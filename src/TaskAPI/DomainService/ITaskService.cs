using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskAPI.Messages;

namespace TaskAPI.DomainService
{
    public interface ITaskService
    {
        Task<bool> AddTask(TaskAdd taskAdd);
        Task<bool> ModifyTask(TaskMod taskMod);

        List<TaskListing> GetTaskMatchAll(SearchMsg searchMsg);
        List<TaskListing> GetTaskMatchAny(SearchMsg searchMsg);
        Task<List<ParentTaskMsg>> GetAllParentTask();
        Task<bool> EndTask(int taskId);


    }
}
