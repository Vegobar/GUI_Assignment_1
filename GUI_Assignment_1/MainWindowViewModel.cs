using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GUI_Assignment_1
{
    public class MainWindowViewModel : BindableBase
    {
        private ObservableCollection<Debitor> debitors;
        private string filename = "";

        private ObservableCollection<Debitor> clonedCollection;

        public MainWindowViewModel()
        {
            debitors = new ObservableCollection<Debitor>
            {
                new Debitor("Lena", "2000", "06/03-2020"),
                new Debitor("Mikkel", "-5000", "15/10-2018")
            };

            var clonedlist = Debitors.Select(objEntity => (Debitor)objEntity.Clone()).ToList();
            clonedCollection = new ObservableCollection<Debitor>(clonedlist);
        }

        public ObservableCollection<Debitor> Debitors
        {
            get { return debitors; }
            set { SetProperty(ref debitors, value); }
        }

        private Debitor _currentDebitor = null;

        public Debitor CurrentDebitor
        {
            get { return _currentDebitor; }
            set { SetProperty(ref _currentDebitor, value); }
        }

        private int currentIndex = 1;

        public int CurrentIndex
        {
            get { return currentIndex; }
            set { SetProperty(ref currentIndex, value); }
        }

        private ICommand _addnewCommand;

        public ICommand AddNewDebtCommand
        {
            get
            {
                return _addnewCommand ?? (_addnewCommand = new DelegateCommand(() =>
                {
                    var newDebitor = new Debitor();
                    var vmD = new DebtViewModel("Adding new debt", newDebitor);
                    var dlg = new Debt();
                    dlg.DataContext = vmD;
                    if (dlg.ShowDialog() == true)
                    {
                        Debitors.Add(newDebitor);
                        clonedCollection.Add(newDebitor.Clone());
                        CurrentDebitor = newDebitor;
                        CurrentIndex = 0;
                    }
                }));
            }
        }

        private ICommand _NewFileCommand;

        public ICommand NewFileCommand
        {
            get { return _NewFileCommand ?? (_NewFileCommand = new DelegateCommand(NewFileCommand_Execute)); }
        }

        private void NewFileCommand_Execute()
        {
            MessageBoxResult res = MessageBox.Show(
                "Any unsaved data will be lost. Are you sure you want to initiate a new file?", "Warning",
                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (res == MessageBoxResult.Yes)
            {
                Debitors.Clear();
                filename = "";
            }
        }

        private ICommand _OpenFileCommand;

        public ICommand OpenFileCommand
        {
            get
            {
                return _OpenFileCommand ?? (_OpenFileCommand = new DelegateCommand<string>(OpenFileCommand_Execute));
            }
        }

        private void OpenFileCommand_Execute(string argFilename)
        {
            if (argFilename == "")
            {
                MessageBox.Show("You must enter a file name in the File Name textbox!", "Unable to save file",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                filename = argFilename;
                var tempDebitors = new ObservableCollection<Debitor>();

                //Create an instance of the Xmlserializer class and specify the type of object to deserialize
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Debitor>));
                try
                {
                    TextReader reader = new StreamReader(filename);
                    //Deserialize all the debitors
                    tempDebitors = (ObservableCollection<Debitor>)serializer.Deserialize(reader);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Unable to open file", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                Debitors = tempDebitors;

                var clonedlist = tempDebitors.Select(objEntity => (Debitor)objEntity.Clone()).ToList();
                clonedCollection = new ObservableCollection<Debitor>(clonedlist);
            }
        }

        private ICommand _SaveAsCommand;

        public ICommand SaveAsCommand
        {
            get { return _SaveAsCommand ?? (_SaveAsCommand = new DelegateCommand<string>(SaveAsCommand_Execute)); }
        }

        private void SaveAsCommand_Execute(string argFilename)
        {
            if (argFilename == "")
            {
                MessageBox.Show("You must enter a file name in the File Name textbox!", "Unable to save file",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                filename = argFilename;
                SaveFileCommand_Execute();
            }
        }

        private ICommand _SaveCommand;

        public ICommand SaveCommand
        {
            get
            {
                return _SaveCommand ?? (_SaveCommand =
                           new DelegateCommand(SaveFileCommand_Execute, SaveFileCommand_CanExecute)
                               .ObservesProperty(() => Debitors.Count));
            }
        }

        private void SaveFileCommand_Execute()
        {
            //Create an instance of the XmlSerializer class and specify the type of object to serialize
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Debitor>));
            TextWriter writer = new StreamWriter(filename);

            //Serialize all the agents
            serializer.Serialize(writer, Debitors);
            writer.Close();
        }

        private bool SaveFileCommand_CanExecute()
        {
            return (filename != "") && (Debitors.Count > 0);
        }

        private ICommand _closeAppCommand;

        public ICommand CloseAppCommand
        {
            get
            {
                return _closeAppCommand ?? (_closeAppCommand = new DelegateCommand(() =>
                {
                    App.Current.MainWindow.Close();
                }));
            }
        }

        private bool dirty = false;

        public bool Dirty
        {
            get { return dirty; }
            set
            {
                SetProperty(ref dirty, value);
                RaisePropertyChanged("Title");
            }
        }

        private ICommand _seeTotalDebtCommand;

        public ICommand SeeTotalDebtCommand
        {
            get
            {
                return _seeTotalDebtCommand ?? (_seeTotalDebtCommand = new DelegateCommand(() =>
                {
                    var tempDebitor = CurrentDebitor.Clone();
                    var newDebitor = new Debitor();
                    newDebitor.Sum = "1";

                    var vm = new TotalDebtViewModel("Add debt", tempDebitor, newDebitor)
                    {
                        Debitors = clonedCollection
                    };

                    var dlg = new TotalDebtView
                    {
                        DataContext = vm,
                        Owner = App.Current.MainWindow
                    };

                    if (dlg.ShowDialog() == true)
                    {
                        tempDebitor.Sum = newDebitor.Sum;
                        CurrentDebitor.Date = tempDebitor.Date;
                        Debitors.Add(tempDebitor);
                        clonedCollection.Add(tempDebitor);

                        int result = Int32.Parse(CurrentDebitor.Sum);
                        int result_temp = Int32.Parse(newDebitor.Sum);

                        int final_result = result + result_temp;

                        CurrentDebitor.Sum = final_result.ToString();

                        Debitors.RemoveAt(Debitors.Count - 1);
                        Dirty = true;
                    }
                }));
            }
        }

        ICommand _ColorCommand;
        public ICommand ColorCommand
        {
            get
            {
                return _ColorCommand ?? (_ColorCommand = new
                  DelegateCommand<String>(ColorCommand_Execute));
            }
        }

        private void ColorCommand_Execute(String colorStr)
        {
            SolidColorBrush newBrush = SystemColors.WindowBrush; // Default color

            try
            {
                if (colorStr != null)
                {
                    if (colorStr != "Default")
                        newBrush = new BrushConverter().ConvertFromString(colorStr) as SolidColorBrush;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Unknown color name, default color is used", "Program error!");
            }

            Application.Current.Resources["BackgroundBrush"] = newBrush;
        }


    }
}
