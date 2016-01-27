namespace Poker.UserInterface
{
    using System.Windows.Forms;

    partial class AddChips
    {
        private Label label1;
        private Button button1;
        private Button button2;
        private TextBox textBox1;

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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();

            // 
            // label1
            // 
            this.label1 = new Label()
            {
                Font = new System.Drawing.Font(
                        "Microsoft Sans Serif",
                        12.5F,
                        System.Drawing.FontStyle.Regular,
                        System.Drawing.GraphicsUnit.Point,
                        ((byte)(204))),
                Location = new System.Drawing.Point(48, 49),
                Name = "label1",
                Size = new System.Drawing.Size(176, 23),
                TabIndex = 0,
                Text = "You ran out of chips !",
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };

            // 
            // button1
            // 
            this.button1 = new Button()
            {
                Font = new System.Drawing.Font(
                            "Microsoft Sans Serif",
                            9F,
                            System.Drawing.FontStyle.Regular,
                            System.Drawing.GraphicsUnit.Point,
                            ((byte)(204))),
                Location = new System.Drawing.Point(12, 226),
                Name = "button1",
                Size = new System.Drawing.Size(75, 23),
                TabIndex = 1,
                Text = "Add Chips",
                UseVisualStyleBackColor = true
            };
            this.button1.Click += new System.EventHandler(this.ButtonAddChips_Click);
           
            // 
            // button2
            // 
            this.button2 = new Button()
            {
                Font = new System.Drawing.Font(
                            "Microsoft Sans Serif",
                            9F,
                            System.Drawing.FontStyle.Regular,
                            System.Drawing.GraphicsUnit.Point,
                            ((byte)(204))),
                Location = new System.Drawing.Point(197, 226),
                Name = "button2",
                Size = new System.Drawing.Size(75, 23),
                TabIndex = 2,
                Text = "Exit",
                UseVisualStyleBackColor = true
            };

            this.button2.Click += new System.EventHandler(this.ButtonExit_Click);
            // 
            // textBox1
            // 
            this.textBox1 = new TextBox()
            {
                Location = new System.Drawing.Point(91, 229),
                Name = "textBox1",
                Size = new System.Drawing.Size(100, 20),
                TabIndex = 3
            };

            // 
            // AddChips
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Name = "AddChips";
            this.Text = "You Ran Out Of Chips";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}