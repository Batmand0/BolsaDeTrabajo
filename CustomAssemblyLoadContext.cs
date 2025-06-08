using System;
using System.IO;
using System.Runtime.Loader;

public class CustomAssemblyLoadContext : AssemblyLoadContext
{
    public CustomAssemblyLoadContext() : base(isCollectible: true)
    {
    }

    // Método público para cargar una DLL no administrada
    public IntPtr LoadNativeLibrary(string unmanagedDllName)
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "lib", unmanagedDllName);
        if (File.Exists(path))
        {
            return LoadUnmanagedDllFromPath(path);
        }

        // Si no se encuentra, devolvemos null o puedes manejarlo según necesites
        return IntPtr.Zero;
    }

    // Sobrescribir el método LoadUnmanagedDll que es protegido
    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "lib", unmanagedDllName);
        if (File.Exists(path))
        {
            return LoadUnmanagedDllFromPath(path);
        }
        return base.LoadUnmanagedDll(unmanagedDllName);
    }
}
