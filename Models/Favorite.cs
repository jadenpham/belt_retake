using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace retake.Models
{
    public class Favorite 
    {
        [Key]
        [Column("fave_id")]
        public int FaveId {get; set;}

        [Column("user_id")]
        public int UserId{get; set;}

        [Column("hobby_id")]
        public int HobbyId {get; set;}

        public UserReg Enthusiast {get; set;}

        public Hobby Hobbies {get; set;}
    }
}