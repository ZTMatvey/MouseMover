using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MouseMover
{
    public partial class MainWindow : Window
    {
        private const int HOTKEY_ID = 2134;
        private readonly HotKeyManager m_hotKeyManager;
        private readonly MousePositionChanger m_mousePositionChanger = new MousePositionChanger();
        public MainWindow()
        {
            InitializeComponent();
            m_hotKeyManager = new HotKeyManager(HOTKEY_ID, this);
            m_hotKeyManager.OnHotKeyPressed += m_mousePositionChanger.Toggle;
            m_mousePositionChanger.Toggled += () =>
            {
                if (!m_mousePositionChanger.IsTaskRunning)
                    RunBtn.Content = "Запуск (Ctrl+Shift+Q)";
                else
                    RunBtn.Content = "Стоп (Ctrl+Shift+Q)";
            };
            InitizalizeTextBoxes();
        }
        private void InitizalizeTextBoxes()
        {
            new TextBoxPair(minX, maxX);
            new TextBoxPair(minY, maxY);
            new TextBoxPair(minDelay, maxDelay);
            var rules = Rules.GetRules();
            minX.Text = rules.MinX.ToString();
            maxX.Text = rules.MaxX.ToString();
            minY.Text = rules.MinY.ToString();
            maxY.Text = rules.MaxY.ToString();
            minDelay.Text = rules.MinDelay.ToString();
            maxDelay.Text = rules.MaxDelay.ToString();
            TextBoxPair.MinMoreThanMax += MinMoreThanMax;
            foreach (var item in this.Descendants<TextBox>())
                new TextBoxValidation(item).StartValidating();
        }
        private void MinMoreThanMax(int id)
        {
            var rules = Rules.GetRules();
            switch (id)
            {
                case 0:
                    minX.Text = rules.MinX.ToString();
                    maxX.Text = rules.MaxX.ToString();
                    break;
                case 1:
                    minY.Text = rules.MinY.ToString();
                    maxY.Text = rules.MaxY.ToString();
                    break;
                case 2:
                    minDelay.Text = rules.MinDelay.ToString();
                    maxDelay.Text = rules.MaxDelay.ToString();
                    break;
                default:
                    throw new Exception("This pair does not exist");
            }
            var dialog = new Dialog("Минимальное значение не может быть больше максимального. Эта пара была возвращена к старому состоянию");
            dialog.ShowDialog();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void Button_Click(object sender, RoutedEventArgs e) => Close();

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            m_hotKeyManager.OnInitialized();
        }

        protected override void OnClosed(EventArgs e)
        {
            m_hotKeyManager.OnClosed();
            base.OnClosed(e);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) => m_mousePositionChanger.Toggle();

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var minx = int.Parse(minX.Text);
            var maxx = int.Parse(maxX.Text);
            var miny = int.Parse(minY.Text);
            var maxy = int.Parse(maxY.Text);
            var mindelay = int.Parse(minDelay.Text);
            var maxdelay = int.Parse(maxDelay.Text);
            var rules = new Rules(minx, maxx, miny, maxy, mindelay, maxdelay);
            Rules.SaveRules(rules);
        }
    }
}
