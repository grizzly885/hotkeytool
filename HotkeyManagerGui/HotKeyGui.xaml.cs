using hotkeyManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace HotkeyManagerGui
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class HotKeyGui : Window
    {
        public HotKeyGui()
        {
            InitializeComponent();

            Process p = new Process();
            p.StartInfo.FileName = @"C:\Program Files (x86)\Notepad++\notepad++.exe";
            //Vista or higher check
            if (Environment.OSVersion.Version.Major >= 6)
            {
                p.StartInfo.Verb = "runas";
            }

            var hotkeyManager = HotKeyManager.Instance;
            hotkeyManager.RegisterHotKey(Keys.N, KeyModifiers.Alt | KeyModifiers.Control,
              () =>
              {
                  p.Start();
                  Console.WriteLine("Zweite Aktion");
              });
        }
    }
}
