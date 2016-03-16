using System;
using System.Drawing;
using System.Windows.Forms;
namespace HotkeySystemTray
{
    public class SystemTrayMenu : Form
    {
        private NotifyIcon _notifyIcon;
        private ContextMenu _contextMenu;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (_notifyIcon != null))
            {
                _notifyIcon.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            _contextMenu = new ContextMenu();
            AddMenuItems(_contextMenu);

            _notifyIcon = new NotifyIcon();
            _notifyIcon.Text = "HotKeyManager";
            _notifyIcon.Icon = new Icon(SystemIcons.Application, 40, 40);

            _notifyIcon.ContextMenu = _contextMenu;
            _notifyIcon.Visible = true;
        }

        private void AddMenuItems(System.Windows.Forms.ContextMenu _contextMenu)
        {
            _contextMenu.MenuItems.Add("Close", OnExit);
            _contextMenu.MenuItems.Add("Info", OnExit);
        }
        #endregion

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;
            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

