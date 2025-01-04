using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.UniversalBrush.Models;

public interface IMainNavModel
{
    Task InitializeData(string dataPath, IControlPaletteModel paletteModel, ControlPaletteExportProvider controlPaletteExportProvider);
    Task HandleAppSuspend();

    IReadOnlyList<INavItem> NavItems { get; }
    INavItem DefaultNavItem { get; }
}