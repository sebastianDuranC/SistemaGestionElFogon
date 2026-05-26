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
            // Obtener ruta actual (sin query string)
            var RutaActual = Contexto.Request.Path.Value;

            // 1. Si es ruta pública, dejar pasar sin verificar
            if (RutasPublicas.Any(R => RutaActual?.StartsWith(R, StringComparison.OrdinalIgnoreCase) == true))
            {
                await _next(Contexto);
                return;
            }

            // Intentar obtener la ruta del motor de vistas para Razor Pages (omitiendo parámetros como /id)
            var Endpoint = Contexto.GetEndpoint();
            var Descriptor = Endpoint?.Metadata.GetMetadata<Microsoft.AspNetCore.Mvc.RazorPages.PageActionDescriptor>();
            var RutaParaVerificar = Descriptor?.ViewEnginePath ?? RutaActual ?? string.Empty;

            // 2. Verificar si el usuario tiene el claim "Permiso" con esta ruta
            // Nota: La verificación de autenticación la maneja UseAuthorization()
            // que redirige al LoginPath configurado. Si llegamos aquí autenticados,
            // solo resta validar los permisos por ruta.
            var TienePermiso = Contexto.User.Claims
                .Any(C => C.Type == "Permiso" && C.Value.Equals(RutaParaVerificar, StringComparison.OrdinalIgnoreCase));

            // 3. Si NO tiene permiso, redirigir a Acceso Denegado
            if (!TienePermiso)
            {
                Contexto.Response.Redirect("/Acceso/AccesoDenegado");
                return;
            }

            // 4. Si tiene permiso, continuar con el request
            await _next(Contexto);
        }
    }
}
