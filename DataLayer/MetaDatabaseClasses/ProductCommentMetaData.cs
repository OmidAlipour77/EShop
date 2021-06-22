using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ProductCommentMetaData
    {
        [Key]
        public int CommentID { get; set; }
        public int ProductID { get; set; }
        [Display(Name ="نام")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید")]
        [MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(350)]
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [EmailAddress(ErrorMessage ="ایمیل وارد شده معتبر نمی باشد")]
        public string Email { get; set; }
        [Display(Name = "وب سایت")]
        [MaxLength(350)]
        public string Website { get; set; }
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(800)]
        public string Comment { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<int> ParentID { get; set; }
    }
    [MetadataType(typeof(ProductCommentMetaData))]
    public partial class ProductComment
    {

    }
}

