using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Mapping;
using GalaSoft.MvvmLight;

namespace MapEditorControl.InnerUtil
{
    [Table(Name = "t_base_scene_monster")]
    public class SceneMonsterPOJO : ObservableObject, ISceneObjectBase
    {

        public SceneMonsterPOJO UserMemberwiseClone()
        {
            return this.MemberwiseClone() as SceneMonsterPOJO;
        }

        //private bool _temp = false;
        //public bool Temp
        //{
        //    get
        //    {
        //        return _temp;
        //    }

        //    set
        //    {
        //        _temp = value;
        //    }
        //}

        private bool _temp = false;

        public bool Temp
        {
            get { return _temp; }
            set
            {
                if (value == _temp)
                {
                    return;
                }
                _temp = value;
                RaisePropertyChanged(() => Temp);
            }
        }



        private POJOStatus _pojoStatus = POJOStatus.Normal;
        public POJOStatus POJOStatus { get
            {
                return _pojoStatus;
            }
            set
            {
                _pojoStatus = value;
            }
        }

        private int _sceneMonsterID;
        [Column(Name = "SCENE_MONSTER_ID"), PrimaryKey, Identity]
        public int SceneMonsterID
        {
            get { return _sceneMonsterID; }
            set
            {
                if (value == _sceneMonsterID)
                {
                    return;
                }
                _sceneMonsterID = value;
                RaisePropertyChanged(() => SceneMonsterID);

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        private int _monsterID;
        [Column(Name = "MONSTER_ID"), NotNull]
        public int MonsterID
        {
            get { return _monsterID; }
            set
            {
                if (value == _monsterID)
                {
                    return;
                }
                _monsterID = value;
                RaisePropertyChanged(() => MonsterID);

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        private int _sceneType;
        [Column(Name = "SCENE_TYPE"), NotNull]
        public int SceneType
        {
            get { return _sceneType; }
            set
            {
                if (value == _sceneType)
                {
                    return;
                }
                _sceneType = value;
                RaisePropertyChanged(() => SceneType);

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        private int _sceneID;
        [Column(Name = "SCENE_ID"), NotNull]
        public int SceneID
        {
            get { return _sceneID; }
            set
            {
                if (value == _sceneID)
                {
                    return;
                }
                _sceneID = value;
                RaisePropertyChanged(() => SceneID);

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        private int _worldLevel;
        [Column(Name = "WORLD_LEVEL"), NotNull]
        public int WorldLevel
        {
            get { return _worldLevel; }
            set
            {
                if (value == _worldLevel)
                {
                    return;
                }
                _worldLevel = value;
                RaisePropertyChanged(() => WorldLevel);

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
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

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
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

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        private short _floor;
        [Column(Name = "FLOOR"), NotNull]
        public short Floor
        {
            get { return _floor; }
            set
            {
                if (value == _floor)
                {
                    return;
                }
                _floor = value;
                RaisePropertyChanged(() => Floor);

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        private short _monsSeq;
        [Column(Name = "MONS_SEQ"), NotNull]
        public short MonsSeq
        {
            get { return _monsSeq; }
            set
            {
                if (value == _monsSeq)
                {
                    return;
                }
                _monsSeq = value;
                RaisePropertyChanged(() => MonsSeq);

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        private int _point;
        [Column(Name = "POINT"), NotNull]
        public int Point
        {
            get { return _point; }
            set
            {
                if (value == _point)
                {
                    return;
                }
                _point = value;
                RaisePropertyChanged(() => Point);

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
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

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        private int _style;
        [Column(Name = "STYLE"), NotNull]
        public int Style
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

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        private string _showExt;
        [Column(Name = "SHOW_EXT")]
        public string ShowExt
        {
            get { return _showExt; }
            set
            {
                if (value == _showExt)
                {
                    return;
                }
                _showExt = value;
                RaisePropertyChanged(() => ShowExt);

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        private int _reliveTime;
        [Column(Name = "RELIVE_TIME"), NotNull]
        public int ReliveTime
        {
            get { return _reliveTime; }
            set
            {
                if (value == _reliveTime)
                {
                    return;
                }
                _reliveTime = value;
                RaisePropertyChanged(() => ReliveTime);

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        private short _monsterType;
        [Column(Name = "MONSTER_TYPE"), NotNull]
        public short MonsterType
        {
            get { return _monsterType; }
            set
            {
                if (value == _monsterType)
                {
                    return;
                }
                _monsterType = value;
                RaisePropertyChanged(() => MonsterType);

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        private string _baseRewardInfo;
        [Column(Name = "BASE_REWARD_INFO")]
        public string BaseRewardInfo
        {
            get { return _baseRewardInfo; }
            set
            {
                if (value == _baseRewardInfo)
                {
                    return;
                }
                _baseRewardInfo = value;
                RaisePropertyChanged(() => BaseRewardInfo);

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }


        private string _taskRewardInfo;
        [Column(Name = "TASK_REWARD_INFO")]
        public string TaskRewardInfo
        {
            get { return _taskRewardInfo; }
            set
            {
                if (value == _taskRewardInfo)
                {
                    return;
                }
                _taskRewardInfo = value;
                RaisePropertyChanged(() => TaskRewardInfo);

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        private int _itemDropID;
        [Column(Name = "ITEM_DROP_ID"), NotNull]
        public int ItemDropID
        {
            get { return _itemDropID; }
            set
            {
                if (value == _itemDropID)
                {
                    return;
                }
                _itemDropID = value;
                RaisePropertyChanged(() => ItemDropID);

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        private string _posArr = "";
        [Column(Name = "POS_ARR"), NotNull]
        public string PosArr
        {
            get { return _posArr; }
            set
            {
                if (value == _posArr)
                {
                    return;
                }
                _posArr = value;
                RaisePropertyChanged(() => PosArr);

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        private string _pos;
        [Column(Name = "POS"), NotNull]
        public string Pos
        {
            get { return _pos; }
            set
            {
                if (value == _pos)
                {
                    return;
                }
                _pos = value;
                RaisePropertyChanged(() => Pos);

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        private int _num;
        [Column(Name = "NUM"), NotNull]
        public int Num
        {
            get { return _num; }
            set
            {
                if (value == _num)
                {
                    return;
                }
                _num = value;
                RaisePropertyChanged(() => Num);

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        private string _skillData;
        [Column(Name = "SKILL_DATA")]
        public string SkillData
        {
            get { return _skillData; }
            set
            {
                if (value == _skillData)
                {
                    return;
                }
                _skillData = value;
                RaisePropertyChanged(() => SkillData);

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }


    }
}
