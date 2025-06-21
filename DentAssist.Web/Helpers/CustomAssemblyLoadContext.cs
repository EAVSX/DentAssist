using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace DentAssist.Web.Helpers
{
    public class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        public IntPtr LoadUnmanagedLibrary(string absolutePath)
        {
            return LoadUnmanagedDll(absolutePath);
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllPath)
        {
            return LoadUnmanagedDllFromPath(unmanagedDllPath);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            return null;
        }
    }
}
