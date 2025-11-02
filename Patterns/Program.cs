// See https://aka.ms/new-console-template for more information
using System;
using System.Text.RegularExpressions;
var t="#FFAA32CC";
var pattern = @"(#FF[A-F0-9]{6})";
var matches = Regex.Matches(t, pattern);
Console.WriteLine($"{matches.Count} matches.");
