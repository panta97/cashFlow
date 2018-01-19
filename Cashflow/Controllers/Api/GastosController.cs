using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Cashflow.Dtos;
using Cashflow.Models;
using Cashflow.Utils;
using Microsoft.AspNet.Identity;

namespace Cashflow.Controllers.Api
{
    public class GastosController : ApiController
    {
        private ApplicationDbContext _context;

        public GastosController()
        {
            _context = new ApplicationDbContext();
        }

        [Route("api/gastos/user/{userId}")]
        public IEnumerable<FlujoDto> GetIngresoByUser(string userId)
        {

//            var gastos = _context.Flujos
//                .Include(f => f.Periodo)
//                .Where(f => f.TipoId == 2 && f.ApplicationUserId == userId)
//                .ToList()
//                .Select(Mapper.Map<Flujo, FlujoDto>);

            var flujosIds = FlujoUtils.GetFlujosByUserId(userId);

            var gastos02 = flujosIds
                .Select(flujoId => _context.Flujos
                .Include(f => f.Periodo)
                .Single(f => f.TipoId == 2 && f.Id == flujoId))
                .ToList();

            return gastos02.Select(Mapper.Map<Flujo, FlujoDto>);
        }

        [Route("api/gastos/user02/{userId}")]
        public IList<IngresoDto> GetGastoDtoByUser(string userId)
        {
            var ingresosDto = new List<IngresoDto>();

//            var ingresos = _context.Flujos
//                .Include(f => f.Periodo)
//                .Where(f => f.TipoId == 2 && f.ApplicationUserId == userId)
//                .ToList();

            var gastos02 = FlujoUtils.GetFlujosByUserId(userId)
                .Select(flujo => _context.Flujos
                .Include(f => f.Periodo)
                .SingleOrDefault(f => f.TipoId == 2 && f.Id == flujo)
                )
                .SkipWhile(f => f == null)
                .ToList();


            foreach (var ingreso in gastos02)
            {
                if (ingreso == null) continue;

                var miembroNombre = _context.FlujoMiembros
                    .Where(fm => fm.FlujoId == ingreso.Id)
                    .Select(fm => fm.Miembro.Nombre).DefaultIfEmpty(" ").Single();

                if (miembroNombre == " ") continue;

                var ingresoDto = new IngresoDto()
                {
                    Id = ingreso.Id,
                    Nombre = ingreso.Nombre,
                    Monto = ingreso.Monto,
                    Fecha = ingreso.Fecha,
                    TipoId = ingreso.TipoId,
                    Periodo = ingreso.Periodo,
                    MiembroNombre = miembroNombre
                };

                ingresosDto.Add(ingresoDto);
            }
            return ingresosDto;
        }

//        public IEnumerable<FlujoDto> GetGastos()
//        {
//            var gastos = _context.Flujos.Where(f => f.TipoId == 2)
//                .ToList()
//                .Select(Mapper.Map<Flujo, FlujoDto>);
//            return gastos;
//        }
//
//        public IHttpActionResult GetGasto(int id)
//        {
//            var usedId = User.Identity.GetUserId();
//            var gasto = _context.Flujos.Where(f => f.ApplicationUserId == usedId).SingleOrDefault(f => f.Id == id);
//
//            if (gasto == null)
//                return NotFound();
//
//            return Ok(Mapper.Map<Flujo, FlujoDto>(gasto));
//        }
//
//        [HttpPost]
//        public IHttpActionResult CreateGasto(FlujoDto gastoDto)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest();
//
//            gastoDto.TipoId = 2;
//
//            var gasto = Mapper.Map<FlujoDto, Flujo>(gastoDto);
//            _context.Flujos.Add(gasto);
//            _context.SaveChanges();
//
//            gastoDto.Id = gasto.Id;
//
//            return Created(new Uri(Request.RequestUri + "/" + gasto.Id), gastoDto);
//        }
//
//        [HttpPut]
//        public void UpdateGasto(int id, FlujoDto flujoDto)
//        {
//            if (!ModelState.IsValid)
//                throw new HttpResponseException(HttpStatusCode.BadRequest);
//
//            var flujoInDb = _context.Flujos.SingleOrDefault(f => f.Id == id);
//
//            if (flujoInDb == null)
//                throw new HttpResponseException(HttpStatusCode.NotFound);
//
//            Mapper.Map<FlujoDto, Flujo>(flujoDto, flujoInDb);
//            _context.SaveChanges();
//        }

        [HttpDelete]
        public void DeleteGasto(int id)
        {
            var flujoIndb = _context.Flujos.SingleOrDefault(f => f.Id == id);

            if(flujoIndb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            var flujosMensuales = _context.FlujosMensuales.Where(fm => fm.FlujoId == id).ToList();
            foreach (var flujoMensual in flujosMensuales)
            {
                _context.FlujosMensuales.Remove(flujoMensual);
            }

            _context.Flujos.Remove(flujoIndb);
            _context.SaveChanges();
        }
    }
}
