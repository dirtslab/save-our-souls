using save_our_souls.ViewModels;

namespace save_our_souls.Views;

public class ConfigPage : ContentPage
{
    public ConfigPage(ConfigVM configVM)
    {
        BindingContext = configVM;

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

        _ = configVM.LoadProfileImageAsync();

        var titleLabel = new Label
        {
            Text = "Configure Game",
            FontFamily = "SourGummySemiBold",
            FontSize = 22,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center
        };

        var singlePlayerOption = CreateLabelOption("Singleplayer", "GameMode", Preferences.Default.Get<bool>("GameMode", true), out var singlePlayerRadioButton);
        var multiPlayerOption = CreateLabelOption("Multiplayer", "GameMode", !Preferences.Default.Get<bool>("GameMode", true), out var multiPlayerRadioButton);

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

        var lessButton = CreateCountButton("-");
        var moreButton = CreateCountButton("+");

        int gameSize = Preferences.Default.Get("GameSize", 5);

        var sizeLabel = new Label
        {
            Text = $"{gameSize}x{gameSize}",
            FontFamily = "SourGummySemiBold",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            FontSize = 30
        };

        moreButton.Clicked += (_, _) =>
        {
            if (gameSize < 9)
            {
                gameSize++;
                sizeLabel.Text = $"{gameSize}x{gameSize}";
            }
        };

        lessButton.Clicked += (_, _) =>
        {
            if (gameSize > 3)
            {
                gameSize--;
                sizeLabel.Text = $"{gameSize}x{gameSize}";
            }
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
        Grid.SetColumn(lessButton, 1);
        Grid.SetColumn(sizeLabel, 2);
        Grid.SetColumn(moreButton, 3);

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
                lessButton,
                sizeLabel,
                moreButton
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

        var colorOptions = new List<Color>
        {
            Colors.Red,
            Colors.Green,
            Colors.Blue,
            Colors.Yellow,
            Colors.Purple,
            Colors.Orange
        };

        var colorButtons = new List<RadioButton>();

        var colorSelect = new Grid();

        for (int i = 0; i < colorOptions.Count; i++)
        {
            colorSelect.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            var colorOption = CreateColorOption(colorOptions[i], "Color", Preferences.Default.Get<int>("Color", 0) == i, out var colorRadioButton);
            Grid.SetColumn(colorOption, i);
            colorSelect.Children.Add(colorOption);
            colorButtons.Add(colorRadioButton);
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

        var player2ColorButtons = new List<RadioButton>();
        var player2ColorSelect = new Grid();

        for (int i = 0; i < colorOptions.Count; i++)
        {
            player2ColorSelect.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            var colorOption = CreateColorOption(colorOptions[i], "Color2", Preferences.Default.Get<int>("Color2", 1) == i, out var colorRadioButton);
            Grid.SetColumn(colorOption, i);
            player2ColorSelect.Children.Add(colorOption);
            player2ColorButtons.Add(colorRadioButton);
        }

        var multiplayerOptions = new VerticalStackLayout
        {
            Spacing = 12,
            IsVisible = !singlePlayerRadioButton.IsChecked,
            Children =
            {
                player2ColorNameLabel,
                player2ColorSelect
            }
        };

        singlePlayerRadioButton.CheckedChanged += (_, e) =>
        {
            if (e.Value)
            {
                multiplayerOptions.IsVisible = false;
            }
        };

        multiPlayerRadioButton.CheckedChanged += (_, e) =>
        {
            if (e.Value)
            {
                multiplayerOptions.IsVisible = true;
            }
        };

        var submitButton = new Button
        {
            Text = "Start",
            FontFamily = "SourGummySemiBold",
            BackgroundColor = Colors.DarkCyan,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        submitButton.Clicked += async (_, _) =>
        {
            var selectedColorIndex = colorButtons.FindIndex(rb => rb.IsChecked);
            if (selectedColorIndex >= 0)
            {
                Preferences.Default.Set("Color", selectedColorIndex);
            }

            var selectedPlayer2ColorIndex = player2ColorButtons.FindIndex(rb => rb.IsChecked);
            if (selectedPlayer2ColorIndex >= 0 && !((selectedColorIndex == selectedPlayer2ColorIndex) && multiPlayerRadioButton.IsChecked))
            {
                Preferences.Default.Set("Color2", selectedPlayer2ColorIndex);
            }
            else
            {
                await DisplayAlertAsync("Whoops!", "Players cannot use the same color!", "OK");
                return;
            }



            var selectedGameMode = singlePlayerRadioButton.IsChecked;
            Preferences.Default.Set("GameMode", selectedGameMode);
            Preferences.Default.Set("GameSize", gameSize);

            try
            {
                await Shell.Current.GoToAsync(nameof(Views.GamePage));
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", ex.Message, "OK");
            }

        };

        var verticalLayout = new VerticalStackLayout
        {
            Padding = 16,
            Spacing = 12,
            Children = { titleLabel, gameModeSelect, gameSizeSelect, colorNameLabel, colorSelect, multiplayerOptions },
            VerticalOptions = LayoutOptions.Center
        };

        Grid.SetRow(nameGrid, 0);
        Grid.SetRow(verticalLayout, 1);
        Grid.SetRow(submitButton, 2);

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
                submitButton
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
}
