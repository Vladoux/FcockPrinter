
namespace fair_mark_desktop
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.materialButton1 = new MaterialSkin.Controls.MaterialButton();
            this.materialProgressBar1 = new MaterialSkin.Controls.MaterialProgressBar();
            this.materialButton3 = new MaterialSkin.Controls.MaterialButton();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.materialSwitch1 = new MaterialSkin.Controls.MaterialSwitch();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "FairCode";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMenuItem,
            this.closeMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(136, 48);
            this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip1_ItemClicked);
            // 
            // openMenuItem
            // 
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.Size = new System.Drawing.Size(135, 22);
            this.openMenuItem.Text = "Развернуть";
            // 
            // closeMenuItem
            // 
            this.closeMenuItem.Name = "closeMenuItem";
            this.closeMenuItem.Size = new System.Drawing.Size(135, 22);
            this.closeMenuItem.Text = "Закрыть";
            // 
            // materialButton1
            // 
            this.materialButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialButton1.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButton1.Depth = 0;
            this.materialButton1.HighEmphasis = true;
            this.materialButton1.Icon = null;
            this.materialButton1.Location = new System.Drawing.Point(410, 170);
            this.materialButton1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialButton1.MinimumSize = new System.Drawing.Size(158, 36);
            this.materialButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButton1.Name = "materialButton1";
            this.materialButton1.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButton1.Size = new System.Drawing.Size(158, 36);
            this.materialButton1.TabIndex = 4;
            this.materialButton1.Text = "Выход";
            this.materialButton1.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButton1.UseAccentColor = false;
            this.materialButton1.UseVisualStyleBackColor = true;
            this.materialButton1.Click += new System.EventHandler(this.materialButton1_Click);
            // 
            // materialProgressBar1
            // 
            this.materialProgressBar1.Depth = 0;
            this.materialProgressBar1.Location = new System.Drawing.Point(6, 135);
            this.materialProgressBar1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialProgressBar1.Name = "materialProgressBar1";
            this.materialProgressBar1.Size = new System.Drawing.Size(562, 5);
            this.materialProgressBar1.TabIndex = 5;
            // 
            // materialButton3
            // 
            this.materialButton3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialButton3.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButton3.Depth = 0;
            this.materialButton3.Enabled = false;
            this.materialButton3.HighEmphasis = true;
            this.materialButton3.Icon = null;
            this.materialButton3.Location = new System.Drawing.Point(6, 170);
            this.materialButton3.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialButton3.MaximumSize = new System.Drawing.Size(200, 36);
            this.materialButton3.MinimumSize = new System.Drawing.Size(158, 36);
            this.materialButton3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButton3.Name = "materialButton3";
            this.materialButton3.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButton3.Size = new System.Drawing.Size(158, 36);
            this.materialButton3.TabIndex = 7;
            this.materialButton3.Text = "Печать";
            this.materialButton3.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButton3.UseAccentColor = false;
            this.materialButton3.UseVisualStyleBackColor = true;
            this.materialButton3.Click += new System.EventHandler(this.materialButton3_Click);
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.Location = new System.Drawing.Point(29, 89);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(65, 19);
            this.materialLabel1.TabIndex = 8;
            this.materialLabel1.Tag = "";
            this.materialLabel1.Text = "¯\\_(ツ)_/¯";
            // 
            // materialSwitch1
            // 
            this.materialSwitch1.AutoSize = true;
            this.materialSwitch1.Depth = 0;
            this.materialSwitch1.Location = new System.Drawing.Point(168, 169);
            this.materialSwitch1.Margin = new System.Windows.Forms.Padding(0);
            this.materialSwitch1.MouseLocation = new System.Drawing.Point(-1, -1);
            this.materialSwitch1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialSwitch1.Name = "materialSwitch1";
            this.materialSwitch1.Ripple = true;
            this.materialSwitch1.Size = new System.Drawing.Size(238, 37);
            this.materialSwitch1.TabIndex = 9;
            this.materialSwitch1.Text = "Принтер по умолчанию ";
            this.materialSwitch1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 223);
            this.Controls.Add(this.materialSwitch1);
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.materialButton3);
            this.Controls.Add(this.materialProgressBar1);
            this.Controls.Add(this.materialButton1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "FairCode";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeMenuItem;
        private MaterialSkin.Controls.MaterialButton materialButton1;
        private MaterialSkin.Controls.MaterialProgressBar materialProgressBar1;
        private MaterialSkin.Controls.MaterialButton materialButton3;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialSwitch materialSwitch1;
    }
}

