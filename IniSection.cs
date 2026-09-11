// Rampastring's INI parser
// http://www.moddb.com/members/rampastring

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Rampastring.Tools;

/// <summary>
/// Represents a [section] in an INI file.
/// </summary>
public class IniSection : IIniSection
{
    public IniSection() { }

    public IniSection(string sectionName)
    {
        SectionName = sectionName;
    }

    public string SectionName { get; set; }
    public List<KeyValuePair<string, string>> Keys = new List<KeyValuePair<string, string>>();

    public void AddKey(string keyName, string value)
    {
        if (keyName == null || value == null)
            throw new ArgumentException("INI keys cannot have null key names or values.");

        if (Keys.FindIndex(kvp => kvp.Key == keyName) > -1)
            throw new InvalidOperationException("The given key already exists in the section!");

        Keys.Add(new KeyValuePair<string, string>(keyName, value));
    }

    public void AddOrReplaceKey(string keyName, string value)
    {
        if (keyName == null || value == null)
            throw new ArgumentException("INI keys cannot have null key names or values.");

        int index = Keys.FindIndex(k => k.Key == keyName);
        if (index > -1)
            Keys[index] = new KeyValuePair<string, string>(keyName, value);
        else
            Keys.Add(new KeyValuePair<string, string>(keyName, value));
    }

    public void RemoveKey(string keyName)
    {
        int index = Keys.FindIndex(k => k.Key == keyName);
        if (index > -1)
            Keys.RemoveAt(index);
    }

    public string GetStringValue(string key, string defaultValue)
    {
        var kvp = Keys.Find(k => k.Key == key);

        if (kvp.Value == null)
            return defaultValue;

        return kvp.Value;
    }

    public int GetIntValue(string key, int defaultValue)
    {
        return Conversions.IntFromString(GetStringValue(key, string.Empty), defaultValue);
    }

    public double GetDoubleValue(string key, double defaultValue)
    {
        return Conversions.DoubleFromString(GetStringValue(key, string.Empty), defaultValue);
    }

    public float GetSingleValue(string key, float defaultValue)
    {
        return Conversions.FloatFromString(GetStringValue(key, string.Empty), defaultValue);
    }

    public void SetStringValue(string key, string value)
    {
        AddOrReplaceKey(key, value);
    }

    public void SetIntValue(string key, int value)
    {
        AddOrReplaceKey(key, value.ToString(CultureInfo.InvariantCulture));
    }

    public void SetDoubleValue(string key, double value)
    {
        AddOrReplaceKey(key, value.ToString(CultureInfo.InvariantCulture));
    }

    public void SetFloatValue(string key, float value)
    {
        AddOrReplaceKey(key, value.ToString(CultureInfo.InvariantCulture));
    }

    public void SetBooleanValue(string key, bool value)
    {
        SetBooleanValue(key, value, BooleanStringStyle.TRUEFALSE);
    }

    public void SetBooleanValue(string key, bool value, BooleanStringStyle booleanStringStyle)
    {
        string strValue = Conversions.BooleanToString(value, booleanStringStyle);
        AddOrReplaceKey(key, strValue);
    }

    public bool GetBooleanValue(string key, bool defaultValue)
    {
        return Conversions.BooleanFromString(GetStringValue(key, String.Empty), defaultValue);
    }

    public void SetListValue<T>(string key, List<T> list, char separator)
    {
        AddOrReplaceKey(key, string.Join(separator.ToString(), list));
    }

    public List<T> GetListValue<T>(string key, char separator, Func<string, T> converter)
    {
        List<T> list = new List<T>();
        string value = GetStringValue(key, string.Empty);
        string[] parts = value.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string part in parts)
        {
            list.Add(converter(part));
        }
        return list;
    }

    public string GetPathStringValue(string key, string defaultValue)
    {
        return GetStringValue(key, defaultValue).Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
    }

    public T GetEnumValue<T>(string key, T defaultValue) where T : struct, Enum
    {
        string value = GetStringValue(key, null);
        if (string.IsNullOrWhiteSpace(value))
            return defaultValue;

        if (Enum.TryParse(value, true, out T result))
            return result;

        return defaultValue;
    }

    public void SetEnumValue<T>(string key, T value) where T : struct, Enum
    {
        SetStringValue(key, value.ToString());
    }

    /// <summary>
    /// Checks if the specified INI key exists in this section.
    /// </summary>
    /// <param name="key">The INI key.</param>
    /// <returns>True if the key exists in this section, otherwise false.</returns>
    public bool KeyExists(string key)
    {
        return Keys.FindIndex(k => k.Key == key) > -1;
    }

    /// <summary>
    /// Creates and returns a deep clone of this INI section.
    /// </summary>
    /// <param name="clonedSectionName">The name given to the cloned section.</param>
    public IniSection Clone(string clonedSectionName)
    {
        var clone = (IniSection)MemberwiseClone();
        clone.SectionName = clonedSectionName;

        // We don't need to do anything for the keys themselves because strings are immutable - just clone the list.
        clone.Keys = Keys.ToList();
        return clone;
    }
}
