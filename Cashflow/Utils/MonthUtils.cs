using System;
using System.Collections.Generic;

namespace Cashflow.Utils
{
    public class MonthUtils
    {
        static public List<decimal> GetMonthList(DateTime fechaInicio, DateTime? fechafin, int periodo, decimal monto)
        {

            var periodoValor = -1;

            switch (periodo)
            {
                case 1:
                case 3:
                    periodoValor = 1;
                    break;

                case 2:
                    periodoValor = 7;
                    break;
            }

            var montosPorMeses = new List<decimal> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            var fecha = fechaInicio;
            var mes = fecha.Month;
            var eventosPorMes = 0;

            if (fechafin == fechaInicio)
            {
                montosPorMeses[mes - 1] = monto;
                return montosPorMeses;
            }

            do
            {
                eventosPorMes++;
                fecha = (periodo == 3) ? fecha.AddMonths(periodoValor) : fecha.AddDays(periodoValor);

                if (mes == fecha.Month) continue;
                montosPorMeses[mes - 1] = eventosPorMes * monto;
                eventosPorMes = 0;
                mes = fecha.Month;
            } while (fecha <= fechafin);


            if (eventosPorMes != 0)
                montosPorMeses[mes - 1] = eventosPorMes * monto;

            return montosPorMeses;
        }

    }
}