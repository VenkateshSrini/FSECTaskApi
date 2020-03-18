using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskAPI.DomainModel;
using TaskAPI.Messages;

namespace TaskAPI.Repository
{
    public interface ITaskRepo
    {
        List<Tasks> GetTaskForAllCriteria(SearchMsg searchMsg);
        List<Tasks> GetTaskForAnyCriteria(SearchMsg searchMsg);
    }
}
