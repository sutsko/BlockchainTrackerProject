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
        [DisplayName("Organization name")]
        public string OrganizationName {get; set;}
        [DisplayName("Organization type")]
        public int OrganizationType {get; set; }
    }
}
