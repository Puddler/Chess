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
using System.Windows.Shapes;
using Chess.VM;

namespace Chess.UI
{
    public partial class NewGameUI : Window
    {
        public static bool Reset = false;

        public NewGameUI()
        {
            InitializeComponent();
            NewGameVM.CloseAction = this.Close;
            this.BotList.Focus();
        }
        
        private void Close()
        {
            Reset = true;
            base.Close();
        }
    }
}