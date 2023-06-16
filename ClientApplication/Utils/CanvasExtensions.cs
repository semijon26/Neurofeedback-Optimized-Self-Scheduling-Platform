using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ClientApplication.Utils;
/// <summary>
///  Dient zur Anpassung von Scrollview in der TaskOVerview.
///  Wird benötigt, sobald der Graph initial nach Eintreffen der Nachricht geladen wird.
///  Passt die Größe der ScrollView dynamisch an.
/// </summary>
public static class CanvasExtensions
{
    public static readonly DependencyProperty AutoSizeProperty =
        DependencyProperty.RegisterAttached("AutoSize", typeof(bool), typeof(CanvasExtensions),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure, OnAutoSizeChanged));

    public static bool GetAutoSize(UIElement element)
    {
        return (bool)element.GetValue(AutoSizeProperty);
    }

    public static void SetAutoSize(UIElement element, bool value)
    {
        element.SetValue(AutoSizeProperty, value);
    }

    private static void OnAutoSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Canvas canvas && (bool)e.NewValue)
        {
            canvas.Loaded += (s, args) =>
            {
                // Überprüfe, ob das Canvas keine Children hat
                if (canvas.Children.Count == 0)
                {
                    canvas.Height = 0;
                    return;
                }

                // Berechne die maximale Höhe basierend auf den Children
                double maxHeight = canvas.Children
                    .OfType<UIElement>()
                    .Max(child => Canvas.GetTop(child) + child.RenderSize.Height);

                // Setze die Höhe des Canvas entsprechend der maximalen Höhe
                canvas.Height = maxHeight;
            };

            // Überwache Änderungen an der Children-Collection
            canvas.LayoutUpdated += (s, args) =>
            {
                // Überprüfe, ob das Canvas keine Children hat
                if (canvas.Children.Count == 0)
                {
                    canvas.Height = 0;
                    return;
                }

                // Berechne die maximale Höhe basierend auf den Children
                double maxHeight = canvas.Children
                    .OfType<UIElement>()
                    .Max(child => Canvas.GetTop(child) + child.RenderSize.Height);

                // Aktualisiere die Höhe des Canvas nur, wenn sich die maximale Höhe geändert hat
                if (Math.Abs(canvas.Height - maxHeight) > double.Epsilon)
                {
                    canvas.Height = maxHeight;
                }
            };
        }

    }
}