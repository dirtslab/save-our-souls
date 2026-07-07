using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace save_our_souls.ViewModels;

public partial class GameVM : ObservableObject
{
    public ICommand SetSInTile { get; }
    public ICommand SetOInTile { get; }

    public ObservableCollection<string> CellLabels { get; }

    public string PlayerIndicator { get; set; }
    private int player;

    public GameVM()
    {
        int size = Preferences.Default.Get("GameSize", 3);
        CellLabels = new ObservableCollection<string>(Enumerable.Repeat(string.Empty, size * size));
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
        }
        else
        {
            player = 1;
            PlayerIndicator = "Player 1";
        }
        OnPropertyChanged(nameof(PlayerIndicator));
    }

    private void SetInBoard(int n, string value)
    {
        if (n < 0 || n >= CellLabels.Count || !CellLabels[n].Equals(string.Empty))
        {
            return;
        }

        CellLabels[n] = value;
        OnPropertyChanged(nameof(CellLabels));
        SwapPlayer();
    }
}
