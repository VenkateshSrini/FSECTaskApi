using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskAPI.DomainModel
{
    [Table("tasks")]
    public class Tasks
    {
        [Column("task_id", TypeName = "integer")]
        [Key]
        public int TaskId { get; set; }
        [Column("task",TypeName ="varchar(40)")]
        public string TaskDeatails { get; set; }
        [Column("start_date", TypeName = "timestamp")]
        public DateTime StartDate { get; set; }
        [Column("end_date", TypeName ="timestamp")]
        public DateTime EndDate { get; set; }
        [Column("parent_id",TypeName ="integer")]
        public int ParentTaskId { get; set; }
        [Column("priority",TypeName ="integer")]
        public int Priortiy { get; set; }
        [Column("status", TypeName ="int")]
        public int Status { get; set; }
        
        public ParentTask ParentTask { get; set; }
    }
}
