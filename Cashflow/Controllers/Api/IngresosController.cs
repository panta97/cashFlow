using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Data.Entity;
using AutoMapper;
using Cashflow.Dtos;
using Cashflow.Models;
using Cashflow.Utils;
using Microsoft.AspNet.Identity;

namespace Cashflow.Controllers.Api
{
    public class IngresosController : ApiController
    {
        private ApplicationDbContext _context;

        public IngresosController()
        {
            _context = new ApplicationDbContext();
        }

//        [Authorize(Roles = "user")]
        [Route("api/ingresos/user/{userId}")]
        public IEnumerable<FlujoDto> GetIngresoByUser(string userId)
        {

//            var ingresos = _context.Flujos
//                .Include(f => f.Periodo)
//                .Where(f => f.TipoId == 1 && f.ApplicationUserId == userId)
//                .ToList()
//
//                .Select(Mapper.Map<Flujo, FlujoDto>);

            var flujosIds = FlujoUtils.GetFlujosByUserId(userId);

            var ingresos2 = flujosIds
                .Select(flujoId => _context.Flujos
                .Include(f => f.Periodo)
                .Single(f => f.TipoId == 1 && f.Id == flujoId))
                .ToList();


            return ingresos2.Select(Mapper.Map<Flujo, FlujoDto>);
        }


        [Route("api/ingresos/user02/{userId}")]
        public IList<IngresoDto> GetIngresoDtoByUser(string userId)
        {


            var ingresosDto = new List<IngresoDto>();


            var ingresos02 = FlujoUtils.GetIngresosOtrosByUserId(userId)
                .Select(flujo => _context.Flujos
                .Include(f => f.Periodo)
                .SingleOrDefault(f => f.TipoId == 1 && f.Id == flujo)
                )
                .SkipWhile(f => f == null)
                .ToList();

            foreach (var ingreso in ingresos02)
            {
                //because you've added miembro field kind of tricky
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

        [Route("api/pensiones/user02/{userId}")]
        public IList<PensionDto> GetPensionDtoByUser(string userId)
        {
            var ingresosDto = new List<PensionDto>();


            var ingresos02 = FlujoUtils.GetIngresosPensionesByUserId(userId)
                .Select(flujo => _context.Flujos
                    .Include(f => f.Periodo)
                    .SingleOrDefault(f => f.TipoId == 1 && f.Id == flujo)
                )
                .SkipWhile(f => f == null)
                .ToList();

            foreach (var ingreso in ingresos02)
            {
                //because you've added miembro field kind of tricky
                if (ingreso == null) continue;

                var miembroNombre = _context.FlujoMiembros
                    .Where(fm => fm.FlujoId == ingreso.Id)
                    .Select(fm => fm.Miembro.Nombre).DefaultIfEmpty(" ").Single();

                var pensionNombre = _context.FlujoPensiones
                    .Where(fp => fp.FlujoId == ingreso.Id)
                    .Select(fp => fp.Pension.Nombre).DefaultIfEmpty(" ").Single();

                if (miembroNombre == " " || pensionNombre == " ") continue;

                var ingresoDto = new PensionDto()
                {
                    Id = ingreso.Id,
                    Nombre = ingreso.Nombre,
                    Monto = ingreso.Monto,
                    Fecha = ingreso.Fecha,
                    TipoId = ingreso.TipoId,
                    MiembroNombre = miembroNombre,
                    PensionNombre = pensionNombre
                };

                ingresosDto.Add(ingresoDto);
            }
            return ingresosDto;
        }

        //        public IEnumerable<FlujoDto> GetIngresos()
        //        {
        //
        //            var name = HttpContext.Current.User.Identity.GetUserId();
        //
        //            var userId = User.Identity.GetUserId();
        //
        ////            var ingresos = _context.Flujos.Where(f => f.TipoId == 1 && f.ApplicationUserId == userId).ToList();
        //            var ingresos = _context.Flujos.Where(f => f.TipoId == 1 && f.ApplicationUserId == name)
        //                .ToList()
        //                .Select(Mapper.Map<Flujo, FlujoDto>);
        //
        //            return ingresos;
        //        }
        //
        //        public IHttpActionResult GetIngreso(int id)
        //        {
        //            var usedId = User.Identity.GetUserId();
        //            var ingreso = _context.Flujos.Where(f => f.ApplicationUserId == usedId).SingleOrDefault(f => f.Id == id);
        //
        //            if (ingreso == null)
        //                return NotFound();
        //
        //            return Ok(Mapper.Map<Flujo, FlujoDto>(ingreso));
        //        }
        //
        //        [HttpPost]
        //        public IHttpActionResult CreateIngreso(FlujoDto ingresoDto)
        //        {
        //            if (!ModelState.IsValid)
        //                return BadRequest();
        //
        //            ingresoDto.TipoId = 1;
        //
        //            var ingreso = Mapper.Map<FlujoDto, Flujo>(ingresoDto);
        //
        //            _context.Flujos.Add(ingreso);
        //            _context.SaveChanges();
        //
        //            ingresoDto.Id = ingreso.Id;
        //
        //            return Created(new Uri(Request.RequestUri + "/" + ingreso.Id), ingresoDto );
        //        }
        //
        //        [HttpPut]
        //        public void UpdateIngreso(int id, FlujoDto  flujoDto)
        //        {
        //            if(!ModelState.IsValid)
        //                throw new HttpResponseException(HttpStatusCode.BadRequest);
        //            
        //            var flujoInDb = _context.Flujos.SingleOrDefault(f => f.Id == id);
        //
        //            if(flujoDto == null)
        //                throw new HttpResponseException(HttpStatusCode.NotFound);
        //
        //            Mapper.Map<FlujoDto, Flujo>(flujoDto, flujoInDb);
        //            _context.SaveChanges();
        //        }

        [HttpDelete]
        public void DeleteIngreso(int id)
        {
            var flujoInDb = _context.Flujos.SingleOrDefault(f => f.Id == id);

            if (flujoInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            var flujosMensuales = _context.FlujosMensuales.Where(fm => fm.FlujoId == id).ToList();
            foreach (var flujoMensual in flujosMensuales)
            {
                _context.FlujosMensuales.Remove(flujoMensual);
            }

            var flujoPensiones = _context.FlujoPensiones.Where(fp => fp.FlujoId == id).ToList();

            foreach (var flujoPension in flujoPensiones)
            {
                _context.FlujoPensiones.Remove(flujoPension);
            }

            _context.Flujos.Remove(flujoInDb);
            _context.SaveChanges();
        }
    }
}
