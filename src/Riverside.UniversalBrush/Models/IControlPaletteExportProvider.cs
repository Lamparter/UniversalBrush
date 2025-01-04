using Riverside.UniversalBrush.ControlPalette;
using Riverside.UniversalBrush.Models;
using System.Threading.Tasks;

namespace Riverside.UniversalBrush.Models
{
    public interface IControlPaletteExportProvider
    {
        Task ShowExportView(string exportData);
        string GenerateExportData(IControlPaletteModel model, ControlPaletteViewModel viewModel, bool showAllColors = false);
    }
}