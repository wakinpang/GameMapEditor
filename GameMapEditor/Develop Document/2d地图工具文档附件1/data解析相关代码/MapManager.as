package game.core.manager {
	
	import flash.display.Bitmap;
	import flash.display.BitmapData;
	import flash.display.Loader;
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.utils.ByteArray;
	
	import game.core.map.MapPanel;
	import game.core.map.MapVo;
	import game.core.utils.Astar;
	import game.core.utils.Dijkstra;
	import game.core.utils.GameTimer;
	import game.core.utils.LoaderInfo;
	import game.core.utils.MassLoader;
	
	import sk.Asset;
	import sk.data.TextureAsy;
	
	public class MapManager {
		
		public static const TILE_WIDTH:int = 256;
		
		public static var mapID:int;
		public static var mapJpg:int;
		public static var mapWidth:int;
		public static var mapHeight:int;
		
		public static var isDoubleLayer:Boolean;
		
		public static var thumb:BitmapData;
		public static var thumbTexture:TextureAsy;
		public static var bg:BitmapData;
		public static var bgTexture:TextureAsy;
		public static var bgupload:Boolean = false;
//		public static const thumbBlockWidth:int = 50;
//		public static const thumbBlockHeight:int = 30;
		
		public static var mapInfoDict:Object = {};
		
		public static var unitLeft:int,unitTop:int,unitRight:int,unitBottom:int;
		
		private static const NO:int = 0;
		private static const LOADING:int = 1;
		private static const OK:int = 2;
		
//		public static var mapLayer:ObjectContainer3D = new ObjectContainer3D();
//		private static var plane:PlaneGeometry = new PlaneGeometry(TILE_WIDTH, TILE_WIDTH);
		private static var tileList:Vector.<MapVo>;
		
		public static var x:int, y:int;
		public static var sightX:int, sightY:int;
		private static var gx:int, gy:int;
		private static var SCREEN_RIGHT:int, SCREEN_BOTTOM:int;
		private static var RIGHT_X:int, RIGHT_Y:int;
		private static var maxCol:int,maxRow:int;
		public static var quakeX:int,quakeY:int;
		
		private static var mapRow:int;
		private static var mapCol:int;
		private static var tileNum:int;
		
		public static var totalTexture:int;
		private static var textureList:Vector.<TextureAsy>;
		private static var bmdList:Vector.<BitmapData>;//内存 跨图释放
//		private static var textureFlag:Vector.<int>;
		private static var _followTarget:*;
		
		private static var lastJpg:int = -1;//记录上一次的地图资源id，避免重复加载
		public static var enterFlag:Boolean = true;
		public static var isSideJudge:Boolean=true;
		/////
		public static var roleLayer:Sprite = new Sprite;
		public static var tagLayer:Sprite = new Sprite;//标记层
//		public static var numberLayer:Sprite = new Sprite;
		private static var onInitComplete:Function;
		
		public static var mapPanel:MapPanel;
		
		//地图目前的释放策略 一个地图里留有内存 然后动态释放显存 跨图释放内存
		public static function first():void{
			
			resize();
			
//			roleLayer.cacheAsBitmap = true
		}
		
		public static function init(info:LoaderInfo,onComplete:Function):void {
			
			enterFlag = true;
			
			onInitComplete = onComplete;
			
			var data:ByteArray = info.data;
			try{
				var len:int = data.readInt();
				var byte:ByteArray = new ByteArray();
				data.readBytes(byte, 0, len);
				byte.uncompress();
			}catch(e:Error){
				MassLoader.errorHandler(info);
				return;
			}
			mapWidth = byte.readInt();
			mapHeight = byte.readInt();
			Astar.init(byte);
			Dijkstra.init(byte);
			data.readBytes(byte, 0);
			if(mapJpg == lastJpg){
				onInitComplete();
				enterFlag = false;
				return;
			}
			lastJpg = mapJpg;
			
			var loader:Loader = new Loader();
			loader.contentLoaderInfo.addEventListener(Event.COMPLETE, onLoadThumb);
			loader.loadBytes(byte,MassLoader.context);
			
			resetMapInfo();
			
			if(textureList){
				for(var i:int = textureList.length-1;i>=0;i--){
					var bpd:TextureAsy = textureList[i];
					if(bpd)bpd.dispose();
					
					var bmd:BitmapData = bmdList[i];
					if(bmd)bmd.dispose();
				}
			}
			
			//textureList 遍历 释放 阀值>100m才会启动释放策略 释放策略时间多久没用的 清除
			
			textureList = new Vector.<TextureAsy>(maxCol*maxRow);
			bmdList = new Vector.<BitmapData>(maxCol*maxRow);
			
			totalTexture = 0;
//			textureFlag = new Vector.<int>(maxCol*maxRow);
			
			bgupload = false;
			
			show();
			
		}
		
		public static function show():void{
			enterFlag = false;
			if(isDoubleLayer){
				
				MassLoader.addDecode(Asset.RES_PATH+"map/"+mapJpg+"/bg.jpg",bgComplete,null,MassLoader.MAP);
			}else{
				
			}
		}
		private static function bgComplete(info:LoaderInfo):void{
			bg = info.content.bitmapData;
			bgResize();
		}
		private static function bgResize():void{
			
			if(bgTexture)bgTexture.dispose();
			bgTexture = new TextureAsy();
			bgTexture.init(bg,'mapbg',StageManager.context,true);
			bgupload = true;
		}
		
		private static var textureW:int,textureH:int;
		private static function onLoadThumb(e:Event):void {
			thumb = (e.target.content as Bitmap).bitmapData;
			
			textureW = Math.pow(2,Math.ceil(Math.log(thumb.width)/Math.LN2));
			textureH = Math.pow(2,Math.ceil(Math.log(thumb.height)/Math.LN2));
			
			MapPanel.uvScale[2] = (thumb.width/textureW) * (256/(thumb.width * 10));
			MapPanel.uvScale[3] = (thumb.height/textureH) * (256/(thumb.height * 10));
			
			if(thumbTexture)thumbTexture.dispose();
			thumbTexture = new TextureAsy();
			thumbTexture.init(thumb,'mapThumb',StageManager.context,true);

			onInitComplete();
		}
		
		public static function resetThumb():void
		{
			if(thumbTexture)thumbTexture.dispose();
			thumbTexture = new TextureAsy();
			thumbTexture.init(thumb,'mapThumb',StageManager.context,true);
			
			textureList = new Vector.<TextureAsy>(maxCol*maxRow);
//			textureFlag = new Vector.<int>(maxCol*maxRow);
			
			MapManager.resize();
		}
		
		public static function watch(tx:int, ty:int):void {
			if(enterFlag)return;
			x = tx;
			y = ty;
			sightX = x - StageManager.W_Half  - quakeX;
			sightY = y - StageManager.H_Half  - quakeY;
			
			if(isSideJudge){
				if(sightX < 0)sightX = 0;
				if(sightY < 0)sightY = 0;
				if(sightX > SCREEN_RIGHT)sightX = SCREEN_RIGHT;
				if(sightY > SCREEN_BOTTOM)sightY = SCREEN_BOTTOM;
			}
//			sightX = x - StageManager.view.width/2;
//			sightY = y - StageManager.view.height/2;
			
			if(sightX < 0)sightX = 0;
			if(sightY < 0)sightY = 0;
			if(sightX > SCREEN_RIGHT)sightX = SCREEN_RIGHT ;
			if(sightY > SCREEN_BOTTOM)sightY = SCREEN_BOTTOM;

//			StageManager.view.camera.x = int(sightX / TILE_WIDTH)* TILE_WIDTH - sightX;
//			StageManager.view.camera.y = int(sightY / TILE_WIDTH)* TILE_WIDTH - sightY;
			//trace('StageManager.view.camera.x',StageManager.view.camera.x);
			mapPanel.x = int(sightX / TILE_WIDTH)* TILE_WIDTH - sightX;
			mapPanel.y = int(sightY / TILE_WIDTH)* TILE_WIDTH - sightY;
			unitLeft = sightX;
			unitTop = sightY;
			unitRight = sightX+StageManager.stage.stageWidth;
			unitBottom = sightY+StageManager.stage.stageHeight;
			
			tagLayer.x = roleLayer.x = -(Math.round(sightX));//quakeX 震动
			tagLayer.y = roleLayer.y = -(Math.round(sightY));
			
			//特效
//			StageManager.mainLayer.x = StageManager.view.camera.x - sightX - StageManager.view.width/2;
//			
//			StageManager.mainLayer.z = StageManager.view.camera.y + 500 / Math.tan(Math.PI / 180 * 30) -
//				(-StageManager.stage.stageHeight * .5 - sightY) / Math.sin(Math.PI / 180 * 30);
//			
			var ngx:int = sightX / TILE_WIDTH;
			var ngy:int = sightY / TILE_WIDTH;
			
			
			//有改变
			if(ngx != gx || ngy != gy) {
				for(var i:int=0; i<tileNum; i++) {
					var col:int = ngx + i % mapCol;
					var row:int = ngy + int(i / mapCol);
					if(col < 0 || row < 0 || col > RIGHT_X || row > RIGHT_Y) continue;
					var tile:MapVo = tileList[i];
					tile.row = row;
					tile.col = col;
					
					var key:uint = row * maxCol + col;
					//去掉flag字典
					if(textureList[key]) {
						tile.texture = textureList[key];
					}else {
						tile.texture = null;
						if(bmdList[key])
						{
							
							var t:TextureAsy = new TextureAsy();
							t.init(bmdList[key],key+'map',StageManager.context,true);
							textureList[key] = t;
							totalTexture += t.size;
							
							tile.texture = t;
						}
						else
						{
							
							MassLoader.addDecodeOnce(Asset.RES_PATH+"map/" + mapJpg + "/" + row + "_" + col + ".jpg", loadeComplete, mapJpg*10000+row*100+col, MassLoader.MAP);
						}
						
					}
					
					//没有缓存素材 和缓存显存
					//tile.setURL("resource/map/" + mapJpg + "/" + row + "_" + col + ".jpg");
				}
				gx = ngx;
				gy = ngy;
				
				checkClear();
			}
			
			
		}

		private static function loadeComplete(info:LoaderInfo):void {
			var key:Number = info.target;
			var data:BitmapData = (info.content as Bitmap).bitmapData;
			var mapResId:uint = int(key/10000);
			if(mapResId != mapJpg){
				data.dispose();
			}else {
				var col:int = (key%100);
				var row:int = int((key%10000)/100);
				//data.getPixel32(0,0);//TODO
				
				var t:TextureAsy = new TextureAsy();
				t.init(data,key+'map',StageManager.context,true);
				textureList[row*maxCol+col] = t;
				
				bmdList[row*maxCol+col] = data;
				
				StageManager.maxMem += (data.height * data.width) * 4;
				
				totalTexture += t.size;
				
				var dx:int = col-gx;
				var dy:int = row-gy;
				if(dy<0||dx<0||dy>=mapRow||dx>=mapCol)return;
				var tile:MapVo = tileList[dy*mapCol+dx];
				tile.texture = textureList[row*maxCol+col];
			}
		}
		
		private static function checkClear():void
		{
			if(totalTexture>max_gc)clear();	
		}
		
		
		
		private static const follow:Number = 0.1;
		private static var tweenX:int;
		private static var tweenY:int;
		/**镜头缓动**/
		public static function tweenTo(targetX:int,targetY:int):void{
			var cameraX:int = x;
			var cameraY:int = y;
			tweenX = targetX;
			tweenY = targetY;
			if(cameraX != tweenX || cameraY != tweenY){
				var dx:int = follow*(tweenX-cameraX);
				if(dx == 0 && tweenX != cameraX)dx = tweenX>cameraX?1:-1;
				var dy:int = follow*(tweenY-cameraY);
				if(dy == 0 && tweenY != cameraY)dy = tweenY>cameraY?1:-1;
				watch(cameraX+dx,cameraY+dy);
			}
		}
		
		public static function run():void{
			if(lastJpg != mapJpg){
				MassLoader.log("地图Dat 还未加载完");
			}
			if(_followTarget){
				watch(_followTarget.x, _followTarget.y);
			}else if(tweenX != int.MIN_VALUE || tweenY != int.MIN_VALUE){
				tweenTo(tweenX, tweenY);
			}
			if(quakeX||quakeY){
				watch(x,y)
			}
		}
		
		public static function set followTarget(target:*):void{
			_followTarget = target;
			tweenX = tweenY = int.MIN_VALUE;
		}
		
		private static function resetTileInfo():void {
			mapRow = Math.ceil(StageManager.stage.stageHeight/ TILE_WIDTH) + 1;
			mapCol = Math.ceil(StageManager.stage.stageWidth / TILE_WIDTH) + 1;
			tileNum = mapRow*mapCol;
			gx = -1;
			gy = -1;
		}
		
		private static function resetMapInfo():void {
			SCREEN_RIGHT = mapWidth - StageManager.stage.stageWidth;
			SCREEN_BOTTOM = mapHeight - StageManager.stage.stageHeight;
			RIGHT_X = Math.ceil((mapWidth - TILE_WIDTH * 2) / TILE_WIDTH) + 1;
			RIGHT_Y = Math.ceil((mapHeight - TILE_WIDTH * 2) / TILE_WIDTH) + 1;
			maxCol = Math.ceil(mapWidth/TILE_WIDTH) + 1;
			maxRow = Math.ceil(mapHeight/TILE_WIDTH) + 1;
			gx = -1;
			gy = -1;
		}
		
		//贴图自释放机制
		private static var max_gc:int = 100*1024*1024;
		private static function clear():void
		{
			var len:int = textureList.length;
			
			for (var i:int=0;i<len;i++) 
			{
				var texture:TextureAsy = textureList[i];
				if(texture == null)continue;
				if(GameTimer.now - texture.lastUseTime > 5000)
				{
					texture.dispose();
					textureList[i] = null;
					
					totalTexture -= texture.size;
				}
			}
			
			MassLoader.log('地图显存释放机制',totalTexture/1024/1024);
		}
		
		public static function resize():void{
			if(mapPanel == null){
				return;
			}
			
			var lastRow:int = mapRow;
			var lastCol:int = mapCol;
			
			resetTileInfo();
			resetMapInfo();
			
			mapPanel.resize();
			
			if(mapRow != lastRow || mapCol != lastCol) {

				mapPanel.clear();
				
				tileList = new Vector.<MapVo>(tileNum);
				for(var j:int=0; j<mapRow; j++) {
					for(var i:int=0; i<mapCol; i++) {
						
						var vo:MapVo = new MapVo;
						vo.x = i* TILE_WIDTH;
						vo.y = -j*TILE_WIDTH;
						mapPanel.addMap(vo);
						
						tileList[j*mapCol+i] = vo;
					}
				}
			}
			if(_followTarget)watch(_followTarget.x, _followTarget.y);
		}
	}
}