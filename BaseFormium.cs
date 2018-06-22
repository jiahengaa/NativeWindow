using NetDimension.Windows.Imports;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace System.Windows
{
    public class BaseFormium : ModernUIForm
    {
        public const uint WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 61456;
        public const int HTCAPTION = 2;
        [DllImport("User32.DLL")]
        public static extern int SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);
        [DllImport("User32.DLL")]
        public static extern bool ReleaseCapture();

        protected IntPtr FormHandle { get; private set; }
        public BaseFormium()
                : this(null)
        {
            InitializeComponent();
        }

        public BaseFormium(string initialUrl)
        {
            FormHandle = this.Handle;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            FormHandle = this.Handle;

            base.OnHandleCreated(e);

        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        #region Hanlde Browser Messages
        protected override void DefWndProc(ref Message m)
        {
            if (m.Msg == (int)DefMessages.WM_NANUI_DRAG_APP_REGION)
            {

                User32.ReleaseCapture();
                User32.SendMessage(Handle, (uint)WindowsMessages.WM_NCLBUTTONDOWN, (IntPtr)HitTest.HTCAPTION, (IntPtr)0);
            }


            if (m.Msg == (int)DefMessages.WM_NANUI_APP_REGION_RBUTTONDOWN)
            {
                var pt = Win32.GetPostionFromPtr(m.LParam);

                var ptToScr = PointToScreen(new Point(pt.x, pt.y));

                ShowSystemMenu(this, ptToScr);
            }

            base.DefWndProc(ref m);
        }
        #endregion

        protected override void OnShown(EventArgs e)
        {
            ShadowEffect = FormShadowType.GlowShadow;
            base.OnShown(e);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BaseFormium
            // 
            this.ClientSize = new System.Drawing.Size(563, 435);
            this.KeyPreview = true;
            this.Name = "BaseFormium";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BaseFormium_MouseDown);
            this.ResumeLayout(false);

        }

        private void BaseFormium_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_SYSCOMMAND, SC_MOVE | HTCAPTION, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BaseFormium frm = new BaseFormium();
            frm.Show();
        }
    }
}
