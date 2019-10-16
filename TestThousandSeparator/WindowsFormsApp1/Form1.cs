using System;
using System.Globalization;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        CultureInfo currentCulture = CultureInfo.CurrentCulture;
        //CultureInfo currentCulture = CultureInfo.GetCultureInfo("id-ID");

        private double ExtractFromThousandSeparator(string textNumber)
        {
            var isDouble = double.TryParse(textNumber, NumberStyles.Number,
                currentCulture, out double number);
            if (isDouble)
                return number;
            return 0;
        }

        private string ApplyThousandSeparator(string textNumber)
        {
            string comma = currentCulture.NumberFormat.NumberDecimalSeparator;
            string separator = currentCulture.NumberFormat.NumberGroupSeparator;

            // if lastChar is comma, do nothing. Allowing input another number
            var lastChar = textNumber.Substring(textNumber.Length > 0 ? textNumber.Length - 1 : 0);
            if (lastChar == comma)
                return textNumber;

            // remove any separators after comma
            var splitted = textNumber.Split(comma.ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);
            if (splitted.Length > 1)
            {
                splitted[1] = splitted[1].Replace(separator, "");
                textNumber = string.Join(comma, splitted);
            }

            // apply separator if text is double
            var isDouble = double.TryParse(textNumber, NumberStyles.Number,
                currentCulture, out double number);
            if (isDouble)
                return string.Format(currentCulture, "{0:#,##0.####}", number);

            // if not, apply nothing
            return textNumber;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            var curPos = textBox.SelectionStart;
            var diff = textBox.TextLength;  // anticipate new thousand separator

            textBox.Text = ApplyThousandSeparator(textBox.Text);

            diff -= textBox.TextLength;
            curPos -= diff;
            textBox.SelectionStart = curPos < 0 ? 0 : curPos;
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            char comma = char.Parse(currentCulture.NumberFormat.NumberDecimalSeparator);

            // Set only handling key control and number decimal
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)
                && (e.KeyChar != comma) && (e.KeyChar != '-'))
                e.Handled = true;

            // Only allow one decimal point
            if ((e.KeyChar == comma) && (textBox.Text.IndexOf(comma) > -1))
                e.Handled = true;

            // allow one negative sign only if the caret is at the front
            if (e.KeyChar == '-' && (textBox.SelectionStart != 0 || textBox.Text.Contains("-")))
                e.Handled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.TextChanged -= TextBox_TextChanged;
            textBox1.TextChanged += TextBox_TextChanged;
            textBox1.KeyPress -= TextBox_KeyPress;
            textBox1.KeyPress += TextBox_KeyPress;
        }

        private void Label1_Click(object sender, EventArgs e)
        {
            string comma = currentCulture.NumberFormat.NumberDecimalSeparator;
            string separator = currentCulture.NumberFormat.NumberGroupSeparator;

            label1.Text = separator + " | " + comma + " | "
                + ExtractFromThousandSeparator(textBox1.Text).ToString();
        }
    }
}
