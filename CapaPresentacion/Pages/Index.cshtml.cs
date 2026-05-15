using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CapaPresentacion.Pages;

public class IndexModel : PageModel
{
    // 1. Creas una propiedad pública
    public string MensajeBienvenida { get; set; } = "";

    public void OnGet()
    {
        // 2. Le asignas valor cuando la página carga
        MensajeBienvenida = "¡Hola desde C#!";
    }
}
