using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using hotkeyManager;


namespace HotkeyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Process p = new Process();
            p.StartInfo.FileName = @"C:\Program Files (x86)\Notepad++\notepad++.exe";
            //Vista or higher check
            if (Environment.OSVersion.Version.Major >= 6)
            {
                p.StartInfo.Verb = "runas";
            }
            

           HotKeyManager manager = new HotKeyManager();
           manager.RegisterHotKey(Keys.N, hotkeyManager.KeyModifiers.Alt | hotkeyManager.KeyModifiers.Control,
               () => { 
                         p.Start();
                         Console.WriteLine("Zweite Aktion");
               });
            manager.RegisterHotKey(Keys.S, hotkeyManager.KeyModifiers.Alt | hotkeyManager.KeyModifiers.Windows, () => Console.WriteLine("Hit as"));
            manager.RegisterHotKey(Keys.A, hotkeyManager.KeyModifiers.Alt | hotkeyManager.KeyModifiers.Control | hotkeyManager.KeyModifiers.Windows, () => Console.WriteLine("HIt wsa s"));
            manager.RegisterHotKey(Keys.A, hotkeyManager.KeyModifiers.Alt, () => Console.WriteLine("HIt a"));


            manager.RegisterHotKey(Keys.S, hotkeyManager.KeyModifiers.Alt | hotkeyManager.KeyModifiers.Windows, () => Console.WriteLine("Hit as"));


            Console.ReadLine();   
        }

        private static void HotKeyManager_HotKeyPressed(object sender, HotKeyEventArgs e)
        {
            Console.WriteLine("Hit me!");
        }

        static void HotKeyManager_HotKeyPressed()
        {
            Console.WriteLine("Hit me!");
        }
    }
}
