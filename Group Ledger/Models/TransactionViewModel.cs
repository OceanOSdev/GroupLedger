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
        public decimal Amount { get; set; }
    }
}