using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Group_Ledger.Models
{
    public class Person
    {
        [Key, ForeignKey("ApplicationUser")]
        public string PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Person other)
            {
                bool p1 = FirstName == other.FirstName;
                bool p2 = LastName == other.LastName;
                bool p3 = ApplicationUser.Email == other.ApplicationUser.Email;
                bool p4 = other.PersonId == PersonId;
                return p4 && p1 && p2 && p3;
            }
            return false;
        }
    }
}