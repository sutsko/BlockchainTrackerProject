using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netCoreMVCCrud1.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        [DisplayName("Article Headline")]
        [Required (ErrorMessage ="This field is required.")]
        public string ArticleHeadline { get; set; }
        [DisplayName("Article Url")]
        public string ArticleUrl { get; set; }
        [DisplayName("Article Description")]
        public string ArticleDescription { get; set; }

        //Hvis det ikke virker
        [DisplayName("Article Date")]
        public string ArticleDate { get; set; }
        public string Confidentiality { get; set; }
        [DisplayName("Organization ID")]
        public int OrganizationId { get; set; }
        [DisplayName("Country")]
        public string Country { get; set; }
        [DisplayName("Industry ID")]
        public int IndustryId { get; set; }
        [DisplayName("Use Case ID")]
        public int UseCaseId { get; set; }
        public string Maturity { get; set; }
        [DisplayName("Technical Vendor")]
        public string TechnicalVendor { get; set; }

    }
}
