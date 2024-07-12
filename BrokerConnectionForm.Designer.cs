namespace MQTT_Intek
{
    partial class BrokerConnectionForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox hostNameInput;
        private System.Windows.Forms.Label hostNameLabel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.hostNameInput = new System.Windows.Forms.TextBox();
            this.hostNameLabel = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.portInput = new System.Windows.Forms.TextBox();
            this.portLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // hostNameInput
            // 
            this.hostNameInput.Location = new System.Drawing.Point(142, 18);
            this.hostNameInput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.hostNameInput.Name = "hostNameInput";
            this.hostNameInput.Size = new System.Drawing.Size(264, 26);
            this.hostNameInput.TabIndex = 0;
            // 
            // hostNameLabel
            // 
            this.hostNameLabel.AutoSize = true;
            this.hostNameLabel.Location = new System.Drawing.Point(18, 23);
            this.hostNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.hostNameLabel.Name = "hostNameLabel";
            this.hostNameLabel.Size = new System.Drawing.Size(87, 20);
            this.hostNameLabel.TabIndex = 3;
            this.hostNameLabel.Text = "Hostname:";
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(174, 93);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(112, 35);
            this.buttonOk.TabIndex = 6;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(294, 93);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(112, 35);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // portInput
            // 
            this.portInput.Location = new System.Drawing.Point(142, 54);
            this.portInput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.portInput.Name = "portInput";
            this.portInput.Size = new System.Drawing.Size(264, 26);
            this.portInput.TabIndex = 8;
            this.portInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.portInput_KeyPress);
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(18, 54);
            this.portLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(42, 20);
            this.portLabel.TabIndex = 9;
            this.portLabel.Text = "Port:";
            // 
            // BrokerConnectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 143);
            this.Controls.Add(this.portLabel);
            this.Controls.Add(this.portInput);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.hostNameLabel);
            this.Controls.Add(this.hostNameInput);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "BrokerConnectionForm";
            this.Text = "Setup new connection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.TextBox portInput;
        private System.Windows.Forms.Label portLabel;
    }

}