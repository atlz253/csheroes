
namespace csheroes.form.camp
{
    partial class CampForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(227, 118);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(300, 100);
            this.button1.TabIndex = 0;
            this.button1.Text = "Нанять абитурента";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(227, 249);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(300, 100);
            this.button2.TabIndex = 1;
            this.button2.Text = "Прокачать студента";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(227, 380);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(300, 100);
            this.button3.TabIndex = 2;
            this.button3.Text = "Зайти в здравпункт";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(227, 504);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(300, 100);
            this.button4.TabIndex = 3;
            this.button4.Text = "Выход";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // CampForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(802, 808);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximumSize = new System.Drawing.Size(818, 847);
            this.MinimumSize = new System.Drawing.Size(818, 847);
            this.Name = "CampForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "CampForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}