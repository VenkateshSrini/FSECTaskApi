using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskAPI.Messages
{
    public class ParentTaskMsg
    {
        public int Parent_ID { get; set; }
        public int ParentTask_ID { get; set; }
        public string Parent_Task_Description { get; set; }
    }
}
