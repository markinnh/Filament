// See https://aka.ms/new-console-template for more information
using DataDefinitions;

var test = DataDefinitions.Models.Material.Default;

//test.Enhancements.AddRange("Matte", "Aero","Tough","Plus","Galaxy");

Console.WriteLine("{0:CG}", test);
var cTest =  DataDefinitions.Models.FilamentColor.Default;
Console.WriteLine("{0:N}", cTest);
Console.WriteLine("{0:S}", cTest);
Console.WriteLine("{0:F}", cTest);
Console.WriteLine("{0:CG}", cTest);
