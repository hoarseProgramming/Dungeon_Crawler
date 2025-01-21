namespace Dungeon_Crawler.GameMacro
{
    internal class Settings
    {
        public Settings(string chosenHeroName = "hoarse Hero", bool isDefaultSettings = true, bool shouldAnimateDiceThrows = false)
        {
            ChosenHeroName = chosenHeroName;
            IsDefaultSettings = isDefaultSettings;
            ShouldAnimateDiceThrows = shouldAnimateDiceThrows;
        }
        public bool IsDefaultSettings { get; set; }
        public string ChosenHeroName { get; set; }
        public bool ShouldAnimateDiceThrows { get; set; }
    }
}
