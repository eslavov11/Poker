namespace Poker.UserInterface
{
    using System.Windows.Forms;

    public partial class PokerTable
    {
        private Button buttonFold;
        private Button buttonCheck;
        private Button buttonCall;
        private Button buttonRaise;
        private ProgressBar pbTimer;
        private TextBox txtBoxHumanChips;
        private Button bAdd;
        private TextBox tbAdd;
        private TextBox potStatus;
        private Button buttonOptions;
        private Button buttonBigBlind;
        private TextBox tbSmallBlind;
        private Button buttonSmallBlind;
        private TextBox tbBigBlind;
        private Label humanStatus;
        private Label label1;
        private TextBox tbRaise;

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
            this.buttonFold = new Button();
            this.buttonCheck = new Button();
            this.buttonCall = new Button();
            this.buttonRaise = new Button();
            this.pbTimer = new ProgressBar();
            this.txtBoxHumanChips = new TextBox();
            this.bAdd = new Button();
            this.tbAdd = new TextBox();
            this.potStatus = new TextBox();
            this.buttonOptions = new Button();
            this.buttonBigBlind = new Button();
            this.tbSmallBlind = new TextBox();
            this.buttonSmallBlind = new Button();
            this.tbBigBlind = new TextBox();
            this.humanStatus = new Label();
            this.label1 = new Label();
            this.tbRaise = new TextBox();
            this.SuspendLayout();
            this.FoldButton();
            this.CheckButton();
            this.CallButton();
            this.RaiseButton();
            this.StartTimer();
            this.AddHumanChips();
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
            this.bots[4].Status.Anchor = this.bots[4].GetAnchorStyles();
            this.bots[4].Status.Location = new System.Drawing.Point(1012, 579);
            this.bots[4].Status.Name = "fifthBotStatus";
            this.bots[4].Status.Size = new System.Drawing.Size(152, 32);
            this.bots[4].Status.TabIndex = 26;
            // 
            // fourthBotStatus
            // 
            this.bots[3].Status.Anchor = this.bots[3].GetAnchorStyles();
            this.bots[3].Status.Location = new System.Drawing.Point(970, 107);
            this.bots[3].Status.Name = "fourthBotStatus";
            this.bots[3].Status.Size = new System.Drawing.Size(123, 32);
            this.bots[3].Status.TabIndex = 27;
            // 
            // thirdBotStatus
            // 
            this.bots[2].Status.Anchor = this.bots[2].GetAnchorStyles();
            this.bots[2].Status.Location = new System.Drawing.Point(755, 107);
            this.bots[2].Status.Name = "thirdBotStatus";
            this.bots[2].Status.Size = new System.Drawing.Size(125, 32);
            this.bots[2].Status.TabIndex = 28;
            // 
            // firstBotStatus
            // 
            this.bots[0].Status.Anchor = this.bots[0].GetAnchorStyles();
            this.bots[0].Status.Location = new System.Drawing.Point(181, 579);
            this.bots[0].Status.Name = "firstBotStatus";
            this.bots[0].Status.Size = new System.Drawing.Size(142, 32);
            this.bots[0].Status.TabIndex = 29;
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
            this.bots[1].Status.Location = new System.Drawing.Point(276, 107);
            this.bots[1].Status.Name = "secondBotStatus";
            this.bots[1].Status.Size = new System.Drawing.Size(133, 32);
            this.bots[1].Status.TabIndex = 31;
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
            this.Controls.Add(this.humanStatus);
            this.Controls.Add(this.bots[1].Status);
            this.Controls.Add(this.bots[0].Status);
            this.Controls.Add(this.bots[2].Status);
            this.Controls.Add(this.bots[3].Status);
            this.Controls.Add(this.bots[4].Status);
            this.Controls.Add(this.tbBigBlind);
            this.Controls.Add(this.buttonSmallBlind);
            this.Controls.Add(this.tbSmallBlind);
            this.Controls.Add(this.buttonBigBlind);
            this.Controls.Add(this.buttonOptions);
            this.Controls.Add(this.potStatus);
            this.Controls.Add(this.bots[0].TextBoxBotChips);
            this.Controls.Add(this.bots[1].TextBoxBotChips);
            this.Controls.Add(this.bots[2].TextBoxBotChips);
            this.Controls.Add(this.bots[3].TextBoxBotChips);
            this.Controls.Add(this.bots[4].TextBoxBotChips);

            this.Controls.Add(this.tbAdd);
            this.Controls.Add(this.bAdd);
            this.Controls.Add(this.txtBoxHumanChips);
            this.Controls.Add(this.pbTimer);
            this.Controls.Add(this.buttonRaise);
            this.Controls.Add(this.buttonCall);
            this.Controls.Add(this.buttonCheck);
            this.Controls.Add(this.buttonFold);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "GLS Texas Poker";
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.ChangeLayout);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void FoldButton()
        {
            this.buttonFold.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonFold.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonFold.Location = new System.Drawing.Point(335, 660);
            this.buttonFold.Name = "buttonFold";
            this.buttonFold.Size = new System.Drawing.Size(130, 62);
            this.buttonFold.TabIndex = 0;
            this.buttonFold.Text = "Fold";
            this.buttonFold.UseVisualStyleBackColor = true;
            this.buttonFold.Click += new System.EventHandler(this.ButtonFold_Click);
        }

        private void CheckButton()
        {
            this.buttonCheck.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCheck.Location = new System.Drawing.Point(494, 660);
            this.buttonCheck.Name = "buttonCheck";
            this.buttonCheck.Size = new System.Drawing.Size(134, 62);
            this.buttonCheck.TabIndex = 2;
            this.buttonCheck.Text = "Check";
            this.buttonCheck.UseVisualStyleBackColor = true;
            this.buttonCheck.Click += new System.EventHandler(this.ButtonCheck_Click);
        }

        private void CallButton()
        {
            this.buttonCall.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCall.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCall.Location = new System.Drawing.Point(667, 661);
            this.buttonCall.Name = "buttonCall";
            this.buttonCall.Size = new System.Drawing.Size(126, 62);
            this.buttonCall.TabIndex = 3;
            this.buttonCall.Text = "Call";
            this.buttonCall.UseVisualStyleBackColor = true;
            this.buttonCall.Click += new System.EventHandler(this.ButtonCall_Click);
        }

        private void RaiseButton()
        {
            this.buttonRaise.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonRaise.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonRaise.Location = new System.Drawing.Point(835, 661);
            this.buttonRaise.Name = "buttonRaise";
            this.buttonRaise.Size = new System.Drawing.Size(124, 62);
            this.buttonRaise.TabIndex = 4;
            this.buttonRaise.Text = "Raise";
            this.buttonRaise.UseVisualStyleBackColor = true;
            this.buttonRaise.Click += new System.EventHandler(this.ButtonRaise_Click);
        }

        private void StartTimer()
        {
            this.pbTimer.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.pbTimer.BackColor = System.Drawing.SystemColors.Control;
            this.pbTimer.Location = new System.Drawing.Point(335, 631);
            this.pbTimer.Maximum = 1000;
            this.pbTimer.Name = "pbTimer";
            this.pbTimer.Size = new System.Drawing.Size(667, 23);
            this.pbTimer.TabIndex = 5;
            this.pbTimer.Value = 1000;
        }

        private void AddHumanChips()
        {
            this.txtBoxHumanChips.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtBoxHumanChips.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtBoxHumanChips.Location = new System.Drawing.Point(755, 553);
            this.txtBoxHumanChips.Name = "txtBoxHumanChips";
            this.txtBoxHumanChips.Size = new System.Drawing.Size(163, 23);
            this.txtBoxHumanChips.TabIndex = 6;
            this.txtBoxHumanChips.Text = "Chips : 0";
        }
        #endregion
    }
}