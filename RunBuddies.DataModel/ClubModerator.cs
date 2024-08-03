using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBuddies.DataModel
{
    public class ClubModerator
    {
        [Key]
        public int ClubModeratorID { get; set; }

        public int UserID { get; set; }     //FK
        public User User { get; set; }      //Navigation

        public List<Club> Clubs { get; set; } //many to one
    }
}
