using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;


namespace TADST
{
    public partial class MainForm : Form
    {
        private readonly ProfileHandler _profileHandler;
        private Profile _activeProfile;
        private FileHandler _fileHandler;
        private int _profileIndex;
        public string SelectedDifficulty { get; set; }
        public bool IsDirty { get; set; }
        public bool IsDirtyEnabled { get; set; }

        public MainForm()
        {
            InitializeComponent();

            ValidateCurrentDirectory();

            _profileHandler = GetProfileHandler();
            _activeProfile = Utilities.DeepClone(_profileHandler.GetProfile(0));
            _fileHandler = new FileHandler(_activeProfile);

            IsDirty = false;
            IsDirtyEnabled = true;

            InitGui();
        }

        private void ValidateCurrentDirectory()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "MPMissions");
            if (!Directory.Exists(path))
            {
                MessageBox.Show("Are you sure you've put TADST in your ArmA game folder? " + Environment.NewLine +
                                "No 'MPMissions' folder is present in the current folder", "Cannot run TADST",
                                MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Close();
            }
        }

        private static ProfileHandler GetProfileHandler()
        {
            var path = Path.Combine(Environment.CurrentDirectory, @"TADST\TADST.profiles");
            return File.Exists(path) ? LoadProfilesFromDisk() : new ProfileHandler();
        }

        private void InitGui()
        {
            cmbProfiles.Items.AddRange(_profileHandler.Profiles.ToArray());
            cmbProfiles.SelectedIndex = 0;

            _activeProfile.ScanMissions();
            UpdateGui();

            cmbMissionFilter.SelectedIndex = 0;

            DefaultDifficultyInfoLabel();
            DefaultPerformanceInfoLabel();
        }

        private void DefaultPerformanceInfoLabel()
        {
            txtPerformanceInfo.Text =
                "Here you can may tune the servers performance and connectivity. Click a setting to get more detailed information.\n\n" +
                "The greatest level of optimization can be achieved by setting the Maximum Messages Send and Minimum Bandwidth parameters.\n\n" +
                "The admin can use the command #monitor to see server resource usage.\n\n" +
                "The server never runs at more than 50 fps. When running slower, it always uses all available CPU processing power to maintain the smoothest possible gameplay.\n\n" +
                "When running at less than 15 fps, the mission currently played is probably too complex for given server.\n\n" +
                "If you see the server is not using bandwidth that it could use, you can try increasing values MaxMsgSend and MinBandwidth.\n\n";
        }

        private void DefaultDifficultyInfoLabel()
        {
            lblProfileDifficultyInfo.Text =
                "In this tab you may set detailed properties for the different difficulty levels in the game.\n\n" +
                "The default difficulty setting for missions is set in the missions tab.\n\n" +
                "Select a property to get detailed information on it. Double click to check or uncheck items.";
        }

        private void UpdateGuiServerRules()
        {
            chkEnableVoting.Checked = _activeProfile.VotingEnabled;
            numVoteMissionPlayers.Value = _activeProfile.VoteMissionPlayers;
            numVoteThreshold.Value = _activeProfile.VoteThreshold*100;

            cmbVerifySignatures.SelectedIndex = _activeProfile.VerifySignatures;
            cmbRequiredSecureId.SelectedIndex = _activeProfile.RequiredSecureId;
            cmbAllowedFilePatching.SelectedIndex = _activeProfile.AllowFilePatching;
            chkRequiredBuild.Checked = _activeProfile.RequiredBuildEnabled;
            txtRequiredBuild.Text = _activeProfile.RequiredBuild.ToString(CultureInfo.InvariantCulture);
            chkKickDuplicates.Checked = _activeProfile.KickDuplicates;

            chkBattlEye.Checked = _activeProfile.BattlEye;


            txtMotd.Text = string.Join(Environment.NewLine, _activeProfile.Motd);
            numMotdInterval.Value = _activeProfile.MotdInterval;
        }


        private void UpdateGui()
        {
            UpdateGuiProfile();
            UpdateGuiServerDetails();
            UpdateGuiRptTimeStamp();
            UpdateGuiServerLogging();
            UpdateGuiServerRules();
            UpdateGuiMissionFilter();
            UpdateGuiMissions();
            UpdateGuiMods();
            UpdateGuiStartupParameters();
            UpdateGuiPerformance();
            UpdateGuiView();
            UpdateGuiDifficulty(GetCurrentDifficulty());
            UpdateServerScripting();

            cmbDefaultDifficulty.SelectedIndex = _activeProfile.DefaultDifficulty;

            chkPersistentBattlefield.Checked = _activeProfile.PersistantBattlefield;
            chkBattlEye.Checked = _activeProfile.BattlEye;

            numCodecQuality.Value = _activeProfile.VonQuality;
            chkDisableVon.Checked = _activeProfile.DisableVon;
        }

        private void UpdateGuiStartupParameters()
        {
            txtStartupParameters.Text = _activeProfile.GetStartupParameters();
        }

        private void UpdateGuiView()
        {
            numTerrainGrid.Value = _activeProfile.TerrainGrid;
            numViewDistance.Value = _activeProfile.ViewDistance;
        }

        private void UpdateGuiPerformance()
        {
            txtMaxMessagesSend.Text = _activeProfile.MaxMsgSend.ToString();
            txtMaxSizeGuaranteed.Text = _activeProfile.MaxSizeGuaranteed.ToString();
            txtMaxSizeNonguaranteed.Text = _activeProfile.MaxSizeNonGuaranteed.ToString();
            txtMinBandwidth.Text = (_activeProfile.MinBandWidth/1024).ToString();
            txtMaxBandwidth.Text = (_activeProfile.MaxBandwidth/1048576).ToString();
            txtMinErrorToSend.Text = _activeProfile.MinErrorToSend.ToString();
            txtMinErrorToSendNear.Text = _activeProfile.MinErrorToSendNear.ToString();
            txtMaxCustomFileSize.Text = (_activeProfile.MaxCustomFileSize/8192).ToString();
            txtMaxPacketSize.Text = _activeProfile.MaxPacketSize.ToString(CultureInfo.InvariantCulture);

            chkEnableHT.Checked = _activeProfile.EnableHt;
        }

        private void UpdateGuiMods()
        {
            lstMods.Items.Clear();
            foreach (var mod in _activeProfile.Mods)
            {
                lstMods.Items.Add(mod.ToString(), mod.IsChecked);
            }

            UpdateGuiNumOfMods();
            UpdateGuiSelectedMods();
        }

        private void UpdateGuiSelectedMods()
        {
            var mods = _activeProfile.Mods.Where(m => m.IsChecked);
            lblSelectedMods.Text = mods.Count().ToString();
            UpdateGuiStartupParameters();
        }

        private void UpdateGuiNumOfMods()
        {
            lblNumOfMods.Text = lstMods.Items.Count.ToString();
        }

        private void UpdateGuiMissions()
        {
            IEnumerable<Mission> missions;

            int selectedIndex = cmbMissionFilter.SelectedIndex < 0 ? 0 : cmbMissionFilter.SelectedIndex;

            if (selectedIndex == 0)
                missions = _activeProfile.Missions;
            else
            {
                string filter = cmbMissionFilter.Items[cmbMissionFilter.SelectedIndex].ToString();
                missions = _activeProfile.Missions.Where(island => island.Island == filter);
            }

            lstMissions.Items.Clear();
            foreach (Mission mission in missions)
            {
                lstMissions.Items.Add(mission.ToString(), mission.IsChecked);
            }

            UpdateGuiNumOfMissions();
            UpdateGuiSelectedMissions();

            cmbDifficulty.SelectedIndex = _activeProfile.MissionDifficulty;
        }

        private void UpdateGuiNumOfMissions()
        {
            lblNumOfMissions.Text = lstMissions.Items.Count.ToString();
        }

        private void UpdateGuiMissionFilter()
        {
            cmbMissionFilter.Items.Clear();
            cmbMissionFilter.Items.Add("All");
            cmbMissionFilter.Items.AddRange(GetIslands().ToArray());
            cmbMissionFilter.SelectedIndex = 0;
        }

        /// <summary>
        /// Get all available islands from the active profiles mission list
        /// </summary>
        /// <returns>List of available islands</returns>
        private List<string> GetIslands()
        {
            var islands = new List<string>();

            foreach (Mission mission in _activeProfile.Missions)
            {
                //string island = mission.ToString().Split('.')[mission.ToString().Split('.').Count() - 2].ToLower();

                string island = mission.Island;

                island = Utilities.ToMixedCase(island);

                if (!islands.Contains(island))
                {
                    islands.Add(island);
                }
            }

            islands.Sort();
            return islands;
        }

        private void UpdateGuiServerLogging()
        {
            txtConsoleLogfile.Text = _activeProfile.ConsoleLogfile;
            txtRankingFile.Text = _activeProfile.RankingFile;
            txtPidfile.Text = _activeProfile.PidFile;
            cmbTimeStampFormat.SelectedIndex = _activeProfile.RptTimeStampIndex;
            chkNetlog.Checked = _activeProfile.Netlog;
        }

        private void UpdateGuiServerDetails()
        {
            txtServerName.Text = _activeProfile.ServerName;
            txtPassword.Text = _activeProfile.Password;
            txtAdminPassword.Text = _activeProfile.AdminPassword;
            txtPort.Text = _activeProfile.Port.ToString();
            numMaxPlayers.Value = _activeProfile.MaxPlayers;
            chkPersistentBattlefield.Checked = _activeProfile.PersistantBattlefield;
            chkAutoinit.Checked = _activeProfile.AutoInit;
            chkLoopback.Checked = _activeProfile.Loopback;


            chkHeadlessEnabled.Checked = _activeProfile.HeadlessEnabled;
            txtHeadlessIp.Text = _activeProfile.HeadlessIps;
            txtLocalIp.Text = _activeProfile.LocalIps;
        }

        private void UpdateGuiProfile()
        {
            txtServerExe.Text = _activeProfile.ServerExePath;
            txtExtraParameters.Text = _activeProfile.ExtraParameters;
            //txtIngameName.Text = _activeProfile.InGameName;

            chkBeta.Checked = _activeProfile.Beta;
            chkTooltips.Checked = _activeProfile.ToolTips;
            chkStartAsIs.Checked = _activeProfile.LaunchAsIs;
            chkAutoExit.Checked = _activeProfile.AutoExit;
        }

        /// <summary>
        /// Update RPT timestamp format list from profile
        /// </summary>
        private void UpdateGuiRptTimeStamp()
        {
            cmbTimeStampFormat.Items.Clear();
            cmbTimeStampFormat.Items.AddRange(_activeProfile.RptTimeStamps.ToArray());
        }

        private void UpdateServerScripting()
        {
            txtDoubleIdDetected.Text = _activeProfile.DoubleIdDetected;
            txtOnUserConnected.Text = _activeProfile.OnUserConnected;
            txtOnUserDisconnected.Text = _activeProfile.OnUserDisconnected;
            txtOnHackedData.Text = _activeProfile.OnHackedData;
            txtOnDifferentData.Text = _activeProfile.OnDifferentData;
            txtOnUnsignedData.Text = _activeProfile.OnUnsignedData;
            txtRegularCheck.Text = _activeProfile.RegularCheck;
        }

        /// <summary>
        /// Update difficulty values in Gui
        /// </summary>
        private void UpdateGuiDifficulty(DifficultySetting difficulty)
        {
            IsDirtyEnabled = false;
            GetDifficultyItems(difficulty.DifficultyItems);
            GetSkillValues(difficulty);
            IsDirtyEnabled = true;
        }

        /// <summary>
        /// Get skill values from selected difficulty
        /// </summary>
        private void GetSkillValues(DifficultySetting difficulty)
        {
            numSkillAI.Value = difficulty.SkillAI;
            numPrecisionAI.Value = difficulty.PrecisionAI;
            cmbAILevelPreset.SelectedIndex = difficulty.AILevelPreset;
        }

        /// <summary>
        /// Update difficulty items listbox in Gui
        /// </summary>
        /// <param name="diffItems">The list of items to use</param>
        private void GetDifficultyItems(IEnumerable<DifficultyItem> diffItems)
        {
            clbDifficultyItems.Items.Clear();

            foreach (DifficultyItem item in diffItems)
            {
                clbDifficultyItems.Items.Add(item.ToString(), item.IsChecked);
            }

        }

        private void btnProfileNew_Click(object sender, EventArgs e)
        {
            NewProfile();
        }

        /// <summary>
        /// Create and save new profile with current settings
        /// </summary>
        private void NewProfile()
        {
            var profileNameForm = new NewProfileForm();

            if (profileNameForm.ShowDialog() == DialogResult.OK)
            {
                _activeProfile.ProfileName = profileNameForm.ProfileName;
                _profileHandler.AddProfile(Utilities.DeepClone(_activeProfile));

                UpdateProfiles();
                cmbProfiles.SelectedIndex = cmbProfiles.Items.Count - 1;
                LoadProfile();
                SaveProfilesToDisk();
            }
        }

        /// <summary>
        /// Write profiles to disk
        /// </summary>
        private void SaveProfilesToDisk()
        {
            var serializer = new BinaryFormatter();

            var filename = Path.Combine(Environment.CurrentDirectory, @"TADST\TADST.profiles");
            using (FileStream fileStream = File.OpenWrite(filename))
            {
                serializer.Serialize(fileStream, _profileHandler);
            }
        }

        /// <summary>
        /// Load profiles from disk
        /// </summary>
        /// <returns>A ProfileHandler object deserialized from disk</returns>
        private static ProfileHandler LoadProfilesFromDisk()
        {
            var serializer = new BinaryFormatter();
            var filename = Path.Combine(Environment.CurrentDirectory, @"TADST\TADST.profiles");

            using (FileStream fileStream = File.OpenRead(filename))
            {
                ProfileHandler profileHandler;

                try
                {
                    profileHandler = (ProfileHandler) serializer.Deserialize(fileStream);
                }
                catch (System.Runtime.Serialization.SerializationException)
                {
                    MessageBox.Show(
                        "Sorry, your 'TADST.profiles' file is either corrupted or incompatible with the current version of TADST. "
                        + Environment.NewLine + Environment.NewLine
                        + "Please remove it or save profile to overwrite it.", "SAVE FILE CORRUPTION!",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    profileHandler = new ProfileHandler();
                }
                return profileHandler;
            }
        }

        /// <summary>
        /// Get profiles from profile handler and put them in the profiles menu
        /// </summary>
        private void UpdateProfiles()
        {
            cmbProfiles.Items.Clear();
            cmbProfiles.Items.AddRange(_profileHandler.Profiles.ToArray());

            if (cmbProfiles.Items.Count > 0)
            {
                cmbProfiles.SelectedIndex = _profileIndex;
                LoadProfile();
            }
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            AddIsDirtyEventHandler(this);
        }

        /// <summary>
        /// Recursive method to add the IsDirty event handler to all controls.
        /// </summary>
        /// <param name="parent"></param>
        private void AddIsDirtyEventHandler(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox || c is MaskedTextBox)
                {
                    c.TextChanged += IsDirtyTrueHandler;
                }
                else if (c is CheckBox)
                {
                    ((CheckBox) c).CheckStateChanged += IsDirtyTrueHandler;
                }
                else if (c is NumericUpDown)
                {
                    ((NumericUpDown) c).ValueChanged += IsDirtyTrueHandler;
                }
                else
                {
                    AddIsDirtyEventHandler(c);
                }
            }
        }

        private void numSkillAI_Click(object sender, EventArgs e)
        {
            lblProfileDifficultyInfo.Text = "AI tactics skill";
        }

        private void numPrecisionAI_Click(object sender, EventArgs e)
        {
            lblProfileDifficultyInfo.Text = "AI shooting precision";
        }

        private void tabContainer_Click(object sender, EventArgs e)
        {
            DefaultDifficultyInfoLabel();
            DefaultPerformanceInfoLabel();
        }

        private void chkTooltips_CheckedChanged(object sender, EventArgs e)
        {
            toolTip1.Active = chkTooltips.Checked;
            _activeProfile.ToolTips = chkTooltips.Checked;
        }

        private void chkRequiredBuild_CheckedChanged(object sender, EventArgs e)
        {
            txtRequiredBuild.Enabled = chkRequiredBuild.Checked;
            _activeProfile.RequiredBuildEnabled = chkRequiredBuild.Checked;
        }

        private void tabPerformance_Click(object sender, EventArgs e)
        {
        }

        private void chkEnableVoting_CheckedChanged(object sender, EventArgs e)
        {
            numVoteMissionPlayers.Enabled = chkEnableVoting.Checked;
            numVoteThreshold.Enabled = chkEnableVoting.Checked;
            _activeProfile.VotingEnabled = chkEnableVoting.Checked;
        }

        private void rbRecruit_CheckedChanged(object sender, EventArgs e)
        {
            this.clbDifficultyItems.Enabled = false;
            UpdateGuiDifficulty(GetCurrentDifficulty());
        }

        private void rbRegular_CheckedChanged(object sender, EventArgs e)
        {
            this.clbDifficultyItems.Enabled = false;
            UpdateGuiDifficulty(GetCurrentDifficulty());
        }

        private void rbVeteran_CheckedChanged(object sender, EventArgs e)
        {
            this.clbDifficultyItems.Enabled = false;
            UpdateGuiDifficulty(GetCurrentDifficulty());
        }

        private void rbExpert_CheckedChanged(object sender, EventArgs e)
        {
            this.clbDifficultyItems.Enabled = false;
            UpdateGuiDifficulty(GetCurrentDifficulty());
        }

        private void rbCustom_CheckedChanged(object sender, EventArgs e)
        {
            this.clbDifficultyItems.Enabled = true;
            UpdateGuiDifficulty(GetCurrentDifficulty());
        }

        private void clbDifficultyItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblProfileDifficultyInfo.Text =
                GetCurrentDifficulty().DifficultyItems[clbDifficultyItems.SelectedIndex].Info;
        }

        private void resetProfile_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("Reset '" + SelectedDifficulty + "' settings to default values?"),
                                "Reset difficulty settings", MessageBoxButtons.YesNo) != DialogResult.Yes) return;


            var factory = new DifficultyFactory();


            _activeProfile.DiffCustom = factory.CreateCustomDifficulty();
            UpdateGuiDifficulty(_activeProfile.DiffCustom);
            IsDirty = true;
        }

        private void lblProfileDifficultyInfo_Click(object sender, EventArgs e)
        {
            DefaultDifficultyInfoLabel();
        }

        private void txtMaxBandwidth_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var value = string.IsNullOrEmpty(txtMaxBandwidth.Text)
                                ? 0
                                : Convert.ToInt32(txtMaxBandwidth.Text)*1048576;
                toolTip1.SetToolTip(txtMaxBandwidth, "Value added to config-file:  " + value + " bits");
                _activeProfile.MaxBandwidth = value;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                txtMaxBandwidth.Text = "0";
            }
        }

        private void txtMinBandwidth_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var value = string.IsNullOrEmpty(txtMinBandwidth.Text) ? 0 : Convert.ToInt32(txtMinBandwidth.Text)*1024;

                toolTip1.SetToolTip(txtMinBandwidth, "Value added to config:  " + value + " bits");
                _activeProfile.MinBandWidth = value;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                txtMinBandwidth.Text = "0";
            }
        }

        private void txtMaxCustomFileSize_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var value = string.IsNullOrEmpty(txtMaxCustomFileSize.Text)
                                ? 0
                                : Convert.ToInt32(txtMaxCustomFileSize.Text)*8192;

                toolTip1.SetToolTip(txtMaxCustomFileSize, "Value added to config:  " + value + " bits");
                _activeProfile.MaxCustomFileSize = value;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                txtMaxCustomFileSize.Text = "0";
            }
        }

        /// <summary>
        /// Check if pressed key is a digit
        /// </summary>
        /// <param name="e">Pressed key</param>
        private static void CheckIfKeyIsDigit(KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private static void CheckIfKeyIsDigitOrDot(KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) &&
                !char.IsControl(e.KeyChar) &&
                e.KeyChar != '.' &&
                e.KeyChar != ',')
            {
                e.Handled = true;
            }
        }

        private static void CheckKeyForFile(KeyPressEventArgs e)
        {
            if (e.KeyChar == '/' ||
                e.KeyChar == '\u005C' ||
                e.KeyChar == ':' ||
                e.KeyChar == '?' ||
                e.KeyChar == '*' ||
                e.KeyChar == '"' ||
                e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }

        private void txtMaxMessagesSend_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckIfKeyIsDigit(e);
        }

        private void txtMaxSizeGuaranteed_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckIfKeyIsDigit(e);
        }

        private void txtMaxSizeNonguaranteed_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckIfKeyIsDigit(e);
        }

        private void txtMinBandwidth_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckIfKeyIsDigit(e);
        }

        private void txtMaxBandwidth_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckIfKeyIsDigit(e);
        }

        private void txtMaxCustomFileSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckIfKeyIsDigit(e);
        }

        private void txtMinErrorToSend_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckIfKeyIsDigitOrDot(e);
        }

        private void txtMinErrorToSendNear_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckIfKeyIsDigitOrDot(e);
        }

        private void txtPerformanceInfo_Click(object sender, EventArgs e)
        {
            DefaultPerformanceInfoLabel();
        }

        private void clbDifficultyItems_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var index = clbDifficultyItems.SelectedIndex;

            DifficultySetting difficulty = GetCurrentDifficulty();

            if (index > -1)
            {
                if (!difficulty.DifficultyItems[index].IsEnabled)
                {
                    e.NewValue = e.CurrentValue;
                }
                else
                {
                    GetCurrentDifficulty().DifficultyItems[index].IsChecked = Convert.ToBoolean(e.NewValue);
                    IsDirty = true;
                }
            }
        }

        private DifficultySetting GetCurrentDifficulty()
        {
            SelectedDifficulty = "custom";
            return _activeProfile.DiffCustom;
        }

        private void numSkillAI_ValueChanged(object sender, EventArgs e)
        {
            GetCurrentDifficulty().SkillAI = numSkillAI.Value;
        }

        private void numPrecisionAI_ValueChanged(object sender, EventArgs e)
        {
            GetCurrentDifficulty().PrecisionAI = numPrecisionAI.Value;
        }

        private void txtMaxMessagesSend_Click(object sender, EventArgs e)
        {
            txtPerformanceInfo.Text =
                "Maximum number of messages that can be sent in one simulation cycle.\n\nIncreasing this value can decrease lag on high upload bandwidth servers.\n\nDefault number of messages is 128";
        }

        private void txtMaxSizeGuaranteed_Click(object sender, EventArgs e)
        {
            txtPerformanceInfo.Text =
                "Maximum size of guaranteed packet in bytes (without headers). Small messages are packed to larger frames.\n\nGuaranteed messages are used for non-repetitive events like shooting.\n\nDefault value is 512 Bytes";
        }

        private void txtMaxSizeNonguaranteed_Click(object sender, EventArgs e)
        {
            txtPerformanceInfo.Text =
                "Maximum size of non-guaranteed packet in bytes (without headers).\n\nNon-guaranteed messages are used for repetitive updates like soldier or vehicle position. \n\nIncreasing this value may improve bandwidth requirement, but it may increase lag.\n\nDefault value is 256 Bytes";
        }

        private void txtMinBandwidth_Click(object sender, EventArgs e)
        {
            txtPerformanceInfo.Text =
                "Bandwidth the server is guaranteed to have. \n\nThis value helps server to estimate bandwidth available. Increasing it to too optimistic values can increase lag and CPU load, as too many messages will be sent but discarded.\n\nEnter in kbit/s, application will convert into bits for the config file. You may hold the pointer over the textfield to see the value converted into bits.\n\nDefault value is 128 kbit/s (131072 bits)";
        }

        private void txtMaxBandwidth_Click(object sender, EventArgs e)
        {
            txtPerformanceInfo.Text =
                "Bandwidth the server is guaranteed to never have. This value helps the server to estimate bandwidth available.\n\nEnter in mbit/s, application will convert into bits for the config file. You may hold the pointer over the textfield to see the value converted into bits.\n\nDefault value is 2000 mbit/s.\n(maximum amount the config will use is 2047mbit/s)";
        }

        private void txtMinErrorToSend_Click(object sender, EventArgs e)
        {
            txtPerformanceInfo.Text =
                "Minimum errors to send updates across network.\n\nLower values can make units observed by binoculars or sniper rifle move smoother, but will impact server performance.\n\nDefault value is 0.001";
        }

        private void txtMinErrorToSendNear_Click(object sender, EventArgs e)
        {
            txtPerformanceInfo.Text =
                "Minimum errors to send updates across network for near units.\n\nUsing larger value can reduce traffic sent for near units. Used to control client to server traffic as well.\n\nDefault: 0.01";
        }

        private void txtMaxCustomFileSize_Click(object sender, EventArgs e)
        {
            txtPerformanceInfo.Text =
                "Maximum custom File Size. Users with custom face or custom sound larger than this size are kicked when trying to connect.\n\nEnter in kilobytes (kB), the launcher will convert the amount to bits for server config. You may hold the pointer over the textfield to see the value converted into bits\n\nDefault file size is 160kB";
        }

        private void numTerrainGrid_Click(object sender, EventArgs e)
        {
            txtPerformanceInfo.Text =
                "Terrain Grid\n\nSet default terrain resolution. This also sets the drawing distance of grass. Lower value will render grass at greater distance. Set to 50 to turn grass off completely.\n\nThis value you choose here may be overidden by scripting in the mission.\n\nDefault Multiplayer value is 25.";
        }

        private void numViewDistance_Click(object sender, EventArgs e)
        {
            txtPerformanceInfo.Text =
                "View Distance.\n\nSets the server default view distance. Minimum is 500 meters, maximum is 10,000 meters.\n\nRender distance is 3/4 of view distance - for 2000 metres, objects will be render up to 1500 metres. \n\nView distance also defines the maximum distance between a unit and any other unit they can know about. Higher view distance will involve more AI simulation cycles for every unit, which might cause lower performance.\n\nThis setting may be overridden by mission scripting.\n\nDefault is 2000";
        }

        private void btnResetPerformance_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("Reset performance tuning settings to default?", "Reset performance",
                                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _activeProfile.SetDefaultPerformance();
                UpdateGuiPerformance();
            }
        }

        private void btnProfileSave_Click(object sender, EventArgs e)
        {
            SaveProfileDialog();
        }

        private void SaveProfileDialog()
        {
            DialogResult dialog = MessageBox.Show(
                "Are you sure you wish to save the profile '" + cmbProfiles.SelectedItem +
                "' with the current settings?"
                , "Save profile?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                SaveProfile();
            }
        }

        private void SaveProfile()
        {
            if (_profileHandler.UpdateProfile(cmbProfiles.SelectedIndex, _activeProfile))
            {
                _profileIndex = cmbProfiles.SelectedIndex;
                UpdateProfiles();
                SaveProfilesToDisk();
            }
        }

        private void cmbProfiles_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (IsDirty)
            {
                if (MessageBox.Show("Switch profile without saving changes?", "Switch without saving?",
                                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    LoadProfile();
                }
                else
                {
                    cmbProfiles.SelectedIndex = _profileIndex;
                }
            }
            else
            {
                LoadProfile();
            }
        }

        private void LoadProfile()
        {
            IsDirtyEnabled = false; // Walkaround, fix this a rainy day

            int index = cmbProfiles.SelectedIndex;
            _activeProfile = Utilities.DeepClone(_profileHandler.GetProfile(index));
            _fileHandler = new FileHandler(_activeProfile);
            cmbMissionFilter.SelectedIndex = 0;
            _activeProfile.ScanMissions();
            _activeProfile.ScanMods();
            _profileIndex = index;
            UpdateGui();

            IsDirtyEnabled = true;
            IsDirty = false;
        }

        private void txtServerName_TextChanged(object sender, EventArgs e)
        {
            _activeProfile.ServerName = txtServerName.Text;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            _activeProfile.Password = txtPassword.Text;
        }

        private void numMaxPlayers_ValueChanged(object sender, EventArgs e)
        {
            _activeProfile.MaxPlayers = (int) numMaxPlayers.Value;
        }

        private void txtAdminPassword_TextChanged(object sender, EventArgs e)
        {
            _activeProfile.AdminPassword = txtAdminPassword.Text;
        }

        private void txtConsoleLogfile_TextChanged(object sender, EventArgs e)
        {
            _activeProfile.ConsoleLogfile = txtConsoleLogfile.Text;
            UpdateGuiStartupParameters();
        }

        private void txtRankingFile_TextChanged(object sender, EventArgs e)
        {
            _activeProfile.RankingFile = txtRankingFile.Text;
            UpdateGuiStartupParameters();
        }

        private void txtPidfile_TextChanged(object sender, EventArgs e)
        {
            _activeProfile.PidFile = txtPidfile.Text;
            UpdateGuiStartupParameters();
        }

        private void cmbTimeStampFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            _activeProfile.RptTimeStampIndex = cmbTimeStampFormat.SelectedIndex;
        }

        private void chkNetlog_CheckedChanged(object sender, EventArgs e)
        {
            _activeProfile.Netlog = chkNetlog.Checked;
            UpdateGuiStartupParameters();
        }

        private void chkPersistentBattlefield_CheckedChanged(object sender, EventArgs e)
        {
            _activeProfile.PersistantBattlefield = chkPersistentBattlefield.Checked;
            chkAutoinit.Enabled = chkPersistentBattlefield.Checked;
            UpdateGuiStartupParameters();
        }

        private void numCodecQuality_ValueChanged(object sender, EventArgs e)
        {
            _activeProfile.VonQuality = (int) numCodecQuality.Value;
        }

        private void chkDisableVon_CheckedChanged(object sender, EventArgs e)
        {
            _activeProfile.DisableVon = chkDisableVon.Checked;
        }

        private void txtServerExe_TextChanged(object sender, EventArgs e)
        {
            _activeProfile.ServerExePath = txtServerExe.Text;
            chkBeta.Checked = (_activeProfile.ServerExePath.Contains(@"\beta") &&
                               _activeProfile.ServerExePath.Contains("arma2"))
                                  ? true
                                  : false;
        }

        private void numVoteMissionPlayers_ValueChanged(object sender, EventArgs e)
        {
            _activeProfile.VoteMissionPlayers = (int) numVoteMissionPlayers.Value;
        }

        private void numVoteThreshold_ValueChanged(object sender, EventArgs e)
        {
            _activeProfile.VoteThreshold = numVoteThreshold.Value/100;
        }

        private void cmbVerifySignatures_SelectedIndexChanged(object sender, EventArgs e)
        {
            _activeProfile.VerifySignatures = cmbVerifySignatures.SelectedIndex;
        }

        private void txtRequiredBuild_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _activeProfile.RequiredBuild = string.IsNullOrEmpty(txtRequiredBuild.Text)
                                                   ? 0
                                                   : Convert.ToInt32(txtRequiredBuild.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                txtRequiredBuild.Text = "";
            }
        }

        private void chkKickDuplicates_CheckedChanged(object sender, EventArgs e)
        {
            _activeProfile.KickDuplicates = chkKickDuplicates.Checked;
        }

        private void numMotdInterval_ValueChanged(object sender, EventArgs e)
        {
            _activeProfile.MotdInterval = (int) numMotdInterval.Value;
        }

        private void cmbDifficulty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMissions.SelectedIndex > -1)
            {
                Mission mission =
                    _activeProfile.Missions.Find(
                        mis => mis.Name == lstMissions.Items[lstMissions.SelectedIndex].ToString());
                mission.Difficulty = cmbDifficulty.SelectedIndex;
                IsDirty = true;
            }
        }

        private void txtMaxMessagesSend_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _activeProfile.MaxMsgSend = string.IsNullOrEmpty(txtMaxMessagesSend.Text)
                                                ? 0
                                                : Convert.ToInt32(txtMaxMessagesSend.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                txtMaxMessagesSend.Text = "0";
            }
        }

        private void txtMaxSizeGuaranteed_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _activeProfile.MaxSizeGuaranteed = string.IsNullOrEmpty(txtMaxSizeGuaranteed.Text)
                                                       ? 0
                                                       : Convert.ToInt32(txtMaxSizeGuaranteed.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                txtMaxSizeGuaranteed.Text = "0";
            }
        }

        private void txtMaxSizeNonguaranteed_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _activeProfile.MaxSizeNonGuaranteed = string.IsNullOrEmpty(txtMaxSizeNonguaranteed.Text)
                                                          ? 0
                                                          : Convert.ToInt32(txtMaxSizeNonguaranteed.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                txtMaxSizeNonguaranteed.Text = "0";
            }
        }

        private void txtMinErrorToSend_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _activeProfile.MinErrorToSend = string.IsNullOrEmpty(txtMinErrorToSend.Text)
                                                    ? 0
                                                    : Convert.ToDouble(txtMinErrorToSend.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                txtMinErrorToSend.Text = "0";
            }
        }

        private void txtMinErrorToSendNear_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _activeProfile.MinErrorToSendNear = string.IsNullOrEmpty(txtMinErrorToSendNear.Text)
                                                        ? 0
                                                        : Convert.ToDouble(txtMinErrorToSendNear.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                txtMinErrorToSendNear.Text = "0";
            }
        }

        private void numTerrainGrid_ValueChanged(object sender, EventArgs e)
        {
            _activeProfile.TerrainGrid = numTerrainGrid.Value;
        }

        private void numViewDistance_ValueChanged(object sender, EventArgs e)
        {
            _activeProfile.ViewDistance = (int) numViewDistance.Value;
        }

        private void txtDoubleIdDetected_TextChanged(object sender, EventArgs e)
        {
            _activeProfile.DoubleIdDetected = txtDoubleIdDetected.Text;
        }

        private void txtOnUserConnected_TextChanged(object sender, EventArgs e)
        {
            _activeProfile.OnUserConnected = txtOnUserConnected.Text;
        }

        private void txtOnUserDisconnected_TextChanged(object sender, EventArgs e)
        {
            _activeProfile.OnUserDisconnected = txtOnUserConnected.Text;
        }

        private void txtOnHackedData_TextChanged(object sender, EventArgs e)
        {
            _activeProfile.OnHackedData = txtOnHackedData.Text;
        }

        private void txtOnDifferentData_TextChanged(object sender, EventArgs e)
        {
            _activeProfile.OnDifferentData = txtOnDifferentData.Text;
        }

        private void txtOnUnsignedData_TextChanged(object sender, EventArgs e)
        {
            _activeProfile.OnUnsignedData = txtOnUnsignedData.Text;
        }

        private void txtRegularCheck_TextChanged(object sender, EventArgs e)
        {
            _activeProfile.RegularCheck = txtRegularCheck.Text;
        }

        private void chkAutoExit_CheckedChanged(object sender, EventArgs e)
        {
            _activeProfile.AutoExit = chkAutoExit.Checked;
        }

        private void btnDeleteProfile_Click(object sender, EventArgs e)
        {
            DeleteProfile();
        }

        /// <summary>
        /// Delete selected profile
        /// </summary>
        private void DeleteProfile()
        {
            int index = cmbProfiles.SelectedIndex;
            string profileName = cmbProfiles.SelectedItem.ToString();

            if (index > -1)
            {
                if (
                    MessageBox.Show("Are you sure you wish to delete the profile '" + profileName + "'?",
                                    "Delete profile?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) ==
                    DialogResult.Yes)
                {
                    _profileHandler.RemoveUser(index);
                    _profileIndex = 0;
                    _fileHandler.DeleteProfile(profileName);
                    UpdateProfiles();
                    SaveProfilesToDisk();
                }
            }
        }

        private void btnResetSettings_Click(object sender, EventArgs e)
        {
            ResetProfile();
        }

        /// <summary>
        /// Reset active profile to default settings
        /// </summary>
        private void ResetProfile()
        {
            if (MessageBox.Show("Reset all settings to default. Are you sure?", "Reset profile?",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                _activeProfile = new Profile();
                UpdateGui();
                IsDirty = true;
            }
        }

        /// <summary>
        /// Event handler that sets the isDirty flag to true
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsDirtyTrueHandler(object sender, EventArgs e)
        {
            if (IsDirtyEnabled)
            {
                IsDirty = true;
                //MessageBox.Show("DIRTY!");
            }
        }

        private void cmbProfiles_Enter(object sender, EventArgs e)
        {
            _profileIndex = cmbProfiles.SelectedIndex;
        }

        private void btnBrowseExe_Click(object sender, EventArgs e)
        {
            BrowseServerFile();
        }

        private void BrowseServerFile()
        {
            var fileDialog = new OpenFileDialog
                                 {
                                     InitialDirectory = Environment.CurrentDirectory,
                                     Filter = "Arma Server Files|*server.exe|Any file|*.*"
                                 };

            fileDialog.ShowDialog();
            txtServerExe.Text = fileDialog.FileName;
        }

        private void btnMonitorRpt_Click(object sender, EventArgs e)
        {
            var rptFile = _fileHandler.GetRptFile();
            if (string.IsNullOrEmpty(rptFile))
            {
                MessageBox.Show("No RPT file present", "TADST", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var file = Path.Combine(Environment.CurrentDirectory, "TADST", _activeProfile.ProfileName, rptFile);
            OpenMonitor(file, "Rpt");
        }

        private void OpenMonitor(string monitorFile, string fileTitle)
        {
            if (string.IsNullOrEmpty(monitorFile))
            {
                MessageBox.Show("No " + fileTitle + "file present", "TADST", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }
            var monForm = new MonitorForm(monitorFile);
            monForm.Show();
        }


        private void btnOpenRpt_Click(object sender, EventArgs e)
        {
            if (_activeProfile.ServerExePath.ToLower().EndsWith("server.exe"))
            {
                if (!_fileHandler.OpenRpt())
                {
                    MessageBox.Show("No RPT file present", "TADST", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("You need to select a proper server executable first.", "TADST", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
        }

        private void txtConsoleLogfile_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckKeyForFile(e);
        }

        private void txtRankingFile_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckKeyForFile(e);
        }

        private void txtPidfile_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckKeyForFile(e);
        }

        private void lstMissions_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateGuiSelectedMissions();
        }

        /// <summary>
        /// Update label with amount of selected missions
        /// </summary>
        private void UpdateGuiSelectedMissions()
        {
            IEnumerable<Mission> missions = _activeProfile.Missions.Where(island => island.IsChecked);
            lblSelectedMissions.Text = missions.Count().ToString();
        }

        private void btnSelectAllMissions_Click(object sender, EventArgs e)
        {
            CheckAllMissions();
        }

        private void btnSelectNoneMissions_Click(object sender, EventArgs e)
        {
            UnCheckAllMissions();
        }

        private void btnInvertMissionSelection_Click(object sender, EventArgs e)
        {
            InvertCheckedMissions();
        }

        /// <summary>
        /// Invert selected missions. Unselected get selected and vice versa
        /// </summary>
        private void InvertCheckedMissions()
        {
            for (int index = 0; index < lstMissions.Items.Count; index++)
            {
                lstMissions.SetItemChecked(index, !lstMissions.GetItemChecked(index));
            }

            for (int index = 0; index < lstMissions.Items.Count; index++)
            {
                Mission mission = _activeProfile.Missions.Find(mis => mis.Name == lstMissions.Items[index].ToString());
                mission.IsChecked = lstMissions.GetItemChecked(index);
            }

            UpdateGuiSelectedMissions();
        }

        /// <summary>
        /// Deselect all missions
        /// </summary>
        private void UnCheckAllMissions()
        {
            for (int index = 0; index < lstMissions.Items.Count; index++)
            {
                Mission mission = _activeProfile.Missions.Find(mis => mis.Name == lstMissions.Items[index].ToString());
                mission.IsChecked = false;
                lstMissions.SetItemChecked(index, false);
            }
            UpdateGuiSelectedMissions();
        }

        /// <summary>
        /// Select all missions
        /// </summary>
        private void CheckAllMissions()
        {
            for (int index = 0; index < lstMissions.Items.Count; index++)
            {
                Mission mission = _activeProfile.Missions.Find(mis => mis.Name == lstMissions.Items[index].ToString());
                mission.IsChecked = true;
                lstMissions.SetItemChecked(index, true);
            }
            UpdateGuiSelectedMissions();
        }

        private void cmbMissionFilter_SelectionChangeCommitted(object sender, EventArgs e)
        {
            UpdateGuiMissions();
        }

        private void lstMissions_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int index = lstMissions.SelectedIndex;

            if (index > -1)
            {
                Mission mission = _activeProfile.Missions.Find(mis => mis.Name == lstMissions.Items[index].ToString());
                mission.IsChecked = Convert.ToBoolean(e.NewValue);
                //mission.Difficulty = cmbDifficulty.SelectedIndex;
                UpdateGuiSelectedMissions();
                IsDirty = true;
            }
        }

        private void btnRefreshMissions_Click(object sender, EventArgs e)
        {
            _activeProfile.ScanMissions();
            UpdateGuiMissionFilter();
            UpdateGuiMissions();
        }

        private void txtMotd_TextChanged(object sender, EventArgs e)
        {
            _activeProfile.Motd.Clear();
            _activeProfile.Motd.AddRange(txtMotd.Text.Split(new[] {Environment.NewLine}, StringSplitOptions.None));
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            var info = new InformationForm();
            info.ShowDialog();
        }

        private void txtRequiredBuild_DoubleClick(object sender, EventArgs e)
        {
            if (File.Exists(txtServerExe.Text))
            {
                string serverPath = txtServerExe.Text; // get server exe filepath
                int index = serverPath.LastIndexOf(@"\", StringComparison.Ordinal) + 1;
                // get index of filename in string
                string serverExeName = serverPath.Substring(index); // get exe name only
                string serverExePath = serverPath.Substring(0, index); // get folder only

                // set game exe filename
                string exeName;
                switch (serverExeName)
                {
                    case "arma2oaserver.exe":
                        exeName = serverExePath + "arma2oa.exe";
                        break;
                    case "arma2server.exe":
                        exeName = serverExePath + "arma2.exe";
                        break;
                    default:
                        exeName = serverExePath + "arma3.exe";
                        break;
                }

                // get buildnumber from game exe
                FileVersionInfo serverExeInfo = FileVersionInfo.GetVersionInfo(exeName);
                txtRequiredBuild.Text = "" + serverExeInfo.FileBuildPart + serverExeInfo.FilePrivatePart;
            }
        }

        private void btnMissionUp_Click(object sender, EventArgs e)
        {
            if (lstMissions.SelectedIndex < 0)
                return;

            int selectedIndex = lstMissions.SelectedIndex;
            int targetIndex = lstMissions.SelectedIndex - 1;
            _activeProfile.Missions.Swap(selectedIndex, targetIndex);
            lstMissions.SwapItems(selectedIndex, targetIndex);

            lstMissions.SelectedIndex = targetIndex > -1 ? targetIndex : 0;

            IsDirty = true;
        }

        private void btnMissionDown_Click(object sender, EventArgs e)
        {
            if (lstMissions.SelectedIndex < 0)
                return;

            int selectedIndex = lstMissions.SelectedIndex;
            int targetIndex = lstMissions.SelectedIndex + 1;

            if (targetIndex < lstMissions.Items.Count)
            {
                _activeProfile.Missions.Swap(selectedIndex, targetIndex);
                lstMissions.SwapItems(selectedIndex, targetIndex);

                lstMissions.SelectedIndex = targetIndex < lstMissions.Items.Count - 1
                                                ? targetIndex
                                                : lstMissions.Items.Count - 1;
                IsDirty = true;
            }
        }

        private void lstMissions_MouseDown(object sender, MouseEventArgs e)
        {
            if (lstMissions.SelectedItem == null)
            {
                return;
            }

            lstMissions.DoDragDrop(lstMissions.SelectedItem, DragDropEffects.Move);
            updateMissionDiffCmb();
        }

        private void lstMissions_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;

            Point point = lstMissions.PointToClient(new Point(e.X, e.Y));

            int index = lstMissions.IndexFromPoint(point);
            int selectedIndex = lstMissions.SelectedIndex;

            if (index < 0)
            {
                index = selectedIndex;
            }

            if (index != selectedIndex)
            {
                _activeProfile.Missions.Swap(selectedIndex, index);
                lstMissions.SwapItems(selectedIndex, index);
                lstMissions.SelectedIndex = index;
                IsDirty = true;
            }

            if (point.Y <= (Font.Height*2))
            {
                lstMissions.TopIndex -= 1;
            }
            else if (point.Y >= lstMissions.Height - (Font.Height*2))
            {
                lstMissions.TopIndex += 1;
            }
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            _activeProfile.SortMissions();
            UpdateGuiMissions();
        }

        private void lstMods_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var index = lstMods.SelectedIndex;

            if (index > -1)
            {
                var mod = _activeProfile.Mods.Find(m => m.Name == lstMods.Items[index].ToString());
                mod.IsChecked = Convert.ToBoolean(e.NewValue);
                UpdateGuiSelectedMods();
                UpdateGuiStartupParameters();
                IsDirty = true;
            }
        }

        private void lstMods_MouseDown(object sender, MouseEventArgs e)
        {
            if (lstMods.SelectedItem == null)
            {
                return;
            }

            lstMods.DoDragDrop(lstMods.SelectedItem, DragDropEffects.Move);
        }

        private void lstMods_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            e.Effect = DragDropEffects.Move;

            Point point = lstMods.PointToClient(new Point(e.X, e.Y));

            int index = lstMods.IndexFromPoint(point);
            int selectedIndex = lstMods.SelectedIndex;

            if (index < 0)
            {
                index = selectedIndex;
            }

            if (index != selectedIndex)
            {
                _activeProfile.Mods.Swap(selectedIndex, index);
                lstMods.SwapItems(selectedIndex, index);
                lstMods.SelectedIndex = index;
                UpdateGuiStartupParameters();
                IsDirty = true;
            }

            if (point.Y <= (Font.Height*2))
            {
                lstMods.TopIndex -= 1;
            }
            else if (point.Y >= lstMods.Height - (Font.Height*2))
            {
                lstMods.TopIndex += 1;
            }
        }


        private void modUpBTN_Click(object sender, EventArgs e)
        {
            if (lstMods.SelectedIndex >= 0)
            {
                int selectedIndex = lstMods.SelectedIndex;
                int targetIndex = lstMods.SelectedIndex - 1;
                _activeProfile.Mods.Swap(selectedIndex, targetIndex);
                lstMods.SwapItems(selectedIndex, targetIndex);

                lstMods.SelectedIndex = targetIndex > -1 ? targetIndex : 0;
                UpdateGuiStartupParameters();
            }
        }

        private void modDownBTN_Click(object sender, EventArgs e)
        {
            if (lstMods.SelectedIndex >= 0)
            {
                int selectedIndex = lstMods.SelectedIndex;
                int targetIndex = lstMods.SelectedIndex + 1;

                if (targetIndex < lstMods.Items.Count)
                {
                    _activeProfile.Mods.Swap(selectedIndex, targetIndex);
                    lstMods.SwapItems(selectedIndex, targetIndex);
                    lstMods.SelectedIndex = targetIndex < lstMods.Items.Count ? targetIndex : lstMods.Items.Count - 1;
                    UpdateGuiStartupParameters();
                }
            }
        }

        private void removeModBTN_Click(object sender, EventArgs e)
        {
            var index = lstMods.SelectedIndex;
            if (index < 0) return;

            _activeProfile.Mods.RemoveAt(index);
            UpdateGuiMods();

            if (index < 0) index = 0;
            if (index >= _activeProfile.Mods.Count) index = _activeProfile.Mods.Count - 1;

            lstMods.SelectedIndex = index;
        }

        private void btnSelectAllMods_Click(object sender, EventArgs e)
        {
            CheckAllMods();
        }

        /// <summary>
        /// Select all mods
        /// </summary>
        private void CheckAllMods()
        {
            for (int index = 0; index < lstMods.Items.Count; index++)
            {
                Mod mod = _activeProfile.Mods.Find(m => m.Name == lstMods.Items[index].ToString());
                mod.IsChecked = true;
                lstMods.SetItemChecked(index, true);
            }
            UpdateGuiSelectedMods();
        }

        /// <summary>
        /// Invert selected mods. Unselected get selected and vice versa
        /// </summary>
        private void InvertCheckedMods()
        {
            for (var index = 0; index < lstMods.Items.Count; index++)
            {
                lstMods.SetItemChecked(index, !lstMods.GetItemChecked(index));
            }

            for (var index = 0; index < lstMods.Items.Count; index++)
            {
                var mod = _activeProfile.Mods.Find(m => m.Name == lstMods.Items[index].ToString());
                mod.IsChecked = lstMods.GetItemChecked(index);
            }

            UpdateGuiSelectedMods();
        }

        /// <summary>
        /// Deselect all mods
        /// </summary>
        private void UnCheckAllMods()
        {
            for (int index = 0; index < lstMods.Items.Count; index++)
            {
                Mod mod = _activeProfile.Mods.Find(m => m.Name == lstMods.Items[index].ToString());
                mod.IsChecked = false;
                lstMods.SetItemChecked(index, false);
            }
            UpdateGuiSelectedMods();
        }

        private void btnSelectNoneMods_Click(object sender, EventArgs e)
        {
            UnCheckAllMods();
        }

        private void btnInvertMods_Click(object sender, EventArgs e)
        {
            InvertCheckedMods();
        }

        private void btnAddModFolder_Click(object sender, EventArgs e)
        {
            var folderDialog = new FolderBrowserDialog();
            folderDialog.ShowDialog();

            var path = folderDialog.SelectedPath;

            if (!string.IsNullOrEmpty(path))
            {
                _activeProfile.Mods.Insert(0, new Mod(path, true));
                UpdateGuiMods();
                IsDirty = true;
            }
        }

        private void btnRefreshMods_Click(object sender, EventArgs e)
        {
            _activeProfile.ScanMods();
            UpdateGuiMods();
        }

        private void btnSortMods_Click(object sender, EventArgs e)
        {
            _activeProfile.SortMods();
            UpdateGuiMods();
        }

        private void btnOpenNetLog_Click(object sender, EventArgs e)
        {
            if (!_fileHandler.OpenNetlog())
            {
                MessageBox.Show("No netlog file present", "TADST", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDeleteNetLog_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete 'net.log'? Are you sure?",
                                "Delete netlog", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                _fileHandler.DeleteNetLog();
            }
        }

        private void btnRotateNetLog_Click(object sender, EventArgs e)
        {
            var newName = "net_" + DateTime.Now.ToShortDateString() + "_" +
                          DateTime.Now.ToLongTimeString().Replace(":", "-").Trim() + ".log";

            if (MessageBox.Show("Rotate netlog file? This will rename current 'net.log' file to '" + newName + "'",
                                "Delete netlog", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                _fileHandler.RotateNetLog(newName);
            }
        }

        private void btnOpenLogFile_Click(object sender, EventArgs e)
        {
            if (!_fileHandler.OpenLogFile())
            {
                MessageBox.Show("No console log file present", "TADST", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnOpenRankingFile_Click(object sender, EventArgs e)
        {
            if (!_fileHandler.OpenRankingFile())
            {
                MessageBox.Show("No ranking file present", "TADST", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnOpenPidFile_Click(object sender, EventArgs e)
        {
            if (!_fileHandler.OpenPidFile())
            {
                MessageBox.Show(
                    "No Process Index file present. The PID file only exists when server is running and is deleted when server is terminated.",
                    "TADST", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtConsoleLogfile_DoubleClick(object sender, EventArgs e)
        {
            txtConsoleLogfile.Text = "logfile_console.log";
        }

        private void txtRankingFile_DoubleClick(object sender, EventArgs e)
        {
            txtRankingFile.Text = "ranking.log";
        }

        private void txtPidfile_DoubleClick(object sender, EventArgs e)
        {
            txtPidfile.Text = "pid.log";
        }

        private void btnLaunch_Click(object sender, EventArgs e)
        {
            try
            {
                if (_fileHandler.LaunchServer(!chkStartAsIs.Checked))
                {
                    if (_activeProfile.AutoExit)
                    {
                        IsDirtyEnabled = false;
                        Close();
                        IsDirtyEnabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("You must select a valid server executable first", "No server exe",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    txtServerExe.Focus();
                    BrowseServerFile();
                }
            }
            catch (IOException exeption)
            {
                MessageBox.Show(exeption.Message);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                ExportFiles();
            }
            catch (IOException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void ExportFiles()
        {
            var folderBrowserDialog = new FolderBrowserDialog
                                          {Description = "Choose target folder to save config files."};

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                _fileHandler.ExportFiles(folderBrowserDialog.SelectedPath);
            }
        }

        /*
        private void _scrollTimer_Tick(object sender, EventArgs e)
        {
            Point cursor = PointToClient(MousePosition);
            if (cursor.Y < lstMods.Bounds.Top)
                lstMods.TopIndex -= 1;
            else if (cursor.Y > lstMods.Bounds.Bottom)
                lstMods.TopIndex += 1;
        }*/

        private void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                _fileHandler.CreateConfigFiles();
                Process.Start(Path.Combine(Environment.CurrentDirectory, "TADST", _activeProfile.ProfileName));
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void txtServerExe_DoubleClick(object sender, EventArgs e)
        {
            var pathOa = Path.Combine(Environment.CurrentDirectory, "arma2oaserver.exe");
            var pathOaBeta = Path.Combine(Environment.CurrentDirectory, "Expansion", "beta", "arma2oaserver.exe");
            var pathA3 = Path.Combine(Environment.CurrentDirectory, "arma3server.exe");

            if (File.Exists(pathOaBeta))
                txtServerExe.Text = pathOaBeta;
            else if (File.Exists(pathOa))
                txtServerExe.Text = pathOa;
            else if (File.Exists(pathA3))
                txtServerExe.Text = pathA3;
        }

        private void txtServerName_DoubleClick(object sender, EventArgs e)
        {
            var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var userName = windowsIdentity != null ? windowsIdentity.Name : _activeProfile.ProfileName;

            if (userName.Contains("\\"))
            {
                userName = userName.Substring(userName.LastIndexOf('\\') + 1);
            }

            txtServerName.Text = userName + "'s Server";
        }

        private void btnDeleteRpt_Click(object sender, EventArgs e)
        {
            string file = _fileHandler.GetRptFile();

            if (string.IsNullOrWhiteSpace(file))
            {
                MessageBox.Show("No RPT file to delete.", "Delete RPT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "TADST", _activeProfile.ProfileName, file)))
            {
                if (MessageBox.Show("Delete '" + file + "'. Are you sure?",
                                    "Delete RPT", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    try
                    {
                        _fileHandler.DeleteRpt();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("No '" + file + "' file present");
            }
        }

        private void chkStartAsIs_CheckedChanged(object sender, EventArgs e)
        {
            _activeProfile.LaunchAsIs = chkStartAsIs.Checked;

            if (chkStartAsIs.Checked)
            {
                var info =
                    new AsIsInfoForm(Path.Combine(Environment.CurrentDirectory, "TADST", _activeProfile.ProfileName));
                info.ShowDialog();
            }
        }

        private void txtExtraParameters_TextChanged(object sender, EventArgs e)
        {
            _activeProfile.ExtraParameters = txtExtraParameters.Text;
            UpdateGuiStartupParameters();
        }

        private void txtExtraParameters_Click(object sender, EventArgs e)
        {
            _activeProfile.ExtraParameters = txtExtraParameters.Text;
            UpdateGuiStartupParameters();
        }

        private void txtPort_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                _activeProfile.Port = string.IsNullOrEmpty(txtPort.Text) ? 0 : Convert.ToInt32(txtPort.Text);
                UpdateGuiStartupParameters();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                txtPort.Text = "0";
            }
        }

        private void txtPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckIfKeyIsDigit(e);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsDirty) return;
            e.Cancel = MessageBox.Show("Close without saving changes. Are you sure?", "Close application",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes;
        }

        private void lstMods_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            var names = (string[]) e.Data.GetData(DataFormats.FileDrop, false);

            foreach (var name in names)
            {
                _activeProfile.Mods.Insert(0, new Mod {Name = name, IsChecked = true});
            }

            UpdateGuiMods();
        }

        private void lstMods_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
        }

        private void btnMonitorNetLog_Click(object sender, EventArgs e)
        {
            var file = Path.Combine(Environment.CurrentDirectory, "net.log");

            if (!File.Exists(file))
            {
                MessageBox.Show("No net log file present", "TADST", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            OpenMonitor(file, "Netlog");
        }

        private void txtRequiredBuild_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckIfKeyIsDigit(e);
        }

        private void txtPort_DoubleClick(object sender, EventArgs e)
        {
            txtPort.Text = "2302";
        }


        private void txtMaxMessagesSend_DoubleClick(object sender, EventArgs e)
        {
            txtMaxMessagesSend.Text = "128";
        }

        private void txtMaxSizeGuaranteed_DoubleClick(object sender, EventArgs e)
        {
            txtMaxSizeGuaranteed.Text = "512";
        }

        private void txtMaxSizeNonguaranteed_DoubleClick(object sender, EventArgs e)
        {
            txtMaxSizeNonguaranteed.Text = "256";
        }

        private void txtMinBandwidth_DoubleClick(object sender, EventArgs e)
        {
            txtMinBandwidth.Text = "128";
        }

        private void txtMaxBandwidth_DoubleClick(object sender, EventArgs e)
        {
            txtMaxBandwidth.Text = "2000";
        }

        private void txtMinErrorToSend_DoubleClick(object sender, EventArgs e)
        {
            txtMinErrorToSend.Text = 0.001.ToString();
        }

        private void txtMinErrorToSendNear_DoubleClick(object sender, EventArgs e)
        {
            txtMinErrorToSendNear.Text = 0.01.ToString();
        }

        private void txtMaxCustomFileSize_DoubleClick(object sender, EventArgs e)
        {
            txtMaxCustomFileSize.Text = "160";
        }

        private void txtPassword_DoubleClick(object sender, EventArgs e)
        {
            txtPassword.Text = Utilities.CreateRandomPassword(5);
        }

        private void txtAdminPassword_DoubleClick(object sender, EventArgs e)
        {
            txtAdminPassword.Text = Utilities.CreateRandomPassword(6);
        }

        private void txtMaxPacketSize_Click(object sender, EventArgs e)
        {
            txtPerformanceInfo.Text =
                "Maximal size of packet sent over network.\n\nUse only if your router or ISP enforce lower packet size and You have connectivity issues with game. \n\nDesync might happen if used MaxSizeGuaranteed/MaxSizeNonguaranteed values over the maxPacketSize. \n\nmaxPacketSize default reduced from 1490 to 1400 since 1.60, thus MaxSize... values over 1300 could be affected negatively.\n\nDefault: 1400.";
        }

        private void txtMaxPacketSize_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _activeProfile.MaxPacketSize = string.IsNullOrEmpty(txtMaxPacketSize.Text)
                                                   ? 0
                                                   : Convert.ToInt32(txtMaxPacketSize.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                txtMaxPacketSize.Text = "0";
            }
        }

        private void cmbRequiredSecureId_SelectedIndexChanged(object sender, EventArgs e)
        {
            _activeProfile.RequiredSecureId = cmbRequiredSecureId.SelectedIndex;
        }

        private void chkBeta_CheckedChanged(object sender, EventArgs e)
        {
            _activeProfile.Beta = chkBeta.Checked;
            UpdateGuiStartupParameters();
        }

        private void cmbDefaultDifficulty_SelectedIndexChanged(object sender, EventArgs e)
        {
            _activeProfile.DefaultDifficulty = cmbDefaultDifficulty.SelectedIndex;
        }


        private void chkBattlEye_CheckedChanged(object sender, EventArgs e)
        {
            _activeProfile.BattlEye = chkBattlEye.Checked;
        }

        private void txtHeadlessIp_DoubleClick(object sender, EventArgs e)
        {
            txtHeadlessIp.Text = "127.0.0.1";
        }

        private void chkHeadlessEnabled_CheckedChanged(object sender, EventArgs e)
        {
            _activeProfile.HeadlessEnabled = chkHeadlessEnabled.Checked;
            txtHeadlessIp.Enabled = chkHeadlessEnabled.Checked;
            txtLocalIp.Enabled = chkHeadlessEnabled.Checked;
        }

        private void lstMissions_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateMissionDiffCmb();
        }

        private void updateMissionDiffCmb()
        {
            int index = lstMissions.SelectedIndex;

            if (index > -1)
            {
                Mission mission = _activeProfile.Missions.Find(mis => mis.Name == lstMissions.Items[index].ToString());
                cmbDifficulty.SelectedIndex = mission.Difficulty;
            }
        }

        private void btnAllMissionsDiff_Click(object sender, EventArgs e)
        {
            foreach (var mission in _activeProfile.Missions)
            {
                mission.Difficulty = cmbDifficulty.SelectedIndex;
            }

            IsDirty = true;
        }

        private void txtMaxPacketSize_DoubleClick(object sender, EventArgs e)
        {
            txtMaxPacketSize.Text = "1400";
        }

        private void txtHeadlessIp_TextChanged(object sender, EventArgs e)
        {
            _activeProfile.HeadlessIps = txtHeadlessIp.Text;
        }

        private void chkEnableHT_CheckedChanged(object sender, EventArgs e)
        {
            _activeProfile.EnableHt = chkEnableHT.Checked;
        }

        private void btnPortCheck_Click(object sender, EventArgs e)
        {
            CheckPorts();
        }

        private void CheckPorts()
        {
            MessageBox.Show("Open or forward the following UDP ports to this computer in your router/firewall:"
                            + Environment.NewLine
                            + _activeProfile.Port + " - Arma3 Game Port" + Environment.NewLine
                            + (_activeProfile.Port + 1) + " - Steam Server Query" + Environment.NewLine
                            + (_activeProfile.Port + 2) + " - Steam Port" + Environment.NewLine
                            + (_activeProfile.Port + 3) + " - VON (reserved, but not used yet)" + Environment.NewLine +
                            Environment.NewLine
                            +
                            "If you start a second server on this machine, suggestion is to leave at least 10 ports between. Set second server port to " +
                            (_activeProfile.Port + 10));
        }

        private void txtServerCommandPassword_TextChanged(object sender, EventArgs e)
        {
            _activeProfile.ServerCommandPassword = txtServerCommandPassword.Text;
        }

        private void txtServerCommandPassword_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtServerCommandPassword.Text = Utilities.CreateRandomPassword(5);
        }

        private void chkEnableUpnp_CheckedChanged(object sender, EventArgs e)
        {
            _activeProfile.Upnp = chkEnableUpnp.Checked;
        }

        private void cmbAllowedFilePatching_SelectedIndexChanged(object sender, EventArgs e)
        {
            _activeProfile.AllowFilePatching = cmbAllowedFilePatching.SelectedIndex;
        }

        private void txtLocalIp_TextChanged(object sender, EventArgs e)
        {
            _activeProfile.LocalIps = txtLocalIp.Text;
        }

        private void cmbAILevelPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAILevelPreset.SelectedIndex == 3)
            {
                numSkillAI.Enabled = true;
                numPrecisionAI.Enabled = true;
            }
            else
            {
                numSkillAI.Enabled = false;
                numPrecisionAI.Enabled = false;
            }
            this.GetCurrentDifficulty().AILevelPreset = this.cmbAILevelPreset.SelectedIndex;
        }

        private void cmbAILevelPreset_Click(object sender, EventArgs e)
        {
            lblProfileDifficultyInfo.Text = "AI Level Preset" + Environment.NewLine + Environment.NewLine +
                                            "0 - AI Level Low" + Environment.NewLine +
                                            "1 - AI Level Normal" + Environment.NewLine +
                                            "2 - AI Level High" + Environment.NewLine +
                                            "3 - AI Level Custom" + Environment.NewLine;
        }

        private void chkLoopback_CheckedChanged(object sender, EventArgs e)
        {
            _activeProfile.Loopback = chkLoopback.Checked;
        }

        private void chkAutoinit_CheckedChanged(object sender, EventArgs e)
        {
            _activeProfile.AutoInit = chkAutoinit.Checked;
            UpdateGuiStartupParameters();
        }

    }
}