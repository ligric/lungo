using System;
using System.ComponentModel;

namespace Latte.Wpf
{

    /// <summary>
    /// Documentation: https://www.cyberforum.ru/wpf-silverlight/thread2650880.html#post14575691
    /// </summary>
    internal class TargetPropertyDescriptor
    {
        /// <summary>Instance name.</summary>
        public string Name { get; }

        /// <summary>Source of date.</summary>
        /// <remarks>obj</remarks>
        public object Source { get; }

        /// <summary>Tracked property name.</summary>
        public string PropertyName { get; }

        /// <summary>Descriptor for property.</summary>
        public PropertyDescriptor PropertyDescriptor { get; }

        private object _value;

        public object Value
        {
            get => _value;
            set
            {
                if ((_value == null && value == null) || (_value != null && _value.Equals(value))) 
                    return;

                if (value.GetType() == PropertyDescriptor.PropertyType)
                {
                    PropertyDescriptor.SetValue(Source, _value = value);
                }
                else
                {
                    try
                    {
                        PropertyDescriptor.SetValue(Source, _value = PropertyDescriptor.Converter.ConvertFrom(value));
                    }
                    catch
                    {
                        _value = value;
                    }
                }

                System.Diagnostics.Debug.WriteLine($"{Name}.Value={value}");
            }
        }

        public event EventHandler<EventArgs> PropertyChanged;

        /// <param name="name">Instance name.</param>
        public TargetPropertyDescriptor(string name, object source, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            Source = source ?? throw new ArgumentNullException(nameof(source));
            PropertyName = propertyName;

            PropertyDescriptor = TypeDescriptor.GetProperties(Source).Find(PropertyName, false)
                ?? throw new ArgumentException("The property not found.", nameof(PropertyName));

            PropertyDescriptor.AddValueChanged(Source, PropertyValueChanged);
        }

        private void PropertyValueChanged(object sender, EventArgs e)
        {
            Value = PropertyDescriptor.GetValue(Source);

            PropertyChanged?.Invoke(sender, e);
        }
    }
}
