# 游戏地图编辑器

## 1.更新履历
>2016-12-30    基本UI风格也界面配置完成。
>
>2017-01-06    完成了对MapEditorControl & NavigationControl的MVVM化改造，且固定了控件编写的模型，以后将进一步套用。
>
>2017-01-20    完成了新建项目以及修改项目配置的功能。
>
>2017-01-27    完成了项目切换和历史记录的功能。
>
>2017-01-29    完成了地图编辑的选区、取消选区以及Ctrl+Z & Ctrl+Y的功能。
>
>2017-02-12    完成了地图编辑的钢笔工具以及出生点选择的功能。
>
>2017-03-25    需求文档上的功能完成完毕。

## 2.已经实现的功能
1. 地图显示；
2. 导航窗口；
3. 新建工程以及修改工程配置；
4. 数据库连接；
5. 项目切换以及历史记录；
6. 地图选区与取消选区；
7. 地图编辑的Ctrl+Z & Ctrl+Y；
8. 钢笔工具；
9. 出生点选择；
10. 场景怪的添加、修改；
11. NPC的添加；
12. 怪物的添加；
13. 导出到data文件；
14. 同步地图、基础怪、场景怪、NPC数据到数据库；
15. 切图；

## 3.RoadMap

*Update Time : 2017-1-20 22:12:24*

**H：黄毅**
**P：庞琨力**

> *Start Date：2016-12-24*  
> *EndTime Date: 2016-12-30*  
> Step 1：  
> 完成初步界面布局（无逻辑实现）  
> P：整体布局（状态栏暂时填充没有内容）、菜单、工具栏 
> H：地图显示组件（读图、分割）、导航栏

> Step 2：  
> *Start Date：2016-12-30*  
> *EndTime Date: 2017-1-20*  
> 完成组件：（设定测试数据）  
> P：资料库组件：怪物库等；  
> H：新建工程页面（包括初步的数据库连接测试）；

> Step 3：  
> *Start Date：2017-1-20*  
> *EndTime Date: 2017-2-12*  
> 完成逻辑：  
> P：菜单功能（切换项目等）；  
> H：地图编辑功能（拉区域、描点等）；

> Step 4： 
> *Start Date：2017-2-12*  
> *EndTime Date: 2017-3-10*  
> 完成数据库和库的对接：  
> P：完成各个库的显示还有拖拽；  
> H：完成和数据库的对接，把数据库的内容对应到Model上；

> Step 5：  
> *Start Date：2017-3-10*  
> *EndTime Date: 2017-3-24*  
> P：完成属性界面，包括修改、新增等；  
> H：完成修改到数据库的同步；  

> Step 6：  
> *Start Date：2017-3-24*  
> *EndTime Date: 2017-3-25*  
> 完成输出到data。  

## 4.其它
1. UserControl全部采用Mvvm模式实现，不导出任何Dependency Property，全部所需属性由Model来控制，UI变更用Messenger来进行通知；

2. MessageDialog是通过CostumeDialog定制生成的；

3. WaitingDialog是通过CostumeDialog定制生成的；

4. 记录历史项目的文件名称为**history.xml**，和程序的EXE文件在同一目录下；

5. 项目的工程文件默认为**<项目工程名>.project**，放在配置项目时指定的目录文件下；

6. 程序主目录下需要有**image文件夹**，文件夹中需要有以下图片：

   > born_point.bmp：出生点图片
   >
   > NPC.png：库中NPC图片
   >
   > StayPoint.png：库中挂机点图片
   >
   > Telereport.png：库中传送点图片
   > 
   > NeutralTele.png: 中立传送点图片
   >
   > tmp_monster.bmp：临时怪物图片