using System.Collections.ObjectModel;

namespace ava;

public class MenuItemModel
{
    public string Header { get; set; } = "";
    public ObservableCollection<MenuItemModel> Children { get; set; } = new();

    public MenuItemModel(string header, params MenuItemModel[] children)
    {
        Header = header;
        foreach (var child in children)
        {
            Children.Add(child);
        }
    }
}
