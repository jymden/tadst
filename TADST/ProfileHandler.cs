using System;
using System.Collections.Generic;
using System.Linq;


namespace TADST
{     
    [Serializable]
    class ProfileHandler
    {
        private List<Profile> _profiles;

        public ProfileHandler()
        {
            Profiles = new List<Profile>();
            InitProfiles();
        }

        public List<Profile> Profiles
        {
            get { return _profiles; }
            set { _profiles = value; }
        }

        private void InitProfiles()
        {
            if (Profiles.Count == 0)
            {
                var defaultProfile = new Profile();
                AddProfile(defaultProfile);
            }
        }

        public void AddProfile(Profile profile)
        {
            if (profile != null)
            {
                Profiles.Add(Utilities.DeepClone(profile));
            }      
        }

        public Profile GetProfile(int index)
        {
            if (RangeCheck(index))
            {
                return _profiles[index];
            }
            return null;
        }

        /// <summary>
        /// Check if the given index is within the range of the list
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool RangeCheck(int index)
        {
            return index > -1 && index < _profiles.Count();
        }

        /// <summary>
        /// Updates selected profile with current settings
        /// </summary>
        /// <param name="index">Index of profile</param>
        /// <param name="tempProfile">Profile object to replace old profile with</param>
        /// <returns>True if profile exists</returns>
        public bool UpdateProfile(int index, Profile tempProfile)
        {
            if (RangeCheck(index))
            {
                Profile profile = Utilities.DeepClone(tempProfile);
                _profiles[index] = profile;
                return true;
            }

            return false;
        }

        public void RemoveUser(int index)
        {
            if (RangeCheck(index))
            {
                _profiles.RemoveAt(index);
                InitProfiles();
            }
        }
    }
}
