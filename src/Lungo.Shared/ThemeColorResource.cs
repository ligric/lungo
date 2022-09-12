using Lungo.Wpf.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace Lungo.Wpf
{
    [MarkupExtensionReturnType(typeof(Brush))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class ThemeColorResource : StaticResourceExtension
    {
        private readonly SolarEclipseService solarEclipseService = new SolarEclipseService();

        public ThemeColorResource()
        {
        }

        public ThemeColorResource(object resourceKey)
            : base(resourceKey)
        {

        }


        public override object? ProvideValue(IServiceProvider serviceProvider)
        {
            ThemeColorsDictionary themeColorsDictionary = (ThemeColorsDictionary)base.ProvideValue(serviceProvider);
            var valueService = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));

            FrameworkElement target = (FrameworkElement)valueService.TargetObject;

            //DependencyProperty? property = valueService.TargetProperty as DependencyProperty;
            //SolidColorBrush? solidColorBrush = target.GetValue(property) as SolidColorBrush;
            Color newColor;
            if (ThemeResourcesService.CurrentTheme == null)
            {
                newColor = themeColorsDictionary.First().Value;
            }
            else
            {
                themeColorsDictionary.TryGetValue(ThemeResourcesService.CurrentTheme, out newColor);
            }

            ThemeResourcesService.AddResourceReference(target, themeColorsDictionary);

            VisualBrush brush = AFasfasfasa.GetEllipseVisualBrush(newColor, out Dictionary<string, DependencyObject> insideElements);
            solarEclipseService.AddElement(target, brush, insideElements);

            return brush;
        }
    }




}
