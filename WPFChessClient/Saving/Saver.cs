using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void SaveUnendedgame() //TO DO
        {

        }
    }
}
