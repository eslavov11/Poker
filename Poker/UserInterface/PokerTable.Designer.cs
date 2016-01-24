namespace Poker.UserInterface
{
    using System.Windows.Forms;

    partial class PokerTable
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
            this.bFold = new Button();
            this.bCheck = new Button();
            this.bCall = new Button();
            this.bRaise = new Button();
            this.pbTimer = new ProgressBar();
            this.tbChips = new TextBox();
            this.bAdd = new Button();
            this.tbAdd = new TextBox();
            this.tbBotChips5 = new TextBox();
            this.tbBotChips4 = new TextBox();
            this.tbBotChips3 = new TextBox();
            this.tbBotChips2 = new TextBox();
            this.tbBotChips1 = new TextBox();
            this.potStatus = new TextBox();
            this.buttonOptions = new Button();
            this.buttonBigBlind = new Button();
            this.tbSmallBlind = new TextBox();
            this.buttonSmallBlind = new Button();
            this.tbBigBlind = new TextBox();
            this.fifthBotStatus = new Label();
            this.fourthBotStatus = new Label();
            this.thirdBotStatus = new Label();
            this.firstBotStatus = new Label();
            this.humanStatus = new Label();
            this.secondBotStatus = new Label();
            this.label1 = new Label();
            this.tbRaise = new TextBox();
            this.SuspendLayout();
            // 
            // bFold
            // 
            this.bFold.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.bFold.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bFold.Location = new System.Drawing.Point(335, 660);
            this.bFold.Name = "bFold";
            this.bFold.Size = new System.Drawing.Size(130, 62);
            this.bFold.TabIndex = 0;
            this.bFold.Text = "Fold";
            this.bFold.UseVisualStyleBackColor = true;
            this.bFold.Click += new System.EventHandler(this.ButtonFold_Click);
            // 
            // bCheck
            // 
            this.bCheck.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.bCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bCheck.Location = new System.Drawing.Point(494, 660);
            this.bCheck.Name = "bCheck";
            this.bCheck.Size = new System.Drawing.Size(134, 62);
            this.bCheck.TabIndex = 2;
            this.bCheck.Text = "Check";
            this.bCheck.UseVisualStyleBackColor = true;
            this.bCheck.Click += new System.EventHandler(this.ButtonCheck_Click);
            // 
            // bCall
            // 
            this.bCall.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.bCall.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bCall.Location = new System.Drawing.Point(667, 661);
            this.bCall.Name = "bCall";
            this.bCall.Size = new System.Drawing.Size(126, 62);
            this.bCall.TabIndex = 3;
            this.bCall.Text = "Call";
            this.bCall.UseVisualStyleBackColor = true;
            this.bCall.Click += new System.EventHandler(this.ButtonCall_Click);
            // 
            // bRaise
            // 
            this.bRaise.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.bRaise.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bRaise.Location = new System.Drawing.Point(835, 661);
            this.bRaise.Name = "bRaise";
            this.bRaise.Size = new System.Drawing.Size(124, 62);
            this.bRaise.TabIndex = 4;
            this.bRaise.Text = "Raise";
            this.bRaise.UseVisualStyleBackColor = true;
            this.bRaise.Click += new System.EventHandler(this.ButtonRaise_Click);
            // 
            // pbTimer
            // 
            this.pbTimer.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.pbTimer.BackColor = System.Drawing.SystemColors.Control;
            this.pbTimer.Location = new System.Drawing.Point(335, 631);
            this.pbTimer.Maximum = 1000;
            this.pbTimer.Name = "pbTimer";
            this.pbTimer.Size = new System.Drawing.Size(667, 23);
            this.pbTimer.TabIndex = 5;
            this.pbTimer.Value = 1000;
            // 
            // tbChips
            // 
            this.tbChips.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.tbChips.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbChips.Location = new System.Drawing.Point(755, 553);
            this.tbChips.Name = "tbChips";
            this.tbChips.Size = new System.Drawing.Size(163, 23);
            this.tbChips.TabIndex = 6;
            this.tbChips.Text = "Chips : 0";
            // 
            // bAdd
            // 
            this.bAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bAdd.Location = new System.Drawing.Point(12, 697);
            this.bAdd.Name = "bAdd";
            this.bAdd.Size = new System.Drawing.Size(75, 25);
            this.bAdd.TabIndex = 7;
            this.bAdd.Text = "AddChips";
            this.bAdd.UseVisualStyleBackColor = true;
            this.bAdd.Click += new System.EventHandler(this.ButtonAddChips_Click);
            // 
            // tbAdd
            // 
            this.tbAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbAdd.Location = new System.Drawing.Point(93, 700);
            this.tbAdd.Name = "tbAdd";
            this.tbAdd.Size = new System.Drawing.Size(125, 20);
            this.tbAdd.TabIndex = 8;
            // 
            // tbBotChips5
            // 
            this.tbBotChips5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBotChips5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbBotChips5.Location = new System.Drawing.Point(1012, 553);
            this.tbBotChips5.Name = "tbBotChips5";
            this.tbBotChips5.Size = new System.Drawing.Size(152, 23);
            this.tbBotChips5.TabIndex = 9;
            this.tbBotChips5.Text = "Chips : 0";
            // 
            // tbBotChips4
            // 
            this.tbBotChips4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBotChips4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbBotChips4.Location = new System.Drawing.Point(970, 81);
            this.tbBotChips4.Name = "tbBotChips4";
            this.tbBotChips4.Size = new System.Drawing.Size(123, 23);
            this.tbBotChips4.TabIndex = 10;
            this.tbBotChips4.Text = "Chips : 0";
            // 
            // tbBotChips3
            // 
            this.tbBotChips3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBotChips3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbBotChips3.Location = new System.Drawing.Point(755, 81);
            this.tbBotChips3.Name = "tbBotChips3";
            this.tbBotChips3.Size = new System.Drawing.Size(125, 23);
            this.tbBotChips3.TabIndex = 11;
            this.tbBotChips3.Text = "Chips : 0";
            // 
            // tbBotChips2
            // 
            this.tbBotChips2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbBotChips2.Location = new System.Drawing.Point(276, 81);
            this.tbBotChips2.Name = "tbBotChips2";
            this.tbBotChips2.Size = new System.Drawing.Size(133, 23);
            this.tbBotChips2.TabIndex = 12;
            this.tbBotChips2.Text = "Chips : 0";
            // 
            // tbBotChips1
            // 
            this.tbBotChips1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbBotChips1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbBotChips1.Location = new System.Drawing.Point(181, 553);
            this.tbBotChips1.Name = "tbBotChips1";
            this.tbBotChips1.Size = new System.Drawing.Size(142, 23);
            this.tbBotChips1.TabIndex = 13;
            this.tbBotChips1.Text = "Chips : 0";
            // 
            // txtPot
            // 
            this.potStatus.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.potStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.potStatus.Location = new System.Drawing.Point(606, 212);
            this.potStatus.Name = "txtPot";
            this.potStatus.Size = new System.Drawing.Size(125, 23);
            this.potStatus.TabIndex = 14;
            this.potStatus.Text = "0";
            // 
            // buttonOptions
            // 
            this.buttonOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonOptions.Location = new System.Drawing.Point(12, 12);
            this.buttonOptions.Name = "buttonOptions";
            this.buttonOptions.Size = new System.Drawing.Size(75, 36);
            this.buttonOptions.TabIndex = 15;
            this.buttonOptions.Text = "BB/SB";
            this.buttonOptions.UseVisualStyleBackColor = true;
            this.buttonOptions.Click += new System.EventHandler(this.ButtonOptions_Click);
            // 
            // buttonBigBlind
            // 
            this.buttonBigBlind.Location = new System.Drawing.Point(12, 254);
            this.buttonBigBlind.Name = "buttonBigBlind";
            this.buttonBigBlind.Size = new System.Drawing.Size(75, 23);
            this.buttonBigBlind.TabIndex = 16;
            this.buttonBigBlind.Text = "Big Blind";
            this.buttonBigBlind.UseVisualStyleBackColor = true;
            this.buttonBigBlind.Click += new System.EventHandler(this.ButtonBigBlind_Click);
            // 
            // tbSmallBlind
            // 
            this.tbSmallBlind.Location = new System.Drawing.Point(12, 228);
            this.tbSmallBlind.Name = "tbSmallBlind";
            this.tbSmallBlind.Size = new System.Drawing.Size(75, 20);
            this.tbSmallBlind.TabIndex = 17;
            this.tbSmallBlind.Text = "250";
            // 
            // bSB
            // 
            this.buttonSmallBlind.Location = new System.Drawing.Point(12, 199);
            this.buttonSmallBlind.Name = "bSB";
            this.buttonSmallBlind.Size = new System.Drawing.Size(75, 23);
            this.buttonSmallBlind.TabIndex = 18;
            this.buttonSmallBlind.Text = "Small Blind";
            this.buttonSmallBlind.UseVisualStyleBackColor = true;
            this.buttonSmallBlind.Click += new System.EventHandler(this.ButtonSmallBlind_Click);
            // 
            // tbBigBlind
            // 
            this.tbBigBlind.Location = new System.Drawing.Point(12, 283);
            this.tbBigBlind.Name = "tbBigBlind";
            this.tbBigBlind.Size = new System.Drawing.Size(75, 20);
            this.tbBigBlind.TabIndex = 19;
            this.tbBigBlind.Text = "500";
            // 
            // fifthBotStatus
            // 
            this.fifthBotStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.fifthBotStatus.Location = new System.Drawing.Point(1012, 579);
            this.fifthBotStatus.Name = "fifthBotStatus";
            this.fifthBotStatus.Size = new System.Drawing.Size(152, 32);
            this.fifthBotStatus.TabIndex = 26;
            // 
            // fourthBotStatus
            // 
            this.fourthBotStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fourthBotStatus.Location = new System.Drawing.Point(970, 107);
            this.fourthBotStatus.Name = "fourthBotStatus";
            this.fourthBotStatus.Size = new System.Drawing.Size(123, 32);
            this.fourthBotStatus.TabIndex = 27;
            // 
            // thirdBotStatus
            // 
            this.thirdBotStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.thirdBotStatus.Location = new System.Drawing.Point(755, 107);
            this.thirdBotStatus.Name = "thirdBotStatus";
            this.thirdBotStatus.Size = new System.Drawing.Size(125, 32);
            this.thirdBotStatus.TabIndex = 28;
            // 
            // firstBotStatus
            // 
            this.firstBotStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.firstBotStatus.Location = new System.Drawing.Point(181, 579);
            this.firstBotStatus.Name = "firstBotStatus";
            this.firstBotStatus.Size = new System.Drawing.Size(142, 32);
            this.firstBotStatus.TabIndex = 29;
            // 
            // humanStatus
            // 
            this.humanStatus.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.humanStatus.Location = new System.Drawing.Point(755, 579);
            this.humanStatus.Name = "humanStatus";
            this.humanStatus.Size = new System.Drawing.Size(163, 32);
            this.humanStatus.TabIndex = 30;
            // 
            // secondBotStatus
            // 
            this.secondBotStatus.Location = new System.Drawing.Point(276, 107);
            this.secondBotStatus.Name = "secondBotStatus";
            this.secondBotStatus.Size = new System.Drawing.Size(133, 32);
            this.secondBotStatus.TabIndex = 31;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(654, 188);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pot";
            // 
            // tbRaise
            // 
            this.tbRaise.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.tbRaise.Location = new System.Drawing.Point(965, 703);
            this.tbRaise.Name = "tbRaise";
            this.tbRaise.Size = new System.Drawing.Size(108, 20);
            this.tbRaise.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Poker.Properties.Resources.poker_table___Copy;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1350, 729);
            this.Controls.Add(this.tbRaise);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.secondBotStatus);
            this.Controls.Add(this.humanStatus);
            this.Controls.Add(this.firstBotStatus);
            this.Controls.Add(this.thirdBotStatus);
            this.Controls.Add(this.fourthBotStatus);
            this.Controls.Add(this.fifthBotStatus);
            this.Controls.Add(this.tbBigBlind);
            this.Controls.Add(this.buttonSmallBlind);
            this.Controls.Add(this.tbSmallBlind);
            this.Controls.Add(this.buttonBigBlind);
            this.Controls.Add(this.buttonOptions);
            this.Controls.Add(this.potStatus);
            this.Controls.Add(this.tbBotChips1);
            this.Controls.Add(this.tbBotChips2);
            this.Controls.Add(this.tbBotChips3);
            this.Controls.Add(this.tbBotChips4);
            this.Controls.Add(this.tbBotChips5);
            this.Controls.Add(this.tbAdd);
            this.Controls.Add(this.bAdd);
            this.Controls.Add(this.tbChips);
            this.Controls.Add(this.pbTimer);
            this.Controls.Add(this.bRaise);
            this.Controls.Add(this.bCall);
            this.Controls.Add(this.bCheck);
            this.Controls.Add(this.bFold);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "GLS Texas Poker";
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.ChangeLayout);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button bFold;
        private Button bCheck;
        private Button bCall;
        private Button bRaise;
        private ProgressBar pbTimer;
        private TextBox tbChips;
        private Button bAdd;
        private TextBox tbAdd;
        private TextBox tbBotChips5;
        private TextBox tbBotChips4;
        private TextBox tbBotChips3;
        private TextBox tbBotChips2;
        private TextBox tbBotChips1;
        private TextBox potStatus;
        private Button buttonOptions;
        private Button buttonBigBlind;
        private TextBox tbSmallBlind;
        private Button buttonSmallBlind;
        private TextBox tbBigBlind;
        private Label firstBotStatus;
        private Label secondBotStatus;
        private Label thirdBotStatus;
        private Label fourthBotStatus;
        private Label fifthBotStatus;
        private Label humanStatus;
        private Label label1;
        private TextBox tbRaise;



    }
}

