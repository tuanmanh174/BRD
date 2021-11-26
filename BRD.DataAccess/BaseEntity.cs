using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BRD.DataAccess
{
    public abstract class BaseEntity<T>
    {
        [Key]
        public virtual T Id { get; set; }
    }
}
