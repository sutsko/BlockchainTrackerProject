using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netCoreMVCCrud1.Models
{
    public class Usecase
    {
        [Key]
        public int UsecaseId { get; set; }
        public string UsecaseName { get; set; }
    }
}
