using System.Web.Mvc;

namespace Cashflow.Areas.Comparar
{
    public class CompararAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Comparar";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Comparar_default",
                "Comparar/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}