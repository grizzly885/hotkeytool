using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using hotkeyManager;

namespace HotkeySystemTray
{
    public static class HotKeySystemTray
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            //var hotkeyManager = HotKeyManager.Instance;
            //GC.KeepAlive(hotkeyManager);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SystemTrayMenu());
        }
    }
}
