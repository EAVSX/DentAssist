namespace DentAssist.Web.Models
{
    // ViewModel simple para mostrar informaci�n de error en las vistas de error del sistema.
    public class ErrorViewModel
    {
        
        public string? RequestId { get; set; }

        
        public bool ShowRequestId
        {
            get
            {
                return !string.IsNullOrEmpty(RequestId);
            }
        }
    }
}
