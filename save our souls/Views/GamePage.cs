using CommunityToolkit.Maui.Behaviors;
using save_our_souls.ViewModels;

namespace save_our_souls.Views;

public class GamePage : ContentPage
{
    public GamePage(GameVM gameVM)
    {
        BindingContext = gameVM;

        Padding = new Thickness(16, 16);

        var boardGrid = new Grid
        {
            RowSpacing = 2,
            ColumnSpacing = 2,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            BackgroundColor = Colors.Black,
            Padding = 2
        };

        int size = Preferences.Default.Get("GameSize", 3);

        for (int i = 0; i < size; i++)
        {
            boardGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            boardGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        }

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                int cellIndex = (row * size) + col;

                var touchBehavior = new TouchBehavior
                {
                    DefaultAnimationDuration = 250,
                    DefaultAnimationEasing = Easing.CubicInOut,
                    PressedOpacity = 0.6,
                    PressedScale = 0.8,
                    ShouldMakeChildrenInputTransparent = true,
                    Command = gameVM.SetSInTile,
                    CommandParameter = cellIndex,
                    LongPressCommand = gameVM.SetOInTile,
                    LongPressCommandParameter = cellIndex
                };

                var cellLabel = new Label
                {
                    FontFamily = "SourGummySemiBold",
                    FontSize = 22,
                    TextColor = Colors.Black,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };

                var cell = new Border
                {
                    BackgroundColor = Colors.White,
                    StrokeThickness = 0,
                    Content = cellLabel
                };

                cell.Behaviors.Add(touchBehavior);

                cellLabel.SetBinding(Label.TextProperty, $"CellLabels[{cellIndex}]", mode: BindingMode.TwoWay);

                boardGrid.Add(cell, col, row);
            }
        }

        SizeChanged += (_, _) =>
        {
            if (Width <= 0 || Height <= 0)
            {
                return;
            }

            var availableWidth = Math.Max(0, Width - Padding.Left - Padding.Right);
            var availableHeight = Math.Max(0, Height - Padding.Top - Padding.Bottom);
            var sideLength = Math.Min(availableWidth, availableHeight);

            boardGrid.WidthRequest = sideLength;
            boardGrid.HeightRequest = sideLength;
        };

        

        Content = boardGrid;
    }
}
