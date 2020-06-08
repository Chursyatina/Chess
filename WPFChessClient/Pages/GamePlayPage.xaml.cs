using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFChessClient.EventArgsClasses;
using WPFChessClient.Interfaces;
using WPFChessClient.Logic;
using WPFChessClient.Structures;
using Figure = WPFChessClient.Logic.Figure;

namespace WPFChessClient.Pages
{
    /// <summary>
    /// Логика взаимодействия для GamePlayPage.xaml
    /// </summary>
    public partial class GamePlayPage : Page, IPageChanger
    {
        public enum Figures { King, Queen, Bishop, Knight, Rook, Pawn }
        public enum FiguresColor { white, black }
        public enum MoveResult { Check, CheckMate, StaleMate}

        Dictionary<char, ImageSource> BoardDisignations;

        Dictionary<string, ImageSource> BlackFigures;
        Dictionary<string, ImageSource> WhiteFigures;

        private BoardDimensions dimensions;

        readonly Brush FirstCellType = new SolidColorBrush(Color.FromRgb(101, 67, 33));

        readonly Brush EditCellType = new SolidColorBrush(Color.FromRgb(235, 233, 57));

        readonly Brush CheckCellType = new SolidColorBrush(Color.FromRgb(204, 29, 0));

        readonly Brush SecondCellType = new SolidColorBrush(Color.FromRgb(127, 95, 66));

        private Presenter Presenter;

        private List<UIElement> Drawable;

        private Point MouseDownPosition;

        private bool IsWhite = true;

        public GamePlayPage()
        {
            InitializeComponent();

            Drawable = new List<UIElement>();

            BoardDisignations = CreateBoardDisignations();
            BlackFigures = BlackDictionaryFilling();
            WhiteFigures = WhiteDictionaryFilling();

            //Presenter = new Presenter(this, Time);
            TextBlockTimerFirst.Background = Brushes.White;

            Exit.Visibility = Visibility.Hidden;
        }

        public event EventHandler<IPageArgs> PageChanged;

        public string FirstPlayerName { get; private set; }
        public string SecondPlayerName { get; private set; }
        public int Time { get; private set; }

        public void SetData(string firstPlayerName, string secondPlayerName, int time)
        {
            FirstPlayerName = firstPlayerName;
            SecondPlayerName = secondPlayerName;
            Time = time;
            Presenter = new Presenter(this, Time);
            SetTextFirstPlayerName(FirstPlayerName);
            SetTextSecondPlayerName(SecondPlayerName);
        }

        public void Start()
        {
            Presenter.CanvasUpdated();
        }

        public void UpdateCanvas()
        {
            if (GameCanvas.RenderSize.Width <= 0 || GameCanvas.RenderSize.Height <= 0) return;

            GameCanvas.Children.Clear();

            foreach (UIElement child in Drawable)
            {
                GameCanvas.Children.Add(child);
            }

            Drawable.Clear();
        }

        public void CreateBoard()
        {
            CreateBoardBase(dimensions);

            CreateCells(dimensions);

            CreateNumbers(dimensions, new Point(dimensions.FullBoard.X, dimensions.PlayBoard.Y));

            CreateNumbers(dimensions, new Point(dimensions.FullBoard.X + dimensions.PlayBoard.Width + dimensions.CellSide / 2, dimensions.PlayBoard.Y));

            CreateLetters(dimensions, new Point(dimensions.PlayBoard.X, dimensions.FullBoard.Y));

            CreateLetters(dimensions, new Point(dimensions.PlayBoard.X, dimensions.FullBoard.Y + dimensions.PlayBoard.Height + dimensions.CellSide / 2));
        }

        private void CreateNumbers(BoardDimensions dimensions, Point startPoint)
        {
            char start = (IsWhite) ? '8' : '1';
            char end = (IsWhite) ? '0' : '9';

            for (int i = 0; start != end; i++)
            {
                Image numberPicture = new Image();
                numberPicture.Source = BoardDisignations[start];
                numberPicture.Width = dimensions.CellSide / 3;
                numberPicture.Height = dimensions.CellSide / 3;

                Thickness margin = new Thickness();

                margin.Left = startPoint.X + (dimensions.CellSide / 2 - numberPicture.Width) / 2;
                margin.Top = startPoint.Y + dimensions.CellSide * i + (dimensions.CellSide - numberPicture.Height) / 2;

                numberPicture.Margin = margin;

                Drawable.Add(numberPicture);

                if (IsWhite) start--;
                else start++;
            }
        }

        private void CreateLetters(BoardDimensions dimensions, Point startPoint)
        {
            char start = (IsWhite) ? 'A' : 'H';
            char end = (IsWhite) ? 'I' : '@';

            for (int i = 0; start != end; i++)
            {
                Image letterPicture = new Image();
                letterPicture.Source = BoardDisignations[start];
                letterPicture.Width = dimensions.CellSide / 3;
                letterPicture.Height = dimensions.CellSide / 3;

                Thickness margin = new Thickness();
                margin.Left = startPoint.X + dimensions.CellSide * i + (dimensions.CellSide - letterPicture.Height) / 2;
                margin.Top = startPoint.Y + (dimensions.CellSide / 2 - letterPicture.Height) / 2;
                letterPicture.Margin = margin;

                Drawable.Add(letterPicture);

                if (IsWhite) start++;
                else start--;
            }
        }

        private void CreateCells(BoardDimensions dimensions)
        {
            for (int i = 0; i < BoardDimensions.CellCount; i++)
            {
                for (int j = 0; j < BoardDimensions.CellCount; j++)
                {

                    Brush color = ((i + j) % 2 == 0) ? FirstCellType : SecondCellType;

                    Rect rect = new Rect(dimensions.PlayBoard.X + dimensions.CellSide * i,
                                         dimensions.PlayBoard.Y + dimensions.CellSide * j,
                                         dimensions.CellSide, dimensions.CellSide);

                    Drawable.Add(GetRectangle(rect, color));
                }
            }
        }

        private void CreateBoardBase(BoardDimensions dimensions)
        {
            Rect firstRect = new Rect(dimensions.FullBoard.X, dimensions.FullBoard.Y, dimensions.FullBoard.Width, dimensions.FullBoard.Width);
            Rect secondRect = new Rect(dimensions.PlayBoard.X - BoardDimensions.Border, dimensions.PlayBoard.Y - BoardDimensions.Border,
                                       dimensions.PlayBoard.Width + BoardDimensions.Border * 2, dimensions.PlayBoard.Width + BoardDimensions.Border * 2);

            Drawable.Add(GetRectangle(firstRect, SecondCellType, FirstCellType));
            Drawable.Add(GetRectangle(secondRect, Brushes.Black, FirstCellType));
        }

        public void CalcBoardDimesions()
        {
            dimensions = new BoardDimensions();

            Rect fullBoard = new Rect();
            Point canvasSide = new Point(GameCanvas.RenderSize.Width, GameCanvas.RenderSize.Height);

            fullBoard.Width = canvasSide.X > canvasSide.Y ? canvasSide.Y : canvasSide.X;
            fullBoard.X = (canvasSide.X - fullBoard.Width) / 2;
            fullBoard.Y = (canvasSide.Y - fullBoard.Width) / 2;

            double cellSide = fullBoard.Width / (BoardDimensions.CellCount + 1);

            Rect playBoard = new Rect(fullBoard.X + cellSide / 2, fullBoard.Y + cellSide / 2, fullBoard.Width - 2 * cellSide / 2, fullBoard.Width - 2 * cellSide / 2);

            dimensions.FullBoard = fullBoard;
            dimensions.PlayBoard = playBoard;
            dimensions.CellSide = cellSide;
            dimensions.CanvasSide = canvasSide;
        }

        private Dictionary<char, ImageSource> CreateBoardDisignations()
        {
            Dictionary<char, ImageSource> dictionary = new Dictionary<char, ImageSource>();
            for (char i = '1'; i < '9'; i++)
            {
                dictionary.Add(i, new BitmapImage(new Uri("../Resources/" + i + ".png", UriKind.Relative)));
            }
            for (char i = 'A'; i <= 'H'; i++)
            {
                dictionary.Add(i, new BitmapImage(new Uri("../Resources/" + i + ".png", UriKind.Relative)));
            }
            return dictionary;
        }

        private Dictionary<string, ImageSource> BlackDictionaryFilling()
        {
            Dictionary<string, ImageSource> blackDictionary = new Dictionary<string, ImageSource>();
            blackDictionary.Add("Bishop", new BitmapImage(new Uri("../Resources/Figures/bB.png", UriKind.Relative)));
            blackDictionary.Add("King", new BitmapImage(new Uri("../Resources/Figures/bK.png", UriKind.Relative)));
            blackDictionary.Add("Knight", new BitmapImage(new Uri("../Resources/Figures/bN.png", UriKind.Relative)));
            blackDictionary.Add("Pawn", new BitmapImage(new Uri("../Resources/Figures/bP.png", UriKind.Relative)));
            blackDictionary.Add("Queen", new BitmapImage(new Uri("../Resources/Figures/bQ.png", UriKind.Relative)));
            blackDictionary.Add("Rook", new BitmapImage(new Uri("../Resources/Figures/bR.png", UriKind.Relative)));
            return blackDictionary;
        }

        private Dictionary<string, ImageSource> WhiteDictionaryFilling()
        {
            Dictionary<string, ImageSource> whiteDictionary = new Dictionary<string, ImageSource>();
            whiteDictionary.Add("Bishop", new BitmapImage(new Uri("../Resources/Figures/wB.png", UriKind.Relative)));
            whiteDictionary.Add("King", new BitmapImage(new Uri("../Resources/Figures/wK.png", UriKind.Relative)));
            whiteDictionary.Add("Knight", new BitmapImage(new Uri("../Resources/Figures/wN.png", UriKind.Relative)));
            whiteDictionary.Add("Pawn", new BitmapImage(new Uri("../Resources/Figures/wP.png", UriKind.Relative)));
            whiteDictionary.Add("Queen", new BitmapImage(new Uri("../Resources/Figures/wQ.png", UriKind.Relative)));
            whiteDictionary.Add("Rook", new BitmapImage(new Uri("../Resources/Figures/wR.png", UriKind.Relative)));
            return whiteDictionary;
        }

        private ImageSource GetFigureSource(Figures figure, FiguresColor color)
        {
            if (color == FiguresColor.white)
            {
                return WhiteFigures[figure.ToString()];
            }
            else
            {
                return BlackFigures[figure.ToString()];
            }
        }

        public void PutFigure(Point figurePosition, Figure figure)
        {
            Image figurePicture = new Image();
            figurePicture.Source = GetFigureSource(figure.Name, figure.Color);
            figurePicture.Width = dimensions.CellSide;
            figurePicture.Height = dimensions.CellSide;

            Thickness margin = new Thickness();
            if (!IsWhite)
            {
                figurePosition = RevercePointCoordinate(figurePosition);
            }

            margin.Left = dimensions.PlayBoard.X + dimensions.CellSide * (figurePosition.X);
            margin.Top = dimensions.PlayBoard.Y + dimensions.CellSide * (figurePosition.Y);

            figurePicture.Margin = margin;

            Drawable.Add(figurePicture);
        }

        private Rectangle GetRectangle(Rect rect, Brush color, Brush externalColor = null)
        {
            Rectangle rectangle = new Rectangle();

            rectangle.Margin = new Thickness(rect.X, rect.Y, 0, 0);
            rectangle.Width = rect.Width;
            rectangle.Height = rect.Height;
            rectangle.Fill = color;


            rectangle.StrokeThickness = BoardDimensions.Border + 0.1;
            rectangle.Stroke = externalColor;

            return rectangle;
        }

        private void QuitToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            PageChanged.Invoke(this, new ChangePageArgs(NamePage.MainMenu));
        }

        private void GameCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Presenter.CanvasUpdated();
        }

        public void EdmitCell(Point cellPosition)
        {
            if (!IsWhite)
            {
                cellPosition = RevercePointCoordinate(cellPosition);
            }
            Rect rect = new Rect(dimensions.PlayBoard.X + dimensions.CellSide * (cellPosition.X),
                                 dimensions.PlayBoard.Y + dimensions.CellSide * (cellPosition.Y),
                                 dimensions.CellSide, dimensions.CellSide);
            Drawable.Add(GetRectangle(rect, EditCellType));
        }

        public void EdmitCheck(Point cellPosition)
        {
            if (!IsWhite)
            {
                cellPosition = RevercePointCoordinate(cellPosition);
            }
            Rect rect = new Rect(dimensions.PlayBoard.X + dimensions.CellSide * (cellPosition.X),
                                 dimensions.PlayBoard.Y + dimensions.CellSide * (cellPosition.Y),
                                 dimensions.CellSide, dimensions.CellSide);
            Drawable.Add(GetRectangle(rect, CheckCellType));
        }

        public void SetTextTimerFirst(String text)
        {
            if (IsWhite)
            {
                TextBlockTimerFirst.Text = text;
            }
            else
            {
                TextBlockTimerSecond.Text = text;
            }
        }

        public void SetTextTimerSecond(String text)
        {
            if (IsWhite)
            {
                TextBlockTimerSecond.Text = text;
            }
            else
            {
                TextBlockTimerFirst.Text = text;
            }
        }

        public void SetTextFirstPlayerName(String text)
        {
            if (IsWhite)
            {
                TextBlockFirstPlayerName.Text = text;
            }
            else
            {
                TextBlockFirstPlayerName.Text = text;
            }
        }
        public void SetTextSecondPlayerName(String text)
        {
            if (IsWhite)
            {
                TextBlockSecondPlayerName.Text = text;
            }
            else
            {
                TextBlockSecondPlayerName.Text = text;
            }
        }

        public void ChangeActiveBackgroung()
        {
            if (TextBlockTimerFirst.Background == Brushes.White)
            {
                TextBlockTimerSecond.Background = Brushes.White;
                TextBlockTimerFirst.Background = Brushes.Transparent;
            }
            else
            {
                TextBlockTimerSecond.Background = Brushes.Transparent;
                TextBlockTimerFirst.Background = Brushes.White;
            }
        }

        public void SetMoveResult(MoveResult result, Player attacker)
        {
            switch (result)
            {
                case MoveResult.CheckMate:
                    SetTextGameResult("Мат");
                    if (attacker.GetFigureColor() == FiguresColor.black)
                        SetTextAttacker("Победа чёрных");
                    else SetTextAttacker("Победа белых");
                    break;
                case MoveResult.StaleMate:
                    SetTextGameResult("Пат");
                    if (attacker.GetFigureColor() == FiguresColor.black)
                        SetTextAttacker("Победа чёрных");
                    else SetTextAttacker("Победа белых");
                    break;
                case MoveResult.Check:
                    SetTextGameResult("Шах");
                    if (attacker.GetFigureColor() == FiguresColor.black)
                        SetTextAttacker("Белым");
                    else SetTextAttacker("Чёрным");
                    break;
            }
        }

        public void SetTimesUpResult(Player looser)
        {
            switch (looser.GetFigureColor())
            {
                case FiguresColor.black:
                    SetTextGameResult("Время");
                    SetTextAttacker("Победа белых");
                    break;
                case FiguresColor.white:
                    SetTextGameResult("Время");
                    SetTextAttacker("Победа чёрных");
                    break;
            }
        }

        public void SetTextGameResult(String text)
        {
            TextBlockGameResult.Text = text;
        }

        public void SetTextAttacker(String text)
        {
            TextBlockWinner.Text = text;
        }

        private Point GetBoardCoordinate(Point position)
        {
            Point coordinate = new Point(-1, -1);
            if (position.X >= dimensions.PlayBoard.X &&
                position.Y >= dimensions.PlayBoard.Y &&
                position.X <= dimensions.PlayBoard.X + dimensions.PlayBoard.Width &&
                position.Y <= dimensions.PlayBoard.Y + dimensions.PlayBoard.Height)
            {
                coordinate.X = (position.X - dimensions.PlayBoard.X) / dimensions.CellSide + 1;
                coordinate.Y = (position.Y - dimensions.PlayBoard.Y) / dimensions.CellSide + 1;
            }
            return coordinate;
        }

        private Point GetRoundedBoardCoordinate(Point coordinate)
        {
            Point rounedeCoordinate = GetBoardCoordinate(coordinate);
            rounedeCoordinate = new Point((int)rounedeCoordinate.X, (int)rounedeCoordinate.Y);
            return rounedeCoordinate;
        }

        private void GameCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPosition = new Point(e.GetPosition((IInputElement)sender).X, e.GetPosition((IInputElement)sender).Y);
            MouseDownPosition = new Point();
            MouseDownPosition = GetRoundedBoardCoordinate(clickPosition);
        }

        private void GameCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point clickPosition = new Point(e.GetPosition((IInputElement)sender).X, e.GetPosition((IInputElement)sender).Y);
            clickPosition = GetRoundedBoardCoordinate(clickPosition);

            if ((MouseDownPosition.X == clickPosition.X) && MouseDownPosition.Y == clickPosition.Y)
            {
                clickPosition.X = clickPosition.X - 1;
                clickPosition.Y = clickPosition.Y - 1;
                if (!IsWhite)
                {
                    clickPosition = RevercePointCoordinate(clickPosition);
                }
                Console.WriteLine(clickPosition);
                Presenter.ClickedOnBoard(new Point(clickPosition.X, clickPosition.Y));
            }
        }

        private Point RevercePointCoordinate(Point position)
        {
            return new Point(7 - position.X, 7 - position.Y);
        }

        public void MakeExitVisible()
        {
            Exit.Visibility = Visibility.Visible;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            PageChanged.Invoke(this, new ChangePageArgs(NamePage.MainMenu));
        }
    }
}
