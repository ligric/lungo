#if WPF
using Lungo.Wpf.Data;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Lungo.Wpf.Services
{
    internal class ThemeResourcesService
    {
        private static string? _currentTheme;
        private static readonly Collection<ThemeColorsInfo> resourceReferences = new Collection<ThemeColorsInfo>();
        public static ReadOnlyCollection<ThemeColorsInfo> ResourceReferences => new(resourceReferences);

        public static event EventHandler<CollectionChangeEventArgs>? ResourceReferencesChanged;
        public static event EventHandler<string>? CurrentThemeChanged;

        public static string? CurrentTheme
        {
            get => _currentTheme;
            set => ChangeCurrentTheme(ref _currentTheme, value);
        }

        internal static void ChangeCurrentTheme(ref string? _currentTheme, string? key)
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

            var themeReferenceInfo = new ThemeColorsInfo(element, value);
            resourceReferences.Add(themeReferenceInfo);
            ResourceReferencesChanged?.Invoke(null, new CollectionChangeEventArgs(CollectionChangeAction.Add, themeReferenceInfo));
        }

        public static void RemoveResourceReference(string key)
        {
            throw new NotImplementedException();
        }
    }
}
#endif