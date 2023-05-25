using System;
using System.Collections.Generic;
using System.Windows;

namespace ClientApplication.Utils;
public interface INavigationService
{
    void NavigateTo(string viewName);
}

public class NavigationService : INavigationService
{
    private readonly Dictionary<string, Type> _viewsDictionary;
    private static NavigationService? _instance;

    private NavigationService()
    {
        _viewsDictionary = new Dictionary<string, Type>();
    }
    public static NavigationService GetInstance()
    {
        if (_instance == null)
        {
            _instance = new NavigationService();
        }
        return _instance;
    }

    public void RegisterView(string viewName, Type viewType)
    {
        _viewsDictionary.Add(viewName, viewType);
    }

    public void NavigateTo(string viewName)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            var viewType = _viewsDictionary[viewName];
            var view = Activator.CreateInstance(viewType);
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow != null) mainWindow.Content = view;
        });
    }
}
