﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BRD.DataModel
{
    public class Account
    {
        
        public int Id { get; set; }
        public string AccountName { get; set; }
    }
}
