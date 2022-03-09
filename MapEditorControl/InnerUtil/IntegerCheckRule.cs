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
    public class InnerIntegerStringInfo : DependencyObject
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
            DependencyProperty.Register("PropertyName", typeof(string), typeof(InnerIntegerStringInfo), new PropertyMetadata());


        public bool AllowEmpty
        {
            get { return (bool)GetValue(AllowEmptyProperty); }
            set
            {
                if (value == (bool)GetValue(AllowEmptyProperty))
                {
                    return;
                }
                SetValue(AllowEmptyProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for AllowEmpty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllowEmptyProperty =
            DependencyProperty.Register("AllowEmpty", typeof(bool), typeof(InnerIntegerStringInfo), new PropertyMetadata(false));

    }


    public class IntegerCheckRule : ValidationRule
    {

        public InnerIntegerStringInfo IntegerStringInfo { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int id = 0;

            if (IntegerStringInfo.AllowEmpty)
            {

                if(value as string == "" || value == null)
                {
                    return ValidationResult.ValidResult;
                }

                if (value as string != null && !int.TryParse(value as string, out id))
                {
                    return new ValidationResult(false, String.Format("{0}必须是一个整数或者为空。", IntegerStringInfo.PropertyName));
                }
            }
            else
            {
                if (value as string != null && !int.TryParse(value as string, out id))
                {
                    return new ValidationResult(false, String.Format("{0}必须是一个整数。", IntegerStringInfo.PropertyName));
                }
            }

            return ValidationResult.ValidResult;
        }
    }
}
