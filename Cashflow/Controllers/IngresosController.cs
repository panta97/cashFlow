using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cashflow.Models;
using Cashflow.Utils;
using Cashflow.ViewModels;
using Microsoft.AspNet.Identity;

namespace Cashflow.Controllers
{
    [Authorize(Roles = "user")]
    public class IngresosController : Controller
    {

        private ApplicationDbContext _context;

        public IngresosController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Ingresos
        public ActionResult Index()
        {
//            var userId = User.Identity.GetUserId();
//            var viewModel = _context.Flujos.Where(f => f.Tipo.Nombre == "Ingreso" && f.ApplicationUserId == userId).Include(f => f.Periodo).ToList();

            return View();
        }

        //when adding a new ingreso you also need to populate the flujos mensuales table and add a field into flujoMiembros table

        public ActionResult New()
        {
            var userId = User.Identity.GetUserId();
            //bootstrap datepicker
            var defaultDate = DateTime.Today;
            var nuevoIngreso = new Flujo()
            {
                Fecha = defaultDate,
                FechaFin = defaultDate,
//                ApplicationUserId = userId
                
            };

            var periodos = _context.Periodos.ToList();
            var miembros = _context.Miembros.Where(m => m.ApplicationUserId == userId);
            var viewModel = new IngresoFormViewModel()
            {
                Flujo = nuevoIngreso,
                Periodos = periodos,
                Miembros = miembros

            }; 

            return View("IngresoForm", viewModel);
        }

        [HttpPost]
        public ActionResult Save(Flujo flujo, Miembro miembro)
        {

//             to enable validation
            if (flujo.Monto < 0)
            {
                var userId = User.Identity.GetUserId();
                var defaultDate = DateTime.Today;
                var nuevoIngreso = new Flujo()
                {
                    Fecha = defaultDate,
                    FechaFin = defaultDate,
                };

                var periodos = _context.Periodos.ToList();
                var miembros = _context.Miembros.Where(m => m.ApplicationUserId == userId);
                var viewModel = new IngresoFormViewModel()
                {
                    Flujo = nuevoIngreso,
                    Periodos = periodos,
                    Miembros = miembros

                };
                return View("IngresoForm", viewModel);
            }


            //si el periodo es ninguno
            if (flujo.PeriodoId == Periodo.Ninguno)
                flujo.FechaFin = flujo.Fecha;

            if (flujo.Id == 0)
            {
                //tabla flujos
//                var userId = User.Identity.GetUserId();
                var tipoIngreso = _context.Tipos.Single(t => t.Nombre == "Ingreso");
                flujo.Tipo = tipoIngreso;
//                flujo.ApplicationUserId = userId;
                _context.Flujos.Add(flujo);


                //para obtener el id
                _context.SaveChanges();

                //tabla flujos mesuales

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

                // tabla flujos miembros
                var newflujoMiembro = new FlujoMiembro()
                {
                    FlujoId = flujo.Id,
                    MiembroId = miembro.Id
                };

                _context.FlujoMiembros.Add(newflujoMiembro);

            }
            else
            {
                var flujoInDb = _context.Flujos.Single(f => f.Id == flujo.Id);
                flujoInDb.Nombre = flujo.Nombre;
                flujoInDb.Monto = flujo.Monto;
                flujoInDb.Fecha = flujo.Fecha;
                flujoInDb.PeriodoId = flujo.PeriodoId;
                flujoInDb.FechaFin = flujo.FechaFin;

                //tabla flujomensual
                var montos = MonthUtils
                    .GetMonthList(flujo.Fecha, flujo.FechaFin, flujo.PeriodoId - 1, Convert.ToDecimal(flujo.Monto));


                for (var mes = 0; mes < 12; mes++)
                {
                    var flujoMensualInDb = _context.FlujosMensuales
                        .Where(fm => fm.MesId == (mes + 1) && fm.FlujoId == flujoInDb.Id)
                        .DefaultIfEmpty(null)
                        .Single();

                    if (flujoMensualInDb == null)
                    {
                        if (montos[mes] == 0) continue;

                        var newFlujoMensual = new FlujoMensual()
                        {
                            FlujoId = flujoInDb.Id,
                            MesId = Convert.ToByte(mes + 1),
                            Monto = montos[mes]
                        };

                        _context.FlujosMensuales.Add(newFlujoMensual);
                    }
                    else
                    {
                        flujoMensualInDb.Monto = montos[mes];
                    }
                }

                //tabla flujo miembro
                var flujoMiembroInDb = _context.FlujoMiembros.Single(fm => fm.FlujoId == flujo.Id);
                flujoMiembroInDb.MiembroId = miembro.Id;
            }        

            _context.SaveChanges();

            return RedirectToAction("Index", "Ingresos");
        }


        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var ingreso = _context.Flujos.SingleOrDefault(i => i.Id == id);

            if (ingreso == null)
                return HttpNotFound();

            var viewModel = new IngresoFormViewModel()
            {
                Flujo = ingreso,
                Periodos = _context.Periodos.ToList(),
                Miembros = _context.Miembros.Where(m => m.ApplicationUserId == userId)

            }; 

            return View("IngresoForm", viewModel);
        }
    }
}