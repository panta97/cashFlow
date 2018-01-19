using System.Collections.Generic;
using System.Linq;
using Cashflow.Models;

namespace Cashflow.Utils
{
    public class FlujoUtils
    {

        private static ApplicationDbContext _context;

        public static List<int> GetFlujosByUserId(string userId)
        {
            _context = new ApplicationDbContext();
            var miembros = _context.Miembros
                .Where(m => m.ApplicationUserId == userId).Select(m => m.Id).ToList();

            var flujosIds = new List<int>();
            foreach (var miembro in miembros)
            {
                var flujoMiembro = _context.FlujoMiembros.Where(fm => fm.MiembroId == miembro)
                    .Select(fm => fm.FlujoId)
                    .ToList();
                flujosIds.AddRange(flujoMiembro);
            }
            return flujosIds;
        }

        public static List<int> GetIngresosOtrosByUserId(string userId)
        {
            _context = new ApplicationDbContext();
            var miembros = _context.Miembros
                .Where(m => m.ApplicationUserId == userId).Select(m => m.Id).ToList();

            var ingresosIds = new List<int>();
            foreach (var miembro in miembros)
            {
                var flujoMiembro = _context.FlujoMiembros.Where(fm => fm.MiembroId == miembro)
                    .Select(fm => fm.FlujoId)
                    .ToList();
                ingresosIds.AddRange(flujoMiembro);
            }

//            TODO: validar por user id
            var ingresosPensionesIds = _context.FlujoPensiones.Select(fp => fp.FlujoId).ToList();


            return ingresosIds.Except(ingresosPensionesIds).ToList();
        }

        public static List<int> GetIngresosPensionesByUserId(string userId)
        {
            return GetFlujosByUserId(userId).Except(GetIngresosOtrosByUserId(userId)).ToList();
        }
    }
}