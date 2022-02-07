using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MouseMover
{
    public sealed class TextBoxValidation
    {
        private readonly TextBox m_textBox;
        private bool isChecking = false;
        private string oldText;
        public TextBoxValidation(TextBox textBox)
        {
            m_textBox = textBox;
        }
        public void StartValidating()
        {
            if (isChecking)
                return;
            m_textBox.GotFocus += (s, e) => oldText = m_textBox.Text;
            m_textBox.TextChanged += Validate;
        }
        public void StopValidating()
        {
            throw new NotImplementedException();
        }
        private void Validate(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
                throw new ArgumentException("Sender is not TextBox");
            var text = textBox.Text;
            var allCharsIsNums = text.All(char.IsDigit);
            if (!allCharsIsNums && text.Length > 0)
            {
                textBox.Text = oldText;
                for(int i = 0; i < text.Length; i++)
                    if(!char.IsDigit(text[i]))
                    {
                        textBox.CaretIndex = i;
                        break;
                    }
            }
            else
                oldText = textBox.Text;
        }
    }
}
