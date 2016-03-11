namespace NFSe
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
            this.rbTesteEnvioLoteRps = new System.Windows.Forms.RadioButton();
            this.btnEnviar = new System.Windows.Forms.Button();
            this.txtResposta = new System.Windows.Forms.TextBox();
            this.rbConsultaCnpj = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // rbTesteEnvioLoteRps
            // 
            this.rbTesteEnvioLoteRps.AutoSize = true;
            this.rbTesteEnvioLoteRps.Checked = true;
            this.rbTesteEnvioLoteRps.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbTesteEnvioLoteRps.Location = new System.Drawing.Point(13, 13);
            this.rbTesteEnvioLoteRps.Name = "rbTesteEnvioLoteRps";
            this.rbTesteEnvioLoteRps.Size = new System.Drawing.Size(148, 17);
            this.rbTesteEnvioLoteRps.TabIndex = 0;
            this.rbTesteEnvioLoteRps.TabStop = true;
            this.rbTesteEnvioLoteRps.Text = "Teste Envio Lote Rps";
            this.rbTesteEnvioLoteRps.UseVisualStyleBackColor = true;
            // 
            // btnEnviar
            // 
            this.btnEnviar.Location = new System.Drawing.Point(259, 13);
            this.btnEnviar.Name = "btnEnviar";
            this.btnEnviar.Size = new System.Drawing.Size(83, 29);
            this.btnEnviar.TabIndex = 1;
            this.btnEnviar.Text = "Enviar";
            this.btnEnviar.UseVisualStyleBackColor = true;
            this.btnEnviar.Click += new System.EventHandler(this.btnEnviar_Click);
            // 
            // txtResposta
            // 
            this.txtResposta.Location = new System.Drawing.Point(12, 102);
            this.txtResposta.Multiline = true;
            this.txtResposta.Name = "txtResposta";
            this.txtResposta.ReadOnly = true;
            this.txtResposta.Size = new System.Drawing.Size(330, 226);
            this.txtResposta.TabIndex = 2;
            // 
            // rbConsultaCnpj
            // 
            this.rbConsultaCnpj.AutoSize = true;
            this.rbConsultaCnpj.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbConsultaCnpj.Location = new System.Drawing.Point(13, 36);
            this.rbConsultaCnpj.Name = "rbConsultaCnpj";
            this.rbConsultaCnpj.Size = new System.Drawing.Size(109, 17);
            this.rbConsultaCnpj.TabIndex = 0;
            this.rbConsultaCnpj.Text = "Consulta CNPJ";
            this.rbConsultaCnpj.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 340);
            this.Controls.Add(this.txtResposta);
            this.Controls.Add(this.btnEnviar);
            this.Controls.Add(this.rbConsultaCnpj);
            this.Controls.Add(this.rbTesteEnvioLoteRps);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NFS-e";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbTesteEnvioLoteRps;
        private System.Windows.Forms.Button btnEnviar;
        private System.Windows.Forms.TextBox txtResposta;
        private System.Windows.Forms.RadioButton rbConsultaCnpj;
    }
}

