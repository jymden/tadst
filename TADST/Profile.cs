using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace TADST
{
    [Serializable]
    internal class Profile
    {
        // Profile settings
        private string _profileName;
        private string _serverExePath;
        private string _extraParameters;
        private string _inGameName;

        private bool _beta;
        private bool _toolTips;
        private bool _launchAsIs;
        private bool _autoExit;

        // Server details tab
        private string _serverName;
        private string _password;
        private int _port;
        private int _maxPlayers;
        private string _adminPassword;
        private bool _upnp;

        private string _consoleLogfile;
        private string _rankingFile;
        private string _pidFile;
        private int _rptTimeStampIndex;
        private List<string> _rptTimeStamps;
        private bool _netlog;
        private bool _headlessEnabled;
        private string _headlessIps;
        private string _localIps;

        private bool _persistantBattlefield;
        private bool _autoInit;

        private bool _disableVon;
        private int _vonQuality;

        private bool _loopback;

        // Server rules tab
        private bool _votingEnabled;
        private int _voteMissionPlayers;
        private decimal _voteThreshold;
        private int _verifySignatures;
        private int _requiredSecureId;
        private bool _requiredBuildEnabled;
        private int _requiredBuild;
        private bool _kickDuplicates;

        private bool _battlEye;
        private int _allowFilePatching;


        private bool _maxPingEnabled;
        private int _maxPing;

        private bool _maxDesyncEnabled;
        private int _maxDesync;

        private bool _maxPacketLossEnabled;
        private int _maxPacketLoss;

        private bool _disconnectTimeoutEnabled;
        private int? _disconnectTimeout;

        private bool _kickClientsOnSlowNetworkEnabled;
        private int _kickClientsOnSlowNetwork;

        private List<string> _motd;
        private int _motdInterval;

        // Missions tab
        private List<Mission> _missions;
        private int _missionDifficulty;

        // Mods tab
        private List<Mod> _mods;
        private string _selectedMods;

        // Difficulty tab
        private DifficultySetting _diffRecruit;
        private DifficultySetting _diffRegular;
        private DifficultySetting _diffVeteran;
        private DifficultySetting _diffExpert;
        private DifficultySetting _diffCustom;
        private int _defaultDifficulty;
        private int _aiLevelPreset;

        // Performance tab
        private int _maxMsgSend;
        private int _maxSizeGuaranteed;
        private int _maxSizeNonGuaranteed;
        private int _minBandWidth;
        private int _maxBandwidth;
        private double _minErrorToSend;
        private double _minErrorToSendNear;
        private int _maxCustomFileSize;
        private int _maxPacketSize;

        private decimal _terrainGrid;
        private int _viewDistance;

        private bool _enableHt;


        // Server Script tab
        private string _serverCommandPassword;

        private string _doubleIdDetected;
        private string _onUserConnected;
        private string _onUserDisconnected;
        private string _onHackedData;
        private string _onDifferentData;
        private string _onUnsignedData;
        private string _regularCheck;

        [OnDeserialized()]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if (_disconnectTimeout == null)
            {
                DisconnectTimeout = 5;
            }
        }

        /// <summary>
        /// Default constructor for default profile settings
        /// </summary>
        public Profile()
        {
            ProfileName = "default";
            InGameName = "server";
            ServerExePath = "";
            ConsoleLogfile = "";
            PidFile = "";
            RankingFile = "";

            Beta = false;
            ToolTips = true;
            LaunchAsIs = false;

            HeadlessEnabled = false;
            HeadlessIps = "127.0.0.1";
            LocalIps = "127.0.0.1";

            DefaultDifficulty = 1;

            RptTimeStamps = new List<string>();
            Motd = new List<string>();
            Missions = new List<Mission>();
            Mods = new List<Mod>();

            CreateRptTimeStamps();
            CreateDefaultDifficulties();

            SetDefaultServerDetails();
            SetDefaultPerformance();
            SetDefaultView();

            ScanMissions();
            ScanMods();
        }

        private void SetDefaultServerDetails()
        {
            Port = 2302;
            MaxPlayers = 10;

            VonQuality = 10;
            VotingEnabled = true;

            VoteMissionPlayers = 3;
            VoteThreshold = 0.33M;

            PersistantBattlefield = false;
            AutoInit = false;

            BattlEye = false;
            AllowFilePatching = 0;

            MaxPingEnabled = false;
            MaxPing = 10;
            MaxDesyncEnabled = false;
            MaxDesync = 0;
            MaxPacketLossEnabled = false;
            MaxPacketLoss = 0;
            DisconnectTimeoutEnabled = false;
            DisconnectTimeout = 90;
            KickClientsOnSlowNetworkEnabled = false;
            KickClientsOnSlowNetwork = 0;

            MotdInterval = 3;
            
            MissionDifficulty = 1;

            Loopback = false;
        }

        private void SetDefaultView()
        {
            TerrainGrid = 25;
            ViewDistance = 2000;
        }

        public void SetDefaultPerformance()
        {
            MaxMsgSend = 128;
            MaxSizeGuaranteed = 512;
            MaxSizeNonGuaranteed = 256;
            MinBandWidth = 131072;
            MaxBandwidth = 2097152000;
            MinErrorToSend = 0.001;
            MinErrorToSendNear = 0.01;
            MaxCustomFileSize = 1310720;
            MaxPacketSize = 1400;
        }

        /// <summary>
        /// Add default timestamp labels to rpt time stamps list.
        /// </summary>
        private void CreateRptTimeStamps()
        {
            string[] timeStamps = {"none", "short", "full"};

            foreach (string timeStamp in timeStamps)
            {
                _rptTimeStamps.Add(timeStamp);
            }
        }

        private void CreateDefaultDifficulties()
        {
            var diffFactory = new DifficultyFactory();

            DiffCustom = diffFactory.CreateCustomDifficulty();
        }


        /// <summary>
        /// Scan MPMissions folder for mission files
        /// </summary>
        /// <returns>All .pbo files in MPMissions folder</returns>
        private IEnumerable<string> ScanMissionsFolder()
        {
            var mpMissions = Path.Combine(Environment.CurrentDirectory, "MPMissions");
            var folder = new DirectoryInfo(mpMissions);

            if (Directory.Exists(mpMissions))
            {
                var fileList = Directory.GetFiles(mpMissions, "*.pbo").Select(Path.GetFileName);
                var folderList = Directory.GetDirectories(mpMissions).Select(Path.GetFileName);
                return fileList.Union(folderList).OrderBy(s => s);
            }

            return null;
        }

        /// <summary>
        /// Get missions file names from MPMissions folder
        /// </summary>
        /// <returns>Names of MPMissions</returns>
        public void ScanMissions()
        {
            var missions = ScanMissionsFolder();

            foreach (var mission in missions.Where(mission => !(_missions.Any(x => x.Name == mission))))
            {
                _missions.Add(new Mission(mission, GetIsland(mission), false));
            }

            ValidateMissions();
        }

        /// <summary>
        /// Validate that missions in list exists on disk
        /// </summary>
        private void ValidateMissions()
        {
            for (var index = 0; index < _missions.Count; index++)
            {
                if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "MPMissions\\" + _missions[index].Name))
                    &&
                    !Directory.Exists(Path.Combine(Environment.CurrentDirectory, "MPMissions\\" + _missions[index].Name)))
                {
                    _missions.RemoveAt(index);
                }
            }
        }

        private static string GetIsland(string mission)
        {
            string[] nameStrings = mission.Split('.');
            string island = "";

            if (nameStrings.Count() > 0)
            {
                island = (nameStrings[nameStrings.Count() - 1].ToLower() == "pbo")
                             ? nameStrings[nameStrings.Count() - 2]
                             : nameStrings[nameStrings.Count() - 1];
            }

            return Utilities.ToMixedCase(island.ToLower());
        }

        /// <summary>
        /// Scan game directory for mod folders
        /// </summary>
        /// <returns></returns>
        private IEnumerable<DirectoryInfo> ScanModFolders()
        {
            var runPath = Environment.CurrentDirectory;
            var runFolder = new DirectoryInfo(Path.GetFullPath(runPath));
            IEnumerable<DirectoryInfo> modFolders = runFolder.GetDirectories();

            return modFolders.OrderBy(x => x.Name);
        }

        /// <summary>
        /// Scan game directory for mods and add them to list
        /// </summary>
        public void ScanMods()
        {
            var modFolders = ScanModFolders();

            foreach (var modFolder in modFolders)
            {
                if (!(_mods.Any(x => x.Name == modFolder.Name)))
                {
                    _mods.Add(new Mod(modFolder.Name, false));
                }
            }

            AddBetaModFolders();
            ValidateMods();
        }

        /// <summary>
        /// Add beta folders to mods list
        /// </summary>
        private void AddBetaModFolders()
        {
            var betaFolders = new[] {@"Expansion\beta", @"Expansion\beta\expansion"};
            foreach (var betaFolder in betaFolders)
            {
                if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, betaFolder))) continue;

                if (!(_mods.Any(x => x.Name == betaFolder)))
                {
                    _mods.Add(new Mod(betaFolder, false));
                }
            }
        }

        /// <summary>
        /// Remove all game folder and validate that all mods actually exists on disk
        /// </summary>
        private void ValidateMods()
        {
            var filesToExclude = new List<string>
                                     {
                                         "addons",
                                         "appcache",
                                         "battleye",
                                         "common",
                                         "expansion",
                                         "dta",
                                         "keys",
                                         "missions",
                                         "mpmissions",
                                         "tadst",
                                         "ta2dst",
                                         "userconfig",
                                         "dll",
                                         "directx",
                                         "config",
                                         "logs"
                                     };
            _mods.RemoveAll(x => filesToExclude.Contains(x.Name.ToLower()));

            for (var index = 0; index < _mods.Count; index++)
            {
                if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, _mods[index].Name)))
                {
                    _mods.RemoveAt(index);
                }
            }
        }

        /// <summary>
        /// Get startup parameters string
        /// </summary>
        /// <returns>String of all startup parameters</returns>
        public string GetStartupParameters()
        {
            var profileFolder = Path.Combine(Environment.CurrentDirectory, "TADST", ProfileName);
            var configFile = Path.Combine(profileFolder, "TADST_config.cfg");
            var basicConfigFile = Path.Combine(profileFolder, "TADST_basic.cfg");
            var parameters = "";

            parameters +=
                " -port=" + Port +
                " \"-config=" + configFile + "\"" +
                " \"-cfg=" + basicConfigFile + "\"" +
                " \"-profiles=" + profileFolder + "\"" +
                " -name=" + ProfileName +
                " -filePatching";

            if (Netlog)
            {
                parameters += " -netlog";
            }

            if (!string.IsNullOrEmpty(PidFile))
            {
                parameters += " -pid=" + PidFile;
            }
            if (!string.IsNullOrEmpty(RankingFile))
            {
                parameters += " -ranking=" + RankingFile;
            }

            if (NumOfCheckedMods() > 0)
            {
                parameters += " \"-mod=" + GetModsString() + "\"";
            }

            if (PersistantBattlefield && AutoInit)
            {
                parameters += " -autoInit";
            }

            // Create beta commandline
            if (Beta)
            {
                if (ServerExePath.EndsWith("arma2oaserver.exe"))
                    parameters += @" -beta=Expansion\beta;Expansion\beta\Expansion";

                if (ServerExePath.EndsWith("arma2server.exe"))
                    parameters += @" -beta=beta";
            }

            if (EnableHt)
            {
                parameters += " -enableHT";
            }

            if (!string.IsNullOrEmpty(ExtraParameters)) parameters += " " + ExtraParameters;

            return parameters;
        }

        /// <summary>
        /// Get string of checked mods for startup parameters
        /// </summary>
        /// <returns>String of mods separated by semicolons</returns>
        public string GetModsString()
        {
            var modsString = "";

            for (var i = 0; i < Mods.Count; i++)
            {
                if (Mods[i].IsChecked)
                    modsString += Mods[i].Name + ";";
            }

            return modsString.TrimEnd(';');
        }

        private int NumOfCheckedMods()
        {
            return Mods.Count(mod => mod.IsChecked);
        }

        /// <summary>
        /// Sort missions by file name
        /// </summary>
        public void SortMissions()
        {
            Missions.Sort((missionA, missionsB) => String.Compare(missionA.Name, missionsB.Name));
        }

        /// <summary>
        /// Sort mods by name
        /// </summary>
        public void SortMods()
        {
            Mods.Sort((modA, modB) => String.Compare(modA.Name, modB.Name));
        }

        public override string ToString()
        {
            return ProfileName;
        }

        public string ProfileName
        {
            get { return _profileName; }
            set { _profileName = value; }
        }

        public string ServerExePath
        {
            get { return _serverExePath; }
            set { _serverExePath = value; }
        }

        public string ExtraParameters
        {
            get { return _extraParameters; }
            set { _extraParameters = value; }
        }

        public string InGameName
        {
            get { return _inGameName; }
            set { _inGameName = value; }
        }

        public string ServerName
        {
            get { return _serverName; }
            set { _serverName = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string AdminPassword
        {
            get { return _adminPassword; }
            set { _adminPassword = value; }
        }

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        public int MaxPlayers
        {
            get { return _maxPlayers; }
            set { _maxPlayers = value; }
        }

        public string ConsoleLogfile
        {
            get { return _consoleLogfile; }
            set { _consoleLogfile = value; }
        }

        public string RankingFile
        {
            get { return _rankingFile; }
            set { _rankingFile = value; }
        }

        public string PidFile
        {
            get { return _pidFile; }
            set { _pidFile = value; }
        }

        public int RptTimeStampIndex
        {
            get { return _rptTimeStampIndex; }
            set { _rptTimeStampIndex = value; }
        }

        public bool Netlog
        {
            get { return _netlog; }
            set { _netlog = value; }
        }

        public bool PersistantBattlefield
        {
            get { return _persistantBattlefield; }
            set { _persistantBattlefield = value; }
        }

        public bool BattlEye
        {
            get { return _battlEye; }
            set { _battlEye = value; }
        }

        public bool DisableVon
        {
            get { return _disableVon; }
            set { _disableVon = value; }
        }

        public int VonQuality
        {
            get { return _vonQuality; }
            set { _vonQuality = value; }
        }

        public bool RequiredBuildEnabled
        {
            get { return _requiredBuildEnabled; }
            set { _requiredBuildEnabled = value; }
        }

        public int RequiredBuild
        {
            get { return _requiredBuild; }
            set { _requiredBuild = value; }
        }

        public bool VotingEnabled
        {
            get { return _votingEnabled; }
            set { _votingEnabled = value; }
        }

        public int VoteMissionPlayers
        {
            get { return _voteMissionPlayers; }
            set { _voteMissionPlayers = value; }
        }

        public decimal VoteThreshold
        {
            get { return _voteThreshold; }
            set { _voteThreshold = value; }
        }

        public int VerifySignatures
        {
            get { return _verifySignatures; }
            set { _verifySignatures = value; }
        }

        public bool KickDuplicates
        {
            get { return _kickDuplicates; }
            set { _kickDuplicates = value; }
        }

        public bool ToolTips
        {
            get { return _toolTips; }
            set { _toolTips = value; }
        }

        public bool AutoExit
        {
            get { return _autoExit; }
            set { _autoExit = value; }
        }

        public DifficultySetting DiffRecruit
        {
            get { return _diffRecruit; }
            set { _diffRecruit = value; }
        }

        public List<string> RptTimeStamps
        {
            get { return _rptTimeStamps; }
            set { _rptTimeStamps = value; }
        }

        public List<string> Motd
        {
            get { return _motd; }
            set { _motd = value; }
        }

        public int MotdInterval
        {
            get { return _motdInterval; }
            set { _motdInterval = value; }
        }

        public List<Mission> Missions
        {
            get { return _missions; }
            set { _missions = value; }
        }

        public int MissionDifficulty
        {
            get { return _missionDifficulty; }
            set { _missionDifficulty = value; }
        }

        public List<Mod> Mods
        {
            get { return _mods; }
            set { _mods = value; }
        }

        public DifficultySetting DiffRegular
        {
            get { return _diffRegular; }
            set { _diffRegular = value; }
        }

        public DifficultySetting DiffVeteran
        {
            get { return _diffVeteran; }
            set { _diffVeteran = value; }
        }

        public DifficultySetting DiffExpert
        {
            get { return _diffExpert; }
            set { _diffExpert = value; }
        }

        public DifficultySetting DiffCustom
        {
            get { return _diffCustom; }
            set { _diffCustom = value; }
        }

        public int AILevelPreset
        {
            get { return _aiLevelPreset; }
            set { _aiLevelPreset = value; }
        }

        public int MaxMsgSend
        {
            get { return _maxMsgSend; }
            set { _maxMsgSend = value; }
        }

        public int MaxSizeGuaranteed
        {
            get { return _maxSizeGuaranteed; }
            set { _maxSizeGuaranteed = value; }
        }

        public int MaxSizeNonGuaranteed
        {
            get { return _maxSizeNonGuaranteed; }
            set { _maxSizeNonGuaranteed = value; }
        }

        public int MaxBandwidth
        {
            get { return _maxBandwidth; }
            set { _maxBandwidth = value; }
        }

        public double MinErrorToSend
        {
            get { return _minErrorToSend; }
            set { _minErrorToSend = value; }
        }

        public double MinErrorToSendNear
        {
            get { return _minErrorToSendNear; }
            set { _minErrorToSendNear = value; }
        }

        public int MaxCustomFileSize
        {
            get { return _maxCustomFileSize; }
            set { _maxCustomFileSize = value; }
        }

        public decimal TerrainGrid
        {
            get { return _terrainGrid; }
            set { _terrainGrid = value; }
        }

        public int ViewDistance
        {
            get { return _viewDistance; }
            set { _viewDistance = value; }
        }

        public string DoubleIdDetected
        {
            get { return _doubleIdDetected; }
            set { _doubleIdDetected = value; }
        }

        public string OnUserConnected
        {
            get { return _onUserConnected; }
            set { _onUserConnected = value; }
        }

        public string OnUserDisconnected
        {
            get { return _onUserDisconnected; }
            set { _onUserDisconnected = value; }
        }

        public string OnHackedData
        {
            get { return _onHackedData; }
            set { _onHackedData = value; }
        }

        public string OnDifferentData
        {
            get { return _onDifferentData; }
            set { _onDifferentData = value; }
        }

        public string OnUnsignedData
        {
            get { return _onUnsignedData; }
            set { _onUnsignedData = value; }
        }

        public string RegularCheck
        {
            get { return _regularCheck; }
            set { _regularCheck = value; }
        }

        public string SelectedMods
        {
            get { return _selectedMods; }
            set { _selectedMods = value; }
        }

        public int MinBandWidth
        {
            get { return _minBandWidth; }
            set { _minBandWidth = value; }
        }

        public bool LaunchAsIs
        {
            get { return _launchAsIs; }
            set { _launchAsIs = value; }
        }

        public int MaxPacketSize
        {
            get { return _maxPacketSize; }
            set { _maxPacketSize = value; }
        }

        public int RequiredSecureId
        {
            get { return _requiredSecureId; }
            set { _requiredSecureId = value; }
        }

        public bool Beta
        {
            get { return _beta; }
            set { _beta = value; }
        }

        public int DefaultDifficulty
        {
            get { return _defaultDifficulty; }
            set { _defaultDifficulty = value; }
        }

        public bool HeadlessEnabled
        {
            get { return _headlessEnabled; }
            set { _headlessEnabled = value; }
        }

        public string HeadlessIps
        {
            get { return _headlessIps; }
            set { _headlessIps = value; }
        }

        public bool EnableHt
        {
            get { return _enableHt; }
            set { _enableHt = value; }
        }

        public string ServerCommandPassword
        {
            get { return _serverCommandPassword; }
            set { _serverCommandPassword = value; }
        }

        public bool Upnp
        {
            get { return _upnp; }
            set { _upnp = value; }
        }

        public int AllowFilePatching
        {
            get { return _allowFilePatching; }
            set { _allowFilePatching = value; }
        }

        public string LocalIps
        {
            get { return _localIps; }
            set { _localIps = value; }
        }

        public bool Loopback
        {
            get { return _loopback; }
            set { _loopback = value; }
        }

        public bool AutoInit
        {
            get { return _autoInit; }
            set { _autoInit = value; }
        }

        public bool MaxPingEnabled
        {
            get { return _maxPingEnabled; }
            set { _maxPingEnabled = value; }
        }

        public int MaxPing
        {
            get { return _maxPing; }
            set { _maxPing = value; }
        }

        public bool MaxDesyncEnabled
        {
            get { return _maxDesyncEnabled; }
            set { _maxDesyncEnabled = value; }
        }

        public int MaxDesync
        {
            get { return _maxDesync; }
            set { _maxDesync = value; }
        }

        public bool MaxPacketLossEnabled
        {
            get { return _maxPacketLossEnabled; }
            set { _maxPacketLossEnabled = value; }
        }

        public int MaxPacketLoss
        {
            get { return _maxPacketLoss; }
            set { _maxPacketLoss = value; }
        }

        public bool DisconnectTimeoutEnabled
        {
            get { return _disconnectTimeoutEnabled; }
            set { _disconnectTimeoutEnabled = value; }
        }

        public bool KickClientsOnSlowNetworkEnabled
        {
            get { return _kickClientsOnSlowNetworkEnabled; }
            set { _kickClientsOnSlowNetworkEnabled = value; }
        }

        public int KickClientsOnSlowNetwork
        {
            get { return _kickClientsOnSlowNetwork; }
            set { _kickClientsOnSlowNetwork = value; }
        }

        public int DisconnectTimeout
        {
            get { return _disconnectTimeout.Value; }
            set { _disconnectTimeout = value; }
        }
    }
}