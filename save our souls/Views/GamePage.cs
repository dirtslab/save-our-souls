using CommunityToolkit.Maui.Behaviors;
using Microsoft.Maui.Controls.Shapes;
using save_our_souls.ViewModels;

namespace save_our_souls.Views;

public class GamePage : ContentPage
{
    public GamePage(GameVM gameVM)
    {
        BindingContext = gameVM;

        Padding = new Thickness(16, 32);

        var boardGrid = new Grid
        {
            RowSpacing = 2,
            ColumnSpacing = 2,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
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

        var sosLineLayer = new Grid
        {
            InputTransparent = true
        };

        var boardOverlay = new Grid
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Children = { boardGrid, sosLineLayer }
        };

        void UpdateSosLines()
        {
            if (boardGrid.Width <= 0 || boardGrid.Height <= 0)
            {
                return;
            }

            sosLineLayer.Children.Clear();

            if (gameVM.SosLineSegments.Count == 0)
            {
                return;
            }

            var cellWidth = (boardGrid.Width - boardGrid.Padding.HorizontalThickness - ((size - 1) * boardGrid.ColumnSpacing)) / size;
            var cellHeight = (boardGrid.Height - boardGrid.Padding.VerticalThickness - ((size - 1) * boardGrid.RowSpacing)) / size;

            foreach (var segment in gameVM.SosLineSegments)
            {
                int startRow = segment.StartIndex / size;
                int startColumn = segment.StartIndex % size;
                int endRow = segment.EndIndex / size;
                int endColumn = segment.EndIndex % size;

                var line = new Line
                {
                    Stroke = segment.LineColor,
                    Opacity = 0.5,
                    StrokeThickness = 5,
                    InputTransparent = true,
                    X1 = boardGrid.Padding.Left + (startColumn * (cellWidth + boardGrid.ColumnSpacing)) + (cellWidth / 2),
                    Y1 = boardGrid.Padding.Top + (startRow * (cellHeight + boardGrid.RowSpacing)) + (cellHeight / 2),
                    X2 = boardGrid.Padding.Left + (endColumn * (cellWidth + boardGrid.ColumnSpacing)) + (cellWidth / 2),
                    Y2 = boardGrid.Padding.Top + (endRow * (cellHeight + boardGrid.RowSpacing)) + (cellHeight / 2)
                };

                sosLineLayer.Children.Add(line);
            }
        }

        boardGrid.SizeChanged += (_, _) => UpdateSosLines();
        gameVM.SosLineSegments.CollectionChanged += (_, _) => UpdateSosLines();

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
            boardOverlay.WidthRequest = sideLength;
            boardOverlay.HeightRequest = sideLength;

            UpdateSosLines();
        };

        Label playerIndicator = new Label
        {
            FontFamily = "SourGummySemiBold",
            FontSize = 40,
            TextColor = Colors.Black,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center
        };

        playerIndicator.SetBinding(Label.TextColorProperty, nameof(GameVM.PlayerIndicatorColor), mode: BindingMode.OneWay);
        playerIndicator.SetBinding(Label.TextProperty, nameof(GameVM.PlayerIndicator), mode: BindingMode.OneWay);

        Label p1Score = new Label
        {
            FontFamily = "SourGummySemiBold",
            FontSize = 30,
            TextColor = Colors.Black,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center
        };

        Label p2Score = new Label
        {
            FontFamily = "SourGummySemiBold",
            FontSize = 30,
            TextColor = Colors.Black,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center
        };

        p1Score.SetBinding(Label.TextColorProperty, nameof(GameVM.P1Color), mode: BindingMode.OneWay);
        p1Score.SetBinding(Label.TextProperty, nameof(GameVM.Player1Score), mode: BindingMode.OneWay, stringFormat: "P1: {0}");
        Grid.SetRow(p1Score, 0);

        p2Score.SetBinding(Label.TextColorProperty, nameof(GameVM.P2Color), mode: BindingMode.OneWay);
        p2Score.SetBinding(Label.TextProperty, nameof(GameVM.Player2Score), mode: BindingMode.OneWay, stringFormat: "P2: {0}");
        Grid.SetRow(p2Score, 1);

        Grid scores = new Grid
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            RowDefinitions =
            {
                new RowDefinition(),
                new RowDefinition()
            },
            Children = { p1Score, p2Score }
        };


        FlexLayout layout = new FlexLayout
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Start,
            Wrap = Microsoft.Maui.Layouts.FlexWrap.Wrap,
            JustifyContent = Microsoft.Maui.Layouts.FlexJustify.SpaceAround,
            Children = { playerIndicator, boardOverlay, scores }
        };

        Content = layout;
    }
}
