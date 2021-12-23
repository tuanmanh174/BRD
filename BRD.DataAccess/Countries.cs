using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BRD.DataAccess
{
    [Table("COUNTRIES")]
    public class Countries
    {
        [Key]
        [Column("COUNTRY_ID", TypeName = "CHAR")]
        [MaxLength(2)]
        public string COUNTRY_ID { get; set; }
        [Column("COUNTRY_NAME", TypeName = "VARCHAR2")]
        [MaxLength(40)]
        public string COUNTRY_NAME { get; set; }
        [Column("REGION_ID", TypeName = "NUMBER")]
        public int REGION_ID { get; set; }
    }
}
