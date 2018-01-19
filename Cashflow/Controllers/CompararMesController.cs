using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Cashflow.Models;
using Cashflow.Utils;
using Microsoft.AspNet.Identity;

namespace Cashflow.Controllers
{

    [Authorize(Roles = "user")]
    public class CompararMesController : Controller
    {

        private ApplicationDbContext _context;

        public CompararMesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }


        // GET: Comparar
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var ingresosPorMes02 = FlujoUtils.GetFlujosByUserId(userId)
                    .Select(flujo => _context.FlujosMensuales
                        .Where(fm => fm.FlujoId == flujo && fm.Flujo.TipoId == 1)
                        .Select(fm => fm.Monto)
                        .DefaultIfEmpty(0)
                        .Sum())
                    .Sum();

//            var ingresosPorMes = _context.FlujosMensuales
//                .Where(fm => fm.Flujo.ApplicationUserId == userId && fm.Flujo.TipoId == 1)
//                .Select(fm => fm.Monto)
//                .DefaultIfEmpty(0)
//                .Sum();

            var gastosPorMes02 = FlujoUtils.GetFlujosByUserId(userId)
                    .Select(flujo => _context.FlujosMensuales
                        .Where(fm => fm.FlujoId == flujo && fm.Flujo.TipoId == 2)
                        .Select(fm => fm.Monto)
                        .DefaultIfEmpty(0)
                        .Sum())
                    .Sum();

//            var gastosPorMes = _context.FlujosMensuales
//                .Where(fm => fm.Flujo.ApplicationUserId == userId && fm.Flujo.TipoId == 2)
//                .Select(fm => fm.Monto)
//                .DefaultIfEmpty(0)
//                .Sum();

            var total = ingresosPorMes02 - gastosPorMes02;
            return View("Index", total);
        }
    }
}