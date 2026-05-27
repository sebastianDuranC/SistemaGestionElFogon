using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using CapaDatos;
using System.Collections.Generic;

namespace CapaPresentacion.Pages.ControlCaja
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ControlCajaBLL controlCajaBll = new ControlCajaBLL();
        private readonly EgresosCajaBLL egresosCajaBll = new EgresosCajaBLL();

        public Entidades.ControlCaja CajaActiva { get; set; }
        public ControlCajaResumen ResumenCajaActiva { get; set; }
        public List<Entidades.ControlCaja> HistoricoCajas { get; set; } = new List<Entidades.ControlCaja>();
        public List<Entidades.EgresosCaja> EgresosTurnoActivo { get; set; } = new List<Entidades.EgresosCaja>();

        public void OnGet()
        {
            CajaActiva = controlCajaBll.ObtenerCajaActiva();
            if (CajaActiva != null)
            {
                ResumenCajaActiva = controlCajaBll.ObtenerResumenCaja(CajaActiva.Id);
                EgresosTurnoActivo = egresosCajaBll.ObtenerEgresosPorCaja(CajaActiva.Id);
            }
            HistoricoCajas = controlCajaBll.ObtenerHistorico();
        }
    }
}
