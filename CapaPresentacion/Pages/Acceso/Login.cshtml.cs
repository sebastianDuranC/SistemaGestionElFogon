using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using CapaNegocio;

namespace CapaPresentacion.Pages.Acceso
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UsuarioBLL _usuarioBll = new UsuarioBLL();
        private readonly PermisoBLL _permisoBll = new PermisoBLL();

        [BindProperty] public string Nombre { get; set; } = "";
        [BindProperty] public string Contra { get; set; } = "";

        public string MensajeError { get; set; } = "";

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var Usuario = _usuarioBll.ValidarCredenciales(Nombre, Contra);

            if (Usuario == null)
            {
                MensajeError = "Usuario o contraseña incorrectos";
                return Page();
            }

            //Obtener las rutas que el rol del usuario puede acceder
            var RutasPermitidas = _permisoBll.ObtenerRutasPermitidas(Usuario.RolId);

            //Construir Claims (datos que se guardan en la cookie)
            var Claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Usuario.Id.ToString()),
                new Claim(ClaimTypes.Name,           Usuario.Nombre),
                new Claim("RolId",                   Usuario.RolId.ToString()),
                new Claim("NombreRol",               Usuario.NombreRol.ToString()),
                new Claim("NegocioId",               Usuario.NegocioId.ToString())
            };

            //Agregar cada ruta permitida como un claim "Permiso"
            //Venta/Index, Cliente/ListarClientes, Usuario/EditarUsuario, etc
            foreach (var Ruta in RutasPermitidas)
            {
                Claims.Add(new Claim("Permiso", Ruta));
            }

            //Crear identidad y principal
            var Identidad = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var Principal = new ClaimsPrincipal(Identidad);

            //Crear la cookie de sesión
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                Principal
            );

            //Redirigir al dashboard
            return RedirectToPage("/Dashboard");
        }
    }
}
