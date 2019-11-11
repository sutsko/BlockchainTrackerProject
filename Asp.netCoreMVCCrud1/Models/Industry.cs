using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asp.netCoreMVCCrud1.Models
{
    public class Industry
    {
        public Industry(string industryName)
        {
            IndustryName = industryName;
        }

        [Key]
        public int IndustryId { get; set; }
        public string IndustryName { get; set; }
        public string IndustryDescription { get; set; }

        [ForeignKey("Sector")]
        public int SectorId { get; set; }
        public virtual Sector Sector { get; set;}
    }
}
