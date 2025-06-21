using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace DentAssist.Web.Helpers
{
    
    // Usada por ejemplo para cargar DLLs necesarias para generación de PDF o integración con librerías externas.
    public class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        // Método para cargar una librería nativa (por ruta absoluta)
        public IntPtr LoadUnmanagedLibrary(string absolutePath)
        {
            return LoadUnmanagedDll(absolutePath);
        }

        // Sobrescribe el método base para cargar una librería nativa desde una ruta específica
        protected override IntPtr LoadUnmanagedDll(string unmanagedDllPath)
        {
            return LoadUnmanagedDllFromPath(unmanagedDllPath);
        }

        // No carga ensamblados administrados adicionales, retorna null para Assembly
        protected override Assembly Load(AssemblyName assemblyName)
        {
            return null;
        }
    }
}
