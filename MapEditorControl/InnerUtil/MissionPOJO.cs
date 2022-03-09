using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Mapping;
using GalaSoft.MvvmLight;

namespace MapEditorControl.InnerUtil
{
    [Table(Name = "t_base_game_task")]
    public class MissionPOJO : ObservableObject
    {
        private int _gameTaskID;
        [Column(Name = "Game_TASK_ID"), PrimaryKey, Identity]
        public int GameTaskID
        {
            get { return _gameTaskID; }
            set
            {
                if (value == _gameTaskID)
                {
                    return;
                }
                _gameTaskID = value;
                RaisePropertyChanged(() => GameTaskID);
            }
        }

        private string _beforeTaskID;
        [Column(Name = "BEFORE_TASK_ID")]
        public string BeforeTaskID
        {
            get { return _beforeTaskID; }
            set
            {
                if (value == _beforeTaskID)
                {
                    return;
                }
                _beforeTaskID = value;
                RaisePropertyChanged(() => BeforeTaskID);
            }
        }

        private short _type;
        [Column(Name = "TYPE"), NotNull]
        public short Type
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

        private short _step;
        [Column(Name = "STEP"), NotNull]
        public short Step
        {
            get { return _step; }
            set
            {
                if (value == _step)
                {
                    return;
                }
                _step = value;
                RaisePropertyChanged(() => Step);
            }
        }

        private short _stepIndex;
        [Column(Name = "STEP_INDEX"), NotNull]
        public short StepIndex
        {
            get { return _stepIndex; }
            set
            {
                if (value == _stepIndex)
                {
                    return;
                }
                _stepIndex = value;
                RaisePropertyChanged(() => StepIndex);
            }
        }

        private int _funType;
        [Column(Name = "FUN_TYPE"), NotNull]
        public int FunType
        {
            get { return _funType; }
            set
            {
                if (value == _funType)
                {
                    return;
                }
                _funType = value;
                RaisePropertyChanged(() => FunType);
            }
        }

        private short _camp;
        [Column(Name = "CAMP"), NotNull]
        public short Camp
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

        private int _level;
        [Column(Name = "LEVEL"), NotNull]
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

        private string _name;
        [Column(Name = "NAME"), NotNull]
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

        private string _introduction;
        [Column(Name = "INTRODUCTION")]
        public string Introduction
        {
            get { return _introduction; }
            set
            {
                if (value == _introduction)
                {
                    return;
                }
                _introduction = value;
                RaisePropertyChanged(() => Introduction);
            }
        }

        private string _targetStr;
        [Column(Name = "TARGET_STR")]
        public string TargetStr
        {
            get { return _targetStr; }
            set
            {
                if (value == _targetStr)
                {
                    return;
                }
                _targetStr = value;
                RaisePropertyChanged(() => TargetStr);
            }
        }

        private string _targetInfo;
        [Column(Name = "TARGET_INFO")]
        public string TargetInfo
        {
            get { return _targetInfo; }
            set
            {
                if (value == _targetInfo)
                {
                    return;
                }
                _targetInfo = value;
                RaisePropertyChanged(() => TargetInfo);
            }
        }

        private int _startNPCID;
        [Column(Name = "START_NPC_ID")]
        public int StartNPCID
        {
            get { return _startNPCID; }
            set
            {
                if (value == _startNPCID)
                {
                    return;
                }
                _startNPCID = value;
                RaisePropertyChanged(() => StartNPCID);
            }
        }

        private int _finishNPCID;
        [Column(Name = "FINISH_NPC_ID")]
        public int FinishNPCID
        {
            get { return _finishNPCID; }
            set
            {
                if (value == _finishNPCID)
                {
                    return;
                }
                _finishNPCID = value;
                RaisePropertyChanged(() => FinishNPCID);
            }
        }

        private int _targetNPCID;
        [Column(Name = "TARGET_NPC_ID")]
        public int TargetNPCID
        {
            get { return _targetNPCID; }
            set
            {
                if (value == _targetNPCID)
                {
                    return;
                }
                _targetNPCID = value;
                RaisePropertyChanged(() => TargetNPCID);
            }
        }

        private int _mapID;
        [Column(Name = "MAP_ID")]
        public int MapID
        {
            get { return _mapID; }
            set
            {
                if (value == _mapID)
                {
                    return;
                }
                _mapID = value;
                RaisePropertyChanged(() => MapID);
            }
        }

        private short _autoFlag;
        [Column(Name = "AUTO_FLAG")]
        public short AutoFlag
        {
            get { return _autoFlag; }
            set
            {
                if (value == _autoFlag)
                {
                    return;
                }
                _autoFlag = value;
                RaisePropertyChanged(() => AutoFlag);
            }
        }

        private short _doubleFlag;
        [Column(Name = "DOUBLE_FLAG"), NotNull]
        public short DoubleFlag
        {
            get { return _doubleFlag; }
            set
            {
                if (value == _doubleFlag)
                {
                    return;
                }
                _doubleFlag = value;
                RaisePropertyChanged(() => DoubleFlag);
            }
        }

        private string _rewardInfo;
        [Column(Name = "REWARD_INFO")]
        public string RewardInfo
        {
            get { return _rewardInfo; }
            set
            {
                if (value == _rewardInfo)
                {
                    return;
                }
                _rewardInfo = value;
                RaisePropertyChanged(() => RewardInfo);
            }
        }

        private string _description;
        [Column(Name = "DESCRIPTION")]
        public string Description
        {
            get { return _description; }
            set
            {
                if (value == _description)
                {
                    return;
                }
                _description = value;
                RaisePropertyChanged(() => Description);
            }
        }

        private string _story;
        [Column(Name = "STORY")]
        public string Story
        {
            get { return _story; }
            set
            {
                if (value == _story)
                {
                    return;
                }
                _story = value;
                RaisePropertyChanged(() => Story);
            }
        }

        private string _promptID;
        [Column(Name = "PROMPT_ID"), NotNull]
        public string PromptID
        {
            get { return _promptID; }
            set
            {
                if (value == _promptID)
                {
                    return;
                }
                _promptID = value;
                RaisePropertyChanged(() => PromptID);
            }
        }


    }
}
