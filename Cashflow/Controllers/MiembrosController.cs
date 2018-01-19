using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cashflow.Models;
using Cashflow.Utils;
using Cashflow.ViewModels;
using Microsoft.AspNet.Identity;

namespace Cashflow.Controllers
{
    public class MiembrosController : Controller
    {
        private ApplicationDbContext _context;

        public MiembrosController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }


        public ActionResult New()
        {
            var userId = User.Identity.GetUserId();

            var viewModel = new Miembro();
            
            return View("MiembroForm", viewModel);
        }


        [HttpPost]
        public ActionResult Save(Miembro miembro)
        {
            if (miembro.Id == 0)
            {
                var userId = User.Identity.GetUserId();
                miembro.ApplicationUserId = userId;
                _context.Miembros.Add(miembro);
            }
            else
            {
                var miembroInDb = _context.Miembros.Single(m => m.Id == miembro.Id);
                miembroInDb.Nombre = miembro.Nombre;
                miembroInDb.FechaNacimiento = miembro.FechaNacimiento;
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Miembros");
        }

        public ActionResult Edit(int id)
        {
            var miembro = _context.Miembros.SingleOrDefault(m => m.Id == id);

            if (miembro == null)
                return HttpNotFound();

            return View("MiembroForm", miembro);
        }

        // GET: Miembros
        public ActionResult Index()
        {

            var userId = User.Identity.GetUserId();
            var miembros = _context.Miembros.Where(m => m.ApplicationUserId == userId).ToList();


            var viewModel = new List<MiembroIndexViewModel>();

            foreach (var miembro in miembros)
            {
                var miembroIndex = new MiembroIndexViewModel()
                {
                    Miembro = miembro,
                    Edad = AgeUtils.GetEdad(miembro.FechaNacimiento)
                };
                viewModel.Add(miembroIndex);
            }

            return View(viewModel);
        }
    }
}