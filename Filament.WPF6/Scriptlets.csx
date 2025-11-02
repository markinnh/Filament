using System;

string[] colors = { "Red", "White", "Blue" };
Console.WriteLine(colors[DateTime.Today.DayOfYear % colors.Length]);

// Create Guid definition
#r "PresentationCore.dll"
System.Windows.Clipboard.SetText($"protected static readonly Guid jsonIdentifer = Guid.Parse(\"{Guid.NewGuid()}\");");

// create Guid string definition
#r "PresentationCore.dll"
System.Windows.Clipboard.SetText($"internal const string jsonIdentifier = \"{Guid.NewGuid()}\";\ninternal static Guid JsonIdentifer {{get;set;}} = new Guid(jsonIdentifier);");
// Demo output
//internal const string jsonIdentifier = "161a6acc-f3c5-4daa-90db-ec2a138903fe";
//internal static Guid JsonIdentifer { get; set; } = new Guid(jsonIdentifier);
