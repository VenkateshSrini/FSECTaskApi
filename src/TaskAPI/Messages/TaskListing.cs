using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskAPI.Messages
{
    public class TaskListing
    {
        public int TaskId { get; set; }
        public string TaskDescription { get; set; }
        public int ParentTaskId { get; set; }
        public string ParentDescription { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
