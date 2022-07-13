
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
            this.defaultPrinterSwitch = new MaterialSkin.Controls.MaterialSwitch();
            this.materialCheckedListBox1 = new MaterialSkin.Controls.MaterialCheckedListBox();
            this.materialCheckbox1 = new MaterialSkin.Controls.MaterialCheckbox();
            this.autoPrintSwitch = new MaterialSkin.Controls.MaterialSwitch();
            this.selectAllSwitch = new MaterialSkin.Controls.MaterialSwitch();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.materialButton2 = new MaterialSkin.Controls.MaterialButton();
            this.materialButton4 = new MaterialSkin.Controls.MaterialButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.contextMenuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            resources.ApplyResources(this.notifyIcon1, "notifyIcon1");
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMenuItem,
            this.closeMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip1_ItemClicked);
            // 
            // openMenuItem
            // 
            this.openMenuItem.Name = "openMenuItem";
            resources.ApplyResources(this.openMenuItem, "openMenuItem");
            // 
            // closeMenuItem
            // 
            this.closeMenuItem.Name = "closeMenuItem";
            resources.ApplyResources(this.closeMenuItem, "closeMenuItem");
            // 
            // materialButton1
            // 
            resources.ApplyResources(this.materialButton1, "materialButton1");
            this.tableLayoutPanel1.SetColumnSpan(this.materialButton1, 2);
            this.materialButton1.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButton1.Depth = 0;
            this.materialButton1.HighEmphasis = true;
            this.materialButton1.Icon = null;
            this.materialButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButton1.Name = "materialButton1";
            this.materialButton1.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButton1.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.materialButton1.UseAccentColor = false;
            this.materialButton1.UseVisualStyleBackColor = true;
            this.materialButton1.Click += new System.EventHandler(this.materialButton1_Click);
            // 
            // materialProgressBar1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.materialProgressBar1, 3);
            this.materialProgressBar1.Depth = 0;
            resources.ApplyResources(this.materialProgressBar1, "materialProgressBar1");
            this.materialProgressBar1.ForeColor = System.Drawing.Color.Lime;
            this.materialProgressBar1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialProgressBar1.Name = "materialProgressBar1";
            this.materialProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // materialButton3
            // 
            resources.ApplyResources(this.materialButton3, "materialButton3");
            this.materialButton3.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButton3.Depth = 0;
            this.materialButton3.HighEmphasis = true;
            this.materialButton3.Icon = null;
            this.materialButton3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButton3.Name = "materialButton3";
            this.materialButton3.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButton3.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButton3.UseAccentColor = false;
            this.materialButton3.UseVisualStyleBackColor = true;
            this.materialButton3.Click += new System.EventHandler(this.materialButton3_Click);
            // 
            // defaultPrinterSwitch
            // 
            resources.ApplyResources(this.defaultPrinterSwitch, "defaultPrinterSwitch");
            this.defaultPrinterSwitch.Depth = 0;
            this.defaultPrinterSwitch.MouseLocation = new System.Drawing.Point(-1, -1);
            this.defaultPrinterSwitch.MouseState = MaterialSkin.MouseState.HOVER;
            this.defaultPrinterSwitch.Name = "defaultPrinterSwitch";
            this.defaultPrinterSwitch.Ripple = true;
            this.defaultPrinterSwitch.UseVisualStyleBackColor = true;
            this.defaultPrinterSwitch.CheckedChanged += new System.EventHandler(this.defaultPrinterSwitch_CheckedChanged);
            // 
            // materialCheckedListBox1
            // 
            resources.ApplyResources(this.materialCheckedListBox1, "materialCheckedListBox1");
            this.materialCheckedListBox1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.SetColumnSpan(this.materialCheckedListBox1, 3);
            this.materialCheckedListBox1.Depth = 0;
            this.materialCheckedListBox1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCheckedListBox1.Name = "materialCheckedListBox1";
            this.materialCheckedListBox1.Striped = false;
            this.materialCheckedListBox1.StripeDarkColor = System.Drawing.Color.Empty;
            // 
            // materialCheckbox1
            // 
            this.materialCheckbox1.Depth = 0;
            resources.ApplyResources(this.materialCheckbox1, "materialCheckbox1");
            this.materialCheckbox1.MouseLocation = new System.Drawing.Point(-1, -1);
            this.materialCheckbox1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCheckbox1.Name = "materialCheckbox1";
            this.materialCheckbox1.ReadOnly = false;
            this.materialCheckbox1.Ripple = true;
            this.materialCheckbox1.UseVisualStyleBackColor = true;
            // 
            // autoPrintSwitch
            // 
            resources.ApplyResources(this.autoPrintSwitch, "autoPrintSwitch");
            this.autoPrintSwitch.Depth = 0;
            this.autoPrintSwitch.MouseLocation = new System.Drawing.Point(-1, -1);
            this.autoPrintSwitch.MouseState = MaterialSkin.MouseState.HOVER;
            this.autoPrintSwitch.Name = "autoPrintSwitch";
            this.autoPrintSwitch.Ripple = true;
            this.autoPrintSwitch.UseVisualStyleBackColor = true;
            this.autoPrintSwitch.CheckedChanged += new System.EventHandler(this.materialSwitch2_CheckedChanged);
            // 
            // selectAllSwitch
            // 
            resources.ApplyResources(this.selectAllSwitch, "selectAllSwitch");
            this.selectAllSwitch.Depth = 0;
            this.selectAllSwitch.MouseLocation = new System.Drawing.Point(-1, -1);
            this.selectAllSwitch.MouseState = MaterialSkin.MouseState.HOVER;
            this.selectAllSwitch.Name = "selectAllSwitch";
            this.selectAllSwitch.Ripple = true;
            this.selectAllSwitch.UseVisualStyleBackColor = true;
            this.selectAllSwitch.CheckStateChanged += new System.EventHandler(this.materialSwitch3_CheckStateChanged);
            // 
            // materialLabel1
            // 
            resources.ApplyResources(this.materialLabel1, "materialLabel1");
            this.tableLayoutPanel1.SetColumnSpan(this.materialLabel1, 2);
            this.materialLabel1.Depth = 0;
            this.materialLabel1.FontType = MaterialSkin.MaterialSkinManager.fontType.Subtitle2;
            this.materialLabel1.ForeColor = System.Drawing.Color.Lime;
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            // 
            // materialButton2
            // 
            resources.ApplyResources(this.materialButton2, "materialButton2");
            this.materialButton2.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButton2.Depth = 0;
            this.materialButton2.HighEmphasis = true;
            this.materialButton2.Icon = null;
            this.materialButton2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButton2.Name = "materialButton2";
            this.materialButton2.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButton2.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButton2.UseAccentColor = false;
            this.materialButton2.UseVisualStyleBackColor = true;
            this.materialButton2.Click += new System.EventHandler(this.materialButton2_Click);
            // 
            // materialButton4
            // 
            resources.ApplyResources(this.materialButton4, "materialButton4");
            this.materialButton4.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButton4.Depth = 0;
            this.materialButton4.HighEmphasis = true;
            this.materialButton4.Icon = null;
            this.materialButton4.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButton4.Name = "materialButton4";
            this.materialButton4.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButton4.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButton4.UseAccentColor = false;
            this.materialButton4.UseVisualStyleBackColor = true;
            this.materialButton4.Click += new System.EventHandler(this.materialButton4_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.selectAllSwitch, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.materialButton1, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.materialButton3, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.materialProgressBar1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.materialLabel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.materialCheckedListBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.defaultPrinterSwitch, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.autoPrintSwitch, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.materialButton2, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.materialButton4, 1, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeMenuItem;
        private MaterialSkin.Controls.MaterialButton materialButton1;
        private MaterialSkin.Controls.MaterialProgressBar materialProgressBar1;
        private MaterialSkin.Controls.MaterialButton materialButton3;
        private MaterialSkin.Controls.MaterialSwitch defaultPrinterSwitch;
        private MaterialSkin.Controls.MaterialCheckedListBox materialCheckedListBox1;
        private MaterialSkin.Controls.MaterialCheckbox materialCheckbox1;
        private MaterialSkin.Controls.MaterialSwitch autoPrintSwitch;
        private MaterialSkin.Controls.MaterialSwitch selectAllSwitch;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialButton materialButton2;
        private MaterialSkin.Controls.MaterialButton materialButton4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

