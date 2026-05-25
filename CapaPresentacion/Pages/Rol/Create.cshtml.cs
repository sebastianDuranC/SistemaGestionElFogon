using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CapaPresentacion.Pages.Rol
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly RolBLL rolBLL = new RolBLL();
        private readonly PermisoBLL permisoBLL = new PermisoBLL();

        [BindProperty]
        public Entidades.Rol NuevoRol { get; set; } = new Entidades.Rol();

        [BindProperty]
        public List<int> SelectedPermisoIds { get; set; } = new List<int>();

        public Dictionary<string, List<Permiso>> PermisosPorModulo { get; set; } = new Dictionary<string, List<Permiso>>();

        public void OnGet()
        {
            CargarPermisos();
        }

        private void CargarPermisos()
        {
            try
            {
                var todosPermisos = permisoBLL.ObtenerTodos();
                PermisosPorModulo = todosPermisos
                    .GroupBy(p => p.Modulo)
                    .ToDictionary(g => g.Key, g => g.ToList());
            }
            catch (Exception ex)
            {
                TempData["Swal_Message"] = "Error al cargar permisos: " + ex.Message;
                TempData["Swal_Icon"] = "error";
                PermisosPorModulo = new Dictionary<string, List<Permiso>>();
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                CargarPermisos();
                return Page();
            }

            try
            {
                rolBLL.CrearRol(NuevoRol, SelectedPermisoIds);
                TempData["Swal_Message"] = "Rol creado exitosamente";
                TempData["Swal_Icon"] = "success";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                TempData["Swal_Message"] = ex.Message;
                TempData["Swal_Icon"] = "error";
                CargarPermisos();
                return Page();
            }
        }
    }
}
