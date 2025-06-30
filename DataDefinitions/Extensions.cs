using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions;
public static partial class Extensions
{
    public static string DoQuote(this string target, char quoteChar = '"') => $"{quoteChar}{target}{quoteChar}";
    public static void AddRange<T>(this ObservableCollection<T> target, params T[] items)
    {
        foreach (var item in items)
            target.Add(item);
    }
}
