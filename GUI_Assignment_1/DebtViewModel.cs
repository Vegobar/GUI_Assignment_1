using Prism.Mvvm;
using System;

namespace GUI_Assignment_1
{
    public class DebtViewModel : BindableBase
    {
        public DebtViewModel(string title, Debitor debt)
        {
            Title = title;
            currentDebt = debt;
            currentDebt.Date = DateTime.Now.ToShortDateString();
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private Debitor currentDebt;

        public Debitor CurrentDebt
        {
            get { return currentDebt; }
            set { SetProperty(ref currentDebt, value); }
        }

        private bool isValid;

        public bool IsValid
        {
            get
            {
                bool isValid = true;
                if (string.IsNullOrWhiteSpace(CurrentDebt.Name))
                    isValid = false;
                if (string.IsNullOrWhiteSpace(CurrentDebt.Sum))
                    isValid = false;

                return isValid;
            }
            set
            {
                SetProperty(ref isValid, value);
            }
        }
    }
}