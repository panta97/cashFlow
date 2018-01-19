using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Services.Description;
using Cashflow.Dtos;
using Cashflow.Models;
using Cashflow.Utils;

namespace Cashflow.Controllers.Api
{
    public class CompararMiembroController : ApiController
    {
        private ApplicationDbContext _context;

        public CompararMiembroController()
        {
            _context = new ApplicationDbContext();
        }

        [Route("api/comparar/miembro/{year}/{userId}")]
        public DetalleMiembroDto GetFlujosByYear(int year, string userId)
        {
            var fisrtDay = new DateTime(year, 1, 1);

            var miembros = new List<string>();
            var ingresosPorMiembro = new List<decimal>();
            var gastosPorMiembro = new List<decimal>();


            miembros = _context.Miembros
                .Where(m => m.ApplicationUserId == userId)
                .Select(m => m.Nombre)
                .ToList();

            var miembrosId = _context.Miembros
                .Where(m => m.ApplicationUserId == userId)
                .Select(m => m.Id)
                .ToList();

            foreach (var miembroId in miembrosId)
            {
                var flujosIdPorMiembro = _context.FlujoMiembros
                    .Where(fm => fm.MiembroId == miembroId && fm.Flujo.TipoId == Tipo.Ingreso)
                    .Select(fm => fm.FlujoId)
                    .ToList();

                var ingreso = flujosIdPorMiembro
                    .Sum(flujoIdPorMiembro => _context.FlujosMensuales
                    .Where(fm => fm.FlujoId == flujoIdPorMiembro)
                    .Select(fm => fm.Monto)
                    .DefaultIfEmpty(0)
                    .Sum());
                ingresosPorMiembro.Add(ingreso);
            }

            foreach (var miembroId in miembrosId)
            {
                var flujosIdPorMiembro = _context.FlujoMiembros
                    .Where(fm => fm.MiembroId == miembroId && fm.Flujo.TipoId == Tipo.Gasto)
                    .Select(fm => fm.FlujoId)
                    .ToList();

                var gasto = flujosIdPorMiembro
                    .Sum(flujoIdPorMiembro => _context.FlujosMensuales
                        .Where(fm => fm.FlujoId == flujoIdPorMiembro)
                        .Select(fm => fm.Monto)
                        .DefaultIfEmpty(0)
                        .Sum());
                gastosPorMiembro.Add(gasto);
            }


            var detalle = new DetalleMiembroDto()
            {
                Miembros = miembros,
                Ingresos = ingresosPorMiembro,
                Gastos = gastosPorMiembro,
                Colors = PieUtils.GetColors()
            };

            return detalle;
        }
    }
}


//namespace Cashflow.Controllers.Api
//{
//    public class CompararMiembroController : ApiController
//    {
//        private ApplicationDbContext _context;
//
//        public CompararMiembroController()
//        {
//            _context = new ApplicationDbContext();
//        }
//
//        [Route("api/comparar/miembro/{year}/{userId}")]
//        public DetalleMiembroDto GetFlujosByYear(int year, string userId)
//        {
//            var fisrtDay = new DateTime(year, 1, 1);
//
//            var miembros = new List<string>();
//            var ingresosPorMiembro = new List<decimal>();
//            var gastosPorMiembro = new List<decimal>();
//
//
//            miembros = _context.Miembros
//                .Where(m => m.ApplicationUserId == userId)
//                .Select(m => m.Nombre)
//                .ToList();
//
//            var miembrosId = _context.Miembros
//                .Where(m => m.ApplicationUserId == userId)
//                .Select(m => m.Id)
//                .ToList();
//
//            foreach (var miembroId in miembrosId)
//            {
//                var flujosIdPorMiembro = _context.FlujoMiembros
//                    .Where(fm => fm.MiembroId == miembroId && fm.Flujo.TipoId == Tipo.Ingreso)
//                    .Select(fm => fm.FlujoId)
//                    .ToList();
//
//                decimal flujo = 0;
//
//                foreach (var flujoIdPorMiembro in flujosIdPorMiembro)
//                {
//                    flujo += _context.FlujosMensuales
//                        .Where(fm => fm.FlujoId == flujoIdPorMiembro)
//                        .Select(fm => fm.Monto)
//                        .DefaultIfEmpty(0)
//                        .Sum();
//                }
//
//            }
//
//
//
//            return null;
//        }
//    }
//}