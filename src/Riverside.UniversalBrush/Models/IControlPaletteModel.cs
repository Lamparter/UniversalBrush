using Riverside.UniversalBrush.ControlPalette;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.UniversalBrush.Models
{
    public interface IControlPaletteModel
    {
        Task InitializeData(StringProvider stringProvider, string dataPath);
        Task HandleAppSuspend();

        void AddOrReplacePreset(Preset preset);
        void ApplyPreset(Preset preset);
        ObservableList<Preset> Presets { get; }
        Preset ActivePreset { get; }
        event Action<IControlPaletteModel> ActivePresetChanged;

        IReadOnlyList<ColorMapping> LightColorMapping { get; }
        IReadOnlyList<ColorMapping> DarkColorMapping { get; }
        ColorPaletteEntry LightRegion { get; }
        ColorPaletteEntry DarkRegion { get; }
        ColorPalette LightBase { get; }
        ColorPalette DarkBase { get; }
        ColorPalette LightPrimary { get; }
        ColorPalette DarkPrimary { get; }
    }
}
