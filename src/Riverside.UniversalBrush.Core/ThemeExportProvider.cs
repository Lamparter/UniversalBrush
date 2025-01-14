﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.UniversalBrush.Core
{
    public class ThemeExportProvider : IExportable
    {
        private bool _isWindowInitializing = false;
        private readonly object _lock = new object();
        private CoreApplicationView _exportWindow;
        private ExportViewModel _exportViewModel;

        public string GenerateExportData(IReadOnlyList<ColorMapping>? darkColorMapping = null, bool showAllColors = false)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!-- Free Public License 1.0.0 Permission to use, copy, modify, and/or distribute this code for any purpose with or without fee is hereby granted. -->");

            sb.AppendLine("<ResourceDictionary xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"");
            sb.AppendLine("                    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"");
            sb.AppendLine("                    xmlns:Windows10version1809=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation?IsApiContractPresent(Windows.Foundation.UniversalApiContract, 7)\"");
            sb.AppendLine("                    xmlns:BelowWindows10version1809=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation?IsApiContractNotPresent(Windows.Foundation.UniversalApiContract, 7)\">");

            sb.AppendLine("    <ResourceDictionary.ThemeDictionaries>");

            sb.AppendLine("        <ResourceDictionary x:Key=\"Default\">");
            sb.AppendLine("            <ResourceDictionary.MergedDictionaries>");
            sb.Append("                <Windows10version1809:ColorPaletteResources");

            if (darkColorMapping != null)
            {
                foreach (var m in darkColorMapping)
                {
                    sb.Append(" ");
                    sb.Append(m.Target.ToString());
                    sb.Append("=\"");
                    sb.Append(m.Source.ActiveColor.ToString());
                    sb.Append("\"");
                }
            }
            sb.AppendLine(" />");
            sb.AppendLine("                <ResourceDictionary>");

            if (darkColorMapping != null)
            {
                foreach (var m in darkColorMapping)
                {
                    sb.AppendLine($"                    <BelowWindows10version1809:Color x:Key=\"System{m.Target.ToString()}Color\">{m.Source.ActiveColor.ToString()}</BelowWindows10version1809:Color>");
                }
            }

            var model = new ThemeModel();
            var viewModel = new ViewModel();

            Windows.UI.Color ChromeAltMediumHigh = model.DarkRegion.ActiveColor;
            ChromeAltMediumHigh.A = 204;

            sb.AppendLine(string.Format("                    <Color x:Key=\"SystemChromeAltMediumHighColor\">{0}</Color>", ChromeAltMediumHigh.ToString()));
            sb.AppendLine(string.Format("                    <Color x:Key=\"SystemChromeAltHighColor\">{0}</Color>", model.DarkBase.Palette[5].ActiveColor.ToString()));
            sb.AppendLine(string.Format("                    <Color x:Key=\"SystemRevealListLowColor\">{0}</Color>", model.DarkBase.Palette[8].ActiveColor.ToString()));
            sb.AppendLine(string.Format("                    <Color x:Key=\"SystemRevealListMediumColor\">{0}</Color>", model.DarkBase.Palette[5].ActiveColor.ToString()));
            sb.AppendLine("                    <AcrylicBrush x:Key=\"SystemControlAcrylicWindowBrush\" BackgroundSource=\"HostBackdrop\" TintColor=\"{ThemeResource SystemChromeAltHighColor}\" TintOpacity=\"0.8\" FallbackColor=\"{ThemeResource SystemChromeMediumColor}\" />");

            sb.AppendLine("                    <!-- Override system shape defaults -->");
            sb.AppendLine(string.Format("                    <CornerRadius x:Key=\"ControlCornerRadius\">{0},{1},{2},{3}</CornerRadius>", viewModel.ControlCornerRadiusValue.TopLeft,
                viewModel.ControlCornerRadiusValue.TopRight, viewModel.ControlCornerRadiusValue.BottomLeft, viewModel.ControlCornerRadiusValue.BottomRight));
            sb.AppendLine(string.Format("                    <CornerRadius x:Key=\"OverlayCornerRadius\">{0},{1},{2},{3}</CornerRadius>", viewModel.OverlayCornerRadiusValue.TopLeft,
                viewModel.OverlayCornerRadiusValue.TopRight, viewModel.OverlayCornerRadiusValue.BottomLeft, viewModel.OverlayCornerRadiusValue.BottomRight));

            sb.AppendLine("                    <!-- Override system borders -->");
            sb.AppendLine(string.Format("                    <Thickness x:Key=\"MenuBarItemBorderThickness\">{0},{1},{2},{3}</Thickness>", viewModel.ControlBorderThicknessValue.Left,
                viewModel.ControlBorderThicknessValue.Top, viewModel.ControlBorderThicknessValue.Right, viewModel.ControlBorderThicknessValue.Bottom));
            sb.AppendLine(string.Format("                    <Thickness x:Key=\"GridViewItemMultiselectBorderThickness\">{0},{1},{2},{3}</Thickness>", viewModel.ControlBorderThicknessValue.Left,
                viewModel.ControlBorderThicknessValue.Top, viewModel.ControlBorderThicknessValue.Right, viewModel.ControlBorderThicknessValue.Bottom));
            sb.AppendLine(string.Format("                    <Thickness x:Key=\"CheckBoxBorderThemeThickness\">{0}</Thickness>", viewModel.ControlBorderThicknessValue.Left,
                viewModel.ControlBorderThicknessValue.Top, viewModel.ControlBorderThicknessValue.Right, viewModel.ControlBorderThicknessValue.Bottom));
            sb.AppendLine(string.Format("                    <x:Double x:Key=\"GridViewItemSelectedBorderThemeThickness\">{0}</x:Double>", Math.Max(Math.Max(Math.Max(viewModel.ControlBorderThicknessValue.Left,
                viewModel.ControlBorderThicknessValue.Top), viewModel.ControlBorderThicknessValue.Right), viewModel.ControlBorderThicknessValue.Bottom)));
            sb.AppendLine(string.Format("                    <x:Double x:Key=\"RadioButtonBorderThemeThickness\">{0}</x:Double>", Math.Max(Math.Max(Math.Max(viewModel.ControlBorderThicknessValue.Left,
                viewModel.ControlBorderThicknessValue.Top), viewModel.ControlBorderThicknessValue.Right), viewModel.ControlBorderThicknessValue.Bottom)));

            sb.AppendLine(string.Format("                    <Thickness x:Key=\"ButtonBorderThemeThickness\">{0},{1},{2},{3}</Thickness>", viewModel.ControlBorderThicknessValue.Left,
                viewModel.ControlBorderThicknessValue.Top, viewModel.ControlBorderThicknessValue.Right, viewModel.ControlBorderThicknessValue.Bottom));
            sb.AppendLine(string.Format("                    <Thickness x:Key=\"CalendarDatePickerBorderThemeThickness\">{0},{1},{2},{3}</Thickness>", viewModel.ControlBorderThicknessValue.Left,
                viewModel.ControlBorderThicknessValue.Top, viewModel.ControlBorderThicknessValue.Right, viewModel.ControlBorderThicknessValue.Bottom));
            sb.AppendLine(string.Format("                    <Thickness x:Key=\"TimePickerBorderThemeThickness\">{0},{1},{2},{3}</Thickness>", viewModel.ControlBorderThicknessValue.Left,
                viewModel.ControlBorderThicknessValue.Top, viewModel.ControlBorderThicknessValue.Right, viewModel.ControlBorderThicknessValue.Bottom));
            sb.AppendLine(string.Format("                    <Thickness x:Key=\"DatePickerBorderThemeThickness\">{0},{1},{2},{3}</Thickness>", viewModel.ControlBorderThicknessValue.Left,
                viewModel.ControlBorderThicknessValue.Top, viewModel.ControlBorderThicknessValue.Right, viewModel.ControlBorderThicknessValue.Bottom));
            sb.AppendLine(string.Format("                    <Thickness x:Key=\"ToggleSwitchOuterBorderStrokeThickness\">{0},{1},{2},{3}</Thickness>", viewModel.ControlBorderThicknessValue.Left,
                viewModel.ControlBorderThicknessValue.Top, viewModel.ControlBorderThicknessValue.Right, viewModel.ControlBorderThicknessValue.Bottom));

            sb.AppendLine(string.Format("                    <Thickness x:Key=\"RepeatButtonBorderThemeThickness\">{0},{1},{2},{3}</Thickness>", viewModel.ControlBorderThicknessValue.Left,
                viewModel.ControlBorderThicknessValue.Top, viewModel.ControlBorderThicknessValue.Right, viewModel.ControlBorderThicknessValue.Bottom));
            sb.AppendLine(string.Format("                    <Thickness x:Key=\"SearchBoxBorderThemeThickness\">{0},{1},{2},{3}</Thickness>", viewModel.ControlBorderThicknessValue.Left,
                viewModel.ControlBorderThicknessValue.Top, viewModel.ControlBorderThicknessValue.Right, viewModel.ControlBorderThicknessValue.Bottom));
            sb.AppendLine(string.Format("                    <Thickness x:Key=\"ToggleButtonBorderThemeThickness\">{0},{1},{2},{3}</Thickness>", viewModel.ControlBorderThicknessValue.Left,
                viewModel.ControlBorderThicknessValue.Top, viewModel.ControlBorderThicknessValue.Right, viewModel.ControlBorderThicknessValue.Bottom));
            sb.AppendLine(string.Format("                    <Thickness x:Key=\"TextControlBorderThemeThickness\">{0},{1},{2},{3}</Thickness>", viewModel.ControlBorderThicknessValue.Left,
                viewModel.ControlBorderThicknessValue.Top, viewModel.ControlBorderThicknessValue.Right, viewModel.ControlBorderThicknessValue.Bottom));

            sb.AppendLine(string.Format("                    <Thickness x:Key=\"ButtonRevealBorderThemeThickness\">{0},{1},{2},{3}</Thickness>", viewModel.ControlBorderThicknessValue.Left,
                viewModel.ControlBorderThicknessValue.Top, viewModel.ControlBorderThicknessValue.Right, viewModel.ControlBorderThicknessValue.Bottom));
            sb.AppendLine(string.Format("                    <Thickness x:Key=\"RepeatButtonRevealBorderThemeThickness\">{0},{1},{2},{3}</Thickness>", viewModel.ControlBorderThicknessValue.Left,
                viewModel.ControlBorderThicknessValue.Top, viewModel.ControlBorderThicknessValue.Right, viewModel.ControlBorderThicknessValue.Bottom));
            sb.AppendLine(string.Format("                    <Thickness x:Key=\"ToggleButtonRevealBorderThemeThickness\">{0},{1},{2},{3}</Thickness>", viewModel.ControlBorderThicknessValue.Left,
                viewModel.ControlBorderThicknessValue.Top, viewModel.ControlBorderThicknessValue.Right, viewModel.ControlBorderThicknessValue.Bottom));

            sb.AppendLine(string.Format("                    <Thickness x:Key=\"AppBarEllipsisButtonRevealBorderThemeThickness\">{0},{1},{2},{3}</Thickness>", viewModel.ControlBorderThicknessValue.Left,
                viewModel.ControlBorderThicknessValue.Top, viewModel.ControlBorderThicknessValue.Right, viewModel.ControlBorderThicknessValue.Bottom));
            sb.AppendLine(string.Format("                    <Thickness x:Key=\"AppBarButtonRevealBorderThemeThickness\">{0},{1},{2},{3}</Thickness>", viewModel.ControlBorderThicknessValue.Left,
                viewModel.ControlBorderThicknessValue.Top, viewModel.ControlBorderThicknessValue.Right, viewModel.ControlBorderThicknessValue.Bottom));
            sb.AppendLine(string.Format("                    <Thickness x:Key=\"AppBarToggleButtonRevealBorderThemeThickness\">{0},{1},{2},{3}</Thickness>", viewModel.ControlBorderThicknessValue.Left,
                viewModel.ControlBorderThicknessValue.Top, viewModel.ControlBorderThicknessValue.Right, viewModel.ControlBorderThicknessValue.Bottom));

            sb.AppendLine(string.Format("                    <Thickness x:Key=\"ListViewItemRevealBorderThemeThickness\">{0},{1},{2},{3}</Thickness>", viewModel.ControlBorderThicknessValue.Left,
                viewModel.ControlBorderThicknessValue.Top, viewModel.ControlBorderThicknessValue.Right, viewModel.ControlBorderThicknessValue.Bottom));
            sb.AppendLine(string.Format("                    <Thickness x:Key=\"GridViewItemRevealBorderThemeThickness\">{0},{1},{2},{3}</Thickness>", viewModel.ControlBorderThicknessValue.Left,
                viewModel.ControlBorderThicknessValue.Top, viewModel.ControlBorderThicknessValue.Right, viewModel.ControlBorderThicknessValue.Bottom));
            sb.AppendLine(string.Format("                    <Thickness x:Key=\"ComboBoxItemRevealBorderThemeThickness\">{0},{1},{2},{3}</Thickness>", viewModel.ControlBorderThicknessValue.Left,
                viewModel.ControlBorderThicknessValue.Top, viewModel.ControlBorderThicknessValue.Right, viewModel.ControlBorderThicknessValue.Bottom));

            sb.AppendLine(string.Format("                    <x:Double x:Key=\"PersonPictureEllipseBadgeStrokeThickness\">{0}</x:Double>", Math.Max(Math.Max(Math.Max(viewModel.ControlBorderThicknessValue.Left,
                viewModel.ControlBorderThicknessValue.Top), viewModel.ControlBorderThicknessValue.Right), viewModel.ControlBorderThicknessValue.Bottom)));

            sb.AppendLine("                    <!-- Override system generated accent colors -->");
            sb.AppendLine(string.Format("                    <Color x:Key=\"SystemAccentColorDark1\">{0}</Color>", model.DarkPrimary.Palette[4].ActiveColor.ToString()));
            sb.AppendLine(string.Format("                    <Color x:Key=\"SystemAccentColorDark2\">{0}</Color>", model.DarkPrimary.Palette[3].ActiveColor.ToString()));
            sb.AppendLine(string.Format("                    <Color x:Key=\"SystemAccentColorDark3\">{0}</Color>", model.DarkPrimary.Palette[2].ActiveColor.ToString()));

            sb.AppendLine(string.Format("                    <Color x:Key=\"SystemAccentColorLight1\">{0}</Color>", model.DarkPrimary.Palette[6].ActiveColor.ToString()));
            sb.AppendLine(string.Format("                    <Color x:Key=\"SystemAccentColorLight2\">{0}</Color>", model.DarkPrimary.Palette[7].ActiveColor.ToString()));
            sb.AppendLine(string.Format("                    <Color x:Key=\"SystemAccentColorLight3\">{0}</Color>", model.DarkPrimary.Palette[8].ActiveColor.ToString()));

            sb.AppendLine(string.Format("                    <Color x:Key=\"RegionColor\">{0}</Color>", model.DarkRegion.ActiveColor.ToString()));
            sb.AppendLine("                    <SolidColorBrush x:Key=\"RegionBrush\" Color=\"{StaticResource RegionColor}\" />");
            if (showAllColors)
            {
                sb.AppendLine(string.Format("                    <Color x:Key=\"BaseColor\">{0}</Color>", model.DarkBase.BaseColor.ActiveColor.ToString()));
                sb.AppendLine(string.Format("                    <Color x:Key=\"BasePalette000Color\">{0}</Color>", model.DarkBase.Palette[0].ActiveColor.ToString()));
                sb.AppendLine(string.Format("                    <Color x:Key=\"BasePalette100Color\">{0}</Color>", model.DarkBase.Palette[1].ActiveColor.ToString()));
                sb.AppendLine(string.Format("                    <Color x:Key=\"BasePalette200Color\">{0}</Color>", model.DarkBase.Palette[2].ActiveColor.ToString()));
                sb.AppendLine(string.Format("                    <Color x:Key=\"BasePalette300Color\">{0}</Color>", model.DarkBase.Palette[3].ActiveColor.ToString()));
                sb.AppendLine(string.Format("                    <Color x:Key=\"BasePalette400Color\">{0}</Color>", model.DarkBase.Palette[4].ActiveColor.ToString()));
                sb.AppendLine(string.Format("                    <Color x:Key=\"BasePalette500Color\">{0}</Color>", model.DarkBase.Palette[5].ActiveColor.ToString()));
                sb.AppendLine(string.Format("                    <Color x:Key=\"BasePalette600Color\">{0}</Color>", model.DarkBase.Palette[6].ActiveColor.ToString()));
                sb.AppendLine(string.Format("                    <Color x:Key=\"BasePalette700Color\">{0}</Color>", model.DarkBase.Palette[7].ActiveColor.ToString()));
                sb.AppendLine(string.Format("                    <Color x:Key=\"BasePalette800Color\">{0}</Color>", model.DarkBase.Palette[8].ActiveColor.ToString()));
                sb.AppendLine(string.Format("                    <Color x:Key=\"BasePalette900Color\">{0}</Color>", model.DarkBase.Palette[9].ActiveColor.ToString()));
                sb.AppendLine(string.Format("                    <Color x:Key=\"BasePalette1000Color\">{0}</Color>", model.DarkBase.Palette[10].ActiveColor.ToString()));
                sb.AppendLine(string.Format("                    <Color x:Key=\"PrimaryColor\">{0}</Color>", model.DarkPrimary.BaseColor.ActiveColor.ToString()));
                sb.AppendLine(string.Format("                    <Color x:Key=\"PrimaryPalette000Color\">{0}</Color>", model.DarkPrimary.Palette[0].ActiveColor.ToString()));
                sb.AppendLine(string.Format("                    <Color x:Key=\"PrimaryPalette100Color\">{0}</Color>", model.DarkPrimary.Palette[1].ActiveColor.ToString()));
                sb.AppendLine(string.Format("                    <Color x:Key=\"PrimaryPalette200Color\">{0}</Color>", model.DarkPrimary.Palette[2].ActiveColor.ToString()));
                sb.AppendLine(string.Format("                    <Color x:Key=\"PrimaryPalette300Color\">{0}</Color>", model.DarkPrimary.Palette[3].ActiveColor.ToString()));
                sb.AppendLine(string.Format("                    <Color x:Key=\"PrimaryPalette400Color\">{0}</Color>", model.DarkPrimary.Palette[4].ActiveColor.ToString()));
                sb.AppendLine(string.Format("                    <Color x:Key=\"PrimaryPalette500Color\">{0}</Color>", model.DarkPrimary.Palette[5].ActiveColor.ToString()));
                sb.AppendLine(string.Format("                    <Color x:Key=\"PrimaryPalette600Color\">{0}</Color>", model.DarkPrimary.Palette[6].ActiveColor.ToString()));
                sb.AppendLine(string.Format("                    <Color x:Key=\"PrimaryPalette700Color\">{0}\", model.DarkPrimary.Palette[7].ActiveColor.ToString()));
                sb.AppendLine(string.Format("                    <Color x:Key=\"PrimaryPalette800Color\">{0}</Color>", model.DarkPrimary.Palette[8].ActiveColor.ToString()));
                sb.AppendLine(string.Format("                    <Color x:Key=\"PrimaryPalette900Color\">{0}</Color>", model.DarkPrimary.Palette[9].ActiveColor.ToString()));
                sb.AppendLine(string.Format("                    <Color x:Key=\"PrimaryPalette1000Color\">{0}</Color>", model.DarkPrimary.Palette[10].ActiveColor.ToString()));
                sb.AppendLine("                    <SolidColorBrush x:Key=\"BaseBrush\" Color=\"{StaticResource BaseColor}\" />");
                sb.AppendLine("                    <SolidColorBrush x:Key=\"BasePalette000Brush\" Color=\"{StaticResource BasePalette000Color}\" />");
                sb.AppendLine("                    <SolidColorBrush x:Key=\"BasePalette100Brush\" Color=\"{StaticResource BasePalette100Color}\" />");
                sb.AppendLine("                    <SolidColorBrush x:Key=\"BasePalette200Brush\" Color=\"{StaticResource BasePalette200Color}\" />");
                sb.AppendLine("                    <SolidColorBrush x:Key=\"BasePalette300Brush\" Color=\"{StaticResource BasePalette300Color}\" />");
                sb.AppendLine("                    <SolidColorBrush x:Key=\"BasePalette400Brush\" Color=\"{StaticResource BasePalette400Color}\" />");
                sb.AppendLine("                    <SolidColorBrush x:Key=\"BasePalette500Brush\" Color=\"{StaticResource BasePalette500Color}\" />");
                sb.AppendLine("                    <SolidColorBrush x:Key=\"BasePalette600Brush\" Color=\"{StaticResource BasePalette600Color}\" />");
                sb.AppendLine("                    <SolidColorBrush x:Key=\"BasePalette700Brush\" Color=\"{StaticResource BasePalette700Color}\" />");
                sb.AppendLine("                    <SolidColorBrush x:Key=\"BasePalette800Brush\" Color=\"{StaticResource BasePalette800Color}\" />");
                sb.AppendLine("                    <SolidColorBrush x:Key=\"BasePalette900Brush\" Color=\"{StaticResource BasePalette900Color}\" />");
                sb.AppendLine("                    <SolidColorBrush x:Key=\"BasePalette1000Brush\" Color=\"{StaticResource BasePalette1000Color}\" />");
                sb.AppendLine("                    <SolidColorBrush x:Key=\"PrimaryPalette000Brush\" Color=\"{StaticResource PrimaryPalette000Color}\" />");
                sb.AppendLine("                    <SolidColorBrush x:Key=\"PrimaryPalette100Brush\" Color=\"{StaticResource PrimaryPalette100Color}\" />");
                sb.AppendLine("                    <SolidColorBrush x:Key=\"PrimaryPalette200Brush\" Color=\"{StaticResource PrimaryPalette200Color}\" />");
                sb.AppendLine("                    <SolidColorBrush x:Key=\"PrimaryPalette300Brush\" Color=\"{StaticResource PrimaryPalette300Color}\" />");
                sb.AppendLine("                    <SolidColorBrush x:Key=\"PrimaryPalette400Brush\" Color=\"{StaticResource PrimaryPalette400Color}\" />");
                sb.AppendLine("                    <SolidColorBrush x:Key=\"PrimaryPalette500Brush\" Color=\"{StaticResource PrimaryPalette500Color}\" />");
                sb.AppendLine("                    <SolidColorBrush x:Key=\"PrimaryPalette600Brush\" Color=\"{StaticResource PrimaryPalette600Color}\" />");
                sb.AppendLine("                    <SolidColorBrush x:Key=\"PrimaryPalette700Brush\" Color=\"{StaticResource PrimaryPalette700Color}\" />");
                sb.AppendLine("                    <SolidColorBrush x:Key=\"PrimaryPalette800Brush\" Color=\"{StaticResource PrimaryPalette800Color}\" />");
                sb.AppendLine("                    <SolidColorBrush x:Key=\"PrimaryPalette900Brush\" Color=\"{StaticResource PrimaryPalette900Color}\" />");
                sb.AppendLine("                    <SolidColorBrush x:Key=\"PrimaryPalette1000Brush\" Color=\"{StaticResource PrimaryPalette1000Color}\" />");
            }
            sb.AppendLine("                </ResourceDictionary>");
            sb.AppendLine("            </ResourceDictionary.MergedDictionaries>");
            sb.AppendLine("        </ResourceDictionary>");

            sb.AppendLine("        <ResourceDictionary x:Key=\"HighContrast\">");
            sb.AppendLine("            <StaticResource x:Key=\"RegionColor\" ResourceKey=\"SystemColorWindowColor\" />");
            sb.AppendLine("            <SolidColorBrush x:Key=\"RegionBrush\" Color=\"{StaticResource RegionColor}\" />");
            sb.AppendLine("        </ResourceDictionary>");

            sb.AppendLine("    </ResourceDictionary.ThemeDictionaries>");
            sb.AppendLine("</ResourceDictionary>");

            var retVal = sb.ToString();
            return retVal;
        }

        public async Task ShowExportView(string exportData)
        {
            CoreApplicationView exportWindow = null;
            bool init = false;
            lock (_lock)
            {
                if (_isWindowInitializing)
                {
                    return;
                }
                if (_exportWindow == null)
                {
                    init = true;
                    _isWindowInitializing = true;
                    _exportWindow = CoreApplication.CreateNewView();
                }
                exportWindow = _exportWindow;
            }

            if (init)
            {
                await _exportWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    _exportViewModel = new ExportViewModel(exportData);
                    ExportView exportView = new ExportView(_exportViewModel);

                    Window.Current.Content = exportView;
                    Window.Current.Activate();
                    var viewId = ApplicationView.GetForCurrentView().Id;
                    _ = ApplicationViewSwitcher.TryShowAsStandaloneAsync(viewId);
                });

                lock (_lock)
                {
                    _isWindowInitializing = false;
                }
            }
            else
            {
                await _exportWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    _exportViewModel.ExportText = exportData;
                    var w = Window.Current.Content;
                    var viewId = ApplicationView.GetForCurrentView().Id;
                    _ = ApplicationViewSwitcher.TryShowAsStandaloneAsync(viewId);
                });
            }
        }
    }
}
