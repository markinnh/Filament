using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions;
[Flags]
public enum AppSetupSteps:short
{
    None = 0,
    AcceptTermsOfUse=0x01,
    SetupDataDirectory=0x02,
    SetupDataFile=0x04
}
public class AppSetupProgress
{
}
