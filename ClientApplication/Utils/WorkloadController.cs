using System;
using System.ComponentModel;
using System.Net.Mime;
using System.Windows;
using System.Windows.Threading;
using ClientApplication.Models;
using Shared;

namespace ClientApplication.Utils;

public class WorkloadController : INotifyPropertyChanged
{
    private static WorkloadController? _instance;
    private bool _isInit = false;
    private WorkloadIntensity _workloadIntensity = WorkloadIntensity.LOW;
    private DispatcherTimer _timer;

    public WorkloadIntensity WorkloadIntensity
    {
        get => _workloadIntensity;
        set
        {
            _workloadIntensity = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WorkloadIntensity)));
        }
    }


    private WorkloadController()
    {
    }

    public static WorkloadController GetInstance()
    {
        if (_instance == null)
        {
            _instance = new WorkloadController();
        }

        return _instance;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void Init(WorkloadViewChangeMode workloadViewChangeMode)
    {
        if (!_isInit)
        {
            if (workloadViewChangeMode == WorkloadViewChangeMode.HIGH_WORKLOAD_GAMES_FOCUS)
            {
                // Wir haben noch keine echte Daten für den Workload, deshalb wird hier für Testzwecke alle 5 Sekunden die 
                // Auslastung geändert

                Application.Current.Dispatcher.Invoke(() =>
                {
                    _timer = new DispatcherTimer
                    {
                        Interval = TimeSpan.FromSeconds(5)
                    };
                    _timer.Tick += Timer_Tick;
                    _timer.Start();
                });
            }

            _isInit = true;
        }
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (WorkloadIntensity == WorkloadIntensity.LOW)
        {
            WorkloadIntensity = WorkloadIntensity.MEDIUM;
        }
        else if (WorkloadIntensity == WorkloadIntensity.MEDIUM)
        {
            WorkloadIntensity = WorkloadIntensity.HIGH;
        }
        else
        {
            WorkloadIntensity = WorkloadIntensity.LOW;
        }
    }
}