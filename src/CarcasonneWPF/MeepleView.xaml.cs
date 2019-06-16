using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using Carcassonne.Model;
using System.Diagnostics;
using Carcassonne.WPF.ViewModel;

namespace Carcassonne.WPF
{
    /// <summary>
    /// Interaction logic for MeepleControl.xaml
    /// </summary>
    public partial class MeepleView : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty OutlinedProperty = DependencyProperty.Register("Outlined", typeof(bool),
            typeof(MeepleView), new PropertyMetadata(false, new PropertyChangedCallback(updateColors)));
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Brush),
            typeof(MeepleView), new PropertyMetadata(s_defaultBrush, new PropertyChangedCallback(updateColors)));

        private static void updateColors(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender is MeepleView mc)
            {
                mc.updateMeepleColoring();
            }
        }

        private static readonly Brush s_defaultBrush = new SolidColorBrush(Colors.DarkGray);

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void notifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
        public MeepleView()
        {
            InitializeComponent();
            DataContextChanged += new DependencyPropertyChangedEventHandler(MeepleControl_DataContextChanged);
        }

        void MeepleControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            updateMeepleColoring();
        }

        public bool Outlined
        {
            get { return (bool)GetValue(OutlinedProperty); }
            set { SetValue(OutlinedProperty, value); }
        }

        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        private Brush m_meepleFill = s_defaultBrush;
        public Brush MeepleFill
        {
            get { return m_meepleFill; }
            private set
            {
                m_meepleFill = value;
                notifyPropertyChanged("MeepleFill");
            }
        }

        private Brush m_meepleStroke = s_defaultBrush;
        public Brush MeepleStroke
        {
            get { return m_meepleStroke; }
            private set
            {
                m_meepleStroke = value;
                notifyPropertyChanged("MeepleStroke");
            }
        }

        private void updateMeepleColoring()
        {
            Brush c = Color;
            if (DataContext is Meeple)
            {
                //var p = (DataContext as Meeple).Player as PlayerViewModel;
                //c = p.Color;
            }
            else if (DataContext is PlayerViewModel)
            {
                c =((PlayerViewModel)DataContext).Color;
            }
            if (Outlined)
            {
                Debug.WriteLine("Outlined!");
                MeepleFill = s_defaultBrush;
                MeepleStroke = c;
            }
            else
            {
                MeepleFill = c;
                MeepleStroke = s_defaultBrush;
            }
        }
    }
}
