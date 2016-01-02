using System;

namespace TADST
{
    [Serializable]
    class Mod
    {
        private string _name;
        private bool _isChecked;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; }
        }

        public Mod()
        {
            Name = "";
            IsChecked = false;
        }

        public Mod(string name)
        {
            Name = name;
            IsChecked = false;
        }

        public Mod(string name, bool isChecked)
        {
            Name = name;
            IsChecked = isChecked;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
