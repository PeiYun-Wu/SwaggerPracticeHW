namespace PennyTest2.DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TokenEmp")]
    public partial class TokenEmp
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(8)]
        public string EMPNO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string ACCESSTOKEN { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LOGINDATEUTC { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LOGOUTDATEUTC { get; set; }

        [StringLength(50)]
        public string CREATOR { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CREATEDATETIMEUTC { get; set; }

        [StringLength(50)]
        public string MODIFER { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? MODIFYDATETIMEUTC { get; set; }

        public virtual employee employee { get; set; }
    }
}
