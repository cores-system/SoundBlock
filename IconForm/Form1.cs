using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace IconForm
{
    public partial class Form1 : Form
    {
        static NotifyIcon ico;
        static globalKeyboardHook gkh = new globalKeyboardHook();

        public Form1()
        {
            InitializeComponent();
            Start();
        }

        Process cli;

        public void Start()
        {
            //
            cli = Process.Start("SoundBlock.exe");

            // Иконка
            ico = new NotifyIcon();
            ico.Icon = Properties.Resources.sound;
            ico.Visible = true;

            //Обрабатываем события двойной клик по иконке
            ico.DoubleClick += delegate(object sender, EventArgs args) 
            {
                Process.Start("http://localhost:5000/");
            };
            
            //Контекстное меню
            CreateContextMenu();

            //
            gkh.KeyUp += new KeyEventHandler(gkh_KeyUp);
        }


        private void CreateContextMenu()
        {
            ico.ContextMenu = new ContextMenu(new[] 
            {
                new MenuItem("Настройки", delegate
                {
                    Process.Start("http://localhost:5000/");
                }),

                new MenuItem("Выход", delegate
                {
                    cli.Kill();
                    cli.Dispose();
                    this.Close();
                })
             });
        }


        private void gkh_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.VolumeDown || e.KeyCode == Keys.VolumeUp)
            {
                try
                {
                    var web = new WebClient();
                    web.DownloadString("http://localhost:5000/api/UpdateLevel?code=" + (e.KeyCode == Keys.VolumeDown ? "DOWN" : "UP"));
                    web.Dispose();
                }
                catch { }
            }
        }
    }
}
