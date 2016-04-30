using System;
using System.Collections.Generic;

namespace TADST
{
    [Serializable]
    internal class DifficultySetting
    {
        private decimal _skillFriendly;
        private decimal _precisionFriendly;
        private decimal _skillEnemy;
        private decimal _precisionEnemy;

        private List<DifficultyItem> _difficultyItems;

        /// <summary>
        /// DifficultySettings
        /// </summary>
        /// <param name="skillFriendly">Friendly units skill (0-1)</param>
        /// <param name="precisionFriendly">Friendly units precicion (0-1)</param>
        /// <param name="skillEnemy">Enemy units skill (0-1)</param>
        /// <param name="precicionEnemy">Enemy units precicio (0-1)</param>
        public DifficultySetting(decimal skillFriendly, decimal precisionFriendly, decimal skillEnemy,
                                 decimal precicionEnemy)
        {
            _difficultyItems = new List<DifficultyItem>();

            SkillFriendly = skillFriendly;
            PrecisionFriendly = precisionFriendly;
            SkillEnemy = skillEnemy;
            PrecisionEnemy = precicionEnemy;
        }

        public DifficultySetting(DifficultySetting original)
        {
            SkillFriendly = original.SkillFriendly;
            PrecisionFriendly = original.PrecisionFriendly;
            SkillEnemy = original.SkillEnemy;
            PrecisionEnemy = original.PrecisionEnemy;
            
            DifficultyItems = new List<DifficultyItem>(original.DifficultyItems);
        }


        public List<DifficultyItem> DifficultyItems
        {
            get { return _difficultyItems; }
            set { _difficultyItems = value; }
        }

        public decimal SkillFriendly
        {
            get { return _skillFriendly; }
            set { _skillFriendly = value; }
        }

        public decimal PrecisionFriendly
        {
            get { return _precisionFriendly; }
            set { _precisionFriendly = value; }
        }

        public decimal SkillEnemy
        {
            get { return _skillEnemy; }
            set { _skillEnemy = value; }
        }

        public decimal PrecisionEnemy
        {
            get { return _precisionEnemy; }
            set { _precisionEnemy = value; }
        }

        public void AddDifficultyItem(DifficultyItem diffItem)
        {
            DifficultyItems.Add(diffItem);
        }

        public DifficultyItem ReturnDifficultyItem(int index)
        {
            return _difficultyItems[index];
        }

    }
}
