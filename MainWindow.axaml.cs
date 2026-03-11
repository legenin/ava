#nullable enable
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.ObjectModel;

namespace ava;

public partial class MainWindow : Window
{
    public ObservableCollection<MenuItemModel> MenuItemsSource { get; set; }

    private bool _isMenuCollapsed = false;

    public bool IsMenuCollapsed => _isMenuCollapsed;

    public MainWindow()
    {
        MenuItemsSource = new ObservableCollection<MenuItemModel>();
        InitializeComponent();
        SetupMenu();
        ToggleMenuBtn.Click += OnToggleMenuClicked;
    }

    private void SetupMenu()
    {
        var menuItems = new[]
        {
            new MenuItemModel("Элемент 1"),
            new MenuItemModel("Элемент 2",
                new MenuItemModel("Подэлемент 2.1"),
                new MenuItemModel("Подэлемент 2.2"),
                new MenuItemModel("Подэлемент 2.3")),
            new MenuItemModel("Элемент 3"),
            new MenuItemModel("Элемент 4"),
            new MenuItemModel("Элемент 5"),
            new MenuItemModel("Элемент 6"),
            new MenuItemModel("Элемент 7"),
            new MenuItemModel("Элемент 8"),
            new MenuItemModel("Элемент 9"),
            new MenuItemModel("Элемент 10")
        };

        foreach (var item in menuItems)
        {
            MenuItemsSource.Add(item);
        }

        DataContext = this;
    }

    private void OnToggleMenuClicked(object? sender, RoutedEventArgs e)
    {
        _isMenuCollapsed = !_isMenuCollapsed;
        UpdateLayout();
    }

    new void UpdateLayout()
    {
        var parent = MenuContainer?.Parent;
        if (parent is Grid grid)
        {
            var columns = grid.ColumnDefinitions;
            if (_isMenuCollapsed)
            {
                columns[0].Width = new GridLength(60);       // ✅ Скрыть меню полностью
                columns[2].Width = new GridLength(0.8, GridUnitType.Star); // Расширить 3-ю часть
            }
            else
            {
                columns[0].Width = new GridLength(0.3, GridUnitType.Star); // Показать меню
                columns[2].Width = new GridLength(0.65, GridUnitType.Star); // Вернуть ширину
            }
        }
    }
}