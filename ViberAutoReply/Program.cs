using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViberAutoReply
{
    class Form1 : Form
    {

        Thread t;

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new Form1());
        }

        public Form1()
        {
            InitializeComponent();
        }

        // Get a handle to an application window.
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName,
            string lpWindowName);

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, StringBuilder lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessageTimeout", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint SendMessageTimeoutText(IntPtr hWnd, int Msg, int countOfChars, StringBuilder text, uint flags, uint uTImeoutj, uint result);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern bool SetCursorPos(uint x, uint y);
 
        [DllImport("user32.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, UIntPtr dwExtraInfo);

        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        private Label label1;

        private const int LB_GETCOUNT = 0x018B;
        private const int LB_GETTEXT = 0x0189;
        private const uint MOUSEEVENTF_MOVE      = 0x0001;
        private const uint MOUSEEVENTF_LEFTDOWN  = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP    = 0x0004;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP   = 0x0010;
        private NotifyIcon notifyIcon1;
        private System.ComponentModel.IContainer components;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem showToolStripMenuItem;
        private ToolStripMenuItem closeTrayToolStripMenuItem;
        private TextBox textBox2;
        private NumericUpDown numericUpDown1;
        private Button button1;
        private Label label2;
        private Label label3;
        private ErrorProvider errorProvider1;
        private const uint MOUSEEVENTF_ABSOLUTE = 0x8000;


        // Send a key to the button when the user double-clicks anywhere 
        // on the form.
        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            // Send the enter key to the button, which raises the click 
            // event for the button. This works because the tab stop of 
            // the button is 0.
        }

        public static List<string> GetListBoxContents(IntPtr listBoxHwnd)
        {
            int cnt = (int)SendMessage(listBoxHwnd, LB_GETCOUNT, IntPtr.Zero, null);
            List<string> listBoxContent = new List<string>();
            for (int i = 0; i < cnt; i++)
            {
                StringBuilder sb = new StringBuilder(256);
                IntPtr getText = SendMessage(listBoxHwnd, LB_GETTEXT, (IntPtr)i, sb);
                listBoxContent.Add(sb.ToString());
            }

            return listBoxContent;
        }

        public static string GetText(IntPtr hwnd)
        {
            var text = new StringBuilder(1024);
            if (SendMessageTimeoutText(hwnd, 0xd, 1024, text, 0x2, 1000, 0) != 0)
            {
                return text.ToString();
            }

            return "";
        }  

        private void performClick(uint x, uint y)
        {
            SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN, x, y, 0, UIntPtr.Zero);
            Thread.Sleep(200);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTUP, x, y, 0, UIntPtr.Zero);
        }

        private void performRightClick(uint x, uint y) 
        {
            SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_RIGHTDOWN, x, y, 0, UIntPtr.Zero);
            Thread.Sleep(200);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_RIGHTUP, x, y, 0, UIntPtr.Zero);

        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeTrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Auto Reply Bot";
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.closeTrayToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(127, 48);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.showToolStripMenuItem.Text = "Show";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // closeTrayToolStripMenuItem
            // 
            this.closeTrayToolStripMenuItem.Name = "closeTrayToolStripMenuItem";
            this.closeTrayToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.closeTrayToolStripMenuItem.Text = "Close tray";
            this.closeTrayToolStripMenuItem.Click += new System.EventHandler(this.closeTrayToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "AUTO REPLY STARTED";
            this.label1.Visible = false;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(133, 60);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(120, 20);
            this.textBox2.TabIndex = 1;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(133, 96);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 2;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(76, 147);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Your auto message:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Idle after (minutes):";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(276, 185);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipTitle = "Minimize to Tray App";
            notifyIcon1.BalloonTipText = "You have successfully minimized your form.";

            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(500);
                this.Hide();
            }

            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void closeTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        internal struct LASTINPUTINFO
        {
            public uint cbSize;

            public uint dwTime;
        }

        public static uint GetIdleTime()
        {
            LASTINPUTINFO lastInPut = new LASTINPUTINFO();
            lastInPut.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(lastInPut);
            GetLastInputInfo(ref lastInPut);

            return ((uint)Environment.TickCount - lastInPut.dwTime);
        }

        string lastMessage = "";
        bool run = true;

        void IdleTime(string text,int minutes) 
        {
            try
            {
                RunAsSTAThread(
                () =>
                {
                    while (run)
                    {
                        uint ticks = GetIdleTime();
                        TimeSpan timespent = TimeSpan.FromMilliseconds(ticks);

                        TimeSpan span3 = TimeSpan.FromMinutes(minutes);

                        if (timespent >= span3)
                        {

                            // Get a handle to the Calculator application. The window class
                            // and window name were obtained using the Spy++ tool.
                            Clipboard.Clear();
                            IntPtr viberHandle = FindWindow((string)null, "Viber +38761328795");

                            // Verify that Calculator is a running process.
                            if (viberHandle == IntPtr.Zero)
                            {
                                MessageBox.Show("Viber is not running.");
                                return;
                            }

                            SetForegroundWindow(viberHandle);

                            performClick(150, 300);

                            performRightClick(500, 570);
                            Thread.Sleep(200);
                            performClick(560, 580);

                            string newMessage = "";

                            newMessage = Clipboard.GetText();

                            if (newMessage == "")
                            {

                                performRightClick(500, 550);
                                Thread.Sleep(200);
                                performClick(560, 560);

                                newMessage = Clipboard.GetText();
                            }

                            if ((lastMessage == newMessage && lastMessage!="") || (newMessage == ""))
                            {
                                Debug.Write("\nLast Message: " + lastMessage);
                                Debug.Write("\nNew Message: " + newMessage);
                            }
                            else
                            {
                                lastMessage = newMessage;
                                Debug.Write("\nLast Message: " + lastMessage);
                                Debug.Write("\nNew Message: " + newMessage);
                                SetForegroundWindow(viberHandle);
                                if (text == "Anes")
                                {
                                    SendKeys.SendWait("[BOT]Auto Reply Message: Nisam trenutno tu.");
                                }
                                else 
                                {
                                    SendKeys.SendWait("[BOT]Auto Reply Message: " + text);
                                }
                                Thread.Sleep(500);
                                SendKeys.SendWait("{ENTER}");
                            }

                            //Widgets.WidgetsMain();
                            //SendKeys.SendWait("Auto Reply Message testiram nesto!");
                            //SendKeys.Send("{ENTER}");
                            /*IntPtr value = new IntPtr(0x1002D8); //ID locate using Spy++
                            var caption = GetText(value);*/

                            //MessageBox.Show(caption.ToString());

                        }
                        else
                        {
                            lastMessage = "";
                        }
                    }
                });

            }
            catch (Exception e)
            {
                Debug.Write("Error u IdleTime: " + e.Message);
                
            }
        
        }

        static void RunAsSTAThread(Action goForIt)
        {
            AutoResetEvent @event = new AutoResetEvent(false);
            Thread thread = new Thread(
                () =>
                {
                    goForIt();
                    @event.Set();
                });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            @event.WaitOne();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
                notifyIcon1.Dispose();
                run = false;
                Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Stop") 
            {
                t.Abort();
                button1.Text = "Start";
                textBox2.Enabled = true;
                numericUpDown1.Enabled = true;
                label1.Visible = false;

                return;
            }

            if (textBox2.Text == "") 
            {
                errorProvider1.SetError(textBox2, "Please enter auto reply message!");
            }
            else{
                errorProvider1.SetError(textBox2, "");
                button1.Text = "Stop";
                textBox2.Enabled = false;
                numericUpDown1.Enabled = false;
                label1.Visible = true;

            try
            {
                t = new Thread(new ThreadStart(() => IdleTime(textBox2.Text.ToString(), Convert.ToInt32(numericUpDown1.Value))));
                t.Start();
            }
            catch (Exception ex)
            {
                t.Abort();
                Debug.Write(ex.Message);
            }
            }
        }

    }
}
