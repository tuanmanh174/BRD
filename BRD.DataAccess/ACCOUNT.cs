using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BRD.DataAccess
{
    [Table("ACCOUNT")]
    public class ACCOUNT 
    {
        [Key]
        public int ID { get; set; }
        public string ACCOUNTNAME { get; set; }
    }
}
