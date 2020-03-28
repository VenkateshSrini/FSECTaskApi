using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskAPI.DomainModel
{
    [Table("parent_tasks")]
    public class ParentTask
    {
        [Key]
        [Column("parent_id", TypeName = "integer") ]
        public int Parent_ID { get; set; }
        [Column("parent_task", TypeName = "int")]
      
        public int Parent_Task { get; set; }
        [Column("parent_task_details", TypeName ="varchar(40)")]
        public string ParentTaskDescription { get; set; }
        [ForeignKey("Parent_Task")]
        public List<Tasks> Tasks { get; set; }
    }
}
