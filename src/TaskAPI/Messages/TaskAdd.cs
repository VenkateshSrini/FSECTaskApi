using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskAPI.Messages
{
    public class TaskAdd
    {
       [Required]
       [StringLength(40,ErrorMessage ="Maxlength exceeded")]
        public string TaskDescription { get; set; }
        [Range(0,30,ErrorMessage ="Min/Max value exceeded")]
        public int Priority { get; set; }
        public int ParentTaskId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        

    }
}
