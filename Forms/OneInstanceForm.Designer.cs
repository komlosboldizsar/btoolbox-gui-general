namespace BToolbox.GUI.Forms
{
    partial class OneInstanceForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            trayIcon = new NotifyIcon(components);
            trayMenu = new ContextMenuStrip(components);
            trayMenuExit = new ToolStripMenuItem();
            trayMenu.SuspendLayout();
            SuspendLayout();
            // 
            // trayIcon
            // 
            trayIcon.ContextMenuStrip = trayMenu;
            trayIcon.Text = "trayIcon";
            trayIcon.Visible = true;
            trayIcon.DoubleClick += trayIcon_DoubleClick;
            // 
            // trayMenu
            // 
            trayMenu.ImageScalingSize = new Size(20, 20);
            trayMenu.Items.AddRange(new ToolStripItem[] { trayMenuExit });
            trayMenu.Name = "contextMenuStrip1";
            trayMenu.Size = new Size(211, 56);
            // 
            // trayMenuExit
            // 
            trayMenuExit.Name = "trayMenuExit";
            trayMenuExit.Size = new Size(210, 24);
            trayMenuExit.Text = "Exit";
            trayMenuExit.Click += trayMenuExit_Click;
            // 
            // OneInstanceForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Name = "OneInstanceForm";
            Text = "OneInstanceForm";
            trayMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        protected NotifyIcon trayIcon;
        protected ContextMenuStrip trayMenu;
        protected ToolStripMenuItem trayMenuExit;
    }
}