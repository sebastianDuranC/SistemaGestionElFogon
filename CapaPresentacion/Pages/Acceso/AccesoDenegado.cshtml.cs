using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CapaPresentacion.Pages.Acceso
{
    [AllowAnonymous]
    public class AccesoDenegadoModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
