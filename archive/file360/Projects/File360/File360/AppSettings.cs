using System;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Collections.Generic;

namespace File360
{
    public class AppSettings
    {
        // Our settings
        IsolatedStorageSettings settings;

        //Key Names
        const string SideBarColorKeyName = "SideBarColor";
        const string PageColorKeyName = "PageColor";

        // The default value of our settings
        const string SideBarColorDefault = "{StaticResource PhoneChromeBrush}";
        const string PageColorDefault = "{StaticResource PhoneChromeBrush}";

        public AppSettings()
        {
            settings = IsolatedStorageSettings.ApplicationSettings;
        }

        public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            // If the key exists
            if (settings.Contains(Key))
            {
                // If the value has changed
                if (settings[Key] != value)
                {
                    // Store the new value
                    settings[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                settings.Add(Key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        public T GetValueOrDefault<T>(string Key, T defaultValue)
        {
            T value;

            // If the key exists, retrieve the value.
            if (settings.Contains(Key))
            {
                value = (T)settings[Key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }
            return value;
        }

        /// <summary>
        /// Save the settings.
        /// </summary>
        public void Save()
        {
            settings.Save();
        }


        /// <summary>
        /// Property to get and set a ListBox Setting Key.
        /// </summary>
        public string SideBarColor
        {
            get
            {
                return GetValueOrDefault<string>(SideBarColorKeyName, SideBarColorDefault);
            }
            set
            {
                if (AddOrUpdateValue(SideBarColorKeyName, value))
                {
                    Save();
                }
            }
        }
        public string PageColor
        {
            get
            {
                return GetValueOrDefault<string>(PageColorKeyName, PageColorDefault);
            }
            set
            {
                if (AddOrUpdateValue(PageColorKeyName, value))
                {
                    Save();
                }
            }
        }
    }
}