using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using Carcassonne.WPF.ViewModel;

namespace Carcassonne.WPF
{
    /// <summary>
    /// Interaction logic for MeepleControl.xaml
    /// </summary>
    public partial class MeepleView : INotifyPropertyChanged
    {
        private static readonly Brush SDefaultBrush = new SolidColorBrush(Colors.DarkGray);
        public static readonly DependencyProperty OutlinedProperty = DependencyProperty.Register("Outlined", typeof(bool),
            typeof(MeepleView), new PropertyMetadata(false, UpdateColors));
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Brush),
            typeof(MeepleView), new PropertyMetadata(SDefaultBrush, UpdateColors));

        private static void UpdateColors(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender is MeepleView mc)
            {
                mc.UpdateMeepleColoring();
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
        public MeepleView()
        {
            InitializeComponent();
            PropertyChanged += (sender, args) => { };
            DataContextChanged += MeepleControl_DataContextChanged;
        }

        private void MeepleControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateMeepleColoring();
        }

        public bool Outlined
        {
            get => (bool)GetValue(OutlinedProperty);
            set => SetValue(OutlinedProperty, value);
        }

        public Brush Color
        {
            get => (Brush)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        private Brush m_meepleFill = SDefaultBrush;
        public Brush MeepleFill
        {
            get => m_meepleFill;
            private set
            {
                m_meepleFill = value;
                NotifyPropertyChanged("MeepleFill");
            }
        }

        private Brush m_meepleStroke = SDefaultBrush;
        public Brush MeepleStroke
        {
            get => m_meepleStroke;
            private set
            {
                m_meepleStroke = value;
                NotifyPropertyChanged("MeepleStroke");
            }
        }

        private void UpdateMeepleColoring()
        {
            var c = Color;
            // if (DataContext is Meeple m )
            // {
            //     //var p = (DataContext as Meeple).Player as PlayerViewModel;
            //     //c = p.Color;
            // }

            // else
            if (DataContext is PlayerViewModel pvm)
            {
                c = pvm.Color;
            }

            if (Outlined)
            {
                Debug.WriteLine("Outlined!");
                MeepleFill = SDefaultBrush;
                MeepleStroke = c;
            }
            else
            {
                MeepleFill = c;
                MeepleStroke = SDefaultBrush;
            }
        }
    }
}
