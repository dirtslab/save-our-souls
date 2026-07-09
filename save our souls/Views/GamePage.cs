using CommunityToolkit.Maui.Behaviors;
using Microsoft.Maui.Controls.Shapes;
using save_our_souls.ViewModels;

namespace save_our_souls.Views;

public class GamePage : ContentPage
{

    private readonly GameVM _gameVM;
    private Grid _boardGrid = null!;
    private Grid _boardOverlay = null!;
    private Grid _sosLineLayer = null!;
    private int _size = Preferences.Default.Get("GameSize", 3);

    public GamePage(GameVM gameVM)
    {
        _gameVM = gameVM;
        BindingContext = _gameVM;

        Padding = new Thickness(16, 32);

        _boardGrid = new Grid
        {
            RowSpacing = 2,
            ColumnSpacing = 2,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            BackgroundColor = Colors.Black,
            Padding = 2
        };

        for (int i = 0; i < _size; i++)
        {
            _boardGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            _boardGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        }

        for (int row = 0; row < _size; row++)
        {
            for (int col = 0; col < _size; col++)
            {
                int cellIndex = (row * _size) + col;

                var touchBehavior = new TouchBehavior
                {
                    DefaultAnimationDuration = 250,
                    DefaultAnimationEasing = Easing.CubicInOut,
                    PressedOpacity = 0.6,
                    PressedScale = 0.8,
                    ShouldMakeChildrenInputTransparent = true,
                    Command = _gameVM.SetSInTile,
                    CommandParameter = cellIndex,
                    LongPressCommand = _gameVM.SetOInTile,
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

                _boardGrid.Add(cell, col, row);
            }
        }

        _sosLineLayer = new Grid
        {
            InputTransparent = true
        };

        _boardOverlay = new Grid
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Children = { _boardGrid, _sosLineLayer }
        };



        _boardGrid.SizeChanged += (_, _) => UpdateSosLines(true);
        _gameVM.SosLineSegments.CollectionChanged += (_, _) => UpdateSosLines();

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
            Children = { playerIndicator, _boardOverlay, scores }
        };

        Content = layout;
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        if (width <= 0 || height <= 0)
        {
            return;
        }

        var availableWidth = Math.Max(0, width - Padding.Left - Padding.Right);
        var availableHeight = Math.Max(0, height - Padding.Top - Padding.Bottom);
        var sideLength = Math.Min(availableWidth, availableHeight);

        _boardGrid.WidthRequest = sideLength;
        _boardGrid.HeightRequest = sideLength;
        _boardOverlay.WidthRequest = sideLength;
        _boardOverlay.HeightRequest = sideLength;
    }

    private void UpdateSosLines(bool clear = false)
    {
        if (_boardGrid.Width <= 0 || _boardGrid.Height <= 0)
        {
            return;
        }

        if (clear)
        {
            _sosLineLayer.Children.Clear();
        }

        if (_gameVM.SosLineSegments.Count == 0)
        {
            return;
        }

        var startSegmentIndex = clear ? 0 : _sosLineLayer.Children.Count;

        if (startSegmentIndex > _gameVM.SosLineSegments.Count)
        {
            _sosLineLayer.Children.Clear();
            startSegmentIndex = 0;
        }

        var cellWidth = (_boardGrid.Width - _boardGrid.Padding.HorizontalThickness - ((_size - 1) * _boardGrid.ColumnSpacing)) / _size;
        var cellHeight = (_boardGrid.Height - _boardGrid.Padding.VerticalThickness - ((_size - 1) * _boardGrid.RowSpacing)) / _size;

        for (int i = startSegmentIndex; i < _gameVM.SosLineSegments.Count; i++)
        {
            var segment = _gameVM.SosLineSegments[i];

            int startRow = segment.StartIndex / _size;
            int startColumn = segment.StartIndex % _size;
            int endRow = segment.EndIndex / _size;
            int endColumn = segment.EndIndex % _size;

            var line = new Line
            {
                Stroke = segment.LineColor,
                Opacity = 0.5,
                StrokeThickness = 5,
                InputTransparent = true,
                X1 = _boardGrid.Padding.Left + (startColumn * (cellWidth + _boardGrid.ColumnSpacing)) + (cellWidth / 2),
                Y1 = _boardGrid.Padding.Top + (startRow * (cellHeight + _boardGrid.RowSpacing)) + (cellHeight / 2),
                X2 = _boardGrid.Padding.Left + (endColumn * (cellWidth + _boardGrid.ColumnSpacing)) + (cellWidth / 2),
                Y2 = _boardGrid.Padding.Top + (endRow * (cellHeight + _boardGrid.RowSpacing)) + (cellHeight / 2)
            };

            _sosLineLayer.Children.Add(line);
        }
    }
}
