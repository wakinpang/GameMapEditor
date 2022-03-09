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

    public class InnerStringInfo : DependencyObject
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
            DependencyProperty.Register("PropertyName", typeof(string), typeof(InnerStringInfo), new PropertyMetadata());

    }


    public class StringCheckRule : ValidationRule
    {
        public InnerStringInfo StringInfo { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if ((value as string) == "")
            {
                return new ValidationResult(false, String.Format("{0}必须不为空。", StringInfo.PropertyName));
            }

            return ValidationResult.ValidResult;
        }
    }
}
