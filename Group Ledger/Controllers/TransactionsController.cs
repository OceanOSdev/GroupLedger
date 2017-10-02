using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using Group_Ledger.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Group_Ledger.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        public ActionResult Index()
        {
            var currentId = User.Identity.GetUserId();
            var transactionsFrom = db.Transactions.Where(t => t.From.Id == currentId);
            var transactionsTo = db.Transactions.Where(t => t.To.Id == currentId);
            return View(new UserTransactionViewModel { TransactionsFromUser = transactionsFrom, TransactionsToUser = transactionsTo });
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Amount,ToFirstName,ToLastName")] TransactionViewModel transaction)
        {
            Transaction trans = new Transaction();
            var currentId = User.Identity.GetUserId();
            var currentUser = db.Users.Find(currentId);

            if (ModelState.IsValid)
            {
                if (!db.Users.Any(u => u.Person.FirstName.ToLower() == transaction.ToFirstName.ToLower() &&
                                       u.Person.LastName.ToLower() == transaction.ToLastName.ToLower()))
                    return Content("Invalid User");

                trans.Verified = false;
                var toUser = db.Users.First(u => u.Person.FirstName.ToLower() == transaction.ToFirstName.ToLower() &&
                                               u.Person.LastName.ToLower() == transaction.ToLastName.ToLower());

                var transactions = db.Transactions.ToList(); // prevents issues accessing data stream
                // TODO: Refactor and optimize
                // check if a transaction exists that has the same flow of money
                if (transactions.Any(t => t.To.Person.Equals(toUser.Person) && t.From.Person.Equals(currentUser.Person)))
                {
                    // if such a transaction exists, just add the value of the amount to the original
                    var tran = transactions.FirstOrDefault(t => t.To.Person.Equals(toUser.Person) && t.From.Person.Equals(currentUser.Person));
                    tran.Amount += transaction.Amount;
                    Edit(tran);
                    return RedirectToAction("Index");
                }

                // check if a transaction exists that has the flow of money opposite from this one
                if (transactions.Any(t => t.To.Person.Equals(currentUser.Person) && t.From.Person.Equals(toUser.Person)))
                {
                    var opTran = transactions.First(t => t.To.Person.Equals(currentUser.Person) && t.From.Person.Equals(toUser.Person));
                    opTran.Amount -= transaction.Amount; // subtract the amount you owe them from the amount they owe you
                    // if they still owe you then just update the amount they owe you
                    if (opTran.Amount > 0)
                    {
                        Edit(opTran);
                        return RedirectToAction("Index");
                    }
                    if (opTran.Amount == 0)
                    {
                        // if the net amount is 0, then both transactions cancel out and should be verified.
                        opTran.Verified = true;
                        Edit(opTran);
                        // Verify the original transaction as well.
                        var tran = transactions.FirstOrDefault(t => t.To.Person.Equals(toUser.Person) && t.From.Person.Equals(currentUser.Person));
                        if (tran != null)
                        {
                            tran.Verified = true;
                            Edit(tran);
                        }
                        return RedirectToAction("Index");
                    }
                    // if you still end up owing them after subtracting the difference
                    // just add the transaction that states how much you owe them
                    // (this works fine since it will just fall through)
                    transaction.Amount = -1 * opTran.Amount;
                    // then verify other transaction
                    opTran.Verified = true;
                    Edit(opTran);

                }

                trans.To = toUser;
                trans.From = currentUser;
                trans.Amount = transaction.Amount;
                db.Transactions.Add(trans);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Verified,Amount")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                if (transaction.Verified) // verified transactions don't need to be shown since the debts have been payed
                    Delete(transaction.Id);
                return RedirectToAction("Index");
            }
            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Verify(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            transaction.Verified = true;
            Edit(transaction);
            return RedirectToAction("Index");
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (!transaction.Verified)
                return Content("You cannot delete a transaction that has not been verified");
            db.Transactions.Remove(transaction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
