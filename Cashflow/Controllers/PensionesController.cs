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
    public class PensionesController : Controller
    {
        private ApplicationDbContext _context;

        public PensionesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Pagos
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New()
        {
            var userId = User.Identity.GetUserId();
            var nuevaPension = new Flujo();

            var miembros = _context.Miembros.Where(m => m.ApplicationUserId == userId).ToList();

            var miembrosAdultos = new List<Miembro>();
            foreach (var miembro in miembros)
            {
                if (AgeUtils.GetEdad(miembro.FechaNacimiento) >= 17)
                {
                    miembrosAdultos.Add(miembro);
                }
            }

            var viewModel = new PensionFormViewModel()
            {
                Flujo = nuevaPension,
                Miembros = miembrosAdultos,
                Pensiones = _context.Pensiones.ToList()

            };
  
            return View("PensionForm", viewModel);
        }

        [HttpPost]
        public ActionResult Save(Flujo flujo, Miembro miembro, Pension pension)
        {
            if (flujo.Id == 0)
            {
                var currentYear = DateTime.Now.Year;

                var pensionSelected = _context.Pensiones.Single(p => p.Id == pension.Id);

                flujo.Fecha = new DateTime(currentYear, 1,1);
                flujo.FechaFin = new DateTime(currentYear, 12, 1);
                flujo.PeriodoId = Periodo.Mensual;
                flujo.TipoId = Tipo.Ingreso;
                flujo.Monto = flujo.Monto * (1 - pensionSelected.Aporte);
                _context.Flujos.Add(flujo);
                _context.SaveChanges();


                var montos = MonthUtils
                    .GetMonthList(flujo.Fecha, flujo.FechaFin, flujo.PeriodoId - 1, Convert.ToDecimal(flujo.Monto));

                for (var mes = 0; mes < 12; mes++)
                {
                    if (montos[mes] == 0) continue;

                    var newFlujoMensual = new FlujoMensual()
                    {
                        FlujoId = flujo.Id,
                        MesId = Convert.ToByte(mes + 1),
                        Monto = montos[mes]
                    };
                    _context.FlujosMensuales.Add(newFlujoMensual);
                }

                var newflujoMiembro = new FlujoMiembro()
                {
                    FlujoId = flujo.Id,
                    MiembroId = miembro.Id
                };

                var newflujoPension = new FlujoPension()
                {
                    FlujoId = flujo.Id,
                    PensionId = pension.Id
                };

                _context.FlujoPensiones.Add(newflujoPension);
                _context.FlujoMiembros.Add(newflujoMiembro);
            }
            else
            {
                var pensionSelected = _context.Pensiones.Single(p => p.Id == pension.Id);

                var flujoInDb = _context.Flujos.Single(f => f.Id == flujo.Id);
                flujoInDb.Nombre = flujo.Nombre;
                flujoInDb.Monto = flujo.Monto * (1 - pensionSelected.Aporte);

                var montos = MonthUtils
                    .GetMonthList(flujoInDb.Fecha, flujoInDb.FechaFin, flujoInDb.PeriodoId - 1, Convert.ToDecimal(flujoInDb.Monto));

                for (var mes = 0; mes < 12; mes++)
                {
                    var flujoMensualInDb = _context.FlujosMensuales
                        .Where(fm => fm.MesId == (mes + 1) && fm.FlujoId == flujoInDb.Id)
                        .DefaultIfEmpty(null)
                        .Single();

                    flujoMensualInDb.Monto = montos[mes];
                }
                var flujoMienbroInDb = _context.FlujoMiembros.Single(fm => fm.FlujoId == flujo.Id);
                flujoMienbroInDb.MiembroId = miembro.Id;
                var flujoPensionInDb = _context.FlujoPensiones.Single(fp => fp.FlujoId == flujo.Id);
                flujoPensionInDb.PensionId = pension.Id;
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Pensiones");
        }

        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var pension = _context.Flujos.SingleOrDefault(i => i.Id == id);
            //                    1800 / (1 - 0.1)

            var miembros = _context.Miembros.Where(m => m.ApplicationUserId == userId).ToList();

            var miembrosAdultos = new List<Miembro>();
            foreach (var miembro in miembros)
            {
                if (AgeUtils.GetEdad(miembro.FechaNacimiento) >= 17)
                {
                    miembrosAdultos.Add(miembro);
                }
            }


            if (pension == null)
                return HttpNotFound();


            var viewModel = new PensionFormViewModel()
            {
                Flujo = pension,
                Miembros = miembrosAdultos,
                Pensiones = _context.Pensiones.ToList()
            };

            return View("PensionForm", viewModel);
        }
    }
}