// See https://aka.ms/new-console-template for more information
// This project is to test the dataserialization for the Json Implementation of DataDefinitions
// it is also used to test all the 'logic' in the datafile 
using DataDefinitions.JsonSupport;
using JsonDatafileTest;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

var jsonFile = new JsonFilamentDocument();
jsonFile.SeedDocument();
var vendorCount = jsonFile.VendorDefns.Count;
var options = new JsonSerializerOptions();
options.WriteIndented = true;
var fileContents=JsonSerializer.Serialize(jsonFile);
Console.WriteLine(fileContents);
Console.WriteLine($"Json File size (approximate){fileContents.Length} bytes.");
jsonFile.SaveFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"Filament", "TestFilamentData.Json"));

var result = JsonFilamentDocument.LoadFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Filament", "TestFilamentData.Json"));
if (result.VendorDefns.Count == vendorCount)
    Console.WriteLine("Able to restore the data from a JSON File");

InsertTestItems.InsertTestObjects(result,vendorCount);
//var streamReader = new StreamReader(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Filament", "TestFilamentData.xml"));
//var xmlReader=new XmlTextReader(streamReader);
//var xmlResults=serializer.Deserialize(xmlReader);
//if (xmlResults is JsonFilamentDocument document)
//    if (document.VendorDefns.Count == vendorCount)
//        Console.WriteLine("Able to restore the data from an xml file");
