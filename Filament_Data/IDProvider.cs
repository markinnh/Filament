using System;
using System.Collections.Generic;
using System.Text;

namespace Filament_Data
{
    public class IDProvider
    {
        public static string FriendlyID()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 16);
        } 
    }
}
