using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;


namespace TADST
{
    internal class FileHandler
    {
        public Profile ActiveProfile { get; set; }

        public FileHandler()
        {
            InitProgramPath();
        }

        public FileHandler(Profile profile)
        {
            ActiveProfile = profile;
            InitProgramPath();
        }

        /// <summary>
        /// Create all necessary folders and files
        /// </summary>
        private void InitProgramPath()
        {
            var path = Path.Combine(GetTADSTPath(), ActiveProfile.ProfileName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private static string GetRunPath()
        {
            return Environment.CurrentDirectory;
        }

        private static string GetTADSTPath()
        {
            return Path.Combine(Environment.CurrentDirectory, "TADST");
        }

        public bool OpenRpt()
        {
            var serverExe = ActiveProfile.ServerExePath.Trim().ToLower();
            string file = "";

            if (serverExe.EndsWith("arma2server.exe")) file = "arma2server.RPT";
            else if (serverExe.EndsWith("arma2oaserver.exe")) file = "arma2oaserver.RPT";
            else if (serverExe.EndsWith("arma3server.exe") || serverExe.EndsWith("arma3server_x64.exe"))
            {
                file = GetArma3Rpt();
            }

            if (string.IsNullOrEmpty(file)) return false;

            file = Path.Combine(GetTADSTPath(), ActiveProfile.ProfileName, file);

            if (File.Exists(file))
            {
                //Process.Start("notepad.exe", file);
                Process.Start(file);
                return true;
            }
            return false;
        }

        public void DeleteRpt()
        {
            var file = GetRptFile();
            if (string.IsNullOrEmpty(file)) return;

            file = Path.Combine(GetTADSTPath(), ActiveProfile.ProfileName, file);
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }

        public bool OpenNetlog()
        {
            var file = Path.Combine(GetRunPath(), "net.log");

            if (File.Exists(file))
            {
                Process.Start("notepad.exe", file);
                return true;
            }
            return false;
        }

        public void DeleteNetLog()
        {
            var file = Path.Combine(GetRunPath(), "net.log");

            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }

        public void RotateNetLog(string newName)
        {
            var file = Path.Combine(GetRunPath(), "net.log");
            var newFile = Path.Combine(GetRunPath(), newName);

            if (File.Exists(file))
            {
                File.Move(file, newFile);
            }
        }

        internal bool OpenLogFile()
        {
            var file = Path.Combine(GetTADSTPath(), ActiveProfile.ProfileName, ActiveProfile.ConsoleLogfile);
            if (File.Exists(file))
            {
                Process.Start("notepad.exe", file);
                return true;
            }
            return false;
        }

        internal bool OpenRankingFile()
        {
            var file = Path.Combine(GetTADSTPath(), ActiveProfile.ProfileName, ActiveProfile.RankingFile);
            if (File.Exists(file))
            {
                Process.Start("notepad.exe", file);
                return true;
            }
            return false;
        }

        internal bool OpenPidFile()
        {
            var file = Path.Combine(GetTADSTPath(), ActiveProfile.ProfileName, ActiveProfile.PidFile);
            if (File.Exists(file))
            {
                Process.Start("notepad.exe", file);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Create server config file 
        /// </summary>
        public void CreateConfigFile()
        {
            var path = Path.Combine(GetTADSTPath(), ActiveProfile.ProfileName);
            var file = Path.Combine(path, "TADST_config.cfg");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            File.WriteAllText(file, GetConfigString());
        }

        /// <summary>
        /// Create string
        /// </summary>
        /// <returns></returns>
        private string GetConfigString()
        {
            var voteThreshold = ActiveProfile.VotingEnabled ? ActiveProfile.VoteThreshold : 1.5m;

            var configSb = new StringBuilder();
            configSb.AppendLine($"// Server config file generated {DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()} with TADST.")
                .AppendLine()
                .AppendLine($"hostName = \"{ActiveProfile.ServerName}\";")
                .AppendLine($"password = \"{ActiveProfile.Password}\";")
                .AppendLine($"passwordAdmin = \"{ActiveProfile.AdminPassword}\";")
                .AppendLine($"serverCommandPassword = \"{ActiveProfile.ServerCommandPassword}\";")
                .AppendLine($"logFile = \"{ActiveProfile.ConsoleLogfile}\";")
                .AppendLine()
                .AppendLine("motd[] = {")
                .AppendLine(GetMotd())
                .AppendLine("};")
                .AppendLine($"motdInterval = {ActiveProfile.MotdInterval};")
                .AppendLine()
                .AppendLine($"maxPlayers = {ActiveProfile.MaxPlayers};")
                .AppendLine($"kickduplicate = {Convert.ToInt32(ActiveProfile.KickDuplicates)};")
                .AppendLine($"verifySignatures = {ActiveProfile.VerifySignatures};")
                .AppendLine($"allowedFilePatching = {ActiveProfile.AllowFilePatching};")
                .AppendLine($"requiredSecureId = {ActiveProfile.RequiredSecureId};");

            if (ActiveProfile.Upnp)
            {
                configSb.AppendLine("upnp = 1;");
            }

            if (ActiveProfile.Loopback)
            {
                configSb.AppendLine("loopback = true;");
            }

            if (ActiveProfile.RequiredBuildEnabled && ActiveProfile.RequiredBuild > 0)
            {
                configSb.AppendLine($"requiredBuild = {ActiveProfile.RequiredBuild};");
            }

            if (ActiveProfile.HeadlessEnabled)
            {
                configSb.AppendLine($"headlessClients[] = {{ {string.Join(",", ActiveProfile.HeadlessIps)} }};");
                configSb.AppendLine($"localClient[] = {{ {string.Join(",", ActiveProfile.LocalIps)} }};");
                configSb.AppendLine();
            }

            if (!ActiveProfile.VotingEnabled)
            {
                configSb.AppendLine("allowedVoteCmds[] = {};");
            }

            configSb.AppendLine($"voteMissionPlayers = {ActiveProfile.VoteMissionPlayers};")
                .AppendLine($"voteThreshold = {voteThreshold.ToString(CultureInfo.InvariantCulture)};")
                .AppendLine($"disableVoN = {Convert.ToInt32(ActiveProfile.DisableVon)};")
                .AppendLine($"vonCodecQuality = {ActiveProfile.VonQuality};")
                .AppendLine($"persistent = {Convert.ToInt32(ActiveProfile.PersistantBattlefield)};")
                .AppendLine($"timeStampFormat = \"{ActiveProfile.RptTimeStamps[ActiveProfile.RptTimeStampIndex]}\";")
                .AppendLine($"BattlEye = {Convert.ToInt32(ActiveProfile.BattlEye)};");

            if (ActiveProfile.HeadlessEnabled)
            {
                configSb.AppendLine("battleyeLicense = 1;");
            }

            if (ActiveProfile.MaxPingEnabled)
            {
                configSb.AppendLine($"maxPing = {ActiveProfile.MaxPing};");
            }

            if (ActiveProfile.MaxDesyncEnabled)
            {
                configSb.AppendLine($"maxDesync = {ActiveProfile.MaxDesync};");
            }

            if (ActiveProfile.MaxPacketLossEnabled)
            {
                configSb.AppendLine($"maxPacketloss = {ActiveProfile.MaxPacketLoss};");
            }

            if (ActiveProfile.DisconnectTimeoutEnabled)
            {
                configSb.AppendLine($"disconnectTimeout = {ActiveProfile.DisconnectTimeout};");
            }

            if (ActiveProfile.KickClientsOnSlowNetworkEnabled)
            {
                configSb.AppendLine($"kickClientsOnSlowNetwork = {ActiveProfile.KickClientsOnSlowNetwork};");
            }

            // Code
            configSb.AppendLine($"doubleIdDetected = '{ActiveProfile.DoubleIdDetected}';")
                .AppendLine($"onUserConnected = '{ActiveProfile.OnUserConnected}';")
                .AppendLine($"onUserDisconnected = '{ActiveProfile.OnUserDisconnected}';")
                .AppendLine($"onHackedData = '{ActiveProfile.OnHackedData}';")
                .AppendLine($"onDifferentData = '{ActiveProfile.OnDifferentData}';")
                .AppendLine($"onUnsignedData = '{ActiveProfile.OnUnsignedData}';")
                .AppendLine($"regularCheck = '{ActiveProfile.RegularCheck}';");

            // Missions
            if (ActiveProfile.Missions.Any())
            {
                configSb.AppendLine("class Missions {")
                    .AppendLine(GetMissions())
                    .AppendLine("};");
            }

            return configSb.ToString();
        }


        private string GetMotd()
        {
            var motd = "";
            for (var i = 0; i < ActiveProfile.Motd.Count; i++)
            {
                motd += "\t\"" + ActiveProfile.Motd[i] + "\"";
                motd += i < ActiveProfile.Motd.Count - 1 ? "," + Environment.NewLine : string.Empty;
            }
            return motd;
        }

        /// <summary>
        /// Return string with mission classes
        /// </summary>
        /// <returns>All checked missions</returns>
        private string GetMissions()
        {
            var missionSb = new StringBuilder();
            var index = 0;

            foreach (var mission in ActiveProfile.Missions)
            {
                if (mission.IsChecked)
                {
                    index++;
                    missionSb
                        .AppendLine($"\tclass Mission_{index} {{")
                        .AppendLine($"\t\ttemplate = \"{mission.Name.Replace(".pbo", "").Trim()}\";")
                        .AppendLine($"\t\tdifficulty = \"{GetDifficulty(mission.Difficulty)}\";")
                        .AppendLine("\t};")
                        .AppendLine();
                }
            }
            return missionSb.ToString();
        }


        private string GetDifficulty(int diff)
        {
            string difficulty;

            //switch (ActiveProfile.MissionDifficulty)
            switch (diff)
            {
                case 0:
                    difficulty = "recruit";
                    break;
                case 1:
                    difficulty = "regular";
                    break;
                case 2:
                    difficulty = "veteran";
                    break;
                case 3:
                    difficulty = "custom";
                    break;
                default:
                    difficulty = "";
                    break;
            }

            return difficulty;
        }


        public void CreateBasicConfigFile()
        {
            var path = Path.Combine(GetTADSTPath(), ActiveProfile.ProfileName);
            var file = Path.Combine(path, "TADST_basic.cfg");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            File.WriteAllText(file, GetBasicConfigString());
        }

        /// <summary>
        /// Create string to write to basic config file
        /// </summary>
        /// <returns></returns>
        private string GetBasicConfigString()
        {

            return new StringBuilder()
                .AppendLine($"// Basic config file generated {DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()} with TADST.")
                .AppendLine()
                .AppendLine($"MaxMsgSend = {ActiveProfile.MaxMsgSend};")
                .AppendLine($"MaxSizeGuaranteed = {ActiveProfile.MaxSizeGuaranteed};")
                .AppendLine($"MaxSizeNonguaranteed = {ActiveProfile.MaxSizeNonGuaranteed};")
                .AppendLine($"MinBandwidth = {ActiveProfile.MinBandWidth};")
                .AppendLine($"MaxBandwidth = {ActiveProfile.MaxBandwidth};")
                .AppendLine($"MinErrorToSend = {ActiveProfile.MinErrorToSend.ToString(CultureInfo.InvariantCulture)};")
                .AppendLine($"MinErrorToSendNear = {ActiveProfile.MinErrorToSendNear.ToString(CultureInfo.InvariantCulture)};")
                .AppendLine("class sockets {")
                .AppendLine($"\tmaxPacketSize = {ActiveProfile.MaxPacketSize};")
                .AppendLine("};")
                .AppendLine("adapter = -1;")
                .AppendLine("3D_Performance = 1;")
                .AppendLine("Resolution_W = 0;")
                .AppendLine("Resolution_H = 0;")
                .AppendLine("Resolution_Bpp = 32;")
                .AppendLine($"terrainGrid = {ActiveProfile.TerrainGrid.ToString(CultureInfo.InvariantCulture)};")
                .AppendLine($"viewDistance = {ActiveProfile.ViewDistance};")
                .AppendLine("Windowed = 0;")
                .ToString();
        }


        private void CreateProfileFile()
        {
            var path = Path.Combine(GetTADSTPath(), ActiveProfile.ProfileName, "Users", ActiveProfile.ProfileName);
            var fileOA = Path.Combine(path, ActiveProfile.ProfileName + ".ArmA2OAProfile");
            var fileA = Path.Combine(path, ActiveProfile.ProfileName + ".ArmA2Profile");
            var fileA3 = Path.Combine(path, ActiveProfile.ProfileName + ".Arma3Profile");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            File.WriteAllText(fileOA, GetProfileString("oa"));
            File.WriteAllText(fileA, GetProfileString("a2"));
            File.WriteAllText(fileA3, GetProfileString("a3"));
        }

        private string GetProfileString(string game)
        {
            string defaultDiff;
            switch (ActiveProfile.DefaultDifficulty)
            {
                case 0:
                    defaultDiff = "recruit";
                    break;
                case 1:
                    defaultDiff = "regular";
                    break;
                case 2:
                    defaultDiff = "veteran";
                    break;
                case 3:
                    defaultDiff = "custom";
                    break;
                default:
                    defaultDiff = "regular";
                    break;
            }

            return new StringBuilder()
                .AppendLine($"difficulty = \"{defaultDiff}\";")
                .AppendLine("class DifficultyPresets {")
                .AppendLine("\tclass CustomDifficulty {")
                .AppendLine(GetProfileOptions(ActiveProfile.DiffCustom, game))
                .AppendLine(GetProfileSkills(ActiveProfile.DiffCustom))
                .AppendLine("\t};")
                .AppendLine("};")
                .ToString();
        }

        /// <summary>
        /// Get difficulty flags string. Formatted for A2 or A3
        /// </summary>
        /// <param name="diff">The difficulty settings object that contains the items</param>
        /// <param name="game">If this is "a3" special Arma 3 settings will be added </param>
        /// <returns></returns>
        private static string GetProfileOptions(DifficultySetting diff, string game)
        {
            var profileOptionsSb = new StringBuilder();

            profileOptionsSb.AppendLine("\t\tclass Options {");

            foreach (var diffItem in diff.DifficultyItems)
            {
                if (diffItem.Name.Contains("(A3)") && game != "a3")
                {
                    continue;
                }
                profileOptionsSb.AppendLine($"\t\t\t{diffItem.GetConfigString()}");
            }

            profileOptionsSb.AppendLine("\t\t};");

            return profileOptionsSb.ToString();
        }

        private static string GetProfileSkills(DifficultySetting difficultySetting)
        {
            return new StringBuilder()
                .AppendLine($"\t\taiLevelPreset = {difficultySetting.AILevelPreset.ToString(CultureInfo.InvariantCulture)}")
                .AppendLine()
                .AppendLine("\t\tclass CustomAILevel {")
                .AppendLine($"\t\t\tskillAI = {difficultySetting.SkillAI.ToString(CultureInfo.InvariantCulture)};")
                .AppendLine($"\t\t\tprecisionAI = {difficultySetting.PrecisionAI.ToString(CultureInfo.InvariantCulture)};")
                .AppendLine("\t\t};")
                .ToString();
        }

        /// <summary>
        /// Add newline characters
        /// </summary>
        /// <param name="numOfLines">Number of newlines to add. Default is 1</param>
        /// <returns>String of newlines</returns>
        private static string NewLine(int numOfLines = 1)
        {
            var newLines = "";
            for (var i = 0; i < numOfLines; i++)
            {
                newLines += Environment.NewLine;
            }
            return newLines;
        }

        /// <summary>
        /// Create all config files
        /// </summary>
        internal void CreateConfigFiles()
        {
            CreateConfigFile();
            CreateBasicConfigFile();
            CreateProfileFile();
        }

        internal void ExportFiles(string targetPath)
        {
            CreateConfigFiles();
            CopyFiles(targetPath);
            CreateParametersFile(targetPath);
        }

        private void CreateParametersFile(string targetPath)
        {
            var textFile = Path.Combine(targetPath, "Startup Parameters.txt");
            var startupParameters = "Startup parameters for copy/paste:" + NewLine(2) +
                                    ActiveProfile.GetStartupParameters();

            File.WriteAllText(textFile, startupParameters);
        }

        private void CopyFiles(string targetPath)
        {
            var fileArray = GetFileArray();

            foreach (var file in fileArray)
            {
                if (!File.Exists(file)) continue;
                var target = Path.Combine(targetPath,
                                          file.Substring(file.LastIndexOf("\\", StringComparison.Ordinal) + 1));
                File.Copy(file, target, true);
            }
        }

        private IEnumerable<string> GetFileArray()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "TADST", ActiveProfile.ProfileName);
            var configFile = Path.Combine(path, "TADST_config.cfg");
            var basicConfigFile = Path.Combine(path, "TADST_basic.cfg");
            var arma2Profile = Path.Combine(path, "Users", ActiveProfile.ProfileName,
                                            ActiveProfile.ProfileName + ".ArmA2Profile");
            var arma2OAProfile = Path.Combine(path, "Users", ActiveProfile.ProfileName,
                                              ActiveProfile.ProfileName + ".ArmA2OAProfile");

            var fileArray = new string[] {configFile, basicConfigFile, arma2Profile, arma2OAProfile};

            return fileArray;
        }

        public bool LaunchServer(bool asIs)
        {
            var file = ActiveProfile.ServerExePath;

            if (!File.Exists(file))
            {
                return false;
            }

            if (asIs) CreateConfigFiles();
            StartServerProcess(file);

            return true;
        }

        private void StartServerProcess(string file)
        {
            /*var serverProcessStartInfo = new ProcessStartInfo {FileName = file};

            var serverProcess = new Process
                                        {
                                            StartInfo = serverProcessStartInfo, 
                                            EnableRaisingEvents = true
                                        };
            serverProcess.Exited += ReLaunch;
            serverProcess.Start();*/


            var serverProcess = new ProcessStartInfo(file)
                                    {
                                        WorkingDirectory = Environment.CurrentDirectory,
                                        Arguments = ActiveProfile.GetStartupParameters()
                                    };

            Process.Start(serverProcess);
        }

        private void ReLaunch(object sender, EventArgs e)
        {
            LaunchServer(true);
        }

        internal void DeleteProfile(string profileName)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "TADST", profileName);

            if (Directory.Exists(path))
            {
                try
                {
                    Directory.Delete(path, true);
                }
                catch (Exception)
                {
                }
            }
        }


        public string GetRptFile()
        {
            var serverExe = ActiveProfile.ServerExePath.Trim().ToLower();
            string file;

            if (serverExe.EndsWith("arma2server.exe")) file = "arma2server.RPT";
            else if (serverExe.EndsWith("arma2oaserver.exe")) file = "arma2oaserver.RPT";
            else if (serverExe.EndsWith("arma3server.exe") || serverExe.EndsWith("arma3server_x64.exe"))
            {
                file = GetArma3Rpt();
            }
            else
            {
                file = "";
            }
            return file;
        }

        private string GetArma3Rpt()
        {
            var folder =
                new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "TADST", ActiveProfile.ProfileName));
            FileInfo[] fileInfo = folder.GetFiles("*.rpt");
            List<String> fileNames = fileInfo.Select(info => info.Name).ToList();

            if (fileNames.Count == 0) return null;

            fileNames.Sort();
            return fileNames[fileNames.Count - 1];
        }

    }
}