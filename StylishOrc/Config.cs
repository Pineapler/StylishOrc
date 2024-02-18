using BepInEx.Configuration;

namespace StylishOrc;

public class Config(ConfigFile file) {
    public ConfigEntry<bool> ModEnabled { get; } = file.Bind("General", "Enabled", true, "Should the mod be enabled?");
    public ConfigEntry<bool> UnlockAll { get; } = file.Bind("General", "UnlockAll", false, "Should all clothing options be available from the start? If false, clothing options will become available as you gain reputation with each character.");
}
