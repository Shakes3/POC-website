using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace POC3.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // this validation is used to auto increment the id(trying this first time to check wheather it will continuely increment it)
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name {get; set;}
        [Required(ErrorMessage = "The Password field is required.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$", 
            // ErrorMessage = "The Password must Abcdef@2001")]
            // ErrorMessage = "The Password must contain at least 8 characters, including at least one uppercase letter, one lowercase letter, one number, and one special character.")]
            ErrorMessage = "The Password must contain the following:\n" +
                   "<ul>\n" +
                   "<li>At least 8 characters</li>\n" +
                   "<li>One uppercase letter</li>\n" +
                   "<li>One lowercase letter</li>\n" +
                   "<li>One number</li>\n" +
                   "<li>One special character</li>\n" +
                   "</ul>")]
        // [NotMapped] // This property won't be mapped to the database
        public string Password { get; set; }
        
        // [NotMapped] // This property won't be mapped to the database
        // [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        // public string ConfirmPassword { get; set; }
        public string Salt { get; set; }
        public DateTime LastLoginDateTime { get; set; }
        public DateTime LastPasswordChangeDateTime { get; set; }
        public bool IsActive { get; set; }
        public int UnsuccessfulLoginAttempts { get; set; }
        public DateTime AccountCreationDateTime { get; set; }
        public DateTime AccountInactiveDateTime { get; set; }
    }
}