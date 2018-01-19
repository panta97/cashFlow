using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cashflow.Models;
using Cashflow.Dtos;

namespace Cashflow.Controllers.Api
{
    public class PensionesController : ApiController
    {
        private ApplicationDbContext _context;

        public PensionesController()
        {
            _context = new ApplicationDbContext();
        }

        [Route("api/pensiones/{flujoId}")]
        public PensionEditDto GetPensionId(int flujoId)
        {

            var flujopension = _context.FlujoPensiones
                .Include(p => p.Pension)
                .Single(fp => fp.FlujoId == flujoId);

//            var pensionEditDto = new PensionEditDto()
//            {
//                PensionId = _context.FlujoPensiones.Where(fp => fp.FlujoId == flujoId).Select(fp => fp.PensionId)
//                    .Single()
//            };

            var pensionEditDto = new PensionEditDto()
            {
                PensionId = flujopension.PensionId,
                Aporte = flujopension.Pension.Aporte
            };

            return pensionEditDto;
        }
    }
}
