#nullable enable
using Avalonia;
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

        var darkThemeMenuItem = this.FindControl<MenuItem>("DarkThemeMenuItem");
        if (darkThemeMenuItem != null)
        {
            darkThemeMenuItem.Click += OnDarkThemeMenuItemClicked;
        }

        var lightThemeMenuItem = this.FindControl<MenuItem>("LightThemeMenuItem");
        if (lightThemeMenuItem != null)
        {
            lightThemeMenuItem.Click += OnLightThemeMenuItemClicked;
        }        

        var exitMenuItem = this.FindControl<MenuItem>("ExitMenuItem");
        if (exitMenuItem != null)
        {
            exitMenuItem.Click += OnExitMenuItemClicked;
        }

        var projectWindowMenuItem = this.FindControl<MenuItem>("ProjectWindowMenuItem");
        if (projectWindowMenuItem != null)
        {
            projectWindowMenuItem.Click += OnProjectWindowMenuItemClicked;
        }

        ToggleMenuBtn.Click += OnToggleMenuClicked;
    }

    private void OnDarkThemeMenuItemClicked(object? sender, RoutedEventArgs e)
    {
        Avalonia.Application.Current!.RequestedThemeVariant = Avalonia.Styling.ThemeVariant.Dark;
    }

    private void OnLightThemeMenuItemClicked(object? sender, RoutedEventArgs e)
    {
        Avalonia.Application.Current!.RequestedThemeVariant = Avalonia.Styling.ThemeVariant.Light;
    }

    private void OnProjectWindowMenuItemClicked(object? sender, RoutedEventArgs e)
    {
        _isMenuCollapsed = false; // Разворачиваем меню
        ToggleMenuBtn.IsVisible = !_isMenuCollapsed;
        MainMenuTree.IsVisible = !_isMenuCollapsed;
        UpdateLayout();           // Обновляем ширину
    }

    private void OnExitMenuItemClicked(object? sender, RoutedEventArgs e)
    {
        Close(); // Закрывает окно и завершает приложение
    }

    private void SetupMenu()
    {
        var menuItems = new[]
        {
            new MenuItemModel("Общие данные"),
            new MenuItemModel("Геометрия",
                new MenuItemModel("Форма корпуса"),
                new MenuItemModel("Отсеки"),
                new MenuItemModel("Надстройки")),
            new MenuItemModel("Гидростатика"),
            new MenuItemModel("Случаи загрузки"),
            new MenuItemModel("Остойчивость"),
            new MenuItemModel("Аварийная остойчивость"),
            new MenuItemModel("Вероятностый индекс деления на отсеки"),
            new MenuItemModel("Интегральный усилия на тихой воде"),
            new MenuItemModel("Расчет надводного борта"),
            new MenuItemModel("Обработка результатов кренования")
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
        ToggleMenuBtn.IsVisible = !_isMenuCollapsed;
        MainMenuTree.IsVisible = !_isMenuCollapsed;
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
                columns[0].Width = new GridLength(0);       // ✅ Скрыть меню полностью
                columns[2].Width = new GridLength(1, GridUnitType.Star); // Расширить 3-ю часть
            }
            else
            {
                columns[0].Width = new GridLength(360); // Показать меню
                columns[2].Width = new GridLength(1, GridUnitType.Star); // Вернуть ширину
            }
        }
    }
}