namespace DentAssist.Web.Models
{
    // ViewModel simple para mostrar información de error en las vistas de error del sistema.
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
