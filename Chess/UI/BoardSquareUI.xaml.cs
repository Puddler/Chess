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
using Chess.VM;

namespace Chess.UI
{
    public partial class BoardSquareUI : UserControl
    {
        public static readonly DependencyProperty FileProperty = DependencyProperty.Register("File", typeof(char), typeof(UserControl));
        public static readonly DependencyProperty RankProperty = DependencyProperty.Register("Rank", typeof(int), typeof(UserControl));

        public char File
        {
            get { return (char)GetValue(FileProperty); }
            set { SetValue(FileProperty, value); }
        }
        
        public int Rank
        {
            get { return (int)GetValue(RankProperty); }
            set { SetValue(RankProperty, value); }
        }
        
        public BoardSquareUI()
        {
            InitializeComponent();            
            this.Loaded +=new RoutedEventHandler(BoardSquareUI_Loaded);
        }

        void BoardSquareUI_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = This.Board[this.File][this.Rank];
        }
    }
}