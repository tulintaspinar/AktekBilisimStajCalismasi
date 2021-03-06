//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace srvy.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class TBL_USERS
    {
        public TBL_USERS()
        {
            this.TBL_SURVEY = new HashSet<TBL_SURVEY>();
            this.TBL_USERSRESPONSE = new HashSet<TBL_USERSRESPONSE>();
        }

        public int ID { get; set; }
        [Display(Name = "E-Mail")]
        [Required(ErrorMessage = "Bo? ge?ilemez")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Bo? ge?ilemez")]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string LoginError { get; set; }
        public string newPassword { get; set; }
        public string PasswordError { get; set; }
        public virtual ICollection<TBL_SURVEY> TBL_SURVEY { get; set; }
        public virtual ICollection<TBL_USERSRESPONSE> TBL_USERSRESPONSE { get; set; }
    }
}
