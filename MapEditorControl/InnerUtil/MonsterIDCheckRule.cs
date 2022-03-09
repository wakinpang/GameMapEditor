using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using MapEditorControl.InnerUtil;
using System.Globalization;

namespace MapEditorControl.InnerUtil
{
    public class InnerMonsterInfo : DependencyObject {
        public ObservableCollection<MonsterSection> CurrentMonsterInfo {
            get { return (ObservableCollection<MonsterSection>)GetValue(CurrentMonsterInfoProperty); }
            set {
                if (value == (ObservableCollection<MonsterSection>)GetValue(CurrentMonsterInfoProperty)){
                    return;
                }
                SetValue(CurrentMonsterInfoProperty, value);
            }
        }

        public static readonly DependencyProperty CurrentMonsterInfoProperty = DependencyProperty.Register(
            "CurrentMonsterInfo", typeof(ObservableCollection<MonsterSection>), typeof(InnerMonsterInfo), new PropertyMetadata());

        public MonsterSection CurrentMosnterSection {
            get { return (MonsterSection)GetValue(CurrentMonsterSectionProperty); }
            set {
                if (value == (MonsterSection)GetValue(CurrentMonsterSectionProperty)) {
                    return;
                }
                SetValue(CurrentMonsterSectionProperty, value);
            }
        }

        public static readonly DependencyProperty CurrentMonsterSectionProperty = DependencyProperty.Register(
            "CurrentMosnterSection", typeof(MonsterSection), typeof(InnerMonsterInfo), new PropertyMetadata());
    }

    public class MonsterIDCheckRule : ValidationRule
    {
        private InnerMonsterInfo _innerMonsterInfo = null;
        public InnerMonsterInfo InnerMonsterInfo {
            get { return _innerMonsterInfo; }
            set {
                _innerMonsterInfo = value;
            }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int id = 0;
            if (!int.TryParse(value as string, out id)) {
                return new ValidationResult(false, "怪物ID必须是个整数。");
            }

            if (id != InnerMonsterInfo.CurrentMosnterSection.MonsterID) {
                var result = from monster in InnerMonsterInfo.CurrentMonsterInfo
                             where monster.MonsterID == id
                             select monster;

                if (result.Count() != 0) {
                    return new ValidationResult(false, "错误的场景ID，该ID已存在！");
                }
            }

            return ValidationResult.ValidResult;
        }
    }
}
