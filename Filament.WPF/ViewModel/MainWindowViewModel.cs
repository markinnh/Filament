using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Filament.WPF.ViewModel
{
    public class Empty { }
    [ViewModelDescriptor(Description ="Main ViewModel",Title ="Main")]
    public class MainWindowViewModel:BaseViewModel<Empty>
    {
        private ICommand saveFileCommand;
        public ICommand SaveFileCommand { get 
            { 
                if(saveFileCommand == null)
                {
                    saveFileCommand = new Helpers.RelayCommand(SaveDataFile);
                }
                return saveFileCommand;
            } 
            set=> saveFileCommand = value; }

        private void SaveFileAction()
        {
            DataFile.SaveFile();
            throw new NotImplementedException();
        }

        protected override void SaveNew()
        {
            throw new NotImplementedException();
        }
    }
}
