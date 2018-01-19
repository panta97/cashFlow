using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cashflow.Dtos;
using Cashflow.Models;

namespace Cashflow.Controllers.Api
{
    public class MiembrosController : ApiController
    {

        private ApplicationDbContext _context;

        public MiembrosController()
        {
            _context = new ApplicationDbContext();
        }

        [Route("api/miembros/{flujoId}")]
        public MiembroEditDto GetMiembroId(int flujoId)
        {

            var miembroEditDto = new MiembroEditDto()
            {
                MiembroId = _context.FlujoMiembros.Where(fm => fm.FlujoId == flujoId).Select(fm => fm.MiembroId)
                    .Single()
            };

            return miembroEditDto;
        }
    }
}
