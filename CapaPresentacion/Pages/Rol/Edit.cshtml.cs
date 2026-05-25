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
    public class EditModel : PageModel
    {
        private readonly RolBLL rolBLL = new RolBLL();
        private readonly PermisoBLL permisoBLL = new PermisoBLL();
        private readonly RolPermisoBLL rolPermisoBLL = new RolPermisoBLL();

        [BindProperty]
        public Entidades.Rol RolEditado { get; set; } = new Entidades.Rol();

        [BindProperty]
        public List<int> SelectedPermisoIds { get; set; } = new List<int>();

        public Dictionary<string, List<Permiso>> PermisosPorModulo { get; set; } = new Dictionary<string, List<Permiso>>();

        public IActionResult OnGet(int id)
        {
            CargarPermisos();
            RolEditado = rolBLL.ObtenerPorId(id);
            if (RolEditado == null)
            {
                TempData["Swal_Message"] = "Rol no encontrado";
                TempData["Swal_Icon"] = "error";
                return RedirectToPage("./Index");
            }
            SelectedPermisoIds = rolPermisoBLL.ObtenerPermisosPorRol(id);
            return Page();
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

        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                CargarPermisos();
                return Page();
            }

            try
            {
                RolEditado.Id = id;
                var resultado = rolBLL.EditarRol(RolEditado, SelectedPermisoIds);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Rol actualizado exitosamente";
                    TempData["Swal_Icon"] = "success";
                    return RedirectToPage("./Index");
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo actualizar el rol";
                    TempData["Swal_Icon"] = "error";
                    CargarPermisos();
                    return Page();
                }
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
