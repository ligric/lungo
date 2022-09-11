using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace Lungo.Wpf.Services
{
    internal class ThemeResourcesService
    {
        private static string _currentTheme = String.Empty;
        private static readonly Collection<ThemeReferenceInfo> resourceReferences = new Collection<ThemeReferenceInfo>();
        public static ReadOnlyCollection<ThemeReferenceInfo> ResourceReferences => new(resourceReferences);

        public static event EventHandler<CollectionChangeEventArgs>? ResourceReferencesChanged;
        public static event EventHandler<string>? CurrentThemeChanged;

        public static string CurrentTheme
        {
            get => _currentTheme;
            set => ChangeCurrentTheme(ref _currentTheme, value);
        }

        internal static void ChangeCurrentTheme(ref string _currentTheme, string key)
        {
            if (_currentTheme == key)
                return;

            _currentTheme = key;
            CurrentThemeChanged?.Invoke(null, key);
        }


        public static void AddResourceReference(FrameworkElement element, ThemeColorsDictionary value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (element == null)
                throw new ArgumentNullException(nameof(element));

            if (CurrentTheme == null)
                CurrentTheme = value.First().Key;

            var themeReferenceInfo = new ThemeReferenceInfo(element, value);
            resourceReferences.Add(themeReferenceInfo);
            ResourceReferencesChanged?.Invoke(null, new CollectionChangeEventArgs(CollectionChangeAction.Add, themeReferenceInfo));
        }

        public static void RemoveResourceReference(string key)
        {
            throw new NotImplementedException();
        }
    }

    internal class ThemeReferenceInfo
    {
        public FrameworkElement Element { get; }
        public ThemeColorsDictionary ThemeColorsDictionary { get; }

        public ThemeReferenceInfo(FrameworkElement element, ThemeColorsDictionary themeColorsDictionary)
        {
            Element = element;
            ThemeColorsDictionary = themeColorsDictionary;
        }
    }
}
