using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System.Collections.Generic;

namespace CapaPresentacion.Pages.Usuario
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly UsuarioBLL usuarioBLL = new UsuarioBLL();
        private readonly RolBLL rolBLL = new RolBLL();

        [BindProperty] public Entidades.Usuario UsuarioEditado { get; set; } = new Entidades.Usuario();
        [BindProperty] public string Contrasena { get; set; } = string.Empty;
        [BindProperty] public string ConfirmarContrasena { get; set; } = string.Empty;
        public List<Entidades.Rol> ListaRoles { get; set; } = new List<Entidades.Rol>();

        public void OnGet(int id)
        {
            ListaRoles = rolBLL.ObtenerTodos();
            
            try
            {
                UsuarioEditado = usuarioBLL.ObtenerPorId(id);
                if (UsuarioEditado == null)
                {
                    TempData["Swal_Message"] = "Usuario no encontrado";
                    TempData["Swal_Icon"] = "error";
                    RedirectToPage();
                }
            }
            catch (Exception ex)
            {
                TempData["Swal_Message"] = ex.Message;
                TempData["Swal_Icon"] = "error";
                RedirectToPage();
            }
        }

        public IActionResult OnPost(int id)
        {
            // Cargar roles siempre para re-renderizar la vista en caso de error
            ListaRoles = rolBLL.ObtenerTodos();

            // Validar si las contraseñas coinciden
            if (!string.IsNullOrEmpty(Contrasena) || !string.IsNullOrEmpty(ConfirmarContrasena))
            {
                if (Contrasena != ConfirmarContrasena)
                {
                    TempData["Swal_Message"] = "Las contraseñas no coinciden.";
                    TempData["Swal_Icon"] = "warning";
                    return Page();
                }
            }

            try
            {
                UsuarioEditado.Id = id;
                UsuarioEditado.Contra = Contrasena;
                var resultado = usuarioBLL.EditarUsuario(UsuarioEditado);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Usuario actualizado exitosamente";
                    TempData["Swal_Icon"] = "success";
                    return RedirectToPage("./Index");
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo actualizar el usuario";
                    TempData["Swal_Icon"] = "error";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                TempData["Swal_Message"] = ex.Message;
                TempData["Swal_Icon"] = "error";
                return Page();
            }
        }
    }
}