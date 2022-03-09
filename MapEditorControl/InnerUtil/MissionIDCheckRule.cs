using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MapEditorControl.InnerUtil
{
    public class InnerMissionInfo : DependencyObject
    {
        public ObservableCollection<MissionPOJO> CurrentMissionsInfo
        {
            get { return (ObservableCollection<MissionPOJO>)GetValue(CurrentMissionsInfoProperty); }
            set
            {
                if (value == (ObservableCollection<MissionPOJO>)GetValue(CurrentMissionsInfoProperty))
                {
                    return;
                }
                SetValue(CurrentMissionsInfoProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentItemsInfo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentMissionsInfoProperty =
            DependencyProperty.Register("CurrentMissionsInfo", typeof(ObservableCollection<MissionPOJO>), typeof(InnerMissionInfo), new PropertyMetadata());


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
            DependencyProperty.Register("AllowEmpty", typeof(bool), typeof(InnerMissionInfo), new PropertyMetadata(false));

    }

    public class MissionIDCheckRule : ValidationRule
    {
        public InnerMissionInfo _currentMissionsInfo = new InnerMissionInfo();
        public InnerMissionInfo CurrentMissionsInfo
        {
            get
            {
                return _currentMissionsInfo;
            }
            set
            {
                if (_currentMissionsInfo != value)
                {
                    _currentMissionsInfo = value;
                }
            }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int id = 0;

            if (CurrentMissionsInfo.AllowEmpty && (value as string == "" || value == null))
            {
                return ValidationResult.ValidResult;
            }

            if (!int.TryParse(value as string, out id))
            {
                if (CurrentMissionsInfo.AllowEmpty)
                {
                    return new ValidationResult(false, "任务ID必须是一个整数或者为空。");
                }
                else
                {
                    return new ValidationResult(false, "任务ID必须是一个整数。");
                }
            }

            var result = from mission in CurrentMissionsInfo.CurrentMissionsInfo
                         where mission.GameTaskID == id
                         select mission;

            if (result.Count() == 0)
            {
                return new ValidationResult(false, "错误的任务ID，不存在这个任务。");
            }

            return ValidationResult.ValidResult;
        }
    }
}
