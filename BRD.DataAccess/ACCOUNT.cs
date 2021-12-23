using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BRD.DataAccess
{
    //[Table("ACCOUNTS", Schema = "orcl")]
    public class Accounts
    {
        [Key]
        public decimal id { get; set; }
        public string accountname { get; set; }
    }
}
