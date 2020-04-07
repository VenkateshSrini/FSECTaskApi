using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskAPI.Messages
{
    public class SearchMsg
    {
        public int TaskId { get; set; } = -1;
        public string TaskDescription { get; set; }
        public int ParentTaskId { get; set; } = -1;
        public int PriorityFrom { get; set; } = -1;
        public int PriorityTo { get; set; } = -1;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
