using System;

namespace TADST
{
    [Serializable]
    class DifficultyItem
    {
        private string _name;
        private string _configName;
        private bool _isChecked;
        private bool _isEnabled;
        private string _info;

        public DifficultyItem() {}

        /// <summary>
        /// Difficulty item
        /// </summary>
        /// <param name="name">Name of item</param>
        /// <param name="configName">Name in config file</param>
        /// <param name="isChecked">True if the item is checked</param>
        /// <param name="isEnabled">True id the item checkstate is enabled</param>
        /// <param name="info">Info about the difficulty setting item</param>
        public DifficultyItem(string name, string configName, bool isChecked, bool isEnabled, string info)
        {
            Name = name;
            ConfigName = configName;
            IsChecked = isChecked;
            IsEnabled = isEnabled;
            Info = info;
        }

        public string GetConfigString()
        {
            return ConfigName + "=" + Convert.ToInt32(IsChecked) + ";";
        }

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

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }

        public string Info
        {
            get { return _info; }
            set { _info = value; }
        }

        public string ConfigName
        {
            get { return _configName; }
            set { _configName = value; }
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
