using System;

namespace Cashflow.Utils
{
    public class AgeUtils
    {
        public static int GetEdad(DateTime fechaNacimiento)
        {
            // Save today's date.
            var hoy = DateTime.Today;
            // Calculate the age.
            var edad = hoy.Year - fechaNacimiento.Year;
            // Go back to the year the person was born in case of a leap year
            if (fechaNacimiento > hoy.AddYears(-edad)) edad--;

            return edad;
        }
    }
}