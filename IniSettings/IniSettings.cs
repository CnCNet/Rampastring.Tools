using System;
using System.Collections.Generic;

namespace Rampastring.Tools.IniSettings;

/// <summary>
/// Class for managing INI settings.
/// </summary>
public class IniSettings
{
    /// <summary>
    /// Parameterless constructor.
    /// If you use this, make sure to set <see cref="SettingsIni"/> afterwards in derived class constructors.
    /// </summary>
    protected IniSettings() { }

    public IniSettings(IniFile iniFile)
    {
        SettingsIni = iniFile;
    }

    public IniSettings(string settingsFilePath)
    {
        SettingsIni = new IniFile(settingsFilePath);
    }

    /// <summary>
    /// The INI file associated with this settings instance.
    /// </summary>
    public IniFile SettingsIni { get; protected set; }

    private readonly List<IIniLoadable> settingList = new List<IIniLoadable>();

    /// <summary>
    /// Registers a setting into this settings instance.
    /// </summary>
    public void AddSetting<T>(IIniLoadable setting)
    {
        if (settingList.Contains(setting))
            throw new InvalidOperationException("The given setting already exists in the setting list!");

        settingList.Add(setting);
    }

    /// <summary>
    /// Clears all settings from this settings instance.
    /// </summary>
    public void Clear()
    {
        settingList.Clear();
    }

    /// <summary>
    /// Loads the potential user-defined values of all settings registered into this instance from the INI file.
    /// </summary>
    public virtual void LoadAll()
    {
        foreach (var setting in settingList)
            setting.LoadValue(SettingsIni);
    }

    /// <summary>
    /// Overrides the default values of this instance's settings with values loaded from the provided INI file.
    /// If a specific setting's value is not found from the INI file, the setting keeps its existing default value.
    /// </summary>
    public virtual void LoadDefaultValueOverrides(IniFile defaultSettingsIni)
    {
        foreach (var setting in settingList)
            setting.LoadDefaultValueOverride(defaultSettingsIni);
    }

    /// <summary>
    /// Writes the potential user-defined values of all settings registered into this instance into the INI file.
    /// Does not write the INI settings file itself; for that, call <see cref="SaveSettingsIni"/> afterwards.
    /// </summary>
    public virtual void WriteAll()
    {
        foreach (var setting in settingList)
            setting.WriteValue(SettingsIni, false);
    }

    /// <summary>
    /// Writes the INI file to the file system. Requires that <see cref="SettingsIni"/> has a valid file path defined.
    /// </summary>
    public void SaveSettingsIni()
    {
        SettingsIni.WriteIniFile();
    }

    /// <summary>
    /// Automatically fetches all members of this class instance that implement
    /// <see cref="IIniLoadable"/> and adds them to this instance's list of settings.
    /// </summary>
    /// <remarks>Setting instances that are null at the time of calling this are ignored.
    ///
    /// The setting must have a public getter.</remarks>
    protected void PopulateWithReflection()
    {
        var type = GetType();
        var propertyInfos = type.GetProperties();

        foreach (var property in propertyInfos)
        {
            var propertyType = property.PropertyType;

            if (!typeof(IIniLoadable).IsAssignableFrom(propertyType))
                continue;

            var settingInstance = property.GetValue(this);
            if (settingInstance != null)
                settingList.Add((IIniLoadable)settingInstance);
        }
    }
}
