using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class FeatureMetaData
    {
        [Key]
        public int FeatureID { get; set; }
        [Display(Name = "عنوان ویژگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string FeatureTitle { get; set; }
    }
    [MetadataType(typeof(FeatureMetaData))]
    public partial class Feature
    {

    }
}
