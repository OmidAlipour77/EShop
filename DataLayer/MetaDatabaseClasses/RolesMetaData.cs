using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class RolesMetaData
    {
        [Key]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name ="ایدی")]
        public int RoleID { get; set; }
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string RoleTitle { get; set; }
        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string RoleName { get; set; }
    }
    [MetadataType(typeof(RolesMetaData))]
    public partial class Roles
    {

    }

}
