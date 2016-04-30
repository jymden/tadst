namespace TADST
{
    /// <summary>
    /// Creates default difficulty items
    /// </summary>
    class DifficultyFactory
    {
        /// <summary>
        /// Create Recruit Settings
        /// </summary>
        /// <returns>Recruit setting object</returns>
        public DifficultySetting CreateRecruitDifficulty()
        {
            var diffSetting = new DifficultySetting(0.65m, 0.37m, 0);
            diffSetting.AddDifficultyItem(new DifficultyItem("Extended Armor", "Armor", true, true, "Extended Armor\n\nDecreases damage of player and his associated group"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Friendly Tag", "FriendlyTag", true, true, "Friendly Tag\n\nDisplays information on friendly units. \nONLY WORKS WITH \n'Weapon Cursor' on."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Enemy Tag", "EnemyTag", false, true, "Enemy Tag\n\nDisplays information on enemy units"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Show detected mines", "MineTag", true, true, "Show detected mines\n\nShows exact location of a mine in the 3d scene."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Extended HUD info", "Hud", true, true, "Hud\n\nDisplay leaders location and your position in formation."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Permanent extended HUD info", "HudPerm", true, true, "Hud Permanent\n\nShows HUD permanently."));
            diffSetting.AddDifficultyItem(new DifficultyItem("HUD waypoints info", "HudWp", true, true, "Hud Waypoint\n\nDisplay Waypoints right after they're ordered."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Permanent HUD waypoints info", "HudWpPerm", true, true, "HUDWpPerm\n\nHUD Waypoints info stays after lost visual contact.."));
            diffSetting.AddDifficultyItem(new DifficultyItem("HUD show group", "HudGroupInfo", true, true, "HUDGroupInfo\n\nShows icons around player's group members.."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Auto report", "AutoSpot", true, true, "Auto Spot\n\nIf you're close enough to an enemy, you'll report it without right-clicking."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Extended map info", "Map", true, true, "Map\n\nShows symbols for all objects known to your gruop on the map. \n\nNote that this will NOT disable the map itself."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Weapon crosshair", "weaponCrosshair", true, true, "Weapon Cursor\n\nShows the crosshair for your weapon."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Auto Guide AT", "AutoGuideAT", true, true, "Auto Guide AT\n\nAT missiles will be automatically guided to their target. If switched off, player has to lock onto the target."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Clock Indicator", "ClockIndicator", true, true, "Clock Indicator\n\nDisplay the clock indicator on the left of your screen when giving/receiving orders like 'At 11 o'clock, enemy man at 200 meters'"));
            diffSetting.AddDifficultyItem(new DifficultyItem("3rd Person View", "ThirdPersonView", true, true, "3rd Person View\n\nTurn 3rd person view and group leader view on or off."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Super AI", "UltraAI", false, true, "Ultra AI\n\nEnables super AI, hear and see more and increased tactics. Affects both friendly and enemy sides."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Auto Aim", "AutoAim", false, true, "Auto Aim\n\nEnables auto aim when you're not looking through your weapon's scope. \nAlso works with crosshair off."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Camera Shake", "CameraShake", false, true, "Camera Shake\n\nView will shake when an impact occurs close by."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Unlimited Saves", "UnlimitedSaves", true, true, "Unlimited Saves\n\nEnables unlimited saving for single player missions. But you can only load the last save state. \n\nOnly kept for reference, since it does nothing in Multiplayer."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Killed by", "DeathMessages", true, true, "Death Message\n\nDisplay death messages \ni.e. 'playerA kill playerB'"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Multiplayer score", "NetStats", true, true, "Net Stats\n\nDisplays score board."));
            diffSetting.AddDifficultyItem(new DifficultyItem("VoN ID", "VonID", true, true, "Voice over Net ID\n\nDisplay Voice over Net player name in window."));      
            diffSetting.AddDifficultyItem(new DifficultyItem("Extended Info Type", "ExtendetInfoType", true, true, "Extended Info Type\n\nNo information available. Only applies to ArmA 3."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Tracers", "Tracers", true, true, "Tracers\n\nDisplay tracers even from small arms that in reality don't have tracers."));
            return diffSetting;
        }

        /// <summary>
        /// Create Regular Settings
        /// </summary>
        /// <returns>Regular setting object</returns>
        public DifficultySetting CreateRegularDifficulty()
        {
            var diffSetting = new DifficultySetting(0.75m, 0.55m, 1);

            diffSetting.AddDifficultyItem(new DifficultyItem("3rd Person View", "ThirdPersonView", true, true, "3rd Person View\n\nTurn 3rd person view and group leader view on or off."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Extended Armor", "armor", true, true, "Armor\n\nDecreases damage of player and his associated group"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Auto Aim", "autoAim", false, true, "Auto Aim\n\nEnables auto aim when you're not looking through your weapon's scope. \nAlso works with crosshair off."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Auto Guide AT", "autoGuideAT", true, true, "Auto Guide AT\n\nAT missiles will be automatically guided to their target. If switched off, player has to lock onto the target."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Auto Spot", "autoSpot", true, true, "Auto Spot\n\nIf you're close enough to an enemy, you'll report it without right-clicking."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Camera Shake", "cameraShake", true, true, "Camera Shake\n\nView will shake when an impact occurs close by."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Clock Indicator", "clockIndicator", true, true, "Clock Indicator\n\nDisplay the clock indicator on the left of your screen when giving/receiving orders like 'At 11 o'clock, enemy man at 200 meters'"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Death Messages", "deathMessages", true, true, "Death Message\n\nDisplay death messages \ni.e. 'playerA kill playerB'"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Enemy Tag", "enemyTag", false, true, "Enemy Tag\n\nDisplays information on enemy units"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Friendly Tag", "friendlyTag", true, true, "Friendly Tag\n\nDisplays information on friendly units. \nONLY WORKS WITH \n'Weapon Cursor' on."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Hud", "hud", true, true, "Hud\n\nDisplay leaders location and your position in formation."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Hud Group Info", "hudGroupInfo", true, true, "Hud Group Information\n\nResult not verified."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Hud Perm", "hudPerm", true, true, "Hud Permanent\n\nShows HUD permanently."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Hud Wp", "hudWp", true, true, "Hud Waypoint\n\nDisplay Waypoints right after they're ordered."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Hud Wp Perm", "hudWpPerm", true, true, "Hud Waypoint Permanent\n\nShows Waypoints permanently."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Map", "map", true, true, "Map\n\nShows symbols for all objects known to your gruop on the map. \n\nNote that this will NOT disable the map itself."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Net Stats", "netStats", true, true, "Net Stats\n\nDisplays score board."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Tracers", "tracers", true, true, "Tracers\n\nDisplay tracers even from small arms that in reality don't have tracers."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Ultra AI", "ultraAI", false, true, "Ultra AI\n\nEnables super AI, hear and see more and increased tactics. Affects both friendly and enemy sides."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Unlimited Saves", "unlimitedSaves", true, true, "Unlimited Saves\n\nEnables unlimited saving for single player missions. But you can only load the last save state. \n\nOnly kept for reference, since it does nothing in Multiplayer."));
            diffSetting.AddDifficultyItem(new DifficultyItem("VoN ID", "vonID", true, true, "Voice over Net ID\n\nDisplay Voice over Net player name in window."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Weapon Crosshair", "weaponCrosshair", true, true, "Weapon Crosshair\n\nShows the crosshair for your weapon."));
            diffSetting.AddDifficultyItem(new DifficultyItem("(A3) Extended Info Type", "ExtendetInfoType", true, true, "Extended Info Type\n\nNo information available. Only applies to ArmA 3."));
            diffSetting.AddDifficultyItem(new DifficultyItem("(A3) Mine Tag", "MineTag", true, true, "Mine Tag\n\nNo information available. Only applies to ArmA 3."));


            return diffSetting;
        }

        /// <summary>
        /// Create Veteran Settings
        /// </summary>
        /// <returns>Veteran setting object</returns>
        public DifficultySetting CreateVeteranDifficulty()
        {
            var diffSetting = new DifficultySetting(0.85m, 0.73m, 2);

            diffSetting.AddDifficultyItem(new DifficultyItem("3rd Person View", "ThirdPersonView", true, true, "3rd Person View\n\nTurn 3rd person view and group leader view on or off."));
            //diffSetting.AddDifficultyItem(new DifficultyItem("Extended Armor", "armor", false, false, "Armor\n\nDecreases damage of player and his associated group"));
            //diffSetting.AddDifficultyItem(new DifficultyItem("Auto Aim", "autoAim", false, false, "Auto Aim\n\nEnables auto aim when you're not looking through your weapon's scope. \nAlso works with crosshair off.\n\nNot available at this difficulty level"));
            //diffSetting.AddDifficultyItem(new DifficultyItem("Auto Guide AT", "autoGuideAT", false, false, "Auto Guide AT\n\nAT missiles will be automatically guided to their target. If switched off, player has to lock onto the target.\n\nNot available at this difficulty level"));
            //diffSetting.AddDifficultyItem(new DifficultyItem("Auto Spot", "autoSpot", false, false, "Auto Spot\n\nIf you're close enough to an enemy, you'll report it without right-clicking.\n\nNot available at this difficulty level"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Camera Shake", "cameraShake", true, true, "Camera Shake\n\nView will shake when an impact occurs close by."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Clock Indicator", "clockIndicator", false, true, "Clock Indicator\n\nDisplay the clock indicator on the left of your screen when giving/receiving orders like 'At 11 o'clock, enemy man at 200 meters'"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Death Messages", "deathMessages", true, true, "Death Message\n\nDisplay death messages \ni.e. 'playerA kill playerB'"));
            //diffSetting.AddDifficultyItem(new DifficultyItem("Enemy Tag", "enemyTag", false, false, "Enemy Tag\n\nDisplays information on enemy units\n\nNot available at this difficulty level"));
            //diffSetting.AddDifficultyItem(new DifficultyItem("Friendly Tag", "friendlyTag", false, false, "Friendly Tag\n\nDisplays information on friendly units. \nONLY WORKS WITH \n'Weapon Cursor' on.\n\nNot available at this difficulty level"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Hud", "hud", true, true, "Hud\n\nDisplay leaders location and your position in formation."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Hud Group Info", "hudGroupInfo", false, true, "Hud Group Information\n\nResult not verified."));
            //diffSetting.AddDifficultyItem(new DifficultyItem("Hud Perm", "hudPerm", false, false, "Hud Permanent\n\nShows HUD permanently.\n\nNot available at this difficulty level"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Hud Wp", "hudWp", true, true, "Hud Waypoint\n\nDisplay Waypoints right after they're ordered."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Hud Wp Perm", "hudWpPerm", false, true, "Hud Waypoint Permanent\n\nShows Waypoints permanently."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Map", "map", false, true, "Map\n\nShows symbols for all objects known to your gruop on the map. \n\nNote that this will NOT disable the map itself."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Net Stats", "netStats", true, true, "Net Stats\n\nDisplays score board."));
            //diffSetting.AddDifficultyItem(new DifficultyItem("Tracers", "tracers", false, false, "Tracers\n\nDisplay tracers even from small arms that in reality don't have tracers.\n\nNot available at this difficulty level"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Ultra AI", "ultraAI", false, true, "Ultra AI\n\nEnables super AI, hear and see more and increased tactics. Affects both friendly and enemy sides."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Unlimited Saves", "unlimitedSaves", false, true, "Unlimited Saves\n\nEnables unlimited saving for single player missions. But you can only load the last save state. \n\nOnly kept for reference, since it does nothing in Multiplayer."));
            diffSetting.AddDifficultyItem(new DifficultyItem("VoN ID", "vonID", true, true, "Voice over Net ID\n\nDisplay Voice over Net player name in window."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Weapon Crosshair", "weaponCrosshair", true, true, "Weapon Crosshair\n\nShows the crosshair for your weapon."));
            diffSetting.AddDifficultyItem(new DifficultyItem("(A3) Extended Info Type", "ExtendetInfoType", false, true, "Extended Info Type\n\nNo information available. Only applies to ArmA 3."));
            //diffSetting.AddDifficultyItem(new DifficultyItem("(A3) Mine Tag", "MineTag", false, false, "Mine Tag\n\nNo information available. Only applies to ArmA 3.\n\nNot available at this difficulty level"));

            return diffSetting;
        }

        /// <summary>
        /// Create Expert Settings
        /// </summary>
        /// <returns>Expert setting object</returns>
        public DifficultySetting CreateExpertDifficulty()
        {
            var diffSetting = new DifficultySetting(1.00m, 1.00m, 2);

            /*
            diffSetting.AddDifficultyItem(new DifficultyItem("3rd Person View", "3rdPersonView", false, false, "3rd Person View\n\nTurn 3rd person view and group leader view on or off.\n\nNot available at this difficulty level"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Extended Armor", "armor", false, false, "Armor\n\nDecreases damage of player and his associated group"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Auto Aim", "autoAim", false, false, "Auto Aim\n\nEnables auto aim when you're not looking through your weapon's scope. \nAlso works with crosshair off.\n\nNot available at this difficulty level"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Auto Guide AT", "autoGuideAT", false, false, "Auto Guide AT\n\nAT missiles will be automatically guided to their target. If switched off, player has to lock onto the target.\n\nNot available at this difficulty level"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Auto Spot", "autoSpot", false, false, "Auto Spot\n\nIf you're close enough to an enemy, you'll report it without right-clicking.\n\nNot available at this difficulty level"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Camera Shake", "cameraShake", true, false, "Camera Shake\n\nView will shake when an impact occurs close by.\n\nAlways set at this difficulty level"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Clock Indicator", "clockIndicator", false, false, "Clock Indicator\n\nDisplay the clock indicator on the left of your screen when giving/receiving orders like 'At 11 o'clock, enemy man at 200 meters'\n\nNot available at this difficulty level"));
            */
            diffSetting.AddDifficultyItem(new DifficultyItem("Death Messages", "deathMessages", false, true, "Death Message\n\nDisplay death messages \ni.e. 'playerA kill playerB'\n\nNot available at this difficulty level"));
            
            /*
            diffSetting.AddDifficultyItem(new DifficultyItem("Enemy Tag", "enemyTag", false, false, "Enemy Tag\n\nDisplays information on enemy units\n\nNot available at this difficulty level"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Friendly Tag", "friendlyTag", false, false, "Friendly Tag\n\nDisplays information on friendly units. \nONLY WORKS WITH \n'Weapon Cursor' on.\n\nNot available at this difficulty level"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Hud", "hud", false, false, "Hud\n\nDisplay leaders location and your position in formation.\n\nNot available at this difficulty level"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Hud Group Info", "hudGroupInfo", false, false, "Hud Group Information\n\nResult not verified.\n\nNot available at this difficulty level"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Hud Perm", "hudPerm", false, false, "Hud Permanent\n\nShows HUD permanently\n\nNot available at this difficulty level"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Hud Wp", "hudWp", false, false, "Hud Waypoint\n\nDisplay Waypoints right after they're ordered.\n\nNot available at this difficulty level"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Hud Wp Perm", "hudWpPerm", false, false, "Hud Waypoint Permanent\n\nShows Waypoints permanently.\n\nNot available at this difficulty level"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Map", "map", false, false, "Map\n\nShows symbols for all objects known to your gruop on the map. \n\nNote that this will NOT disable the map itself.\n\nNot available at this difficulty level"));
            */
            diffSetting.AddDifficultyItem(new DifficultyItem("Net Stats", "netStats", false, true, "Net Stats\n\nDisplays score board."));
            //diffSetting.AddDifficultyItem(new DifficultyItem("Tracers", "tracers", false, false, "Tracers\n\nDisplay tracers even from small arms that in reality don't have tracers.\n\nNot available at this difficulty level"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Ultra AI", "ultraAI", false, true, "Ultra AI\n\nEnables super AI, hear and see more and increased tactics. Affects both friendly and enemy sides."));
            //diffSetting.AddDifficultyItem(new DifficultyItem("Unlimited Saves", "unlimitedSaves", false, false, "Unlimited Saves\n\nEnables unlimited saving for single player missions. But you can only load the last save state. \n\nOnly kept for reference, since it does nothing in Multiplayer.\n\nNot available at this difficulty level"));
            diffSetting.AddDifficultyItem(new DifficultyItem("VoN ID", "vonID", false, true, "Voice over Net ID\n\nDisplay Voice over Net player name in window."));
            /*
            diffSetting.AddDifficultyItem(new DifficultyItem("Weapon Crosshair", "weaponCrosshair", false, false, "Weapon Crosshair\n\nShows the crosshair for your weapon.\n\nNot available at this difficulty level"));
            diffSetting.AddDifficultyItem(new DifficultyItem("(A3) Extended Info Type", "ExtendetInfoType", false, false, "Extended Info Type\n\nNo information available. Only applies to ArmA 3.\n\nNot available at this difficulty level"));
            diffSetting.AddDifficultyItem(new DifficultyItem("(A3) Mine Tag", "MineTag", false, false, "Mine Tag\n\nNo information available. Only applies to ArmA 3.\n\nNot available at this difficulty level"));
            */
            return diffSetting;
        }

        /// <summary>
        /// Create Custom Settings
        /// </summary>
        /// <returns>Custom setting object</returns>
        public DifficultySetting CreateCustomDifficulty()
        {
            var diffSetting = new DifficultySetting(0.75m, 0.55m, 3);

            diffSetting.AddDifficultyItem(new DifficultyItem("3rd Person View", "ThirdPersonView", true, true, "3rd Person View\n\nTurn 3rd person view and group leader view on or off."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Extended Armor", "armor", true, true, "Armor\n\nDecreases damage of player and his associated group"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Auto Aim", "autoAim", false, true, "Auto Aim\n\nEnables auto aim when you're not looking through your weapon's scope. \nAlso works with crosshair off."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Auto Guide AT", "autoGuideAT", true, true, "Auto Guide AT\n\nAT missiles will be automatically guided to their target. If switched off, player has to lock onto the target."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Auto Spot", "autoSpot", true, true, "Auto Spot\n\nIf you're close enough to an enemy, you'll report it without right-clicking."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Camera Shake", "cameraShake", true, true, "Camera Shake\n\nView will shake when an impact occurs close by."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Clock Indicator", "clockIndicator", true, true, "Clock Indicator\n\nDisplay the clock indicator on the left of your screen when giving/receiving orders like 'At 11 o'clock, enemy man at 200 meters'"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Death Messages", "deathMessages", true, true, "Death Message\n\nDisplay death messages \ni.e. 'playerA kill playerB'"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Enemy Tag", "enemyTag", false, true, "Enemy Tag\n\nDisplays information on enemy units"));
            diffSetting.AddDifficultyItem(new DifficultyItem("Friendly Tag", "friendlyTag", true, true, "Friendly Tag\n\nDisplays information on friendly units. \nONLY WORKS WITH \n'Weapon Cursor' on."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Hud", "hud", true, true, "Hud\n\nDisplay leaders location and your position in formation."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Hud Group Info", "hudGroupInfo", true, true, "Hud Group Information\n\nResult not verified."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Hud Perm", "hudPerm", true, true, "Hud Permanent\n\nShows HUD permanently."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Hud Wp", "hudWp", true, true, "Hud Waypoint\n\nDisplay Waypoints right after they're ordered."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Hud Wp Perm", "hudWpPerm", true, true, "Hud Waypoint Permanent\n\nShows Waypoints permanently."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Map", "map", true, true, "Map\n\nShows symbols for all objects known to your gruop on the map. \n\nNote that this will NOT disable the map itself."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Net Stats", "netStats", true, true, "Net Stats\n\nDisplays score board."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Tracers", "tracers", true, true, "Tracers\n\nDisplay tracers even from small arms that in reality don't have tracers."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Ultra AI", "ultraAI", false, true, "Ultra AI\n\nEnables super AI, hear and see more and increased tactics. Affects both friendly and enemy sides."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Unlimited Saves", "unlimitedSaves", true, true, "Unlimited Saves\n\nEnables unlimited saving for single player missions. But you can only load the last save state. \n\nOnly kept for reference, since it does nothing in Multiplayer."));
            diffSetting.AddDifficultyItem(new DifficultyItem("VoN ID", "vonID", true, true, "Voice over Net ID\n\nDisplay Voice over Net player name in window."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Weapon Crosshair", "weaponCrosshair", true, true, "Weapon Crosshair\n\nShows the crosshair for your weapon."));
            diffSetting.AddDifficultyItem(new DifficultyItem("(A3) Extended Info Type", "ExtendetInfoType", true, true, "Extended Info Type\n\nNo information available. Only applies to ArmA 3."));
            diffSetting.AddDifficultyItem(new DifficultyItem("(A3) Mine Tag", "MineTag", true, true, "Mine Tag\n\nNo information available. Only applies to ArmA 3."));


            return diffSetting;
        }

    }
}
