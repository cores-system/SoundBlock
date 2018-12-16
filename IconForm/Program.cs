using System;
using System.Threading;
using System.Windows.Forms;

namespace IconForm
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            bool createdNew;
            Mutex M = new Mutex(true, "IconForm", out createdNew);
            if (!createdNew)
                return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
