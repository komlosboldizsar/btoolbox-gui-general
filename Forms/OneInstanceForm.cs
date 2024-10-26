using BToolbox.GUI.Helpers;
using BToolbox.OneInstance;
using System.ComponentModel;

namespace BToolbox.GUI.Forms
{
    public partial class OneInstanceForm : Form
    {

        public OneInstanceForm() => InitializeComponent();

        public OneInstanceForm(bool oneInstanceMode, bool hideOnStartup)
        {
            InitializeComponent();
            _oneInstanceMode = oneInstanceMode;
            _hideOnStartup = hideOnStartup;
            lastWindowStateNotMinimized = WindowState;
            if (oneInstanceMode)
            {
                trayIcon.Visible = true;
                OneInstanceGuard.ShowMessageReceived += OneInstanceShowMessageReceived;
            }
            if (hideOnStartup)
            {
                _hidingOnStartup = true;
                ShowInTaskbar = false;
                Opacity = 0;
            }
        }

        #region One instance mode
        private readonly bool _oneInstanceMode;
        private readonly bool _hideOnStartup;
        private bool _hidingOnStartup;

        protected override bool ShowWithoutActivation => _hidingOnStartup;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (_hideOnStartup)
            {
                Hide();
                Opacity = 1;
                ShowInTaskbar = true;
                _hidingOnStartup = false;
            }
        }

        private void OneInstanceShowMessageReceived()
            => this.InvokeIfRequired(() =>
            {
                Show();
                WindowState = lastWindowStateNotMinimized;
                Activate();
            });
        #endregion

        #region Closing, minimizing, restoring
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (_oneInstanceMode && !closingFromTrayMenu)
            {
                e.Cancel = true;
                Hide();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (WindowState != FormWindowState.Minimized)
                lastWindowStateNotMinimized = WindowState;
        }

        FormWindowState lastWindowStateNotMinimized;
        #endregion

        #region Tray
        private void trayIcon_DoubleClick(object sender, EventArgs e)
            => Show();

        private void trayMenuExit_Click(object sender, EventArgs e)
        {
            closingFromTrayMenu = true;
            Close();
        }

        bool closingFromTrayMenu = false;
        #endregion


    }
}