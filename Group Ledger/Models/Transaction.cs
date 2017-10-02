namespace Group_Ledger.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public bool Verified { get; set; }
        public virtual ApplicationUser From { get; set; }
        public virtual ApplicationUser To { get; set; }
        public decimal Amount { get; set; }
    }
}