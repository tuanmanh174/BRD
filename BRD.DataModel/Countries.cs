using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BRD.DataModel
{
    public class Countries
    {
        [Key]
        public string COUNTRY_ID { get; set; }
        public string COUNTRY_NAME { get; set; }
        public int REGION_ID { get; set; }
    }
}
