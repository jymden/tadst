using System;
using System.Collections.Generic;

namespace TADST
{
    [Serializable]
    internal class DifficultySetting
    {
        private decimal _skillAI;
        private decimal _precisionAI;
        private int _aiLevelPreset;

        private List<DifficultyItem> _difficultyItems;

        /// <summary>
        /// DifficultySettings
        /// </summary>
        /// <param name="skillFriendly">Friendly units skill (0-1)</param>
        /// <param name="PrecisionAI">Friendly units precicion (0-1)</param>
        /// <param name="skillEnemy">Enemy units skill (0-1)</param>
        /// <param name="precicionEnemy">Enemy units precicio (0-1)</param>
        public DifficultySetting(decimal skillAI, decimal precisionAI, int aiLevelPreset)
        {
            _difficultyItems = new List<DifficultyItem>();

            SkillAI = skillAI;
            PrecisionAI = precisionAI;
            AILevelPreset = aiLevelPreset;
        }

        public DifficultySetting(DifficultySetting original)
        {
            SkillAI = original.SkillAI;
            PrecisionAI = original.PrecisionAI;
            
            DifficultyItems = new List<DifficultyItem>(original.DifficultyItems);
        }


        public List<DifficultyItem> DifficultyItems
        {
            get { return _difficultyItems; }
            set { _difficultyItems = value; }
        }

        public decimal SkillAI
        {
            get { return _skillAI; }
            set { _skillAI = value; }
        }

        public decimal PrecisionAI
        {
            get { return _precisionAI; }
            set { _precisionAI = value; }
        }

        public int AILevelPreset
        {
            get { return _aiLevelPreset; }
            set { _aiLevelPreset = value; }
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
