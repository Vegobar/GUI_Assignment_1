using Prism.Mvvm;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace GUI_Assignment_1
{
    public class Debitor : BindableBase, IDataErrorInfo
    {
        public Debitor()
        {
        }

        public Debitor(string dName, string dSum, string dDate)
        {
            _name = dName;
            _sum = dSum;
            _date = dDate;
        }

        private string _name = "";
        private string _sum = "";
        private string _date;

        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
            }
        }

        public string Sum
        {
            get { return _sum; }
            set { SetProperty(ref _sum, value); }
        }

        public string Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        public Debitor Clone()
        {
            return this.MemberwiseClone() as Debitor;
        }

        public string Error { get; }

        public string this[string columnName]
        {
            get
            {
                return Validate(columnName);
            }
        }

        private string Validate(string propertyName)
        {
            string validationMessage = string.Empty;

            if (propertyName == "Name")
            {
                if (!Regex.IsMatch(Name, @"^[a-zA-Z]+$"))

                    validationMessage = "Invalid data. You need to enter a valid name";
            }

            if (propertyName == "Sum")
            {
                Regex regex = new Regex(@"^(-?[1-9]+\d*([.]\d+)?)$|^(-?0[.]\d*[1-9]+)$|^0$|^0.0$");

                if (!regex.IsMatch(Sum))
                    validationMessage = "Invalid data. You need to enter a valid sum";
            }

            return validationMessage;
        }
    }
}