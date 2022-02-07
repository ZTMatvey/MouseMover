using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MouseMover
{
    public sealed class TextBoxPair
    {
        private readonly TextBox m_from;
        private readonly TextBox m_to;
        private static int NextPairId;
        public readonly int PairId;
        public static Action<int> MinMoreThanMax;
        public TextBoxPair(TextBox from, TextBox to)
        {
            if (from == null || to == null)
                throw new ArgumentException("Null in arguments");
            m_from = from;
            m_to = to;
            PairId = NextPairId++;
            CheckTextLength(m_from);
            CheckTextLength(m_to);
            Initizlize();
        }
        private void Initizlize()
        {
            m_from.LostFocus += CheckCorrect;
            m_to.LostFocus += CheckCorrect;
        }
        private void CheckTextLength(TextBox textBox)
        {
            if (textBox.Text.Length == 0)
                textBox.Text = "0";
        }
        private void CheckCorrect(object sender, System.Windows.RoutedEventArgs e)
        {
            var textBox = CheckTextBoxType(sender);
            CheckTextLength(textBox);
            var valueOfFrom = int.Parse(m_from.Text);
            var valueOfTo = int.Parse(m_to.Text);
            if (valueOfFrom > valueOfTo)
                MinMoreThanMax?.Invoke(PairId);
        }

        private TextBox CheckTextBoxType(object sender)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
                throw new ArgumentException("Sender is not TextBox");
            return textBox;
        }
    }
}
