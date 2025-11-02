using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDefinitions.Models;

namespace ExportSelect.Helpers;

class CodeGenerator
{
    internal static string GenerateColorCode(List<FilamentColor> selectedColors)
    {
        var sb = new StringBuilder();
        sb.AppendLine("// Generated Filament Colors");
        sb.AppendLine("var filamentColors = new List<FilamentColor>");
        sb.AppendLine("{");
        for(int i = 0; i < selectedColors.Count; i++)
        {
            var color = selectedColors[i];
            var lineEnding = (i == selectedColors.Count - 1) ? "" : ",";
            sb.AppendLine($"    {color.ToString("CG")}{lineEnding}");
        }
        sb.AppendLine("};");
        return sb.ToString();
    }
}
