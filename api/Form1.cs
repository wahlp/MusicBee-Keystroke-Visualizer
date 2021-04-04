using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Gma.System.MouseKeyHook;

namespace MusicBeePlugin
{
    public partial class Form1 : Form
    {
        private Label label1;
        private Label label2;
        private System.Windows.Forms.Timer timer1;
        private int counter = 12;

        private IKeyboardMouseEvents m_GlobalHook;

        public void Subscribe()
        {
            m_GlobalHook = Hook.AppEvents();
            
            m_GlobalHook.KeyPress += GlobalHookKeyPress;
        }

        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        {
            //Debug.WriteLine("KeyPress: \t{0}", e.KeyChar);
            //var key = e.KeyChar.ToString();

            // update text
            label1.Visible = true;
            label1.Text += e.KeyChar;
            // reset countdown timer
            counter = 12;
            timer1.Start();
        }

        public void Unsubscribe()
        {
            m_GlobalHook.KeyPress -= GlobalHookKeyPress;

            //It is recommened to dispose it
            m_GlobalHook.Dispose();
        }
        
        [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
            
        public Form1()
        {
            this.ShowInTaskbar = false;

            // set window to stay on top of only MusicBee
            var mbWindow = Process.GetProcessesByName("MusicBee").FirstOrDefault();
            if (mbWindow != null)
            {
                var owner = mbWindow.MainWindowHandle;
                var owned = this.Handle;
                var i = SetWindowLong(owned, -8 /*GWL_HWNDPARENT*/, owner);
            }
            this.KeyPreview = true;


            InitializeComponent();

            Debug.WriteLine("Debug.WriteLine test");
            Subscribe();

            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 100; // 0.1s
        }

        // hide from taskbar
        protected override CreateParams CreateParams
        {
            get
            {
                var Params = base.CreateParams;
                Params.ExStyle |= 0x80;

                return Params;
            }
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(123)))), ((int)(((byte)(46)))));
            this.label1.Location = new System.Drawing.Point(636, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(441, 51);
            this.label1.TabIndex = 0;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Black;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(123)))), ((int)(((byte)(46)))));
            this.label2.Location = new System.Drawing.Point(790, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 26);
            this.label2.TabIndex = 1;
            this.label2.Text = "Plugin is live";
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1200, 468);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "Form1";
            this.TransparencyKey = System.Drawing.Color.White;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        //private void keydownEvent(object sender, KeyEventArgs e)
        //{
        //    //var key = e.KeyCode.ToString();
        //    //// fix D0, D1, etc.
        //    //if (key.Length == 2 && key.Substring(0, 1) == "D") {
        //    //    key = key.Substring(1, 1);
        //    //}
            
        //    //if (key.Length == 1) {
        //    //    // update text
        //    //    label1.Visible = true;
        //    //    label1.Text += key;
        //    //    // set countdown timer
        //    //    counter = 3;
        //    //    timer1.Start();
        //    //}
        //}

        private void timer1_Tick(object sender, EventArgs e)
        {
            counter--;
            if (counter == 0)
            {
                // countdown finished
                timer1.Stop();
                // reset text and hide
                label1.Text = "";
                label1.Visible = false;
            }
        }
    }
}
