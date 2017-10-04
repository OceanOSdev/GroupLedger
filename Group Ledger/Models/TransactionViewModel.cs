using System.ComponentModel.DataAnnotations;

namespace Group_Ledger.Models
{
    public class TransactionViewModel
    {
        [Display(Name = "First Name")]
        public string ToFirstName { get; set; }
        [Display(Name = "Last Name")]
        public string ToLastName { get; set; }
        [Display(Name = "Amount ($)")]
        [Range(typeof(decimal),"0", "39614081257132168796771975168", ErrorMessage = "You cannot send a negative amount")]
        public decimal Amount { get; set; }
    }
}