using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MapEditorControl.InnerUtil
{
    public class InnerItemsInfo : DependencyObject
    {
        public ObservableCollection<ItemPOJO> CurrentItemsInfo
        {
            get { return (ObservableCollection<ItemPOJO>)GetValue(CurrentItemsInfoProperty); }
            set
            {
                if (value == (ObservableCollection<ItemPOJO>)GetValue(CurrentItemsInfoProperty))
                {
                    return;
                }
                SetValue(CurrentItemsInfoProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentItemsInfo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentItemsInfoProperty =
            DependencyProperty.Register("CurrentItemsInfo", typeof(ObservableCollection<ItemPOJO>), typeof(InnerItemsInfo), new PropertyMetadata());


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
            DependencyProperty.Register("AllowEmpty", typeof(bool), typeof(InnerItemsInfo), new PropertyMetadata(false));

    }

    public class ItemIDCheckRule : ValidationRule
    {

        public InnerItemsInfo _currentItemsInfo = new InnerItemsInfo();
        public InnerItemsInfo CurrentItemsInfo
        {
            get
            {
                return _currentItemsInfo;
            }
            set
            {
                if (_currentItemsInfo != value)
                {
                    _currentItemsInfo = value;
                }
            }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int id = 0;

            if (CurrentItemsInfo.AllowEmpty && (value as string == "" || value == null))
            {
                return ValidationResult.ValidResult;
            }

            if(!int.TryParse(value as string, out id))
            {
                if (CurrentItemsInfo.AllowEmpty)
                {
                    return new ValidationResult(false, "物品ID必须是一个整数或者为空。");
                }
                else
                {
                    return new ValidationResult(false, "物品ID必须是一个整数。");
                }
            }

            var result = from item in CurrentItemsInfo.CurrentItemsInfo
                         where item.ItemID == id
                         select item;

            if(result.Count() == 0)
            {
                return new ValidationResult(false, "错误的物品ID，不存在这个物品。");
            }

            return ValidationResult.ValidResult;
        }
    }
}
