using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using System.Collections.ObjectModel;

namespace MapEditorControl.InnerUtil
{
    public class InnerNpcInfo : DependencyObject
    {
        public ObservableCollection<NpcSection> CurrentNpcsInfo
        {
            get { return (ObservableCollection<NpcSection>)GetValue(CurrentNpcsInfoProperty); }
            set
            {
                if (value == (ObservableCollection<NpcSection>)GetValue(CurrentNpcsInfoProperty))
                {
                    return;
                }
                SetValue(CurrentNpcsInfoProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentNpcsInfo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentNpcsInfoProperty =
            DependencyProperty.Register("CurrentNpcsInfo", typeof(ObservableCollection<NpcSection>), typeof(InnerNpcInfo), new PropertyMetadata());

        public NpcSection CurrentNpc {
            get { return (NpcSection)GetValue(CurrentNpcProperty); }
            set {
                if (value == (NpcSection)GetValue(CurrentNpcProperty)) {
                    return;
                }
                SetValue(CurrentNpcProperty, value);
            }
        }

        public static readonly DependencyProperty CurrentNpcProperty = DependencyProperty.Register(
            "CurrentNpc", typeof(NpcSection), typeof(InnerNpcInfo), new PropertyMetadata());
    }

    public class NpcIDCheckRule : ValidationRule {
        private InnerNpcInfo _innerNpcInfo;
        public InnerNpcInfo InnerNpcInfo {
            get { return _innerNpcInfo; }
            set {
                _innerNpcInfo = value;
            }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int id = 0;
            if (!int.TryParse(value as string, out id)) {
                return new ValidationResult(false, "NPC的ID必须是一个整数!");
            }

            if (id != InnerNpcInfo.CurrentNpc.NPCId && InnerNpcInfo.CurrentNpcsInfo != null) {
                var result = from npc in InnerNpcInfo.CurrentNpcsInfo
                             where npc.NPCId == id
                             select npc;

                if (result.Count() > 0) {
                    return new ValidationResult(false, "该NPC的ID已经存在!");
                }
            }

            return ValidationResult.ValidResult;
        }
    }
}
