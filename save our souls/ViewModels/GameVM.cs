using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace save_our_souls.ViewModels;

public partial class GameVM : ObservableObject
{
    public ICommand SetSInTile { get; }
    public ICommand SetOInTile { get; }

    public ObservableCollection<string> CellLabels { get; }

    public GameVM()
    {
        int size = Preferences.Default.Get("GameSize", 3);
        CellLabels = new ObservableCollection<string>(Enumerable.Repeat(string.Empty, size * size));

        SetSInTile = new Command<int>(SetSInBoard);
        SetOInTile = new Command<int>(SetOInBoard);
    }

    private void SetSInBoard(int n)
    {
        if (n < 0 || n >= CellLabels.Count || !CellLabels[n].Equals(string.Empty))
        {
            return;
        }

        CellLabels[n] = "S";
        OnPropertyChanged(nameof(CellLabels));
    }

    private void SetOInBoard(int n)
    {
        if (n < 0 || n >= CellLabels.Count || !CellLabels[n].Equals(string.Empty))
        {
            return;
        }

        CellLabels[n] = "O";
        OnPropertyChanged(nameof(CellLabels));
    }
}
