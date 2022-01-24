
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
            this.materialButton1.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButton1.Depth = 0;
            this.materialButton1.HighEmphasis = true;
            this.materialButton1.Icon = null;
            this.materialButton1.Location = new System.Drawing.Point(454, 440);
            this.materialButton1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialButton1.MinimumSize = new System.Drawing.Size(10, 36);
            this.materialButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButton1.Name = "materialButton1";
            this.materialButton1.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButton1.Size = new System.Drawing.Size(74, 36);
            this.materialButton1.TabIndex = 4;
            this.materialButton1.Text = "Выход";
            this.materialButton1.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.materialButton1.UseAccentColor = false;
            this.materialButton1.UseVisualStyleBackColor = true;
            this.materialButton1.Click += new System.EventHandler(this.materialButton1_Click);
            // 
            // materialProgressBar1
            // 
            this.materialProgressBar1.Depth = 0;
            this.materialProgressBar1.ForeColor = System.Drawing.Color.Lime;
            this.materialProgressBar1.Location = new System.Drawing.Point(6, 340);
            this.materialProgressBar1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialProgressBar1.Name = "materialProgressBar1";
            this.materialProgressBar1.Size = new System.Drawing.Size(522, 5);
            this.materialProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.materialProgressBar1.TabIndex = 5;
            // 
            // materialButton3
            // 
            this.materialButton3.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButton3.Depth = 0;
            this.materialButton3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.materialButton3.Enabled = false;
            this.materialButton3.HighEmphasis = true;
            this.materialButton3.Icon = null;
            this.materialButton3.Location = new System.Drawing.Point(3, 440);
            this.materialButton3.Margin = new System.Windows.Forms.Padding(4, 6, 4, 10);
            this.materialButton3.MaximumSize = new System.Drawing.Size(400, 36);
            this.materialButton3.MinimumSize = new System.Drawing.Size(200, 36);
            this.materialButton3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButton3.Name = "materialButton3";
            this.materialButton3.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButton3.Size = new System.Drawing.Size(400, 36);
            this.materialButton3.TabIndex = 7;
            this.materialButton3.Text = "Печать";
            this.materialButton3.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButton3.UseAccentColor = false;
            this.materialButton3.UseVisualStyleBackColor = true;
            this.materialButton3.Click += new System.EventHandler(this.materialButton3_Click);
            // 
            // defaultPrinterSwitch
            // 
            this.defaultPrinterSwitch.AutoSize = true;
            this.defaultPrinterSwitch.Depth = 0;
            this.defaultPrinterSwitch.Location = new System.Drawing.Point(6, 392);
            this.defaultPrinterSwitch.Margin = new System.Windows.Forms.Padding(0);
            this.defaultPrinterSwitch.MouseLocation = new System.Drawing.Point(-1, -1);
            this.defaultPrinterSwitch.MouseState = MaterialSkin.MouseState.HOVER;
            this.defaultPrinterSwitch.Name = "defaultPrinterSwitch";
            this.defaultPrinterSwitch.Ripple = true;
            this.defaultPrinterSwitch.Size = new System.Drawing.Size(238, 37);
            this.defaultPrinterSwitch.TabIndex = 9;
            this.defaultPrinterSwitch.Text = "Принтер по умолчанию ";
            this.defaultPrinterSwitch.UseVisualStyleBackColor = true;
            // 
            // materialCheckedListBox1
            // 
            this.materialCheckedListBox1.AutoScroll = true;
            this.materialCheckedListBox1.BackColor = System.Drawing.SystemColors.Control;
            this.materialCheckedListBox1.Depth = 0;
            this.materialCheckedListBox1.Location = new System.Drawing.Point(6, 104);
            this.materialCheckedListBox1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCheckedListBox1.Name = "materialCheckedListBox1";
            this.materialCheckedListBox1.Size = new System.Drawing.Size(522, 230);
            this.materialCheckedListBox1.Striped = false;
            this.materialCheckedListBox1.StripeDarkColor = System.Drawing.Color.Empty;
            this.materialCheckedListBox1.TabIndex = 10;
            // 
            // materialCheckbox1
            // 
            this.materialCheckbox1.Depth = 0;
            this.materialCheckbox1.Location = new System.Drawing.Point(0, 0);
            this.materialCheckbox1.Margin = new System.Windows.Forms.Padding(0);
            this.materialCheckbox1.MouseLocation = new System.Drawing.Point(-1, -1);
            this.materialCheckbox1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCheckbox1.Name = "materialCheckbox1";
            this.materialCheckbox1.ReadOnly = false;
            this.materialCheckbox1.Ripple = true;
            this.materialCheckbox1.Size = new System.Drawing.Size(104, 37);
            this.materialCheckbox1.TabIndex = 0;
            this.materialCheckbox1.Text = "materialCheckbox1";
            this.materialCheckbox1.UseVisualStyleBackColor = true;
            // 
            // autoPrintSwitch
            // 
            this.autoPrintSwitch.AutoSize = true;
            this.autoPrintSwitch.Depth = 0;
            this.autoPrintSwitch.Location = new System.Drawing.Point(6, 355);
            this.autoPrintSwitch.Margin = new System.Windows.Forms.Padding(0);
            this.autoPrintSwitch.MouseLocation = new System.Drawing.Point(-1, -1);
            this.autoPrintSwitch.MouseState = MaterialSkin.MouseState.HOVER;
            this.autoPrintSwitch.Name = "autoPrintSwitch";
            this.autoPrintSwitch.Ripple = true;
            this.autoPrintSwitch.Size = new System.Drawing.Size(240, 37);
            this.autoPrintSwitch.TabIndex = 11;
            this.autoPrintSwitch.Text = "Автоматическая печать";
            this.autoPrintSwitch.UseVisualStyleBackColor = true;
            this.autoPrintSwitch.CheckedChanged += new System.EventHandler(this.materialSwitch2_CheckedChanged);
            // 
            // selectAllSwitch
            // 
            this.selectAllSwitch.AutoSize = true;
            this.selectAllSwitch.Depth = 0;
            this.selectAllSwitch.Location = new System.Drawing.Point(8, 64);
            this.selectAllSwitch.Margin = new System.Windows.Forms.Padding(0);
            this.selectAllSwitch.MouseLocation = new System.Drawing.Point(-1, -1);
            this.selectAllSwitch.MouseState = MaterialSkin.MouseState.HOVER;
            this.selectAllSwitch.Name = "selectAllSwitch";
            this.selectAllSwitch.Ripple = true;
            this.selectAllSwitch.Size = new System.Drawing.Size(153, 37);
            this.selectAllSwitch.TabIndex = 12;
            this.selectAllSwitch.Text = "Выбрать всё";
            this.selectAllSwitch.UseVisualStyleBackColor = true;
            this.selectAllSwitch.CheckStateChanged += new System.EventHandler(this.materialSwitch3_CheckStateChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(534, 479);
            this.Controls.Add(this.selectAllSwitch);
            this.Controls.Add(this.autoPrintSwitch);
            this.Controls.Add(this.materialCheckedListBox1);
            this.Controls.Add(this.defaultPrinterSwitch);
            this.Controls.Add(this.materialButton3);
            this.Controls.Add(this.materialProgressBar1);
            this.Controls.Add(this.materialButton1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(534, 479);
            this.MinimumSize = new System.Drawing.Size(534, 479);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FairCode";
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
        private MaterialSkin.Controls.MaterialSwitch defaultPrinterSwitch;
        private MaterialSkin.Controls.MaterialCheckedListBox materialCheckedListBox1;
        private MaterialSkin.Controls.MaterialCheckbox materialCheckbox1;
        private MaterialSkin.Controls.MaterialSwitch autoPrintSwitch;
        private MaterialSkin.Controls.MaterialSwitch selectAllSwitch;
    }
}

