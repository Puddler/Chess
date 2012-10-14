using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.Foundation;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Chess.VM
{
    public class NewGameVM : VMBase
    {
        public static NewGameVM VM;
        public static Action CloseAction;
        
        private ObservableCollection<IChessBot> _bots;
        public ObservableCollection<IChessBot> Bots
        {
            get
            {
                if (_bots == null)
                {
                    _bots = new ObservableCollection<IChessBot>();
                }
                return _bots;
            }
            set
            {
                _bots = value;
                RaisePropertyChanged("Bots");
            }
        }

        private IChessBot _selectedBot;
        public IChessBot SelectedBot
        {
            get
            {
                return _selectedBot;
            }
            set
            {
                _selectedBot = value;
                RaisePropertyChanged("SelectedBot");
            }
        }

        private RelayCommand _startGameCommand;
        public RelayCommand StartGameCommand
        {
            get
            {
                if (_startGameCommand == null)
                {
                    _startGameCommand = new RelayCommand(this.StartGameExecute);
                }
                return _startGameCommand;
            }
        }

        public NewGameVM()
        {
            foreach (Type type in Assembly.GetCallingAssembly().GetTypes())
            {
                if(type.GetInterfaces().Contains(typeof(IChessBot)))
                {
                    this.Bots.Add((IChessBot)Activator.CreateInstance(type));
                }
            }
            this.Bots = new ObservableCollection<IChessBot>(this.Bots.OrderBy(o => o.Name));
            if(this.Bots.Count > 0) this.SelectedBot = this.Bots[0];
        }

        private void StartGameExecute()
        {
            This.Bot = this.SelectedBot;
            CloseAction();
        }
    }
}