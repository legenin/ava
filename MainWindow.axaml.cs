#nullable enable
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using ava.Models;

namespace ava;

public partial class MainWindow : Window
{
    public ObservableCollection<Person> Persons { get; set; }
    public ObservableCollection<MenuItemModel> MenuItemsSource { get; set; }
    public ObservableCollection<DataMatrixRow> DataMatrix { get; set; }
    public LineChartModel LineChart { get; set; }

    private bool _isMenuCollapsed = false;

    public bool IsMenuCollapsed => _isMenuCollapsed;

    public MainWindow()
    {
        Persons = new ObservableCollection<Person>();
        MenuItemsSource = new ObservableCollection<MenuItemModel>();
        DataMatrix = new ObservableCollection<DataMatrixRow>();
        LineChart = new LineChartModel();

        Console.WriteLine("MainWindow constructor started.");

        InitializeComponent();
        SetupPersons();
        SetupMenu();
        SetupDataMatrix(); // ✅ Заполняем таблицу данными
        UpdateChartFromDataMatrix(); // ✅ Обновляем график на основе данных

        // Устанавливаем DataContext, чтобы привязка сработала
        DataContext = this;

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

        var createBtn2 = this.FindControl<Button>("CreateBtn2");
        if (createBtn2 != null)
        {
            createBtn2.Click += OnCreateBtnClicked;
        }

        var deleteBtn2 = this.FindControl<Button>("DeleteBtn2");
        if (deleteBtn2 != null)
        {
            deleteBtn2.Click += OnDeleteBtnClicked;

            var editBtn2= this.FindControl<Button>("EditBtn2");
            if (editBtn2 != null)
            {
                editBtn2.Click += OnEditBtnClicked;
            }
        }

        ToggleMenuBtn.Click += OnToggleMenuClicked;

        Console.WriteLine("MainWindow initialized successfully.");
    }

    private void OnCreateBtnClicked(object? sender, RoutedEventArgs e)
    {
        var newPerson = new Person("Имя", "Фамилия", false);
        Persons.Add(newPerson);

        Console.WriteLine($"Создана новая запись: {newPerson.FullName}");
    }

    private void OnDeleteBtnClicked(object? sender, RoutedEventArgs e)
    {
        var dataGrid = this.FindControl<DataGrid>("PersonsDataGrid");
        if (dataGrid != null && dataGrid.SelectedItem is Person selectedPerson)
        {
            Persons.Remove(selectedPerson);
            Console.WriteLine($"Удалена запись: {selectedPerson.FullName}");
        }
        else
        {
            Console.WriteLine("Нет выбранной записи для удаления.");
        }
    }

    private void OnEditBtnClicked(object? sender, RoutedEventArgs e)
    {
        var dataGrid = this.FindControl<DataGrid>("PersonsDataGrid");
        if (dataGrid != null && dataGrid.SelectedItem is Person selectedPerson)
        {
            // Пример: изменить имя на "Отредактировано"
            selectedPerson.FirstName = "Отредактировано";
            Console.WriteLine($"Отредактирована запись: {selectedPerson.FullName}");
        }
        else
        {
            Console.WriteLine("Нет выбранной записи для редактирования.");
        }
    }
    
    private void SetupPersons()
    {
        Persons.Add(new Person("Иван", "Иванов", false));
        Persons.Add(new Person("Петр", "Петров", true));
        Persons.Add(new Person("Мария", "Сидорова", false));
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

    // ✅ Метод для заполнения таблицы данными
    private void SetupDataMatrix()
    {
        var random = new Random();
        for (int i = 0; i < 10; i++)
        {
            DataMatrix.Add(new DataMatrixRow
            {
                Index = i,
                Col1 = Math.Round((random.NextDouble() * 100) - 50, 2),
                Col2 = Math.Round((random.NextDouble() * 100) - 50, 2),
                Col3 = Math.Round((random.NextDouble() * 100) - 50, 2),
                Col4 = Math.Round((random.NextDouble() * 100) - 50, 2),
                Col5 = Math.Round((random.NextDouble() * 100) - 50, 2)
            });
        }
    }

    // ✅ Метод для обновления графика на основе таблицы
    private void UpdateChartFromDataMatrix()
    {
        // Подготовим данные для графика
        var seriesData = new List<(string Label, List<(double X, double Y)> Points)>
        {
            ("Столбец 1", DataMatrix.Select(r => ((double)r.Index, r.Col1)).ToList()),
            ("Столбец 2", DataMatrix.Select(r => ((double)r.Index, r.Col2)).ToList()),
            ("Столбец 3", DataMatrix.Select(r => ((double)r.Index, r.Col3)).ToList()),
            ("Столбец 4", DataMatrix.Select(r => ((double)r.Index, r.Col4)).ToList()),
            ("Столбец 5", DataMatrix.Select(r => ((double)r.Index, r.Col5)).ToList()) 

        };

        LineChart.UpdateDataMultipleSeries(seriesData);
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