using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System.Collections.Generic;

namespace CapaPresentacion.Pages.Usuario
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly UsuarioBLL usuarioBLL = new UsuarioBLL();
        private readonly RolBLL rolBLL = new RolBLL();
        [BindProperty] public Entidades.Usuario NuevoUsuario { get; set; } = new Entidades.Usuario();
        [BindProperty] public string Contrasena { get; set; } = string.Empty;
        [BindProperty] public string ConfirmarContrasena { get; set; } = string.Empty;
        public List<Entidades.Rol> ListaRoles { get; set; } = new List<Entidades.Rol>();

        public void OnGet()
        {
            ListaRoles = rolBLL.ObtenerTodos();
        }

        public IActionResult OnPost()
        {
            // Validación simple en presentación: verificar que las contraseñas coincidan
            if (Contrasena != ConfirmarContrasena)
            {
                // Enviar mensaje a la vista para mostrar con SweetAlert2
                TempData["Swal_Message"] = "Las contraseñas no coinciden.";
                TempData["Swal_Icon"] = "warning";
                ListaRoles = rolBLL.ObtenerTodos();
                return Page();
            }

            NuevoUsuario.Contra = Contrasena;
            NuevoUsuario.NegocioId = 1;
            NuevoUsuario.Estado = true;

            try
            {
                var resultado = usuarioBLL.CrearUsuario(NuevoUsuario);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Usuario creado exitosamente";
                    TempData["Swal_Icon"] = "success";
                    return RedirectToPage("./Index");
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo crear el usuario.";
                    TempData["Swal_Icon"] = "error";
                    ListaRoles = rolBLL.ObtenerTodos();
                    return Page();
                }
            }
            catch (System.Exception ex)
            {
                TempData["Swal_Message"] = ex.Message;
                TempData["Swal_Icon"] = "error";
                ListaRoles = rolBLL.ObtenerTodos();
                return Page();
            }
        }
    }
}
