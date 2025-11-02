using System;

string[] colors = { "Red", "White", "Blue" };
Console.WriteLine(colors[DateTime.Today.DayOfYear % colors.Length]);