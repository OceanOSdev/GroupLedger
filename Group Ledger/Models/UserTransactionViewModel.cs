using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Group_Ledger.Models
{
    public class UserTransactionViewModel
    {
        // where the current user is the sender of the transaction
        [Display(Name = "People You Owe")]
        public IEnumerable<Transaction> TransactionsFromUser { get; set; }
        // where the current user is the recipient of the transaction
        [Display(Name="People Who Owe You")]
        public IEnumerable<Transaction> TransactionsToUser { get; set; }
    }
}