using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AutoClick
{
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        public void LeftMouseClick(int xpos, int ypos)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }

        private bool doClick = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void threadClick_DoWork(object sender, DoWorkEventArgs e)
        {
            while (doClick)
            {
                LeftMouseClick(Cursor.Position.X, Cursor.Position.Y);
                Thread.Sleep(100);
                e.Cancel = true;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F2)
            {
                doClick = !doClick;
                if (doClick)
                {
                    threadClick.RunWorkerAsync();
                    label2.Text = "Started";
                    label2.ForeColor = Color.Green;
                }
                else
                {
                    while (threadClick.IsBusy)
                    {
                        label2.Text = "Stopping";
                        label2.ForeColor = Color.Yellow;
                        Application.DoEvents();
                        continue;
                    }

                    label2.Text = "Stopped";
                    label2.ForeColor = Color.Red;

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
