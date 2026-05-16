namespace CapaPresentacion.Middlewares
{
    public class VerificadorPermisosMiddleware
    {
        private readonly RequestDelegate _next;

        // Rutas públicas que no necesitan verificación de permisos
        private static readonly string[] RutasPublicas =
        {
            "/Acceso/Login",
            "/Acceso/AccesoDenegado",
            "/Acceso/Logout"
        };

        public VerificadorPermisosMiddleware(RequestDelegate Next)
            => _next = Next;

        public async Task InvokeAsync(HttpContext Contexto)
        {
            var RutaActual = Contexto.Request.Path.Value?.ToLower();

            // 1. Si es ruta pública, dejar pasar sin verificar
            if (RutasPublicas.Any(R => RutaActual?.StartsWith(R, StringComparison.OrdinalIgnoreCase) == true))
            {
                await _next(Contexto);
                return;
            }

            // 2. Si no está autenticado, lo manda al login (ya lo maneja Cookie Auth)
            if (Contexto.User?.Identity?.IsAuthenticated != true)
            {
                Contexto.Response.Redirect("/Acceso/Login");
                return;
            }

            // 3. Verificar si el usuario tiene el claim "Permiso" con esta ruta
            var TienePermiso = Contexto.User.Claims
                .Where(C => C.Type == "Permiso")
                .Any(C => C.Value.Equals(RutaActual, StringComparison.OrdinalIgnoreCase));

            // 4. Si NO tiene permiso, redirigir a Acceso Denegado
            if (!TienePermiso)
            {
                Contexto.Response.Redirect("/Acceso/AccesoDenegado");
                return;
            }

            // 5. Si tiene permiso, continuar con el request
            await _next(Contexto);
        }
    }
}
