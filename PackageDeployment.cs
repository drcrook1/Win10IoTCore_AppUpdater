using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Core;

namespace UpdateApp
{
    public sealed class PackageDeployment
    {
        public PackageDeployment()
        {
        }
    }
}


//catch (UnauthorizedAccessException uex)
//{
//    StdErrorText += "Exception Thrown: " + uex.Message + "\n";
//    StdErrorText += "\nMake sure you're allowed to run the specified exe; either\n" +
//                         "\t1) Add the exe to the AppX package, or\n" +
//                         "\t2) Add the absolute path of the exe to the allow list:\n" +
//                         "\t\tHKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\EmbeddedMode\\ProcessLauncherAllowedExecutableFilesList.\n\n" +
//                         "Also, make sure the <iot:Capability Name=\"systemManagement\" /> has been added to the AppX manifest capabilities.\n";
//}
//catch (Exception ex)
//{
//    StdErrorText += "Exception Thrown:" + ex.Message + "\n";
//    StdErrorText += ex.StackTrace + "\n";
//}