using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions.LiteDBSupport;

public enum UIPlatform
{
    WPF,
    WinUI3
}
/// <summary>
/// Provides a single point of reference for Files used by the Filament Database, currently it only uses a LiteDb JSON type database
/// </summary>
public class FileLocator
{
    const string dbName = "FilamentData.litedb";
    public static string DatabasePath(UIPlatform platform = UIPlatform.WPF,string path="Filament",string dbName= "FilamentDataV3_0.litedb") => platform switch 
    {
        UIPlatform.WPF=> Path.Combine(DirLocation(UIPlatform.WPF,path), dbName),
        UIPlatform.WinUI3 =>Path.Combine(DirLocation(UIPlatform.WPF,path),dbName),
        _=>string.Empty
    };
    private static string DirLocation(UIPlatform platform = UIPlatform.WPF, string path = "Filament") => platform switch
    {
        UIPlatform.WPF => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), path),
        UIPlatform.WinUI3 => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        _ => string.Empty
    };
}
