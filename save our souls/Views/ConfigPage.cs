namespace save_our_souls.Views;

public class ConfigPage : ContentPage
{
    public ConfigPage()
    {
        var titleLabel = new Label
        {
            Text = "Configure Game",
            FontFamily = "SourGummySemiBold",
            FontSize = 22,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center
        };

        var singlePlayerOption = CreateGameModeOption("Singleplayer", true);
        var multiPlayerOption = CreateGameModeOption("Multiplayer");

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

        int gameSize = 5;

        var sizeLabel = new Label
        {
            Text = "5x5",
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

        Grid.SetColumn(lessButton, 0);
        Grid.SetColumn(sizeLabel, 1);
        Grid.SetColumn(moreButton, 2);

        var gameSizeSelect = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star)
            },
            Children =
            {
                lessButton,
                sizeLabel,
                moreButton
            }
        };

        var submitButton = new Button
        {
            Text = "Let's Play!",
            FontFamily = "SourGummySemiBold",
            BackgroundColor = Colors.DarkCyan
        };

        var verticalLayout = new VerticalStackLayout
        {
            Padding = 16,
            Spacing = 12,
            Children = { titleLabel, gameModeSelect, gameSizeSelect }
        };

        Grid.SetRow(verticalLayout, 0);
        Grid.SetRow(submitButton, 1);

        var gridLayout = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition(GridLength.Star),
                new RowDefinition(100)
            }
            Children =
            {
                verticalLayout,
                submitButton
            }
        };

        Content = new ScrollView
        {
            Content = verticalLayout
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

    private static View CreateGameModeOption(string text, bool isChecked = false)
    {
        var optionLabel = new Label
        {
            Text = text,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            FontAttributes = FontAttributes.Bold,
            FontFamily = "SourGummySemiBold"
        };

        var optionBorder = new Border
        {
            HeightRequest = 48,
            StrokeThickness = 2,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = new CornerRadius(10) },
            Stroke = Color.FromArgb("#808080"),
            BackgroundColor = Color.FromArgb("#1AFFFFFF"),
            Content = optionLabel
        };

        var optionRadioButton = new RadioButton
        {
            GroupName = "GameMode",
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
