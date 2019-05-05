using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EvaluationApp.Models;
using EvaluationApp.Extensions;

namespace EvaluationApp.Controllers
{
    [Authorize]
    public class evaluationsController : Controller
    {
        private evalsDBEntities db = new evalsDBEntities();

        // GET: evaluations
        public ActionResult Index()
        {
            var evaluations = db.evaluations.Include(e => e.course).Include(e => e.student);
            if (User.IsInRole("Instructor"))
            {
                var sid = int.Parse(IdentityExtensions.GetSchoolId(User.Identity));
                var instructorCourses = db.courses.Where(c => c.instructorid == sid);
                var ctitles = instructorCourses.AsEnumerable().Select(t => t.title);
                evaluations = from e in evaluations where ctitles.Contains(e.coursetitle) select e;
            }else if (User.IsInRole("Student"))
            {
                var sid = int.Parse(IdentityExtensions.GetSchoolId(User.Identity));
                evaluations = from e in evaluations where e.studentid == sid select e;
            }
            return View(evaluations.ToList());
        }

        // GET: evaluations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            evaluation evaluation = db.evaluations.Find(id);
            if (evaluation == null)
            {
                return HttpNotFound();
            }
            return View(evaluation);
        }

        // GET: evaluations/Create
        [Authorize(Roles = "Student")]
        public ActionResult Create()
        {
            ViewBag.coursetitle = new SelectList(db.courses, "title", "title");
            ViewBag.courseid = new SelectList(db.courses, "Id", "title");
            ViewBag.studentid = new SelectList(db.students, "Id", "FirstName");
            return View();
        }

        // POST: evaluations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public ActionResult Create([Bind(Include = "Id,coursetitle,studentname,studentid,courseid,a1,a2,a3,a4,b1,b2,b3,b4,b5,b6,c1,c2,d1,d2,e1,e2")] evaluation evaluation)
        {
            evaluation.courseid = (db.courses.Single(c => c.title == evaluation.coursetitle)).Id;
            var sid = int.Parse(IdentityExtensions.GetSchoolId(User.Identity));
            var currstudent = db.students.Single(s => s.Id == sid);
            evaluation.studentname = currstudent.FirstName + " " + currstudent.LastName;
            evaluation.studentid = currstudent.Id;
            if (ModelState.IsValid)
            {
                db.evaluations.Add(evaluation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.courseid = new SelectList(db.courses, "Id", "title", evaluation.courseid);
            ViewBag.studentid = new SelectList(db.students, "Id", "FirstName", evaluation.studentid);
            return View(evaluation);
        }

        // GET: evaluations/Edit/5
        [Authorize(Roles = "Student")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            evaluation evaluation = db.evaluations.Find(id);
            if (evaluation == null)
            {
                return HttpNotFound();
            }
            ViewBag.coursetitle = new SelectList(db.courses, "title", "title");
            ViewBag.courseid = new SelectList(db.courses, "Id", "title", evaluation.courseid);
            ViewBag.studentid = new SelectList(db.students, "Id", "FirstName", evaluation.studentid);
            return View(evaluation);
        }

        // POST: evaluations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public ActionResult Edit([Bind(Include = "Id,coursetitle,studentname,studentid,courseid,a1,a2,a3,a4,b1,b2,b3,b4,b5,b6,c1,c2,d1,d2,e1,e2")] evaluation evaluation)
        {
            evaluation.courseid = (db.courses.Single(c => c.title == evaluation.coursetitle)).Id;
            var sid = int.Parse(IdentityExtensions.GetSchoolId(User.Identity));
            var currstudent = db.students.Single(s => s.Id == sid);
            evaluation.studentname = currstudent.FirstName + " " + currstudent.LastName;
            evaluation.studentid = currstudent.Id;
            if (ModelState.IsValid)
            {
                db.Entry(evaluation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.coursetitle = new SelectList(db.courses, "title", "title");
            ViewBag.courseid = new SelectList(db.courses, "Id", "title", evaluation.courseid);
            ViewBag.studentid = new SelectList(db.students, "Id", "FirstName", evaluation.studentid);
            return View(evaluation);
        }

        // GET: evaluations/Delete/5
        [Authorize(Roles = "Student")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            evaluation evaluation = db.evaluations.Find(id);
            if (evaluation == null)
            {
                return HttpNotFound();
            }
            return View(evaluation);
        }

        // POST: evaluations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Student")]
        public ActionResult DeleteConfirmed(int id)
        {
            evaluation evaluation = db.evaluations.Find(id);
            db.evaluations.Remove(evaluation);
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
