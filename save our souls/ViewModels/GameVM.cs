using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace save_our_souls.ViewModels;

public partial class GameVM : ObservableObject
{
    public ICommand SetSInTile { get; }

    public ObservableCollection<string> CellLabels { get; }

    public GameVM()
    {
        int size = Preferences.Default.Get("GameSize", 3);
        CellLabels = new ObservableCollection<string>(Enumerable.Repeat(string.Empty, size * size));

        SetSInTile = new Command<int>(SetSInBoard);
    }

    private void SetSInBoard(int n)
    {
        if (n < 0 || n >= CellLabels.Count)
        {
            return;
        }

        CellLabels[n] = "S";
        OnPropertyChanged(nameof(CellLabels));
    }
}
