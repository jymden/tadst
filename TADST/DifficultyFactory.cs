namespace TADST
{
    /// <summary>
    /// Creates default difficulty items
    /// </summary>
    class DifficultyFactory
    {
        
        /// <summary>
        /// Create Custom Settings
        /// </summary>
        /// <returns>Custom setting object</returns>
        public DifficultySetting CreateCustomDifficulty()
        {
            var diffSetting = new DifficultySetting(0.75m, 0.55m, 3);

            diffSetting.AddDifficultyItem(new DifficultyItem("Reduced Damage", "reducedDamage", false, true, "Reduced Damage\n\nDecreases damage dealt to the player and members of his group."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Group Indicators", "groupIndicators", true, true, "Group Indicators\n\nShows indication icons on units in player's group."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Friendly Name Tags", "friendlyTags", true, true, "Friendly Name Tags\n\nFriendly unit identification. Works only when the player aims at a unit."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Enemy Name Tags", "enemyTags", true, true, "Enemy Name Tags\n\nEnemy unit identification. Works only when the player aims at a unit."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Detected Mines", "detectedMines", true, true, "Detected Mines\n\nShows icons indicating exact positions of mines. show = display the indicators on mines at 200m and closer."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Commands", "commands", true, true, "Commands\n\nDisplays command icons like target markers, get in command and others."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Waypoints", "waypoints", true, true, "Commands\n\nManages visibility of waypoint markers."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Weapon Info", "weaponInfo", true, true, "Commands\n\nShows Weapon Info box (contains round and magazine count, firing mode, etc.)."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Stance Indicator", "stanceIndicator", true, true, "Stance Indicator\n\nDisplays stance indication (contains actual stance, weapon resting and deployment)."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Stamina Bar", "staminaBar", true, true, "Stamina Bar\n\nManages visibility of stamina indicator. true = fades in when stamina changes."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Crosshair", "weaponCrosshair", true, true, "Crosshair\n\nShows weapon crosshair both in 1st and 3rd person view."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Vision Aid", "visionAid", true, true, "Vision Aid\n\nHelps to detect units in visual range and identify friend from foe."));
            diffSetting.AddDifficultyItem(new DifficultyItem("3rd Person View", "thirdPersonView", true, true, "3rd Person View\n\nEnables camera switching to 3rd person view."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Camera Shake", "cameraShake", true, true, "Camera Shake\n\nShakes camera if player is near explosion, passing vehicle or if player's vehicle is under G-load."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Score Table", "scoreTable", true, true, "Score Table\n\n	Displays table with kills, deaths and overall score in multiplayer."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Killed By", "deathMessages", true, true, "Killed By\n\nShows in chat window who killed the player."));
            diffSetting.AddDifficultyItem(new DifficultyItem("VON ID", "vonID", true, true, "VON ID\n\nIndicates who is speaking through VON communication."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Extended Map Content", "mapContent", true, true, "Extended Map Content\n\nShows friendly units, enemy units and detected mines on the map."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Automatic Reporting", "autoReport", true, true, "Automatic Reporting\n\nEnables automatic reporting of various situations by the player."));
            diffSetting.AddDifficultyItem(new DifficultyItem("Multiple Saves", "multipleSaves", true, true, "Multiple Saves\n\nAllows player to create multiple saves in a mission."));

            return diffSetting;
        }

    }
}
