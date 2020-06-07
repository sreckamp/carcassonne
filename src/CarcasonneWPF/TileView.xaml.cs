using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using Carcassonne.Model;

//TODO: Make an "Over Region" property?
//TODO: Separate the Flower from the Paths/Cities edge region separate from Tile Region for Flower.
namespace Carcassonne.WPF
{
    public partial class TileView
    {
        public TileView()
        {
            InitializeComponent();
        }
    }
}