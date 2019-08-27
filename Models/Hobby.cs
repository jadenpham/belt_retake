using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace retake.Models
{
    public class Hobby
    {
        [Key]
        [Column("hobby_id")]
        public int HobbyId {get; set;}

        [Required]
        [Column("name")]
        [MinLength(2, ErrorMessage = "Please input at least 2 characters")]
        public string Name {get; set;}

        [Required]
        [Column("desciption")]
        [MinLength(2, ErrorMessage = "Please input at least 2 characters")]
        public string Description {get; set;}

        [Required]
        [Column("diff_lvl")]
        public string Difficulty {get; set;}

        [Column("created_at")]
        public DateTime CreatedAt {get; set;} = DateTime.Now;

        [Column("updated_at")]
        public DateTime UpdatedAt {get; set;} = DateTime.Now;

        [Column("user_id")]
        public int UserId{get; set;}

        public UserReg Creator {get; set;}

        public List<Favorite> Faves {get; set;}
    }
}