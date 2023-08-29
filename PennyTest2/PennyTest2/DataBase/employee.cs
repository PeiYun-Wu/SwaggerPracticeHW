namespace PennyTest2.DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("employee")]
    public partial class employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public employee()
        {
            TokenEmp = new HashSet<TokenEmp>();
        }

        [Key]
        [StringLength(8)]
        public string EMPNO { get; set; }

        [Required]
        [StringLength(10)]
        public string NAME { get; set; }

        [Required]
        [StringLength(20)]
        public string EMPPASS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TokenEmp> TokenEmp { get; set; }
    }
}
