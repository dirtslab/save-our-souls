using save_our_souls.ViewModels;

namespace save_our_souls.Views;

public class ConfigPage : ContentPage
{
    private readonly ConfigVM _configVM;
    private readonly List<RadioButton> _colorButtons = new();
    private readonly List<RadioButton> _player2ColorButtons = new();
    private RadioButton _singlePlayerRadioButton = null!;
    private RadioButton _multiPlayerRadioButton = null!;
    private VerticalStackLayout _multiplayerOptions = null!;
    private Button _lessButton = null!;
    private Button _moreButton = null!;
    private Button _submitButton = null!;
    private int _gameSize = Preferences.Default.Get("GameSize", 5);
    private Label _sizeLabel = null!;

    public ConfigPage(ConfigVM configVM)
    {
        _configVM = configVM;
        BindingContext = _configVM;

        var nameLabel = new Label
        {
            Text = "Welcome!",
            FontFamily = "SourGummySemiBold",
            FontSize = 22,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center
        };
        nameLabel.SetBinding(Label.TextProperty, new Binding(nameof(ConfigVM.Username), stringFormat: "Welcome, {0}!"));

        var profileImage = new Image
        {
            WidthRequest = 100,
            HeightRequest = 100,
            Aspect = Aspect.AspectFill,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        profileImage.SetBinding(Image.SourceProperty, nameof(ConfigVM.ProfileImageUri));

        Grid.SetColumn(nameLabel, 0);
        Grid.SetColumn(profileImage, 1);

        var nameGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(GridLength.Auto)
            },
            Children =
            {
                nameLabel,
                profileImage
            },
            HorizontalOptions = LayoutOptions.Center,
            ColumnSpacing = 12
        };

        var titleLabel = new Label
        {
            Text = "Configure Game",
            FontFamily = "SourGummySemiBold",
            FontSize = 22,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center
        };

        var singlePlayerOption = CreateLabelOption("Singleplayer", "GameMode", Preferences.Default.Get<bool>("GameMode", true), out _singlePlayerRadioButton);
        var multiPlayerOption = CreateLabelOption("Multiplayer", "GameMode", !Preferences.Default.Get<bool>("GameMode", true), out _multiPlayerRadioButton);

        var gameModeSelect = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star)
            },
            ColumnSpacing = 12
        };

        gameModeSelect.Children.Add(singlePlayerOption);
        Grid.SetColumn(singlePlayerOption, 0);

        gameModeSelect.Children.Add(multiPlayerOption);
        Grid.SetColumn(multiPlayerOption, 1);

        _lessButton = CreateCountButton("-");
        _moreButton = CreateCountButton("+");

        _sizeLabel = new Label
        {
            Text = $"{_gameSize}x{_gameSize}",
            FontFamily = "SourGummySemiBold",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            FontSize = 30
        };



        var sizeNameLabel = new Label
        {
            Text = "Size:",
            FontFamily = "SourGummySemiBold",
            TextColor = Colors.Black,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            FontSize = 20
        };

        Grid.SetColumn(sizeNameLabel, 0);
        Grid.SetColumn(_lessButton, 1);
        Grid.SetColumn(_sizeLabel, 2);
        Grid.SetColumn(_moreButton, 3);

        var gameSizeSelect = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star)
            },
            Children =
            {
                sizeNameLabel,
                _lessButton,
                _sizeLabel,
                _moreButton
            }
        };

        var colorNameLabel = new Label
        {
            Text = "Player 1 Color:",
            FontFamily = "SourGummySemiBold",
            TextColor = Colors.Black,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            FontSize = 20
        };

        var colorOptions = ConfigColors.ColorOptions;

        var colorSelect = new Grid();

        for (int i = 0; i < colorOptions.Count; i++)
        {
            colorSelect.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            var colorOption = CreateColorOption(colorOptions[i], "Color", Preferences.Default.Get<int>("Color", 0) == i, out var colorRadioButton);
            Grid.SetColumn(colorOption, i);
            colorSelect.Children.Add(colorOption);
            _colorButtons.Add(colorRadioButton);
        }

        var player2ColorNameLabel = new Label
        {
            Text = "Player 2 Color:",
            FontFamily = "SourGummySemiBold",
            TextColor = Colors.Black,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            FontSize = 20
        };

        var player2ColorSelect = new Grid();

        for (int i = 0; i < colorOptions.Count; i++)
        {
            player2ColorSelect.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            var colorOption = CreateColorOption(colorOptions[i], "Color2", Preferences.Default.Get<int>("Color2", 1) == i, out var colorRadioButton);
            Grid.SetColumn(colorOption, i);
            player2ColorSelect.Children.Add(colorOption);
            _player2ColorButtons.Add(colorRadioButton);
        }

        _multiplayerOptions = new VerticalStackLayout
        {
            Spacing = 12,
            IsVisible = !_singlePlayerRadioButton.IsChecked,
            Children =
            {
                player2ColorNameLabel,
                player2ColorSelect
            }
        };



        _submitButton = new Button
        {
            Text = "Start",
            FontFamily = "SourGummySemiBold",
            BackgroundColor = Colors.DarkCyan,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };



        var verticalLayout = new VerticalStackLayout
        {
            Padding = 16,
            Spacing = 12,
            Children = { titleLabel, gameModeSelect, gameSizeSelect, colorNameLabel, colorSelect, _multiplayerOptions },
            VerticalOptions = LayoutOptions.Center
        };

        Grid.SetRow(nameGrid, 0);
        Grid.SetRow(verticalLayout, 1);
        Grid.SetRow(_submitButton, 2);

        var gridLayout = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition(100),
                new RowDefinition(GridLength.Star),
                new RowDefinition(100)
            },
            Children =
            {
                nameGrid,
                verticalLayout,
                _submitButton
            }
        };

        Content = new ScrollView
        {
            Content = gridLayout
        };
    }

    private static Button CreateCountButton(string text)
    {
        var button = new Button
        {
            Text = text,
            TextColor = Colors.Black,
            FontFamily = "SourGummySemiBold",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            FontSize = 24,
            WidthRequest = 48,
            HeightRequest = 48,
            BackgroundColor = Colors.LightGray,
            BorderColor = Color.FromArgb("#808080"),
            BorderWidth = 2,
            CornerRadius = 999
        };

        VisualStateManager.SetVisualStateGroups(button, new VisualStateGroupList
        {
            new VisualStateGroup
            {
                Name = "CommonStates",
                States =
                {
                    new VisualState { Name = "Normal" },
                    new VisualState
                    {
                        Name = "Pressed",
                        Setters =
                        {
                            new Setter{ Property = Button.BackgroundColorProperty, Value = Color.FromArgb("#224EA1FF") },
                            new Setter{ Property = Button.BorderColorProperty, Value = Color.FromArgb("#4EA1FF") }
                        }
                    }
                }
            }
        });

        return button;
    }

    private static View CreateColorOption(Color color, string group, bool isChecked, out RadioButton optionRadioButton)
    {
        var colorBox = new BoxView
        {
            Color = color,
            WidthRequest = 40,
            HeightRequest = 40,
            CornerRadius = 999,
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        return CreateModeOption(colorBox, group, isChecked, out optionRadioButton);
    }

    private static View CreateLabelOption(string text, string group, bool isChecked, out RadioButton optionRadioButton)
    {
        var label = new Label
        {
            Text = text,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            FontFamily = "SourGummySemiBold",
            FontSize = 20
        };
        return CreateModeOption(label, group, isChecked, out optionRadioButton);
    }

    private static View CreateModeOption(View contents, string group, bool isChecked, out RadioButton optionRadioButton)
    {

        var optionBorder = new Border
        {
            HeightRequest = 48,
            StrokeThickness = 2,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = new CornerRadius(10) },
            Stroke = Color.FromArgb("#808080"),
            BackgroundColor = Color.FromArgb("#1AFFFFFF"),
            Content = contents
        };

        optionRadioButton = new RadioButton
        {
            GroupName = group,
            IsChecked = isChecked,
            Opacity = 0,
            Content = string.Empty,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill
        };

        optionBorder.Triggers.Add(new DataTrigger(typeof(Border))
        {
            Binding = new Binding(nameof(RadioButton.IsChecked), source: optionRadioButton),
            Value = true,
            Setters =
            {
                new Setter { Property = Border.StrokeProperty, Value = Color.FromArgb("#4EA1FF") },
                new Setter { Property = Border.BackgroundColorProperty, Value = Color.FromArgb("#224EA1FF") }
            }
        });

        return new Grid
        {
            Children =
            {
                optionBorder,
                optionRadioButton
            }
        };
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = _configVM.LoadProfileImageAsync();

        _singlePlayerRadioButton.CheckedChanged -= OnSinglePlayerCheckedChanged;
        _singlePlayerRadioButton.CheckedChanged += OnSinglePlayerCheckedChanged;

        _multiPlayerRadioButton.CheckedChanged -= OnMultiPlayerCheckedChanged;
        _multiPlayerRadioButton.CheckedChanged += OnMultiPlayerCheckedChanged;

        _lessButton.Clicked -= OnLessButtonClicked;
        _lessButton.Clicked += OnLessButtonClicked;

        _moreButton.Clicked -= OnMoreButtonClicked;
        _moreButton.Clicked += OnMoreButtonClicked;

        _submitButton.Clicked -= OnSubmitButtonClicked;
        _submitButton.Clicked += OnSubmitButtonClicked;
    }

    protected override void OnDisappearing()
    {
        _singlePlayerRadioButton.CheckedChanged -= OnSinglePlayerCheckedChanged;
        _multiPlayerRadioButton.CheckedChanged -= OnMultiPlayerCheckedChanged;
        _lessButton.Clicked -= OnLessButtonClicked;
        _moreButton.Clicked -= OnMoreButtonClicked;
        _submitButton.Clicked -= OnSubmitButtonClicked;
        base.OnDisappearing();
    }

    private void OnSinglePlayerCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            _multiplayerOptions.IsVisible = false;
        }
    }

    private void OnMultiPlayerCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            _multiplayerOptions.IsVisible = true;
        }
    }

    private void OnMoreButtonClicked(object sender, EventArgs e)
    {
        if (_gameSize < 9)
        {
            _gameSize++;
            _sizeLabel.Text = $"{_gameSize}x{_gameSize}";
        }
    }


    private void OnLessButtonClicked(object sender, EventArgs e)
    {
        if (_gameSize > 3)
        {
            _gameSize--;
            _sizeLabel.Text = $"{_gameSize}x{_gameSize}";
        }
    }

    private async void OnSubmitButtonClicked(object sender, EventArgs e)
    {
        var selectedColorIndex = _colorButtons.FindIndex(rb => rb.IsChecked);

        var selectedPlayer2ColorIndex = _player2ColorButtons.FindIndex(rb => rb.IsChecked);
        if (((selectedColorIndex == selectedPlayer2ColorIndex) && _multiPlayerRadioButton.IsChecked))
        {
            await DisplayAlertAsync("Whoops!", "Players cannot use the same color!", "OK");
            return;
        }

        var selectedGameMode = _singlePlayerRadioButton.IsChecked;

        _configVM.savePreferences(selectedGameMode, _gameSize, selectedColorIndex, selectedPlayer2ColorIndex);

        try
        {
            await Shell.Current.GoToAsync(nameof(Views.GamePage));
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }

    }

}
