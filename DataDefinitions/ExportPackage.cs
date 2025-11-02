using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataDefinitions.Interfaces;
using DataDefinitions.Models;

namespace DataDefinitions;

public class ExportPackage : Observable
{
    public Guid ExportID
    {
        get; set;
    }
    public bool IsArray
    {
        get; set;
    }

    public object Content
    {
        get; set;
    }
    private string _about;
    public string About
    {
        get => _about;
        set => Set(ref _about, value);
    }
    public static ExportPackage CreateNew(Guid guid, object content, bool isArray = false)
    {
        if (guid == Guid.Empty)
            throw new ArgumentException("guid cannot be empty when exporting data in ExportPackage", nameof(guid));
        if (content == null)
            throw new ArgumentNullException(nameof(content), "content cannot be null when exporting data in ExportPackage");

        return new ExportPackage() { ExportID = guid, Content = content, IsArray = isArray };
    }

    public object Resolve()
    {
        if (!IsArray)
        {
            var ty = GetExportedType();
            if (Content.ToString() is string str && !string.IsNullOrEmpty(str))
            {


                return JsonSerializer.Deserialize(str, ty);
            }
            else return null;
        }
        else
            return ResolveArray();
    }
    internal object ResolveArray()
    {
        if (Content.ToString() is string content && !string.IsNullOrEmpty(content))
        {
            switch (ExportID)
            {
                case var id when id == Guid.Parse(FilamentColor.FilamentColorIdentifier): // ExampleType1
                    return JsonSerializer.Deserialize<List<FilamentColor>>(content);
                case var id when id == Guid.Parse(VendorDefn.VendorDefnIdentifier): // ExampleType2
                    return JsonSerializer.Deserialize<List<VendorDefn>>(content);
                case var id when id == Guid.Parse(FilamentDefn.FilamentDefnIdentifier): // ExampleType3
                    return JsonSerializer.Deserialize<List<FilamentDefn>>(content);
                default:
                    throw new NotImplementedException($"Array deserialization not implemented for ExportID: {ExportID}");
            }
        }
        return null;
    }
    private Type GetExportedType()
    {
        var sharables = Assembly.GetCallingAssembly().GetTypes().Where(x => x.GetInterfaces().Contains(typeof(ISharingSupport)));
        return sharables.FirstOrDefault(x => x.GetField("identifier", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null)?.Equals(ExportID) ?? false);
    }
}
public class ImportPackage : Observable
{
    public Guid ExportID
    {
        get; set;
    }
    public bool IsArray
    {
        get; set;
    }

    public string Content
    {
        get; set;
    }
    private string _about;
    public string About
    {
        get => _about;
        set => Set(ref _about, value);
    }

}