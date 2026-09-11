using System;
using System.Collections.Generic;
using System.IO;

namespace Rampastring.Tools;

/// <summary>
/// Provides operations for parsing, handling and writing INI files.
/// </summary>
public interface IIniFile
{
    /// <summary>
    /// Gets or sets a value that determines whether the parser should only parse
    /// pre-determined (via <see cref="AddSection(string)"/>) sections or all sections in the INI file.
    /// </summary>
    bool AllowNewSections { get; set; }

    /// <summary>
    /// Use <see cref="IniFile.FilePath"/> instead.
    /// </summary>
    string FileName { get; set; }

    /// <summary>
    /// Adds a section into the INI file.
    /// </summary>
    /// <param name="section">The section to add.</param>
    void AddSection(IniSection section);

    /// <summary>
    /// Creates and adds a section into the INI file.
    /// </summary>
    /// <param name="sectionName">The name of the section to add.</param>
    void AddSection(string sectionName);

    /// <summary>
    /// Removes the given section from the INI file.
    /// Uses case-insensitive string comparison when looking for the section.
    /// </summary>
    /// <param name="sectionName">The name of the section to remove.</param>
    void RemoveSection(string sectionName);

    /// <summary>
    /// Combines two INI sections, with the second section overriding
    /// in case conflicting keys are present. The combined section
    /// then over-writes the second section.
    /// </summary>
    /// <param name="firstSectionName">The name of the first INI section.</param>
    /// <param name="secondSectionName">The name of the second INI section.</param>
    void CombineSections(string firstSectionName, string secondSectionName);

    /// <summary>
    /// Erases all existing keys of a section.
    /// Does nothing if the section does not exist.
    /// </summary>
    /// <param name="sectionName">The name of the section.</param>
    void EraseSectionKeys(string sectionName);

    /// <summary>
    /// Returns a boolean value from the INI file.
    /// </summary>
    /// <param name="section">The name of the key's section.</param>
    /// <param name="key">The name of the INI key.</param>
    /// <param name="defaultValue">The value to return if the section or key wasn't found,
    /// or converting the key's value to a boolean failed.</param>
    /// <returns>The given key's value if the section and key was found and
    /// the value is a valid boolean. Otherwise the given defaultValue.</returns>
    bool GetBooleanValue(string section, string key, bool defaultValue);

    /// <summary>
    /// Parses and returns a list value in the INI file.
    /// </summary>
    /// <typeparam name="T">The type of the list elements.</typeparam>
    /// <param name="section">The name of the INI section.</param>
    /// <param name="key">The INI key.</param>
    /// <param name="separator">The separator between the list elements.</param>
    /// <param name="converter">The function that converts the list elements from strings to the given type.</param>
    /// <returns>A list that contains the parsed elements.</returns>
    List<T> GetListValue<T>(string section, string key, char separator, Func<string, T> converter);

    /// <summary>
    /// Returns a double-precision floating point value from the INI file.
    /// </summary>
    /// <param name="section">The name of the key's section.</param>
    /// <param name="key">The name of the INI key.</param>
    /// <param name="defaultValue">The value to return if the section or key wasn't found,
    /// or converting the key's value to a double failed.</param>
    /// <returns>The given key's value if the section and key was found and
    /// the value is a valid double. Otherwise the given defaultValue.</returns>
    double GetDoubleValue(string section, string key, double defaultValue);

    /// <summary>
    /// Returns an integer value from the INI file.
    /// </summary>
    /// <param name="section">The name of the key's section.</param>
    /// <param name="key">The name of the INI key.</param>
    /// <param name="defaultValue">The value to return if the section or key wasn't found,
    /// or converting the key's value to an integer failed.</param>
    /// <returns>The given key's value if the section and key was found and
    /// the value is a valid integer. Otherwise the given defaultValue.</returns>
    int GetIntValue(string section, string key, int defaultValue);

    /// <summary>
    /// Parses and returns a path string from the INI file.
    /// The path string has all of its directory separators ( / \ )
    /// replaced with an environment-specific one.
    /// </summary>
    string GetPathStringValue(string section, string key, string defaultValue);

    /// <summary>
    /// Parses and returns an enum value from the INI file.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="section">The name of the key's INI section.</param>
    /// <param name="key">The INI key.</param>
    /// <param name="defaultValue">The value to return if the INI key doesn't exist
    /// or has no valid value that would belong to the enum type.</param>
    T GetEnumValue<T>(string section, string key, T defaultValue) where T : struct, Enum;

    /// <summary>
    /// Sets the enum value of a key in a INI section.
    /// If the section or key do not exist, they are created.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="section">The name of the key's INI section.</param>
    /// <param name="key">The INI key.</param>
    /// <param name="value">The value of the INI key.</param>
    void SetEnumValue<T>(string section, string key, T value) where T : struct, Enum;

    /// <summary>
    /// Returns an INI section from the file, or null if the section doesn't exist.
    /// </summary>
    /// <param name="name">The name of the section.</param>
    /// <returns>The section of the file; null if the section doesn't exist.</returns>
    IniSection GetSection(string name);

    /// <summary>
    /// Gets the names of all INI keys in the specified INI section.
    /// </summary>
    List<string> GetSectionKeys(string sectionName);

    /// <summary>
    /// Gets the names of all sections in the INI file.
    /// </summary>
    List<string> GetSections();

    /// <summary>
    /// Returns a single-precision floating point value from the INI file.
    /// </summary>
    /// <param name="section">The name of the key's section.</param>
    /// <param name="key">The name of the INI key.</param>
    /// <param name="defaultValue">The value to return if the section or key wasn't found,
    /// or converting the key's value to a float failed.</param>
    /// <returns>The given key's value if the section and key was found and
    /// the value is a valid float. Otherwise the given defaultValue.</returns>
    float GetSingleValue(string section, string key, float defaultValue);

    /// <summary>
    /// Returns a string value from the INI file.
    /// </summary>
    /// <param name="section">The name of the key's section.</param>
    /// <param name="key">The name of the INI key.</param>
    /// <param name="defaultValue">The value to return if the section or key wasn't found.</param>
    /// <returns>The given key's value if the section and key was found. Otherwise the given defaultValue.</returns>
    string GetStringValue(string section, string key, string defaultValue);
    string GetStringValue(string section, string key, string defaultValue, out bool success);

    /// <summary>
    /// Checks whether a specific INI key exists in a specific INI section.
    /// </summary>
    /// <param name="sectionName">The name of the INI section.</param>
    /// <param name="keyName">The name of the INI key.</param>
    /// <returns>True if the key exists, otherwise false.</returns>
    bool KeyExists(string sectionName, string keyName);

    /// <summary>
    /// Removes a key from the given section in the INI file.
    /// </summary>
    /// <param name="sectionName">The name of the section to remove the key from.</param>
    /// <param name="key">The key to remove from the section.</param>
    void RemoveKey(string sectionName, string key);
    void Parse(bool applyBaseIni);

    /// <summary>
    /// Clears all data from this IniFile instance and then re-parses the input INI file.
    /// </summary>
    void Reload();

    /// <summary>
    /// Checks whether a section exists. Returns true if the section
    /// exists, otherwise returns false.
    /// </summary>
    /// <param name="sectionName">The name of the INI section.</param>
    /// <returns></returns>
    bool SectionExists(string sectionName);

    /// <summary>
    /// Sets the boolean value of a key in the INI file.
    /// </summary>
    /// <param name="section">The name of the key's section.</param>
    /// <param name="key">The name of the INI key.</param>
    /// <param name="value">The value to set to the key.</param>
    void SetBooleanValue(string section, string key, bool value);

    /// <summary>
    /// Sets the list value of a key in the INI section.
    /// The list elements are converted to strings using the list element's
    /// ToString method and the given separator is applied between the elements.
    /// </summary>
    /// <typeparam name="T">The type of the list elements.</typeparam>
    /// <param name="section">The INI section.</param>
    /// <param name="key">The INI key.</param>
    /// <param name="list">The list.</param>
    /// <param name="separator">The separator between list elements.</param>
    void SetListValue<T>(string section, string key, List<T> list, char separator);

    /// <summary>
    /// Sets the double value of a specific key of a specific section in the INI file.
    /// </summary>
    /// <param name="section">The name of the key's section.</param>
    /// <param name="key">The name of the INI key.</param>
    /// <param name="value">The value to set to the key.</param>
    void SetDoubleValue(string section, string key, double value);

    /// <summary>
    /// Sets the integer value of a specific key of a specific section in the INI file.
    /// </summary>
    /// <param name="section">The name of the key's section.</param>
    /// <param name="key">The name of the INI key.</param>
    /// <param name="value">The value to set to the key.</param>
    void SetIntValue(string section, string key, int value);
    void SetSingleValue(string section, string key, double value, int decimals);
    void SetSingleValue(string section, string key, float value);

    /// <summary>
    /// Sets the float value of a key in the INI file.
    /// </summary>
    /// <param name="section">The name of the key's section.</param>
    /// <param name="key">The name of the INI key.</param>
    /// <param name="value">The value to set to the key.</param>
    /// <param name="decimals">Defines how many decimal places the stringified float should have.</param>
    void SetSingleValue(string section, string key, float value, int decimals);

    /// <summary>
    /// Sets the string value of a specific key of a specific section in the INI file.
    /// </summary>
    /// <param name="section">The name of the key's section.</param>
    /// <param name="key">The name of the INI key.</param>
    /// <param name="value">The value to set to the key.</param>
    void SetStringValue(string section, string key, string value);

    /// <summary>
    /// Writes the INI file to the path that was
    /// given to the instance on creation.
    /// </summary>
    void WriteIniFile();

    /// <summary>
    /// Writes the INI file's contents to the specified path.
    /// </summary>
    /// <param name="filePath">The path of the file to write to.</param>
    void WriteIniFile(string filePath);

    /// <summary>
    /// Writes the INI file to a specified stream.
    /// </summary>
    /// <param name="stream">The stream to write the INI file to.</param>
    void WriteIniStream(Stream stream);
}
