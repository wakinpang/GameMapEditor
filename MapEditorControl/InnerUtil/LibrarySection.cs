using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Mapping;

namespace MapEditorControl.InnerUtil
{
    // Monster and NPC's base class
    public interface IBaseObjectBase
    {

    }

    [Table(Name = "t_base_map")]
    public class MapSection : INotifyPropertyChanged
    {

        private POJOStatus _pojoStatus = POJOStatus.Normal;
        public POJOStatus POJOStatus {
            get
            {
                return _pojoStatus;
            }
            set
            {
                _pojoStatus = value;
            }
        }

        bool _isChecked = false;
        int _mapID = 0;
        short _type = 0;
        String _name = "";
        int _width = 0;
        int _height = 0;
        int _posX = 0;
        int _posY = 0;
        short _lineNum = 1;
        short _camp = 0;
        int _level = 1;

        short _pkType = 0;
        String _pkArea = "";
        String _dropInfo = "";
        int _soundId = 0;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (value == _isChecked)
                {
                    return;
                }
                this._isChecked = value;
                NotifyPropertyChanged("IsChecked");
            }
        }

        [Column(Name = "MAP_ID"), PrimaryKey, Identity]
        public int MapID
        {
            get { return _mapID; }
            set
            {
                if(this._mapID == value)
                {
                    return;
                }

                this._mapID = value;
                NotifyPropertyChanged("MapID");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "TYPE"), NotNull]
        public short Type
        {
            get { return _type; }
            set
            {
                if(this._type == value)
                {
                    return;
                }

                this._type = value;
                NotifyPropertyChanged("Type");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "NAME"), NotNull]
        public String Name
        {
            get { return _name; }
            set
            {

                if(this._name == value)
                {
                    return;
                }

                this._name = value;
                NotifyPropertyChanged("Name");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "WIDTH"), NotNull]
        public int Width
        {
            get { return _width; }
            set
            {
                if(this._width == value)
                {
                    return;
                }

                this._width = value;
                NotifyPropertyChanged("Width");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "HEIGH"), NotNull]
        public int Height
        {
            get { return _height; }
            set
            {
                if(this._height == value)
                {
                    return;
                }

                this._height = value;
                NotifyPropertyChanged("Height");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "POS_X"), NotNull]
        public int PosX
        {
            get { return _posX; }
            set
            {
                if (this._posX == value)
                {
                    return;
                }

                this._posX = value;
                NotifyPropertyChanged("PosX");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "POS_Y"), NotNull]
        public int PosY
        {
            get { return _posY; }
            set
            {
                if(this._posY == value)
                {
                    return;
                }

                this._posY = value;
                NotifyPropertyChanged("PosY");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "LINE_NUM"), NotNull]
        public short LineNum
        {
            get { return _lineNum; }
            set
            {
                if(this._lineNum == value)
                {
                    return;
                }

                this._lineNum = value;
                NotifyPropertyChanged("LineNum");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "CAMP"), NotNull]
        public short Camp
        {
            get { return _camp; }
            set
            {
                if(this._camp == value)
                {
                    return;
                }

                this._camp = value;
                NotifyPropertyChanged("Camp");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "LEVEL"), NotNull]
        public int Level
        {
            get { return _level; }
            set
            {
                if(this._level == value)
                {
                    return;
                }

                this._level = value;
                NotifyPropertyChanged("Level");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "PK_TYPE"), NotNull]
        public short PkType
        {
            get { return _pkType; }
            set
            {
                if(this._pkType == value)
                {
                    return;
                }

                this._pkType = value;
                NotifyPropertyChanged("PkType");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "PK_AREA")]
        public String PkArea
        {
            get { return _pkArea; }
            set
            {
                if(this._pkArea == value)
                {
                    return;
                }

                this._pkArea = value;
                NotifyPropertyChanged("PkArea");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "DROP_INFO")]
        public String DropInfo
        {
            get { return _dropInfo; }
            set
            {
                if(this._dropInfo == value)
                {
                    return;
                }

                this._dropInfo = value;
                NotifyPropertyChanged("DropInfo");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "SOUND_ID"), NotNull]
        public int SoundId
        {
            get { return _soundId; }
            set
            {
                if(this._soundId == value)
                {
                    return;
                }

                this._soundId = value;
                NotifyPropertyChanged("SoundId");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }

    public class MusicSection : INotifyPropertyChanged {
        String _name = "";
        bool _isChecked = false;

        public String Name
        {
            get { return _name; }
            set
            {
                if (value != "")
                {
                    this._name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        public bool IsChecked {
            get { return _isChecked; }
            set {
                if (value == _isChecked) {
                    return;
                }
                this._isChecked = value;
                NotifyPropertyChanged("IsChecked");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }

    [Table(Name = "t_base_npc")]
    public class NpcSection : IBaseObjectBase, INotifyPropertyChanged
    {
        int _id = 0;
        NpcType _type;
        String _name = "";
        int _style = 0;
        int _mapId = 0;
        int _posX = 0;
        int _posY = 0;
        String _detail = "";
        String _styleExt = "";
        string _path = "";
        string _cPath = "";

        public int X, Y;


        private bool _temp;

        public bool Temp
        {
            get { return _temp; }
            set
            {
                _temp = value;
                NotifyPropertyChanged("Temp");
            }
        }


        private POJOStatus _pojoStatus = POJOStatus.Normal;
        public POJOStatus POJOStatus {
            get
            {
                return _pojoStatus;
            }
            set
            {
                _pojoStatus = value;
            }
        }

        [Column(Name = "NPC_ID"), PrimaryKey, Identity]
        public int NPCId
        {
            get { return _id; }
            set
            {
                if(value == _id)
                {
                    return;
                }

                this._id = value;
                NotifyPropertyChanged("NPCId");

                if (POJOStatus != POJOStatus.Inserted && POJOStatus != POJOStatus.Deleted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "TYPE"), NotNull]
        public NpcType Type
        {
            get { return _type; }
            set
            {
                if(value == _type)
                {
                    return;
                }

                this._type = value;
                NotifyPropertyChanged("Type");

                if (POJOStatus != POJOStatus.Inserted && POJOStatus != POJOStatus.Deleted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "NAME"), NotNull]
        public String Name
        {
            get { return _name; }
            set
            {
                if(value == _name)
                {
                    return;
                }

                _name = value;
                NotifyPropertyChanged("Name");

                if (POJOStatus != POJOStatus.Inserted && POJOStatus != POJOStatus.Deleted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "STYLE")]
        public int Style
        {
            get { return _style; }
            set
            {
                if(value == _style)
                {
                    return;
                }

                this._style = value;
                NotifyPropertyChanged("Style");

                if (POJOStatus != POJOStatus.Inserted && POJOStatus != POJOStatus.Deleted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "MAP_ID"), NotNull]
        public int MapId
        {
            get { return _mapId; }
            set
            {

                if(_mapId == value)
                {
                    return;
                }

                this._mapId = value;
                NotifyPropertyChanged("MapId");

                if (POJOStatus != POJOStatus.Inserted && POJOStatus != POJOStatus.Deleted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "POS_X"), NotNull]
        public int PosX
        {
            get { return _posX; }
            set
            {
                if(value == _posX)
                {
                    return;
                }

                this._posX = value;
                NotifyPropertyChanged("PosX");

                if (POJOStatus != POJOStatus.Inserted && POJOStatus != POJOStatus.Deleted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "POS_Y"), NotNull]
        public int PosY
        {
            get { return _posY; }
            set
            {
                if(value == _posY)
                {
                    return;
                }

                this._posY = value;
                NotifyPropertyChanged("PosY");

                if (POJOStatus != POJOStatus.Inserted && POJOStatus != POJOStatus.Deleted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "DETAIL")]
        public String Detail
        {
            get { return _detail; }
            set
            {

                if(value == _detail)
                {
                    return;
                }

                this._detail = value;
                NotifyPropertyChanged("Detail");

                if (POJOStatus != POJOStatus.Inserted && POJOStatus != POJOStatus.Deleted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "STYLE_EXT")]
        public String StyleExt
        {
            get { return _styleExt; }
            set
            {
                if(_styleExt == value)
                {
                    return;
                }

                this._styleExt = value;
                NotifyPropertyChanged("StyleExt");

                if (POJOStatus != POJOStatus.Inserted && POJOStatus != POJOStatus.Deleted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        public string Path
        {
            get { return _path; }
            set
            {
                if(_path == value)
                {
                    return;
                }

                _path = value;
                NotifyPropertyChanged("Path");
            }
        }

        public string CPath
        {
            get { return _cPath; }
            set
            {
                if(_cPath == value)
                {
                    return;
                }

                _cPath = value;
                NotifyPropertyChanged("CPath");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }

    [Table(Name = "t_base_monster")]
    public class MonsterSection : IBaseObjectBase, INotifyPropertyChanged
    {
        int _monsterId = 0;
        String _name = "";
        short _lvl = 0;
        int _exp = 0;
        int _hp = 0;
        int _mp = 0;
        int _minAtk = 0;
        int _maxAtk = 0;
        int _mAtk = 0;
        int _def = 0;
        int _mDef = 0;
        int _atkRate = 0;
        int _missRate = 0;
        int _mAtkRate = 0;
        int _mMissRate = 0;
        int _atkCdtm = 0;
        int _atkRange = 0;
        int _exAtk = 0;
        int _exDmg = 0;
        int _move = 0;
        SByte _beatBack = 0;
        int _actionInterval = 0;
        int _walkRange = 0;
        int _chaseRange = 0;
        int _hpResume = 0;

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

        [Column(Name = "MONSTER_ID"), PrimaryKey, Identity]
        public int MonsterID
        {
            get { return _monsterId; }
            set
            {
                if(this._monsterId == value)
                {
                    return;
                }

                this._monsterId = value;
                NotifyPropertyChanged("Id");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "NAME"), NotNull]
        public String Name
        {
            get { return _name; }
            set
            {
                if(this._name == value)
                {
                    return;
                }

                this._name = value;
                NotifyPropertyChanged("Name");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "LEVEL"), NotNull]
        public short Level
        {
            get { return _lvl; }
            set
            {
                if(this._lvl == value)
                {
                    return;
                }

                this._lvl = value;
                NotifyPropertyChanged("Level");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "DROP_EXP"), NotNull]
        public int Exp
        {
            get { return _exp; }
            set
            {
                if(this._exp == value)
                {
                    return;
                }

                this._exp = value;
                NotifyPropertyChanged("Exp");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "HP"), NotNull]
        public int Hp
        {
            get { return _hp; }
            set
            {
                if(this._hp == value)
                {
                    return;
                }

                this._hp = value;
                NotifyPropertyChanged("Hp");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "MP"), NotNull]
        public int Mp
        {
            get { return _mp; }
            set
            {

                if(this._mp == value)
                {
                    return;
                }

                this._mp = value;
                NotifyPropertyChanged("Mp");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "MIN_ATK"), NotNull]
        public int MinAtk
        {
            get { return _minAtk; }
            set
            {

                if(this._minAtk == value)
                {
                    return;
                }

                this._minAtk = value;
                NotifyPropertyChanged("MinAtk");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "MAX_ATK"), NotNull]
        public int MaxAtk
        {
            get { return _maxAtk; }
            set
            {

                if(this._maxAtk == value)
                {
                    return;
                }

                this._maxAtk = value;
                NotifyPropertyChanged("MaxAtk");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "M_ATK"), NotNull]
        public int MAtk
        {
            get { return _mAtk; }
            set
            {
                if(this._mAtk == value)
                {
                    return;
                }

                this._mAtk = value;
                NotifyPropertyChanged("MAtk");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "DEF"), NotNull]
        public int Def
        {
            get { return _def; }
            set
            {

                if(this._def == value)
                {
                    return;
                }

                this._def = value;
                NotifyPropertyChanged("Def");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "M_DEF"), NotNull]
        public int MDef
        {
            get { return _mDef; }
            set
            {
                if (this._mDef == value)
                {
                    return;
                }

                this._mDef = value;
                NotifyPropertyChanged("MDef");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "ATK_RATE"), NotNull]
        public int AtkRate
        {
            get { return _atkRate; }
            set
            {
                if(this._atkRate == value)
                {
                    return;
                }
                this._atkRate = value;
                NotifyPropertyChanged("AtkRate");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "MISS_RATE"), NotNull]
        public int MissRate
        {
            get { return _missRate; }
            set
            {
                if(this._missRate == value)
                {
                    return;
                }

                this._missRate = value;
                NotifyPropertyChanged("MissRate");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "MATK_RATE"), NotNull]
        public int MAtkRate
        {
            get { return _mAtkRate; }
            set
            {
                if(this._mAtkRate == value)
                {
                    return;
                }

                this._mAtkRate = value;
                NotifyPropertyChanged("MAtkRate");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "MMISS_RATE"), NotNull]
        public int MMissRate
        {
            get { return _mMissRate; }
            set
            {
                if(this._mMissRate == value)
                {
                    return;
                }

                this._mMissRate = value;
                NotifyPropertyChanged("MMissRate");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "ATKCDTM"), NotNull]
        public int AtkCdtm
        {
            get { return _atkCdtm; }
            set
            {
                if(this._atkCdtm == value)
                {
                    return;
                }

                this._atkCdtm = value;
                NotifyPropertyChanged("AtkCdtm");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "ATKRANGE"), NotNull]
        public int AtkRange
        {
            get { return _atkRange; }
            set
            {
                if(this._atkRange == value)
                {
                    return;
                }

                this._atkRange = value;
                NotifyPropertyChanged("AtkRange");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "EXATK"), NotNull]
        public int ExAtk
        {
            get { return _exAtk; }
            set
            {
                if(this._exAtk == value)
                {
                    return;
                }

                this._exAtk = value;
                NotifyPropertyChanged("ExAtk");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "EXDMG"), NotNull]
        public int ExDmg
        {
            get { return _exDmg; }
            set
            {

                if (this._exDmg == value)
                {
                    return;
                }

                this._exDmg = value;
                NotifyPropertyChanged("ExDmg");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "MOVER"), NotNull]
        public int Move
        {
            get { return _move; }
            set
            {
                if(this._move == value)
                {
                    return;
                }

                this._move = value;
                NotifyPropertyChanged("Move");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "BEAT_BACK"), NotNull]
        public SByte BeatBack
        {
            get { return _beatBack; }
            set
            {
                if(this._beatBack == value)
                {
                    return;
                }

                this._beatBack = value;
                NotifyPropertyChanged("BeatBack");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "ACTION_INTERVAL"), NotNull]
        public int ActionInterval
        {
            get { return _actionInterval; }
            set
            {
                if(this._actionInterval == value)
                {
                    return;
                }

                this._actionInterval = value;
                NotifyPropertyChanged("ActionInterval");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "WALK_RANGE"), NotNull]
        public int WalkRange
        {
            get { return _walkRange; }
            set
            {
                if(this._walkRange == value)
                {
                    return;
                }

                this._walkRange = value;
                NotifyPropertyChanged("WalkRange");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "CHASE_RANGE"), NotNull]
        public int ChaseRange
        {
            get { return _chaseRange; }
            set
            {
                if(this._chaseRange == value)
                {
                    return;
                }

                this._chaseRange = value;
                NotifyPropertyChanged("ChaseRange");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        [Column(Name = "HP_RESUME"), NotNull]
        public int HpResume
        {
            get { return _hpResume; }
            set
            {
                if(this._hpResume == value)
                {
                    return;
                }

                this._hpResume = value;
                NotifyPropertyChanged("HpResume");

                if (POJOStatus != POJOStatus.Inserted)
                {
                    POJOStatus = POJOStatus.Updated;
                }
            }
        }

        public MonsterSection UserMemberwiseClone() {
            /*MonsterSection clone = new MonsterSection()
            {
                MonsterID = this.MonsterID,
                Name = this.Name,
                Level = this.Level,
                Exp = this.Exp,
                Hp = this.Hp,
                Mp = this.Mp,
                MinAtk = this.MinAtk,
                MaxAtk = this.MaxAtk,
                MAtk = this.MAtk,
                Def = this.Def,
                MDef = this.MDef,
                AtkRate = this.AtkRate,
                MissRate = this.MissRate,
                MAtkRate = this.MAtkRate,
                MMissRate = this.MMissRate,
                AtkCdtm = this.AtkCdtm,
                AtkRange = this.AtkRange,
                ExAtk = this.ExAtk,
                ExDmg = this.ExDmg,
                Move = this.Move,
                BeatBack = this.BeatBack,
                ActionInterval = this.ActionInterval,
                WalkRange = this.WalkRange,
                ChaseRange = this.ChaseRange,
                HpResume = this.HpResume,
            };*/
            return (MonsterSection)this.MemberwiseClone();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
