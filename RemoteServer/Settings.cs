using System;
using System.Windows.Forms;
using System.Drawing.Imaging ;
using System.Text;
using Microsoft.Win32;

namespace RemoteServer {
    public class Settings
    {//This class is just used to share setting and to then save them to the registry
        public static bool FirstTime = true;
        public static string MainProgramName = "RNC Remote desktop server";
        public static string PassWord = "letmein";
        public static FrmService FormService = null;
        public static int Port = 4000;
        public static bool Debug = false;
        public static int ScreenServerX = 1920;
        public static int ScreenServerY = 1080;
    }
}
