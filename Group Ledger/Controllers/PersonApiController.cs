using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Group_Ledger.Models;

namespace Group_Ledger.Controllers
{
    public class PersonApiController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PersonApi
        public IQueryable<PersonViewModel> GetApplicationUsers()
        {

            return db.Users.Select(u => new PersonViewModel { FirstName = u.Person.FirstName, LastName = u.Person.LastName, Id = u.Id });
        }

        // GET: api/PersonApi/5
        [ResponseType(typeof(PersonViewModel))]
        public IHttpActionResult GetApplicationUser(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return Ok(applicationUser);
        }

        public IQueryable<PersonViewModel> GetApplicationUsers(string first, string last = "")
        {
            var onlyProvidedFirst = db.Users.Where(u => u.Person.FirstName.Contains(first))
                .Select(u => new PersonViewModel { FirstName = u.Person.FirstName, LastName = u.Person.LastName, Id = u.Id });
            var providedLast = db.Users.Where(u => u.Person.FirstName.Contains(first) && u.Person.LastName.Contains(last)).Select(u => new PersonViewModel { FirstName = u.Person.FirstName, LastName = u.Person.LastName, Id = u.Id });

            if (last == "") return onlyProvidedFirst;
            return providedLast;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ApplicationUserExists(string id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}