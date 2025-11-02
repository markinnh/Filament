using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Filament.WPF6.Helpers;
public class RegexValidationRule:ValidationRule
{
    public string Pattern { get; set; } = string.Empty;
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is string strValue && !string.IsNullOrEmpty(Pattern))
        {
            if (Regex.IsMatch(strValue, Pattern))
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, $"The input does not match the pattern: {Pattern}");
            }
        }
        return ValidationResult.ValidResult;
    }
}

