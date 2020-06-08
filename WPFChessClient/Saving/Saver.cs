using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFChessClient.EventArgsClasses;
using WPFChessClient.Logic;
using static WPFChessClient.Pages.GamePlayPage;

namespace WPFChessClient.Saving
{
    class Saver
    {
        private string GlobalPath;
        private string SubPath;
        private string FilePath;
        private string LeaderTablePath;
        private string UnendedGamePath;

        private FileInfo fileInfo;
        private FileInfo LeaderTableInfo;

        private string FirstPlayer;
        private string SecondPlayer;

        List<LeaderTablePlayer> Leaders;

        public Saver()
        {
            LeaderTablePath = "../../Saving/Saves/LeaderTable.txt";
            LeaderTableInfo = new FileInfo(LeaderTablePath);
            DowloadTable();
        }

        public List<LeaderTablePlayer> GetLeaderTable()
        {
            return Leaders;
        }

        public Saver(string firstPlayer, string secondPlayer, int time)
        {
            FirstPlayer = firstPlayer;
            SecondPlayer = secondPlayer;

            string GlobalPath = "../../Saving";
            string Subpath = "Saves/Games/UnfinishedGames";

            DirectoryInfo dirinfo = new DirectoryInfo(GlobalPath);
            if (!dirinfo.Exists)
            {
                dirinfo.Create();
                
            }
            dirinfo.CreateSubdirectory(Subpath);

            FilePath = "../../Saving/Saves/Games/" + firstPlayer +" vs " +  secondPlayer +" " + time + ".txt";
            fileInfo = new FileInfo(FilePath);

            LeaderTablePath = "../../Saving/Saves/LeaderTable.txt";
            LeaderTableInfo = new FileInfo(LeaderTablePath);
            if (!LeaderTableInfo.Exists)
            {
                LeaderTableInfo.Create();

            }
            

            Leaders = new List<LeaderTablePlayer>();

            DowloadTable();
        }
        
        public void SaveEndedGame(Player firstPlayer, Player secondPlayer)
        {
            fileInfo.Create();
            EditLeaderTable(firstPlayer, secondPlayer);
            SaveTable();
        }

        private void EditLeaderTable(Player firstPlayer, Player secondPlayer)
        {
            SetInLeaderTable();
            foreach (LeaderTablePlayer player in Leaders)
            {
                if (player.Name == FirstPlayer)
                {
                    player.Games++;
                    if (!firstPlayer.CheckWin()) player.Wins++;
                }
                if (player.Name == SecondPlayer)
                {
                    player.Games++;
                    if (!secondPlayer.CheckWin()) player.Wins++;
                }
            }
        }

        private void SetInLeaderTable()
        {
            bool firstCount = false;
            bool secondCount = false;

            foreach (LeaderTablePlayer player in Leaders)
            {
                if (FirstPlayer == player.Name) firstCount = true;
                if (SecondPlayer == player.Name) secondCount = true;
            }

            if (!firstCount)
            {
                LeaderTablePlayer player = new LeaderTablePlayer();
                player.Games = 0;
                player.Name = FirstPlayer;
                player.Wins = 0;
                Leaders.Add(player);
            }
            if (!secondCount)
            {
                LeaderTablePlayer player = new LeaderTablePlayer();
                player.Games = 0;
                player.Name = SecondPlayer;
                player.Wins = 0;
                Leaders.Add(player);
            }
        }

        private void DowloadTable()
        {
            using (FileStream fstream = File.OpenRead(LeaderTablePath))
            {
                // преобразуем строку в байты
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);
                // декодируем байты в строку
                string textFromFile = System.Text.Encoding.Default.GetString(array);
                Leaders = JsonConvert.DeserializeObject<List<LeaderTablePlayer>>(textFromFile);
            }
        }

        public void SaveTable()
        {
            string leaders = JsonConvert.SerializeObject(Leaders);

            using (FileStream fstream = new FileStream(LeaderTablePath, FileMode.Create))
            {
                // преобразуем строку в байты
                byte[] array = System.Text.Encoding.Default.GetBytes(leaders);
                // запись массива байтов в файл
                fstream.Write(array, 0, array.Length);
            }
        }

        public void SaveUnendedgame(SaveGameArgs e) 
        {
            UnendedGamePath = "../../Saving/Saves/Games/UnfinishedGames/" + FirstPlayer + " vs " + SecondPlayer + " " + e.Timer;
            DirectoryInfo dirInfo = new DirectoryInfo(UnendedGamePath);
            dirInfo.Create();

            string firstPlayerPath = UnendedGamePath + "/FirstPlayer.txt";
            FileInfo firstPlayerInfo = new FileInfo(firstPlayerPath);
            //firstPlayerInfo.Create();
            string firstPlayer = JsonConvert.SerializeObject(e.FirstPlayer);
            using (FileStream fstream = new FileStream(firstPlayerPath, FileMode.Create))
            {
                // преобразуем строку в байты
                byte[] array = System.Text.Encoding.Default.GetBytes(firstPlayer);
                // запись массива байтов в файл
                fstream.Write(array, 0, array.Length);
            }

            string secondPlayerPath = UnendedGamePath + "/SecondPlayer.txt";
            FileInfo secondPlayerInfo = new FileInfo(secondPlayerPath);
            //secondPlayerInfo.Create();
            string secondPlayer = JsonConvert.SerializeObject(e.SecondPlayer);
            using (FileStream fstream = new FileStream(secondPlayerPath, FileMode.Create))
            {
                // преобразуем строку в байты
                byte[] array = System.Text.Encoding.Default.GetBytes(secondPlayer);
                // запись массива байтов в файл
                fstream.Write(array, 0, array.Length);
            }

            string currentPlayerPath = UnendedGamePath + "/CurrentPlayer.txt";
            FileInfo currentPlayerInfo = new FileInfo(currentPlayerPath);
            //currentPlayerInfo.Create();
            string currentPlayer = JsonConvert.SerializeObject(e.CurrentPlayer);
            using (FileStream fstream = new FileStream(currentPlayerPath, FileMode.Create))
            {
                // преобразуем строку в байты
                byte[] array = System.Text.Encoding.Default.GetBytes(currentPlayer);
                // запись массива байтов в файл
                fstream.Write(array, 0, array.Length);
            }

            string boardPath = UnendedGamePath + "/Board.txt";
            FileInfo boardInfo = new FileInfo(boardPath);
            //boardInfo.Create();
            string board = JsonConvert.SerializeObject(e.Board);
            using (FileStream fstream = new FileStream(boardPath, FileMode.Create))
            {
                // преобразуем строку в байты
                byte[] array = System.Text.Encoding.Default.GetBytes(board);
                // запись массива байтов в файл
                fstream.Write(array, 0, array.Length);
            }

            string timerPath = UnendedGamePath + "/Timer.txt";
            FileInfo timerInfo = new FileInfo(timerPath);
            //timerInfo.Create();
            string timer = JsonConvert.SerializeObject(e.Timer);
            using (FileStream fstream = new FileStream(timerPath, FileMode.Create))
            {
                // преобразуем строку в байты
                byte[] array = System.Text.Encoding.Default.GetBytes(timer);
                // запись массива байтов в файл
                fstream.Write(array, 0, array.Length);
            }
        }
    }
}
