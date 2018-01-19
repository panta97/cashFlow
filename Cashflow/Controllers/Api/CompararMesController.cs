using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cashflow.Dtos;
using Cashflow.Models;
using Cashflow.Utils;

namespace Cashflow.Controllers.Api
{
    public class CompararMesController : ApiController
    {
        private ApplicationDbContext _context;

        public CompararMesController()
        {
            _context = new ApplicationDbContext();
        }
        

        [Route("api/comparar/{year}/{userId}")]
        public DetalleAnualDto GetFlujosByYear(int year, string userId)
        {

            var firstDay = new DateTime(year, 1, 1);

            var ingresos = new List<decimal>();
            var gastos = new List<decimal>();

            decimal diferencia = 0;
            decimal ingresoAnterior = 0;
            decimal gastoAnterior = 0;

            for (var m = firstDay.Month; m <= 12; m++)
            {



                var ingresosPorMes02 = FlujoUtils.GetFlujosByUserId(userId)
                    .Select(flujo => _context.FlujosMensuales
                        .Where(fm => fm.FlujoId == flujo && fm.Flujo.TipoId == Tipo.Ingreso && fm.MesId == m)
                        .Select(fm => fm.Monto)
                        .DefaultIfEmpty(0)
                        .Sum())
                    .Sum();

//                var ingresosPorMes = _context.FlujosMensuales
//                    .Where(fm => fm.Flujo.Fecha.Year == year)
//                    .Where(fm => fm.Flujo.ApplicationUserId == userId && fm.Flujo.TipoId == 1 && fm.MesId == m)
//                    .Select(fm => fm.Monto)
//                    .DefaultIfEmpty(0)
//                    .Sum();

                var gastosPorMes02 = FlujoUtils.GetFlujosByUserId(userId)
                    .Select(flujo => _context.FlujosMensuales
                        .Where(fm => fm.FlujoId == flujo && fm.Flujo.TipoId == Tipo.Gasto && fm.MesId == m)
                        .Select(fm => fm.Monto)
                        .DefaultIfEmpty(0)
                        .Sum())
                    .Sum();

//                var gastosPorMes = _context.FlujosMensuales
//                     .Where(fm => fm.Flujo.Fecha.Year == year)
//                     .Where(fm => fm.Flujo.ApplicationUserId == userId && fm.Flujo.TipoId == 2 && fm.MesId == m)
//                     .Select(fm => fm.Monto)
//                     .DefaultIfEmpty(0)
//                     .Sum();

                if (m == 1)
                {
                    ingresos.Add(ingresosPorMes02);
                    gastos.Add(gastosPorMes02);

                    ingresoAnterior = ingresosPorMes02;
                    gastoAnterior = gastosPorMes02;

                }
                else
                {
                    diferencia = ingresoAnterior - gastoAnterior + ingresosPorMes02;
                    ingresos.Add(diferencia);
                    gastos.Add(gastosPorMes02);

                    ingresoAnterior = diferencia;
                    gastoAnterior = gastosPorMes02;
//                    diferencia = Convert.ToDecimal(0);
                }

            }

            var detalle = new DetalleAnualDto()
            {
                Ingresos = ingresos,
                Gastos = gastos
            };

            return detalle;
        }
    }
}
