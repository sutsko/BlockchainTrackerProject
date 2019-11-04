using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [DisplayName("Article Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ArticleDate { get; set; }
        public string Confidentiality { get; set; }
        
        [ForeignKey("Organization")]
        [DisplayName("Organization")]
        public int OrganizationId { get; set; }
        
        [DisplayName("Country")]
        public string Country { get; set; }
        
        [ForeignKey("Industry")]
        [DisplayName("Industry")]
        public int IndustryId { get; set; }
        
        [ForeignKey("Usecase")]
        [DisplayName("Use Case")]
        public int UseCaseId { get; set; }
        
        public string Maturity { get; set; }
        
        [DisplayName("Technical Vendor")]
        public string TechnicalVendor { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual Industry Industry { get; set; }
        public virtual Usecase Usecase { get; set; }

        [NotMapped]
        public List<Organization> OrganizationList { get; set; }
        [NotMapped]
        public List<Industry> IndustryList { get; set; }
        [NotMapped]
        public List<Usecase> UsecaseList { get; set; }
    }
}
