using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditorControl.InnerUtil
{
    public enum ButtonType
    {
        ProjectPath,
        MapSourcePath,
        MapSourceOutputPath,
        NpcPicturePath,
        MonsterPicturePath,
        MapSoundPath,
    }

    public enum MenuItemType
    {
        NewProject,
        ProjectConfig,
        OpenProject,
        
        SelectTool,
        AreaTool,
        PenTool,
        PointTool,

        Output,
        CutMap,
    }

    public enum CostumeDialogButtonType
    {
        OK,
        Cancle,
        OKAndCancle,

        Dummy,
    }

    //public enum TileType
    //{
    //    Normal,
    //    Selected,
    //    SelectedTranslucent,
    //}

    public enum NpcType
    {
        Person = 1,
        StayPoint,
        Telereport,
        NeutralTele,
    }
}
