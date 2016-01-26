namespace Poker.UserInterface
{
    using System;
    using System.Windows.Forms;

    public partial class AddChips : Form
    {
        public AddChips()
        {
            this.InitializeComponent();
            this.ControlBox = false;
            this.label1.BorderStyle = BorderStyle.FixedSingle;
        }

        public int ChipsAmount { get; set; }

        public void ButtonAddChips_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (int.Parse(this.textBox1.Text) > 100000000)
            {
                MessageBox.Show("The maximium chips you can add is 100000000");

                return;
            }

            if (!int.TryParse(this.textBox1.Text, out parsedValue))
            {
                MessageBox.Show("This is chipsAmount number only field");
            }
            else if (int.TryParse(this.textBox1.Text, out parsedValue) && int.Parse(this.textBox1.Text) <= 100000000)
            {
                this.ChipsAmount = int.Parse(this.textBox1.Text);
                this.Close();
            }
        }

        private void ButtonExit_Click(object sender, EventArgs e)
        {
            var message = "Are you sure?";
            var title = "Quit";
            var result = MessageBox.Show(
            message,title,
            MessageBoxButtons.YesNo, 
            MessageBoxIcon.Question);

            switch (result)
            {
                case DialogResult.No:
                    break;
                case DialogResult.Yes:
                    Application.Exit();
                    break;
            }
        }
    }
}
