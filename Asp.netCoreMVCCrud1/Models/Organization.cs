using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netCoreMVCCrud1.Models
{
    public class Organization
    {
        [Key]
        public int OrganizationId {get; set; }
        public string OrganizationName {get; set;}
        public int OrganizationType {get; set; }
    }
}
