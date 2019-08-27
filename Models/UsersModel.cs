using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace retake.Models
{
    public class UserReg
    {
        [Key]
        [Column("id")]
        public int UserId {get; set;}

        [Required]
        [Column("f_name")]
        [Display(Name="First Name")]
        [MinLength(2,ErrorMessage="First name must be at least 2 characters.")]
        public string FirstName {get; set;}

        [Required]
        [Column("l_name")]
        [Display(Name="Last Name")]
        [MinLength(2,ErrorMessage="Last name must be at least 2 characters.")]
        public string LastName {get; set;}


        [Required]
        [Column("user_name")]
        [MinLength(3,ErrorMessage="Username must be at least 3 characters.")]
        public string UserName {get; set;}


        [Required]
        [Column("email")]
        [EmailAddress]
        public string Email {get; set;}

        [Required]
        [DataType(DataType.Password)]
        [Column("pw")]
        [MinLength(8,ErrorMessage="Password must be at least 8 characters.")]
        public string Password {get; set;}

        [Column("created_at")]
        public DateTime CreatedAt{get; set;} = DateTime.Now;
        
        [Column("updated_at")]
        public DateTime UpdatedAt {get; set;} = DateTime.Now;

        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm {get; set;}

        public List<Hobby> Hobbies {get; set;}

        public List<Favorite> Fave {get; set;}

    }

     public class UserLog
    {
        [Required]
        public string UserName {get; set;}

        [Required]
        [DataType(DataType.Password)]
        public string Password {get; set;}
    }

}
 