namespace PennyTest2.DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("task")]
    public partial class Task
    {
        [Required]
        [StringLength(8)]
        public string EMPNO { get; set; }

        [StringLength(50)]
        public string TASKID { get; set; }

        [Required]
        [StringLength(50)]
        public string MEMO { get; set; }

        [StringLength(50)]
        public string STATE { get; set; }
        [StringLength(50)]
        public string SERIALNO { get; set; }
    }
}
