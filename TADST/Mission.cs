using System;
using System.Collections.Generic;

namespace TADST
{
    [Serializable]
    class Mission
    {
        private string _name;
        private string _island;
        private bool _isChecked;
        private int _difficulty;
        private List<MissionParameter> _missionParameters;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Island
        {
            get { return _island; }
            set { _island = value; }
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; }
        }

        public List<MissionParameter> MissionParameters
        {
            get { return _missionParameters; }
            set { _missionParameters = value; }
        }

        public int Difficulty
        {
            get { return _difficulty; }
            set { _difficulty = value; }
        }

        public Mission() {}

        public Mission(string name, string island, bool isChecked)
        {
            Name = name;
            Island = island;
            IsChecked = isChecked;
            Difficulty = 1;
        }

        public Mission(string name, string island, bool isChecked, int difficulty)
        {
            Name = name;
            Island = island;
            IsChecked = isChecked;
            Difficulty = difficulty;
        }

        public Mission(Mission mission)
        {
            Name = mission.Name;
            Island = mission.Island;
            IsChecked = mission.IsChecked;
            Difficulty = mission.Difficulty;
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
