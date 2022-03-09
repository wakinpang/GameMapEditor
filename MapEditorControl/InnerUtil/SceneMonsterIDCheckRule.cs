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
    public class InnerSceneMonsterInfo : DependencyObject
    {

        public ObservableCollection<SceneMonsterPOJO> CurrentSceneMonstersInfo
        {
            get { return (ObservableCollection<SceneMonsterPOJO>)GetValue(CurrentSceneMonstersInfoProperty); }
            set
            {
                if (value == (ObservableCollection<SceneMonsterPOJO>)GetValue(CurrentSceneMonstersInfoProperty))
                {
                    return;
                }
                SetValue(CurrentSceneMonstersInfoProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentSceneMonstersInfo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentSceneMonstersInfoProperty =
            DependencyProperty.Register("CurrentSceneMonstersInfo", typeof(ObservableCollection<SceneMonsterPOJO>), typeof(InnerSceneMonsterInfo), new PropertyMetadata());


        public bool Modifying
        {
            get { return (bool)GetValue(ModifyingProperty); }
            set
            {
                if (value == (bool)GetValue(ModifyingProperty))
                {
                    return;
                }
                SetValue(ModifyingProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Modifying.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModifyingProperty =
            DependencyProperty.Register("Modifying", typeof(bool), typeof(InnerSceneMonsterInfo), new PropertyMetadata());


        public SceneMonsterPOJO CurrentSceneMonsterPOJO
        {
            get { return (SceneMonsterPOJO)GetValue(CurrentSceneMonsterPOJOProperty); }
            set
            {
                if (value == (SceneMonsterPOJO)GetValue(CurrentSceneMonsterPOJOProperty))
                {
                    return;
                }
                SetValue(CurrentSceneMonsterPOJOProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentSceneMonsterPOJO.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentSceneMonsterPOJOProperty =
            DependencyProperty.Register("CurrentSceneMonsterPOJO", typeof(SceneMonsterPOJO), typeof(InnerSceneMonsterInfo), new PropertyMetadata());


        public int CurrentMapID
        {
            get { return (int)GetValue(CurrentMapIDProperty); }
            set
            {
                if (value == (int)GetValue(CurrentMapIDProperty))
                {
                    return;
                }
                SetValue(CurrentMapIDProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentMapID.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentMapIDProperty =
            DependencyProperty.Register("CurrentMapID", typeof(int), typeof(InnerSceneMonsterInfo), new PropertyMetadata());

    }

    public class SceneMonsterIDCheckRule : ValidationRule
    {

        private InnerSceneMonsterInfo _innerSceneMonsterInfo = null;
        public InnerSceneMonsterInfo InnerSceneMonsterInfo
        {
            get
            {
                return _innerSceneMonsterInfo;
            }

            set
            {
                _innerSceneMonsterInfo = value;
            }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int id = 0;
            if (!int.TryParse(value as string, out id))
            {
                return new ValidationResult(false, "场景怪ID必须是一个整数。");
            }

            //if (InnerSceneMonsterInfo.Modifying)
            //{

            if (id != InnerSceneMonsterInfo.CurrentSceneMonsterPOJO.SceneMonsterID)
            {
                var result = from monster in InnerSceneMonsterInfo.CurrentSceneMonstersInfo
                             where monster.SceneMonsterID == id && monster.SceneID == InnerSceneMonsterInfo.CurrentMapID
                             select monster;

                var cont = result.Count();

                if (result.Count() != 0)
                {
                    return new ValidationResult(false, "错误的场景怪ID，该ID已经存在。");
                }
            }
            //}
            //else
            //{
            //    var result = from monster in InnerSceneMonsterInfo.CurrentSceneMonstersInfo
            //                 where monster.SceneMonsterID == id
            //                 select monster;

            //    if (result.Count() != 0)
            //    {
            //        return new ValidationResult(false, "错误的场景怪ID，该ID已经存在。");
            //    }
            //}

            return ValidationResult.ValidResult;

        }
    }
}
