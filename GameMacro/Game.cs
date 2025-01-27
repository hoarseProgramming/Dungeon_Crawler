using Dungeon_Crawler.Services;
using MongoDB.Bson.Serialization.Attributes;

namespace Dungeon_Crawler.GameMacro
{
    internal class Game
    {
        [BsonId]
        public int Id { get; set; } = 0;
        [BsonIgnore]
        public bool IsRunning = false;
        public Hero Hero { get; set; }

        public LevelData CurrentLevel { get; set; }

        public List<LevelData> Levels { get; set; } = [];

        public Settings Settings { get; set; }
        public Log GameLog { get; set; } = new();

        private AppData AppData;

        public void CreateNewGame(Settings settings, AppData appData)
        {
            //TODO: Load all levels ;-)
            Levels.Clear();

            Settings = settings;

            LevelData levelOne = new LevelData(1, this);

            Hero = new Hero(new Position(), false, "", this);

            levelOne.Load(settings, Hero);

            Levels.Add(levelOne);
            CurrentLevel = levelOne;

            AppData = appData;
        }
        public async Task RunGameLoop()
        {
            Console.CursorVisible = false;
            Hero.HasExitedGame = false;
            IsRunning = true;
            while (IsRunning)
            {
                await CurrentLevel.NewTurn();
                if (Hero.HasExitedGame)
                {
                    return;
                }
                CurrentLevel.DrawLevel();
                if (!Hero.IsAlive)
                {
                    ConsoleWriter.PrintGameOverMessage();
                    Console.ReadKey(true);

                    IsRunning = false;
                    Console.Clear();

                    if (Id != 0)
                    {
                        Console.CursorVisible = true;
                        ConsoleWriter.PrintDeleting();

                        AppData.SavedGames[Id - 1] = null;
                        bool hasDeletedSuccessfully = await DataBaseHandler.DeleteGameFromDatabase(this);

                        if (hasDeletedSuccessfully)
                        {
                            ConsoleWriter.PrintDeleteOutcome(hasDeletedSuccessfully);
                        }
                        else
                        {
                            ConsoleWriter.PrintDeleteOutcome(hasDeletedSuccessfully);
                            AppData.HasEstablishedConnectionToDatabase = false;
                        }

                        Console.CursorVisible = false;
                        Console.ReadKey();
                    }
                }
            }
        }

        public void DeMongoGame(AppData appData)
        {
            foreach (var level in Levels)
            {
                level.DeMongoLevel(this);
            }
            var currentLevelNumber = CurrentLevel.levelNumber;

            CurrentLevel = Levels[currentLevelNumber - 1];

            Hero = CurrentLevel.Hero;

            Hero.SetGame(this);

            AppData = appData;

        }
        public void levelElement_LogMessageSent(object sender, LogMessageSentEventArgs e)
        {

            GameLog.AddLogMessage(e.LogMessage);

            if (GameLog.LogMessages.Count <= 1)
            {
                ConsoleWriter.PrintInGameLogMessages(e.LogMessage, null);
            }
            else
            {
                var mostRecentLogMessage = GameLog.LogMessages[GameLog.LogMessages.Count - 2];

                ConsoleWriter.PrintInGameLogMessages(e.LogMessage, mostRecentLogMessage);
            }


            if (!(sender is Hero && e.LogMessage.MessageType == MessageType.Movement))
            {
                Thread.Sleep(400);
            }
        }
        internal async Task<SaveGameOutcome> SaveGame()
        {
            SaveGameOutcome outcome = new();

            bool saveIsSuccesful = false;

            if (AppData.SavedGames.Any(g => g?.Id == Id))
            {
                ConsoleWriter.PrintSaving();




                saveIsSuccesful = await DataBaseHandler.SaveGameToDataBase(this);
            }
            else
            {
                Id = AppData.SelectSaveFileForSaving();

                ConsoleWriter.PrintSaving();

                if (Id != 0)
                {
                    saveIsSuccesful = await DataBaseHandler.SaveGameToDataBase(this);

                    if (saveIsSuccesful)
                    {
                        AppData.SavedGames[Id - 1] = this;
                    }
                }
                else
                {
                    outcome.DidNotChooseSaveFile = true;
                }
            }

            outcome.SaveIsSuccessful = saveIsSuccesful;
            return outcome;
        }

        internal async Task RunInGameMenu()
        {
            Console.ForegroundColor = ConsoleColor.White;

            int currentLogIndex = GameLog.LogMessages.Count;

            ConsoleKeyInfo input = new();

            while (input.Key != ConsoleKey.Escape && input.Key != ConsoleKey.B)
            {
                ConsoleWriter.PrintInGameMenu();

                input = Console.ReadKey(true);

                if (input.Key == ConsoleKey.S)
                {
                    if (AppData.HasEstablishedConnectionToDatabase)
                    {
                        SaveGameOutcome outcome = await SaveGame();

                        Console.CursorVisible = false;

                        if (outcome.SaveIsSuccessful)
                        {
                            ConsoleWriter.PrintSaveOutcome(outcome.SaveIsSuccessful);

                            Console.ReadKey();

                            GameLog.AddLogMessage(new LogMessage(Hero, "Game saved", MessageType.Save));
                        }
                        else
                        {
                            if (!outcome.DidNotChooseSaveFile)
                            {
                                ConsoleWriter.PrintSaveOutcome(outcome.SaveIsSuccessful);

                                AppData.HasEstablishedConnectionToDatabase = false;

                                Console.ReadKey();
                            }
                        }
                    }
                    else
                    {
                        ConsoleWriter.PrintSaveOutcome(false);

                        Console.ReadKey();
                    }
                }
                else if (input.Key == ConsoleKey.Escape)
                {
                    IsRunning = false;
                    Hero.HasExitedGame = true;
                }
                else if (input.Key == ConsoleKey.L)
                {
                    Console.Clear();

                    while (input.Key != ConsoleKey.Backspace)
                    {
                        Console.SetCursorPosition(0, 0);
                        ConsoleWriter.PrintMaximumTenInMenuLogMessages(GameLog.LogMessages, currentLogIndex);

                        input = Console.ReadKey(true);

                        if (input.Key == ConsoleKey.UpArrow)
                        {
                            if (currentLogIndex - 10 <= 10)
                            {
                                currentLogIndex = 10;
                            }
                            else
                            {
                                currentLogIndex -= 10;
                            }
                        }
                        else if (input.Key == ConsoleKey.DownArrow)
                        {
                            if (currentLogIndex + 10 >= GameLog.LogMessages.Count)
                            {
                                currentLogIndex = GameLog.LogMessages.Count;
                            }
                            else
                            {
                                currentLogIndex += 10;
                            }
                        }
                    }
                }

            }

            Console.Clear();

            if (!Hero.HasExitedGame)
            {
                CurrentLevel.DrawLevel();
            }
        }
    }
}
