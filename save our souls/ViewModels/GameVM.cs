using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace save_our_souls.ViewModels;

public sealed class SosLineSegment
{
    public int StartIndex { get; }
    public int EndIndex { get; }
    public Color LineColor { get; }

    public SosLineSegment(int startIndex, int endIndex, Color lineColor)
    {
        StartIndex = startIndex;
        EndIndex = endIndex;
        LineColor = lineColor;
    }
}

public partial class GameVM : ObservableObject
{
    public ICommand SetSInTile { get; }
    public ICommand SetOInTile { get; }

    public ObservableCollection<string> CellLabels { get; }
    public ObservableCollection<SosLineSegment> SosLineSegments { get; }

    public string PlayerIndicator { get; set; }
    public Color PlayerIndicatorColor { get; set; }
    public Color P1Color { get; set; }
    public Color P2Color { get; set; }

    public int Player1Score { get; set; }
    public int Player2Score { get; set; }

    private int player;
    private int boardSize;

    public GameVM()
    {
        boardSize = Preferences.Default.Get("GameSize", 3);
        CellLabels = new ObservableCollection<string>(Enumerable.Repeat(string.Empty, boardSize * boardSize));
        SosLineSegments = [];
        Player1Score = Player2Score = 0;

        P1Color = ConfigColors.ColorOptions[Preferences.Default.Get("Color", 0)];
        P2Color = ConfigColors.ColorOptions[Preferences.Default.Get("Color2", 0)];

        player = 0;
        SwapPlayer();

        SetSInTile = new Command<int>(n => SetInBoard(n, "S"));
        SetOInTile = new Command<int>(n => SetInBoard(n, "O"));
    }

    private void SwapPlayer()
    {
        if (player == 1)
        {
            player = 2;
            PlayerIndicator = "Player 2";
            PlayerIndicatorColor = P2Color;
        }
        else
        {
            player = 1;
            PlayerIndicator = "Player 1";
            PlayerIndicatorColor = P1Color;
        }
        OnPropertyChanged(nameof(PlayerIndicator));
        OnPropertyChanged(nameof(PlayerIndicatorColor));
    }

    private void SetInBoard(int n, string value)
    {
        if (n < 0 || n >= CellLabels.Count || !CellLabels[n].Equals(string.Empty))
        {
            return;
        }

        CellLabels[n] = value;
        OnPropertyChanged(nameof(CellLabels));

        if (!CheckForSOS(n))
        {
            SwapPlayer();
        }
    }

    private bool CheckForSOS(int n)
    {
        int cPlayer = player;
        int row = n / boardSize;
        int column = n % boardSize;
        int value = GetAtPosition(row, column);
        bool hasSos = false;

        if (value == 0)
        {
            (int dr, int dc)[] directions =
            [
                (-1, -1), (-1, 0), (-1, 1),
                (0, -1),            (0, 1),
                (1, -1),  (1, 0),   (1, 1)
            ];

            foreach (var (dr, dc) in directions)
            {
                int middleRow = row + dr;
                int middleColumn = column + dc;
                int endRow = row + (2 * dr);
                int endColumn = column + (2 * dc);

                if (GetAtPosition(middleRow, middleColumn) == 1 &&
                    GetAtPosition(endRow, endColumn) == 0)
                {
                    AddSosLine(row, column, endRow, endColumn, cPlayer);
                    hasSos = true;
                }
            }
        }
        else if (value == 1)
        {
            (int dr, int dc)[] axisDirections =
            [
                (0, 1),
                (1, 0),
                (1, 1),
                (1, -1)
            ];

            foreach (var (dr, dc) in axisDirections)
            {
                int startRow = row - dr;
                int startColumn = column - dc;
                int endRow = row + dr;
                int endColumn = column + dc;

                if (GetAtPosition(startRow, startColumn) == 0 &&
                    GetAtPosition(endRow, endColumn) == 0)
                {
                    AddSosLine(startRow, startColumn, endRow, endColumn, cPlayer);
                    hasSos = true;
                }
            }
        }

        return hasSos;
    }

    private void AddSosLine(int startRow, int startColumn, int endRow, int endColumn, int cPlayer)
    {
        int startIndex = startRow * boardSize + startColumn;
        int endIndex = endRow * boardSize + endColumn;
        Color color;


        bool exists = SosLineSegments.Any(s =>
            (s.StartIndex == startIndex && s.EndIndex == endIndex) ||
            (s.StartIndex == endIndex && s.EndIndex == startIndex));

        if (exists) return;

        if (cPlayer == 1)
        {
            color = ConfigColors.ColorOptions[Preferences.Default.Get("Color", 0)];
            Player1Score++;
        }
        else
        {
            color = ConfigColors.ColorOptions[Preferences.Default.Get("Color2", 0)];
            Player2Score++;
        }

        SosLineSegments.Add(new SosLineSegment(startIndex, endIndex, color));

        OnPropertyChanged(nameof(Player1Score));
        OnPropertyChanged(nameof(Player2Score));
    }

    private int GetAtPosition(int row, int column)
    {
        if (row < 0 || column < 0 || row >= boardSize || column >= boardSize)
        {
            return -1;
        }

        return CellLabels[boardSize * row + column] switch
        {
            "S" => 0,
            "O" => 1,
            _ => -1
        };
    }
}
