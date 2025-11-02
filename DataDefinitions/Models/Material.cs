using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using LiteDB;

namespace DataDefinitions.Models;
// TODO: complete the conversion to Material class from MaterialType enum
// TODO: implement IFormattable to allow code generation from definitions in this class.
public class Material : DatabaseObject, IFormattable
{
    public static Material Default = new()
    {
        Name = "Polylactic Acid",
        ShortName = "PLA"
        
    };

    private static bool inDataOps;
    public static bool InDataOps
    {
        get => inDataOps;
        set => inDataOps = value;
    }
    [JsonIgnore, BsonIgnore]
    public override bool InDataOperations => InDataOps;
    [JsonIgnore, BsonIgnore]
    public override bool CanEdit => base.CanEdit;
    [JsonIgnore, BsonIgnore]
    public override bool InDatabase => MaterialId != default;
    [JsonIgnore, BsonIgnore]
    public override bool IsValid => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(ShortName);
    [JsonIgnore]
    public int MaterialId
    {
        get;
        set;
    }
    private string name;
    [BsonField("Nm")]
    public string Name
    {
        get => name;
        set => Set(ref name, value);
    }

    private string shorthandName;
    [BsonField("ShrtNm")]
    public string ShortName
    {
        get => shorthandName;
        set => Set(ref shorthandName, value);
    }
    private ObservableCollection<Descriptor> enhancements;
    [BsonField("Enhance"),JsonPropertyName("Enhance")]
    public ObservableCollection<Descriptor> Enhancements
    {
        get => enhancements;
        set
        {
            if(Set(ref enhancements, value))
                Enhancements.CollectionChanged += Enhancements_CollectionChanged;
        }
    }


    [BsonCtor]
    public Material(int materialId, string name, string shortName, ObservableCollection<Descriptor> enhancements)
    {
        Debug.WriteLine("Material BsonCtor called");
        MaterialId = materialId;
        Name = name;
        ShortName = shortName;
        Enhancements = enhancements;
        //Enhancements.CollectionChanged += Enhancements_CollectionChanged;
    }

    private void Enhancements_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        Debug.WriteLine($"Enhancements collection changed Action {e.Action}");
        IsModified = true;
    }

    public Material()
    {
        Enhancements = new ObservableCollection<Descriptor>();
    }
    public override string ToString() => ToString("N",null);
    public string ToString(string format, IFormatProvider formatProvider)
    {
        var specifier = string.IsNullOrEmpty(format) ? "N" : format;
        //if (string.IsNullOrEmpty(format))
        //    specifier = "N";
        //else
        //    specifier = format;

        switch (specifier)
        {
            case "N":
                var enhString = $", Enhancements {Enhancements.Count}";
                return $"{Name} - ({ShortName}){(Enhancements.Count > 0 ? enhString : string.Empty)}";
            case "R":
                var enhString1 = $", Enhancements {Enhancements.Count}";
                return $"{ShortName} - ({Name}){(Enhancements.Count > 0 ? enhString1 : string.Empty)}";
            case "F":
                StringBuilder sb = new StringBuilder();
                sb.Append($"{Name} - ({ShortName})");
                if (enhancements.Count > 0)
                {
                    sb.Append(" - (");
                    for (var i = 0; i < enhancements.Count; i++)
                    {
                        sb.Append(enhancements[i]);
                        sb.Append(i + 1 == enhancements.Count ? ")" : ", ");
                    }
                }
                return sb.ToString();
            case "S":
                return $"{ShortName}";
            case "B":
                return $"{Name} - ({ShortName})";
            case "CG":
                StringBuilder builder = new StringBuilder();
                builder.Append($"new (){{ Name = {Name.DoQuote()}, ShortName = {ShortName.DoQuote()}");
                if (Enhancements.Count > 0)
                {
                    builder.Append(", Enhancements = new ObservableCollection<Descriptor>(new Descriptor[]{");
                    for (var i = 0; i < Enhancements.Count; i++)
                    {
                        builder.Append(Enhancements[i].ToString(specifier, formatProvider));
                        builder.Append(i + 1 == Enhancements.Count ? "})" : ", ");
                    }
                }
                builder.Append("}");
                return builder.ToString();
            default:
                throw new NotSupportedException($"the format specifier '{specifier}' is not supported") ;

        }
        //throw new NotImplementedException();
    }
}
public class Descriptor : DataObject, IFormattable
{
    private string name;

    public string Name
    {
        get => name;
        set => Set(ref name, value);
    }
    //private string description;
    //[BsonField("Desc")]
    //public string Description
    //{
    //    get => description;
    //    set => Set(ref description, value);
    //}
    private bool mmsCompatible;
    [BsonField("MMSComp"),JsonPropertyName("MMSComp")]
    public bool MMSCompatible
    {
        get => mmsCompatible;
        set => Set(ref mmsCompatible, value);
    }
    private bool abrasive;
    [BsonField("Abras"), JsonPropertyName("Abras")]
    public bool Abrasive
    {
        get => abrasive;
        set => Set(ref abrasive, value);
    }

    public Descriptor()
    {
    }
    [BsonCtor]
    public Descriptor(string name, bool mmsCompatible,bool abrasive)
    {
        Name = name;
        MMSCompatible=mmsCompatible;
        Abrasive = abrasive;
    }
    public string ToString(string format, IFormatProvider formatProvider)
    {
        var specifier = string.IsNullOrEmpty(format) ? "N" : format;

        switch (specifier)
        {
            case "N":
                return $"{Name}";
            case "F":
                return $"{Name} - Comp : ({MMSCompatible})";
            case "B":
                return Name;
            case "CG":
                return $"new Descriptor({Name.DoQuote()},{MMSCompatible});";
            default:
                throw new NotSupportedException($"the format specifier '{specifier}' is not supported") ;

        }
    }
}