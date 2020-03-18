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
        [Column("parent_id", TypeName ="serial") ]
        public int Parent_ID { get; set; }
        [Column("parent_task", TypeName = "varchar(100)")]
        public string Parent_Task { get; set; }
    }
}
