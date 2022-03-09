using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Mapping;
using GalaSoft.MvvmLight;

namespace MapEditorControl.InnerUtil
{
    [Table(Name = "t_base_item")]
    public class ItemPOJO : ObservableObject
    {
        private int _itemID;
        [Column(Name = "ITEM_ID"), PrimaryKey, Identity]
        public int ItemID
        {
            get { return _itemID; }
            set
            {
                if (value == _itemID)
                {
                    return;
                }
                _itemID = value;
                RaisePropertyChanged(() => ItemID);
            }
        }

        private string _name;
        [Column(Name = "NAME")]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name)
                {
                    return;
                }
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        private int _level;
        [Column(Name = "Level"), NotNull]
        public int Level
        {
            get { return _level; }
            set
            {
                if (value == _level)
                {
                    return;
                }
                _level = value;
                RaisePropertyChanged(() => Level);
            }
        }

        private int _career;
        [Column(Name = "CAREER"), NotNull]
        public int Career
        {
            get { return _career; }
            set
            {
                if (value == _career)
                {
                    return;
                }
                _career = value;
                RaisePropertyChanged(() => Career);
            }
        }

        private int _sex;
        [Column(Name = "SEX"), NotNull]
        public int Sex
        {
            get { return _sex; }
            set
            {
                if (value == _sex)
                {
                    return;
                }
                _sex = value;
                RaisePropertyChanged(() => Sex);
            }
        }

        private int _camp;
        [Column(Name = "CAMP"), NotNull]
        public int Camp
        {
            get { return _camp; }
            set
            {
                if (value == _camp)
                {
                    return;
                }
                _camp = value;
                RaisePropertyChanged(() => Camp);
            }
        }

        private int _zzLevel;
        [Column(Name = "ZZ_LEVEL"), NotNull]
        public int ZZLevel
        {
            get { return _zzLevel; }
            set
            {
                if (value == _zzLevel)
                {
                    return;
                }
                _zzLevel = value;
                RaisePropertyChanged(() => ZZLevel);
            }
        }

        private int _wxLevel;
        [Column(Name = "WX_LEVEL"), NotNull]
        public int WXLevel
        {
            get { return _wxLevel; }
            set
            {
                if (value == _wxLevel)
                {
                    return;
                }
                _wxLevel = value;
                RaisePropertyChanged(() => WXLevel);
            }
        }

        private int _isBind;
        [Column(Name = "IS_BIND"), NotNull]
        public int IsBind
        {
            get { return _isBind; }
            set
            {
                if (value == _isBind)
                {
                    return;
                }
                _isBind = value;
                RaisePropertyChanged(() => IsBind);
            }
        }

        private int _maxNum;
        [Column(Name = "MAX_NUM"), NotNull]
        public int MaxNum
        {
            get { return _maxNum; }
            set
            {
                if (value == _maxNum)
                {
                    return;
                }
                _maxNum = value;
                RaisePropertyChanged(() => MaxNum);
            }
        }

        private int _minDiamondPrice;
        [Column(Name = "MIN_DIAMOND_PRICE"), NotNull]
        public int MinDiamondPrice
        {
            get { return _minDiamondPrice; }
            set
            {
                if (value == _minDiamondPrice)
                {
                    return;
                }
                _minDiamondPrice = value;
                RaisePropertyChanged(() => MinDiamondPrice);
            }
        }

        private int _maxDiamondPrice;
        [Column(Name = "MAX_DIAMOND_PRICE"), NotNull]
        public int MaxDiamondPrice
        {
            get { return _maxDiamondPrice; }
            set
            {
                if (value == _maxDiamondPrice)
                {
                    return;
                }
                _maxDiamondPrice = value;
                RaisePropertyChanged(() => MaxDiamondPrice);
            }
        }

        private int _sellPrice;
        [Column(Name = "SELL_PRICE"), NotNull]
        public int SellPrice
        {
            get { return _sellPrice; }
            set
            {
                if (value == _sellPrice)
                {
                    return;
                }
                _sellPrice = value;
                RaisePropertyChanged(() => SellPrice);
            }
        }

        private int _type;
        [Column(Name = "TYPE"), NotNull]
        public int Type
        {
            get { return _type; }
            set
            {
                if (value == _type)
                {
                    return;
                }
                _type = value;
                RaisePropertyChanged(() => Type);
            }
        }

        private int _subType;
        [Column(Name = "SUB_TYPE"), NotNull]
        public int SubType
        {
            get { return _subType; }
            set
            {
                if (value == _subType)
                {
                    return;
                }
                _subType = value;
                RaisePropertyChanged(() => SubType);
            }
        }

        private int _useType;
        [Column(Name = "USE_TYPE"), NotNull]
        public int UseType
        {
            get { return _useType; }
            set
            {
                if (value == _useType)
                {
                    return;
                }
                _useType = value;
                RaisePropertyChanged(() => UseType);
            }
        }

        private int _quality;
        [Column(Name = "QUALITY"), NotNull]
        public int Quality
        {
            get { return _quality; }
            set
            {
                if (value == _quality)
                {
                    return;
                }
                _quality = value;
                RaisePropertyChanged(() => Quality);
            }
        }

        private string _pro;
        [Column(Name = "PRO"), NotNull]
        public string Pro
        {
            get { return _pro; }
            set
            {
                if (value == _pro)
                {
                    return;
                }
                _pro = value;
                RaisePropertyChanged(() => Pro);
            }
        }

        private int _effectValue;
        [Column(Name = "EFFECT_VALUE")]
        public int EffectValue
        {
            get { return _effectValue; }
            set
            {
                if (value == _effectValue)
                {
                    return;
                }
                _effectValue = value;
                RaisePropertyChanged(() => EffectValue);
            }
        }

        private string _desc;
        [Column(Name = "DESC"), NotNull]
        public string Desc
        {
            get { return _desc; }
            set
            {
                if (value == _desc)
                {
                    return;
                }
                _desc = value;
                RaisePropertyChanged(() => Desc);
            }
        }

        private int _icon;
        [Column(Name = "ICON"), NotNull]
        public int Icon
        {
            get { return _icon; }
            set
            {
                if (value == _icon)
                {
                    return;
                }
                _icon = value;
                RaisePropertyChanged(() => Icon);
            }
        }

        private int _holeNum;
        [Column(Name = "HOLE_NUM"), NotNull]
        public int HoleNum
        {
            get { return _holeNum; }
            set
            {
                if (value == _holeNum)
                {
                    return;
                }
                _holeNum = value;
                RaisePropertyChanged(() => HoleNum);
            }
        }

        private int _avoidTime;
        [Column(Name = "AVOID_TIME"), NotNull]
        public int AvoidTime
        {
            get { return _avoidTime; }
            set
            {
                if (value == _avoidTime)
                {
                    return;
                }
                _avoidTime = value;
                RaisePropertyChanged(() => AvoidTime);
            }
        }

        private int _promptUse;
        [Column(Name = "PROMPT_USE"), NotNull]
        public int PromptUse
        {
            get { return _promptUse; }
            set
            {
                if (value == _promptUse)
                {
                    return;
                }
                _promptUse = value;
                RaisePropertyChanged(() => PromptUse);
            }
        }

        private string _style;
        [Column(Name = "STYLE"), NotNull]
        public string Style
        {
            get { return _style; }
            set
            {
                if (value == _style)
                {
                    return;
                }
                _style = value;
                RaisePropertyChanged(() => Style);
            }
        }

        private string _desc1;
        [Column(Name = "DESC1"), NotNull]
        public string Desc1
        {
            get { return _desc1; }
            set
            {
                if (value == _desc1)
                {
                    return;
                }
                _desc1 = value;
                RaisePropertyChanged(() => Desc1);
            }
        }

    }
}
