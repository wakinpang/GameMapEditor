using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MapEditorControl.InnerUtil
{
    public class InnerFloatStringInfo : DependencyObject
    {

        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set
            {
                if (value == (string)GetValue(PropertyNameProperty))
                {
                    return;
                }
                SetValue(PropertyNameProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for PropertyName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register("PropertyName", typeof(string), typeof(InnerFloatStringInfo), new PropertyMetadata());
    }


    public class FloatCheckRule : ValidationRule
    {
        public InnerFloatStringInfo FloatStringInfo { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            float v = 0;

            if (value != null && (value as string) != "" && !float.TryParse(value as string, out v))
            {
                return new ValidationResult(false, String.Format("{0}必须是一个浮点数或者为空", FloatStringInfo.PropertyName));
            }

            return ValidationResult.ValidResult;
        }
    }
}
