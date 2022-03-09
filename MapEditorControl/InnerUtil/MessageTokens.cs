using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MapEditorControl.InnerUtil
{
    using TileType = Byte;
    // Message
    public enum MapEditorControlMessageTokens
    {
        UpdateZoomFromView,
        UpdateBackgroudMapFromView,
        UpdateTileWidthFromView,
        UpdateTileHeightFromView,

        UpdateDefaultBornPointImageFromView,
        UpdateBornPointFromViewModel,

        UpdateAreaToolSelectedFromView,
        UpdateDragToolSelectedFromView,
        UpdatePenToolSelectedFromView,
        UpdatePointToolSelectedFromView,
        UpdateTransparentSelectedFromView,

        UpdateToolBarCanEditFromViewModel,

        UpdateMapPixelWidthFromViewModel,
        UpdateMapPixelHeightFromViewModel,

        ArrangedFromView,

        DoArrangeFromViewModel,

        DisplayTilesAndSelectedTile,
        DisplayTilesAndSelectedTileForce,

        AdjustSelectedTilePosition,

        UpdateOffsetRatioFromMapEditorViewModel,
        UpdateBackgroundSizeFromViewModel,

        UpdateVaildFromViewModel,

        SetInitialMousePointLeftButtonDown,
        SetInitialMousePointRightButtonDown,
        DrawDragingArea,
        DrawDragedArea,
        UpdateSelectedArea,
        UpdateUnselectedArea,
        CancelDrag,
        UndrawSelectedArea,
        DrawAppointedArea,

        SetInitialPenPoint,
        AddNewPenPoint,
        AddLastPenPoint,
        CancelPenPoints,

        NotifyAreaChanged,

        SetNewObjectCollection,
        ObjectCollectionAdded,
        ObjectCollectionRemoved,
        ObjectCollectionReplaced,
        ObjectCollectionMoved,
        ObjectCollectionReseted,
        DoMapObjectBind,
        ShowNewObjects,

        SelectMapObject,
        MoveMapObject,
        RemoveMapObject,
        ResetMapObject,

        DeleteMapObjectFromViewModel,

        RaiseDropEventFromView,

        UpdateBornPointXFromView,
        UpdateBornPointYFromView,

        ReinitializeView,

        UpdateCurrentSceneMonsters,
        UpdateCurrentNpcs,
        UpdateCurrentMapIDFromView,
        UpdateSafetyFromView,
        UpdateFishingFromView,
    }

    public enum NavigationControlMessageTokens
    {
        UpdateBackgroudMapFromView,
        UpdateZoomFromView,
        UpdateContentRatioFromView,
        UpdateContentScrollRatioFromView,

        ArrangedFromView,

        DoArrangeFromViewModel,
        AdjustAreaFromViewModel,
        AdjustAreaWithDragFromViewModel,

        UpdateContentRatioFromViewModel,

        UpdateBackgroundShowFromViewModel,
        UpdateZoomFromViewModel,

        UpdateVaildFromViewModel,

    }

    public enum ProjectConfigControlMessageTokens
    {
        UpdateProjectNameFromView,
        UpdateProjectPathFromView,
        UpdateDatabaseIPFromView,
        UpdateDatabasePortFromView,
        UpdateDatabaseNameFromView,
        UpdateDatabaseUserNameFromView,
        UpdateDatabasePasswordFromView,
        UpdateMapSourcePathFromView,
        UpdateMapOutputSourcePathFromView,
        UpdateNpcPicturePathFromView,
        UpdateMonsterPathFromView,
        UpdateMapSoundPathFromView,

        UpdateProjectNameFromViewModel,
        UpdateProjectPathFromViewModel,
        UpdateDatabaseIPFromViewModel,
        UpdateDatabasePortFromViewModel,
        UpdateDatabaseNameFromViewModel,
        UpdateDatabaseUserNameFromViewModel,
        UpdateDatabasePasswordFromViewModel,
        UpdateMapSourcePathFromViewModel,
        UpdateMapOutputSourcePathFromViewModel,
        UpdateNpcPicturePathFromViewModel,
        UpdateMonsterPathFromViewModel,
        UpdateMapSoundPathFromViewModel,

        OKEventFromViewModel,
        CancelEventFromViewModel,
        TestConnectEventFromViewModel,
    }

    public enum MenuControlMessageTokens
    {
        NewProjectEventFromViewModel,
        ProjectConfigEventFromViewModel,
        OpenProjectEventFromViewModel,
        SelectHistoryEventFromViewModel,

        SelectToolEventFromViewModel,
        AreaToolEventFromViewModel,
        PenToolEventFromViewModel,
        PointToolFromViewModel,
        OutputEventFromViewModel,

        UpdateProjectExistFromView,
        UpdateCurrentMapValidFromView,

        //SelectHistoryFromViewModel,
        ChangeHistorySectionFromOutside,
        CutMapEventFromViewModel,
    }

    public enum MenuMessageTokens
    {
        ShowNewProjectWindowFromViewModel,
        HideNewProjectWindowFromViewModel,

        OpenProjectFromViewModel,

        UpdateProjectExist,
        ModifyProjectConfig,

        FetchProjectDataFromDatabase,

        UpdateCurrentProjectConfig,
    }

    public enum LibraryControlMessageTokens
    {
        UpdateProjectExistFromModel,
        UpdateMapValidFromModel,

        UpdateMapFromOutside,
        UpdateMusicFromOutside,
        UpdateStyleFromOutside,
        UpdateMonsterFromOutside,

        ChangeDataFromView,
        //Music
        CheckedMusicChangedFromView,
        ChangeCheckedMusic,
        MusicSectionChangedFromView,
        CurrentMusicSectionChangedEventFromViewModel,
        CallMusicErrorDialogFromViewModel,

        //Monster
        CallNewDialogFromView,
        CallChangeDialogFromView,
        ShowDialogFromViewModel,
        ChangeCurrentMonsterFromLibrary,
        UpdataCurrentMonster,

        //Map
        MapCheckedFromView,
        MapSectionChangedFromView,
        CurrentMapSectionChangedEventFromViewModel,

        FetchMapMonsterData,
        FetchMapNpcData,
        //FetchCurrentMapSceneMonsterPOJOsData,
    }

    public enum MainWindowTokens
    {
        ShowMessageDialog,

        UpdateCostumeDialogCallback,
        UpdateWaitingDialogCallback,

        UpdateMusicSections,
        UpdateMapSections,
        UpdateMonsterSections,
        UpdateMainWindowTitle,

        SendDropCursorPositionAndAddMapObject,
        GetDropCursorPosition,

        UpdateWaitingMessageDialogTitle,
        UpdateWaitingMessageDialogMessage,

        UpdateCurrentSceneMonsters,
        UpdateCurrentNpcs,
        UpdateCurrentItemsInfo,
        
        UpdateCurrentMapSceneMonsterPOJOs,
        UpdateCurrentMissionsInfo,
        UpdateCurrentMapNpcs,
    }

    public enum CostumeMessageDialogMessageTokens
    {
        UpdateTitleFromView,
        UpdateMessageFromView,
        UpdateButtonTypeFromView,

        UpdateButtonTypeFromViewModel,

        ShowCostumeMessageDialog,
        HideCostumeMessageDialog,

        OKEventFromViewModel,
        CancelEventFromViewModel,

    }

    public enum CostumeWaitingDialogMesssageTokens
    {
        UpdateTitleFromView,
        UpdateMessageFromView,
        UpdateCancelVaildFromView,

        ShowCostumeWaitingDialog,
        HideCostumeWaitingDialog,

        CancelEventFromViewModel,
    }

    public enum ToolBarControlMessageTokens
    {
        UpdateDragToolSelectedFromViewModel,
        UpdateAreaToolSelectedFromViewModel,
        UpdatePenToolSelectedFromViewModel,
        UpdatePointToolSelectedFromViewModel,
        UpdateTransparentSelectedFromViewModel,

        UpdateCanEditFromView,
        UpdateProjectValidFromView,

        SyncHandlerFromViewModel,

        SyncMapData,
        SyncMonsterData,
        SyncSceneMonsterData,
        SyncNpcData,

        UpdateSafetyFromViewModel,
        UpdateFishingFromViewModel,
    }

    public enum MonsterConfigControlMessageTokens
    {
        CurrentMonsterChangedFromView,
        CurrentMonsterChangedFriomModel,
        MonsterCollectionChangedFromModel,

        MonsterOkEventFromView,

        HideDailogFromView,
        HideDailogFromViewModel,

        CallMonsterCollectionUpdate,

        NotifyUpdateErrorHapendFromViewModel,
    }

    public enum MapObjectSpriteMessageTokens
    {
        UpdateIDFromView,
        UpdateNameFromView,
        UpdateXFromView,
        UpdateYFromView,
        UpdateSpriteImageSourceFromView,
        UpdateSelectedFromView,

        //UpdateIDFromViewModel,
        //UpdateNameFromViewModel,
        UpdateXFromViewModel,
        UpdateYFromViewModel,
        //UpdateSpriteImageSourceFromViewModel,
    }

    public enum PropertyControlMessageTokens
    {
        UpdateCurrentMapSectionFromView,
        UpdateCurrentMonsterSectionFromView,
        UpdateCurrentSceneMonsterPOJOFromView,
        UpdateCurrentMapSceneMonsterPOJOsFromView,
        UpdateCurrentNpcSectionFromView,
        UpdateCurrentNpcSectionFromModel,
        UpdateCurrentNpcCollectionFromModel,

        RaiseShowDropInfoEventFromViewModel,
        RaiseEditSceneMonsterButtonEventFromViewModel,
        RaiseNewSceneMonsterButtonEventFromViewModel,

        ShowDropInfoDialog,
        HideDropInfoDialog,

        FetchItemData,
        FetchMissionData,

        SetCurrentItemTabToMap,

        UpdateCurrentSceneMonsterPOJOFromViewModel,

        ShowBaseRewardInfoDialog,
        HideBaseRewardInfoDialog,

        RaiseShowBaseRewardEditControlEventFromViewModel,
        RaiseShowMissionRewardEditControlEventFromViewModel,

        ShowMissionRewardInfoDialog,
        HideMissionRewardInfoDialog,
        UpdateCurrentMapIDFromView,
    }

    public enum DropItemInfoListControlMessageTokens
    {
        UpdateDropItemInfoStringFromView,
        UpdateCurrentItemsInfoFromView,

        UpdateDropItemInfoStringFromViewModel,

        CancelEventFromViewModel,

        NotifyUpdateErrorHapendFromViewModel,
    }

    public enum SceneMonsterConfigControlMessageTokens
    {
        UpdateCurrentSceneMonsterPOJOFromView,
        UpdateModifyingFromView,
        UpdateCurrentSceneMonsterPOJOsFromView,
        UpdateCurrentMonsterPicturePathFromView,

        RaiseCancelEvent,
        RaiseOKEvent,

        ShowSceneMonsterPOJOConfigDialog,
        HideSceneMonsterPOJOConfigDialog,

        NotifyUpdateErrorHapendFromViewModel,
        UpdateCurrentMapIDFromView,
    }

    public enum BaseRewardInfoControlMessageTokens
    {
        UpdateRewardStringFromView,

        UpdateRewardStringFromViewModel,

        CancelEventFromViewModel,
    }
    
    public enum MissionRewardInfoControlMessageTokens
    {
        UpdateMissionStringFromView,

        UpdateMissionStringFromViewModel,

        NotifyUpdateErrorHapendFromViewModel,

        UpdateCurrentMissionPOJOsFromView,
        UpdateCurrentItemPOJOsFromView,

        RaiseCancelEventFromViewModel,
    }

    public enum OkTypes
    {
        Create,
        Change,
    }

    public class MessageParameterTileDisplay
    {
        public int PixelWidth { get; set; }
        public int PixelHeight { get; set; }
        public double Zoom { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
    }

    public class MessageParameterSelectedTileStruct
    {
        public double XDelta { get; set; }
        public double YDelta { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class MessageParameterNavigationDisplay
    {
        public int PixelWidth { get; set; }
        public int PixelHeight { get; set; }
        public double Zoom { get; set; }
        public double WidthRatio { get; set; }
        public double HeightRatio { get; set; }
    }

    public class MessageParameterDialogTitleAndMessage
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }

    public class MessageParameterCostumeMessageDialogParams
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public CostumeDialogButtonType ButtonType { get; set; }
        public Action OKFunc { get; set; }
        public Action CancleFunc { get; set; }
    }

    public class MessageParameterCostumeWaitingDialogParams
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public bool CanCancel { get; set; }
        public object Task { get; set; }
    }

    public struct SelectedAreaPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public struct AreaPointState
    {
        public int X { get; set; }
        public int Y { get; set; }
        public TileType Type { get; set; }
    }

    //Info Class'es base class
    public class BaseMapInfo {

    }

    public class DropMapMonsterInfo : BaseMapInfo
    {
        public Point Position { get; set; }
        public MonsterSection Monster { get; set; }
        public DragEventArgs Args { get; set; }
    }

    public class DropMapNpcInfo : BaseMapInfo
    {
        public Point Position { get; set; }
        public NpcSection Npc { get; set; }
        public DragEventArgs Args { get; set; }
    }

    public class MapObjectAndSceneObjectInfo
    {
        public IBaseObjectBase BaseObject { get; set; }
        public ISceneObjectBase SceneBaseObject { get; set; }
    }

    public class MonsterDictoryAndMonsterObject
    {
        public string MonsterDictory { get; set; }
        public ObservableCollection<MapObjectAndSceneObjectInfo> Info { get; set; }
    }

    public class NpcDictoryAndNpcObject
    {
        public string NpcDictory { get; set; }
        public ObservableCollection<MapObjectAndSceneObjectInfo> Info { get; set; }
    }

    public class ValidateErrorHappendChecker
    {
        public bool Flag { get; set; }
    }

    public enum TileTypeBit
    {
        Selected = 1,
        Translucent = 2,
        Safety = 4,
        Fishing = 8,
    }
}
