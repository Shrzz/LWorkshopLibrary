namespace LWorkshopServer
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.rtbMain = new System.Windows.Forms.RichTextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.dgMain = new System.Windows.Forms.DataGridView();
            this.btn2 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.btnClearLog = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgMain)).BeginInit();
            this.SuspendLayout();
            // 
            // rtbMain
            // 
            this.rtbMain.Location = new System.Drawing.Point(12, 12);
            this.rtbMain.Name = "rtbMain";
            this.rtbMain.Size = new System.Drawing.Size(530, 368);
            this.rtbMain.TabIndex = 0;
            this.rtbMain.Text = "";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 400);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(385, 22);
            this.textBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(548, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(166, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Получить лист users";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dgMain
            // 
            this.dgMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgMain.Location = new System.Drawing.Point(548, 99);
            this.dgMain.Name = "dgMain";
            this.dgMain.RowHeadersWidth = 51;
            this.dgMain.RowTemplate.Height = 24;
            this.dgMain.Size = new System.Drawing.Size(470, 281);
            this.dgMain.TabIndex = 4;
            // 
            // btn2
            // 
            this.btn2.Location = new System.Drawing.Point(548, 70);
            this.btn2.Name = "btn2";
            this.btn2.Size = new System.Drawing.Size(166, 23);
            this.btn2.TabIndex = 5;
            this.btn2.Text = "Очистить грид";
            this.btn2.UseVisualStyleBackColor = true;
            this.btn2.Click += new System.EventHandler(this.btn2_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(548, 41);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(166, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Получить лист books";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.Location = new System.Drawing.Point(403, 400);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(139, 23);
            this.btnSendMessage.TabIndex = 7;
            this.btnSendMessage.Text = "Отправить";
            this.btnSendMessage.UseVisualStyleBackColor = true;
            this.btnSendMessage.Click += new System.EventHandler(this.btnSendMessage_Click);
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(908, 399);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(110, 23);
            this.btnClearLog.TabIndex = 8;
            this.btnClearLog.Text = "Очистить лог";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1030, 450);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.btnSendMessage);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btn2);
            this.Controls.Add(this.dgMain);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.rtbMain);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dgMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.RichTextBox rtbMain;
        public System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.Button button1;
        public System.Windows.Forms.DataGridView dgMain;
        private System.Windows.Forms.Button btn2;
        public System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnSendMessage;
        private System.Windows.Forms.Button btnClearLog;
    }
}

