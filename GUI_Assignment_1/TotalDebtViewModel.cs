using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GUI_Assignment_1
{
    public class TotalDebtViewModel : BindableBase
    {
        public TotalDebtViewModel(string title, Debitor debitor, Debitor SumKeeper)
        {
            Title = title;
            _currentDebitor = debitor;
            value_rest = SumKeeper;
            _currentDebitor.Date = DateTime.Now.ToShortDateString();
            //debitors_log = debitors;
        }

        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                SetProperty(ref title, value);
            }
        }

        private Debitor _currentDebitor;

        public Debitor CurrentDebitor
        {
            get { return _currentDebitor; }
            set
            {
                SetProperty(ref _currentDebitor, value);
            }
        }

        private Debitor value_rest;

        public Debitor CurrentDebitor_sum_keeper
        {
            get { return value_rest; }
            set
            {
                SetProperty(ref value_rest, value);
            }
        }

        private ObservableCollection<Debitor> debitors;

        public ObservableCollection<Debitor> Debitors
        {
            get { return new ObservableCollection<Debitor>(SortForCurrentDebitor(debitors)); }
            set
            {
                SetProperty(ref debitors, value);
            }
        }

        public List<Debitor> SortForCurrentDebitor(ObservableCollection<Debitor> searchable)
        {
            ObservableCollection<Debitor> list = new ObservableCollection<Debitor>(searchable);
            return list.Where(i => i.Name == CurrentDebitor.Name).ToList();
        }
    }
}