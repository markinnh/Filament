using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DataDefinitions;
using DataDefinitions.Interfaces;
namespace Filament.WPF6.Helpers;

internal class JsonFileSupport
{
    internal static string GetExportJsonFilename(string suggestedName,string startingPath)
    {
        var sfd= new Microsoft.Win32.SaveFileDialog
        {
            Title = "Export Definition",
            Filter = "JSON Files|*.json|All Files|*.*",
            FileName = suggestedName,
            InitialDirectory = startingPath
        };
        if (sfd.ShowDialog() == true)
            return sfd.FileName;
        else
            throw new OperationCanceledException("Export operation was cancelled by the user");
    }
    internal static string GetImportJsonFilename(string startingPath)
    {
        var ofd = new Microsoft.Win32.OpenFileDialog
        {
            Title = "Import Definition",
            Filter = "JSON Files|*.json|All Files|*.*",
            InitialDirectory = startingPath
        };
        if (ofd.ShowDialog() == true)
            return ofd.FileName;
        else
            throw new OperationCanceledException("Import operation was cancelled by the user");
    }
    internal static void ExportManyToGJsonPackage<T>(string fileName, IEnumerable<T> items) where T : class
    {
        if (items != null && items.Any())
        {
            var first = items.First();
            var guid = Guid.Empty;
            if (first is ISharingSupport shareable)
                guid = shareable.SharingID;
            else
                throw new InvalidOperationException($"Type {typeof(T).FullName} does not implement ISharingSupport");
            //var content = JsonSerializer.Serialize(items);
            var package = ExportPackage.CreateNew(guid, items, isArray: true);
            package.About = $"Exported {items.Count()} items of '{typeof(T).Name}' on {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var packageContent = JsonSerializer.Serialize(package, options);
            System.IO.File.WriteAllText(fileName, packageContent);
        }
    }
    internal static void ExportToGJsonPackage<T>(string fileName, T item) where T : class, ISharingSupport
    {
        if (item != null)
        {

            var guid = item.SharingID;
            var content = item;
            var package = ExportPackage.CreateNew(guid, content, isArray: false);
            package.About = $"Exported a {typeof(T).Name} on {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition= System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
            var packageContent = JsonSerializer.Serialize(package, options);
            System.IO.File.WriteAllText(fileName, packageContent);
        }
    }
    internal static void ImportFromGJsonPackage<T>(string fileName, out object? result) 
    {

        if (System.IO.File.Exists(fileName))
        {
            var content = System.IO.File.ReadAllText(fileName);
            var package = JsonSerializer.Deserialize<ExportPackage>(content);
            if (package != null)
            {
                try
                {
                    var answer = package.Resolve();
                    result = answer is T t ? t: answer is List<T> arr ? arr : null;
                    return;
                    
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Error deserializing an item of type {typeof(T).FullName} from the import package {fileName}", ex);
                }
            }
            
        }
        result = null;
        throw new InvalidOperationException($"Error importing from package {fileName}, file does not exist or could not be read");
    }
}