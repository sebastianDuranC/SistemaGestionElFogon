using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CapaPresentacion.Pages
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
