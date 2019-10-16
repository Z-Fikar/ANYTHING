using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        IFormatProvider currentCulture = System.Globalization.CultureInfo.CurrentCulture;

        private string ApplyThousandSeparator(string textNumber)
        {
            var lastChar = textNumber.Substring(textNumber.Length > 0 ? textNumber.Length - 1 : 0);
            if (lastChar == ".")
                return textNumber;

            var isDouble = double.TryParse(textNumber, out double number);
            if (isDouble)
                return string.Format(currentCulture, "{0:#,##0.####}", number);

            return textNumber;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = ApplyThousandSeparator(textBox.Text);
            textBox.SelectionStart = textBox.TextLength;
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Set only handling key control and number decimal

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                e.Handled = true;

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
                e.Handled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.TextChanged -= TextBox_TextChanged;
            textBox1.TextChanged += TextBox_TextChanged;
            textBox1.KeyPress -= TextBox_KeyPress;
            textBox1.KeyPress += TextBox_KeyPress;
        }
    }
}
