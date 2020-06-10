using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFChessClient.EventArgsClasses;
using WPFChessClient.Logic;
using WPFChessClient.Structures;
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
            UnendedGamePath = "../../Saving/Saves/Games/UnfinishedGames/" + FirstPlayer + " vs " + SecondPlayer + " " + e.FirstPlayer.Time +"-"+ e.SecondPlayer.Time;
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
            string board = JsonConvert.SerializeObject(GetIntBoard(e.Board));
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

            string firstPlayerNamePath = UnendedGamePath + "/FIrstName.txt";
            FileInfo firsNameInfo = new FileInfo(firstPlayerNamePath);
            string firstPlayerName = JsonConvert.SerializeObject(FirstPlayer);
            using (FileStream fstream = new FileStream(firstPlayerNamePath, FileMode.Create))
            {
                // преобразуем строку в байты
                byte[] array = System.Text.Encoding.Default.GetBytes(firstPlayerName);
                // запись массива байтов в файл
                fstream.Write(array, 0, array.Length);
            }

            string secondPlayerNamePath = UnendedGamePath + "/SecondName.txt";
            FileInfo secondNameInfo = new FileInfo(secondPlayerNamePath);
            string secondPlayerName = JsonConvert.SerializeObject(SecondPlayer);
            using (FileStream fstream = new FileStream(secondPlayerNamePath, FileMode.Create))
            {
                // преобразуем строку в байты
                byte[] array = System.Text.Encoding.Default.GetBytes(secondPlayerName);
                // запись массива байтов в файл
                fstream.Write(array, 0, array.Length);
            }
        }

        public ChangePageToUnendedGameArgs DowloadUnendedGame(string selectedFile)
        {
            ChangePageToUnendedGameArgs args = new ChangePageToUnendedGameArgs();

            //UnendedGamePath = "../../Saving/Saves/Games/UnfinishedGames/WhitePlayer vs BlackPlayer 25-26";

            using (FileStream fstream = File.OpenRead(selectedFile + "/FirstName.txt"))
            {
                // преобразуем строку в байты
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);
                // декодируем байты в строку
                string textFromFile = System.Text.Encoding.Default.GetString(array);
                args.FirstPlayerName = JsonConvert.DeserializeObject<string>(textFromFile);
            }

            using (FileStream fstream = File.OpenRead(selectedFile + "/SecondName.txt"))
            {
                // преобразуем строку в байты
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);
                // декодируем байты в строку
                string textFromFile = System.Text.Encoding.Default.GetString(array);
                args.SecondPlayerName = JsonConvert.DeserializeObject<string>(textFromFile);
            }

            using (FileStream fstream = File.OpenRead(selectedFile + "/CurrentPlayer.txt"))
            {
                // преобразуем строку в байты
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);
                // декодируем байты в строку
                string textFromFile = System.Text.Encoding.Default.GetString(array);
                args.CurrentPlayer = JsonConvert.DeserializeObject<Player>(textFromFile);
            }
            
            using (FileStream fstream = File.OpenRead(selectedFile + "/FirstPlayer.txt"))
            {
                // преобразуем строку в байты
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);
                // декодируем байты в строку
                string textFromFile = System.Text.Encoding.Default.GetString(array);
                args.FirstPlayer = JsonConvert.DeserializeObject<Player>(textFromFile);
            }

            using (FileStream fstream = File.OpenRead(selectedFile + "/SecondPlayer.txt"))
            {
                // преобразуем строку в байты
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);
                // декодируем байты в строку
                string textFromFile = System.Text.Encoding.Default.GetString(array);
                args.SecondPlayer = JsonConvert.DeserializeObject<Player>(textFromFile);
            }

            using (FileStream fstream = File.OpenRead(selectedFile + "/Board.txt"))
            {
                // преобразуем строку в байты
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);
                // декодируем байты в строку
                string textFromFile = System.Text.Encoding.Default.GetString(array);
                int[,] figures = JsonConvert.DeserializeObject<int[,]>(textFromFile);
                args.Board = GetFigureboard(figures);
            }

            return args;
        }

        private int[,] GetIntBoard(Figure[,] board)
        {
            int[,] intFigures = new int[BoardDimensions.CellCount, BoardDimensions.CellCount];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] != null && board[i,j].Name == Figures.King && board[i, j].Color == FiguresColor.white)
                    {
                        intFigures[i, j] = 3;
                    }
                    if (board[i, j] != null && board[i, j].Name == Figures.King && board[i, j].Color == FiguresColor.black)
                    {
                        intFigures[i, j] = 4;
                    }

                    if (board[i, j] != null && board[i, j].Name == Figures.Pawn && board[i, j].Color == FiguresColor.white)
                    {
                        intFigures[i, j] = 1;
                    }
                    if (board[i, j] != null && board[i, j].Name == Figures.Pawn && board[i, j].Color == FiguresColor.black)
                    {
                        intFigures[i, j] = 2;
                    }

                    if (board[i, j] != null && board[i, j].Name == Figures.Bishop && board[i, j].Color == FiguresColor.white)
                    {
                        intFigures[i, j] = 5;
                    }
                    if (board[i, j] != null && board[i, j].Name == Figures.Bishop && board[i, j].Color == FiguresColor.black)
                    {
                        intFigures[i, j] = 6;
                    }

                    if (board[i, j] != null && board[i, j].Name == Figures.Queen && board[i, j].Color == FiguresColor.white)
                    {
                        intFigures[i, j] = 7;
                    }
                    if (board[i, j] != null && board[i, j].Name == Figures.Queen && board[i, j].Color == FiguresColor.black)
                    {
                        intFigures[i, j] = 8;
                    }

                    if (board[i, j] != null && board[i, j].Name == Figures.Rook && board[i, j].Color == FiguresColor.white)
                    {
                        intFigures[i, j] = 9;
                    }
                    if (board[i, j] != null && board[i, j].Name == Figures.Rook && board[i, j].Color == FiguresColor.black)
                    {
                        intFigures[i, j] = 10;
                    }

                    if (board[i, j] != null && board[i, j].Name == Figures.Knight && board[i, j].Color == FiguresColor.white)
                    {
                        intFigures[i, j] = 11;
                    }
                    if (board[i, j] != null && board[i, j].Name == Figures.Knight && board[i, j].Color == FiguresColor.black)
                    {
                        intFigures[i, j] = 12;
                    }
                }
            }

            return intFigures;
        }

        private Figure[,] GetFigureboard(int [,] board)
        {
            Figure[,] figures = new Figure[BoardDimensions.CellCount, BoardDimensions.CellCount];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i,j] == 1)
                    {
                        figures[i, j] = new Pawn(Figures.Pawn, FiguresColor.white);
                    }
                    if (board[i, j] == 2)
                    {
                        figures[i, j] = new Pawn(Figures.Pawn, FiguresColor.black);
                    }

                    if (board[i, j] == 3)
                    {
                        figures[i, j] = new King(Figures.King, FiguresColor.white);
                    }
                    if (board[i, j] == 4)
                    {
                        figures[i, j] = new King(Figures.King, FiguresColor.black);
                    }

                    if (board[i, j] == 5)
                    {
                        figures[i, j] = new Bishop(Figures.Bishop, FiguresColor.white);
                    }
                    if (board[i, j] == 6)
                    {
                        figures[i, j] = new Bishop(Figures.Bishop, FiguresColor.black);
                    }

                    if (board[i, j] == 7)
                    {
                        figures[i, j] = new Queen(Figures.Queen, FiguresColor.white);
                    }
                    if (board[i, j] == 8)
                    {
                        figures[i, j] = new Queen(Figures.Queen, FiguresColor.black);
                    }

                    if (board[i, j] == 9)
                    {
                        figures[i, j] = new Rook(Figures.Rook, FiguresColor.white);
                    }
                    if (board[i, j] == 10)
                    {
                        figures[i, j] = new Rook(Figures.Rook, FiguresColor.black);
                    }

                    if (board[i, j] == 11)
                    {
                        figures[i, j] = new Knight(Figures.Knight, FiguresColor.white);
                    }
                    if (board[i, j] == 12)
                    {
                        figures[i, j] = new Knight(Figures.Knight, FiguresColor.black);
                    }
                }
            }

            return figures;
        }

        public List<string> GetUnendedGamesList()
        {
            List<string> unendedGames = new List<string>();

            UnendedGamePath = "../../Saving/Saves/Games/UnfinishedGames";

            string[] dirs;

            if (Directory.Exists(UnendedGamePath))
            {
                dirs = Directory.GetDirectories(UnendedGamePath);
            }
            else
            {
                return unendedGames;
            }

            for (int i = 0; i < dirs.Length; i ++)
            {
                unendedGames.Add(dirs[i]);
            }

                return unendedGames;
        }
    }
}
