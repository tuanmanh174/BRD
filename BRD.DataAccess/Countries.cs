using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BRD.DataAccess
{
    [Table("orcl.countries")]
    public class Countries
    {
        [Key]
        public string COUNTRY_ID { get; set; }
        public string COUNTRY_NAME { get; set; }
        public int REGION_ID { get; set; }
    }
}
