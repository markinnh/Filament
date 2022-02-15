using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filament.UWP.Helpers
{
    public static class StringExtensions
    {
        public static string MakeProper(this string target)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool makeNextUpper = false;
            bool atNextChar = false;
            foreach(char c in target)
            {
                if(makeNextUpper)
                    atNextChar = true;
                if (char.IsWhiteSpace(c)) { 
                    makeNextUpper = true;
                    atNextChar = false;
                }
                stringBuilder.Append(makeNextUpper ? char.ToUpper(c) : c);

                if (atNextChar)
                {
                    makeNextUpper = false;
                    atNextChar= false;
                }
            }
            return stringBuilder.ToString();
        }
    }
}
