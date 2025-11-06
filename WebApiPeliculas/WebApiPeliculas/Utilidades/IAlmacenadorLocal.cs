namespace WebApiPeliculas.Utilidades
{
    public interface IAlmacenadorLocal
    {
        Task BorrarArchivo(string contenedor, string ruta);
        Task<string> EditarArchivo(string contenedor, string ruta, IFormFile archivo);
        Task<string> GuardarArchivo(string contenedor, IFormFile archivo);
    }
}
