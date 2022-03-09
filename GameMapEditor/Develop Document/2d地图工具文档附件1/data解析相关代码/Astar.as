package game.core.utils{
	import flash.geom.Point;
	import flash.utils.ByteArray;
	
	public class Astar
	{
		public static var map:Vector.<Vector.<int>>;//障碍数组
		private static var raw:Vector.<Vector.<int>>;//原始障碍数组，不会被动态障碍修改
		private static var size:int = 25;//障碍栅格大小
		private static var pool:Vector.<Node>;//对象池
		private static var dx:Vector.<int> = Vector.<int>([-1,0,1,0,-1, 1,1,-1]);//估值时x偏移
		private static var dy:Vector.<int> = Vector.<int>([0,-1,0,1,-1,-1,1, 1]);//估值时y偏移
		private static const NEAR_START:int = 100;//太近的终点不启动搜索，值为距离的平方
		private static var openVector:Vector.<Node>;
		private static var dataVector:Vector.<Vector.<Node>>;
		
		private static var width:int,height:int;
		
		private static var poolLength:int = 0;
		private static var first:Node;
		
		private static var road:Array = [];//存储A*搜索出来未经平滑的结果
		private static const B:uint = 0x1;//障碍掩码 0障碍 1通行
		private static const F:uint = 0x8;//钓鱼掩码
		private static const P:uint = 16;//擂台 安全区域
		
		private static const NORMAL:int = 1;
		private static const ARRIVE:int = 2;
		public static const FISH:int = 3;
		public static var mode:int = NORMAL;//A*算法工作模式，区别正常寻路和路径可达判断
		public static var reach:Boolean;
		private static var age:int = 0;
		
		/**设置障碍数组**/
		public static function init(byte:ByteArray):void {
			if(pool == null){
				pool = new Vector.<Node>();
				openVector = new Vector.<Node>();
				openVector[0] = null;
				first = new Node;
				first.index = 0;
				first.value = int.MAX_VALUE;
				first.cost = 0;
				first.last = first;
			}
			byte.position = 0;
			var mapCol:int = Math.ceil(byte.readInt() / size);
			var mapRow:int = Math.ceil(byte.readInt() / size);
			map = new Vector.<Vector.<int>>(mapRow);
			raw = new Vector.<Vector.<int>>(mapRow);
			dataVector = new Vector.<Vector.<Node>>(mapRow);
			for(var j:int = 0;j < mapRow;j++){
				var rowVector:Vector.<int> = new Vector.<int>(mapCol);
				map[j] = rowVector;
				for(var i:int = 0;i < mapCol;i++){
					rowVector[i] = byte.readByte();
				}
				raw[j] = rowVector.concat();
				dataVector[j] = new Vector.<Node>(mapCol);
			}
			width = mapCol;
			height = mapRow;
		}
		/**搜索**/
		public static function find(x:Number,y:Number,tx:Number,ty:Number,near:int = 0):Array{
			reach = false;
			if(x<0 || y<0)return [];
			//障碍数组的行数和列数
			var mapCol:int = height;
			var mapRow:int = width;
			var node:Node,v:int,n:int,i:int;
			var data:Node; //当前正在搜索的节点
			var startX:int = int(x/size);
			var startY:int = int(y/size);
			//寻路目标
			var targetX:int = int(tx/size);
			var targetY:int = int(ty/size);
			if(targetX<0 ||targetX >= mapRow || targetY<0 || targetY >= mapCol){
				return [];
			}
			//如果起点是障碍，找最近点走出去，再向目标寻路
			var pos:Point;
			if(isBlock(startX, startY)){
				pos = getRoundStartNode(startX, startY);
				if(!pos){
					return [];
				}
				startX = pos.x;
				startY = pos.y;
				x = (startX + 0.5) * size;
				y = (startY + 0.5) * size;
			}
			/*
			if(isBlock(targetX,targetY)){
				pos = getRoundStartNode(targetX, targetY);
				if(pos == null)return [];
				targetX = pos.x;
				targetY = pos.y;
				tx = (targetX+0.5)*size;
				ty = (targetY+0.5)*size;
			}*/
			//根据距离估算寻路最大步数限制
			var diffX:int = tx-x,diffY:int = ty-y;
			var distance:int = Math.round(Math.sqrt(diffX*diffX+diffY*diffY));
			//距离为10000时，寻路步数为20000,按8000*6000地图对角线计算
			var targetBlock:Boolean = isBlock(targetX,targetY);
			var MAX:int;
			if(mode != FISH){
				MAX = ((targetBlock?0:350)+(targetBlock?50:500)*distance/10000)*2
			}else{
				MAX = 100+500*distance/10000;
				mode = NORMAL;
			}
			//测试直接行走能否通行
			reach = true;
			if(!targetBlock && !pathCheck(x,y,tx,ty)){
				if(near){
					if(distance < near)return [];
					else{
						var rate:Number = 1 - near/distance;
						return [Math.round(x+diffX*rate),Math.round(y+diffY*rate)];
					}
				}else{
					return [tx,ty];
				}
			}
			//封闭列表与开放列表
			var OPEN:Vector.<Node> = openVector;//开放列表
			var open:int = 1;//开放列表节点
			//var DATA:Object = {};//节点信息列表
			var DATA:Vector.<Vector.<Node>> = dataVector;//节点信息列表
			var nowX:int,nowY:int;
			var step:int = 1;//搜索的步数
			var poolIndex:int = poolLength;
			data = first;//起点
			data.x = startX;
			data.y = startY;
			data.close = false;
			DATA[startY][startX] = data;
			OPEN[open++] = data;
			age++;
			//A*主循环
			while(true){
				if(open == 1 || step > MAX){
					//目标不可达或者超过最大步数，选取离目标最近的点
					reach = false;
					if(mode == ARRIVE)return null;
					data = getNearNode(pool,poolIndex);
					if(data == null)return [];
					tx = (data.x+0.5)*size;
					ty = (data.y+0.5)*size;
					break;
				}
				//二叉堆取堆顶元素
				node = OPEN[--open];
				data = OPEN[1];
				//重新调整元素顺序
				n = 1;
				v = node.value;
				while(true){
					var n1:int = n*2;
					//左子节点索引超出范围
					if(n1 >= open){
						node.index = n;
						break;
					}
					var child:Node;
					//右子节点堆索引超出范围
					var n2:int = n1+1;
					if(n2 >= open){
						child = OPEN[n1];
					}else{
						child = OPEN[n1].value<OPEN[n2].value? OPEN[n1]: OPEN[n2];
					}

					if(child.value < v){
						OPEN[n] = child;
						node.index = child.index;
						child.index = n;
						n = node.index;
					}else{
						node.index = n;
						break;
					}
				}
				OPEN[n] = node;
				//if(node.index != n){
				//	trace("error:",node.index,n);
				//}
				//二叉堆取顶层元素
				
				data.close = true;
				nowX = data.x;
				nowY = data.y;
				if(nowX == targetX && nowY == targetY)break;
				var offsetX:int = data.x-data.last.x;
				var offsetY:int = data.y-data.last.y;
				//估值
				for(i = 0;i < 8;i++){
					var nx:int = nowX+dx[i];
					var ny:int = nowY+dy[i];
					if((ny < 0 || ny >= mapCol)||(nx < 0 || nx >= mapRow))continue;
					if(i < 4){
						//if(isBlock(nx,ny))continue;
						if((map[ny][nx]&B)==0)continue;
					}else{
						//斜方向需判定邻近两格是否可通行
						//if(isBlock(nx,ny) || isBlock(nowX,ny) || isBlock(nx,nowY))continue;
						if((map[ny][nx]&B)==0||(map[ny][nowX]&B)==0||(map[nowY][nx]&B)==0)continue;
					}

					//var id:int = mapRow*ny+nx;
					var cost:Number,value:Number;
					node = DATA[ny][nx];
					if(node != null && node.age==age && node.x==nx && node.y==ny){
						if(node.close)continue;
					}else{
						node = null;
					}
					//if(node != null && node.close)continue;
					cost = data.cost + (i<4?1:Math.SQRT2);
					var d1:int = nx-targetX;
					var d2:int = ny-targetY;
					value = cost+Math.sqrt((d1*d1+d2*d2))*2; //几何估值
					if(node != null){
						if(value >= node.value)continue;
						n = node.index;
					}else{
						node = poolIndex ? pool[--poolIndex] : pool[poolLength++] = new Node();
						node.x = nx;
						node.y = ny;
						node.age = age;
						node.close = false;
						DATA[ny][nx] = node;
						n = open++;
					}
					node.last = data;
					node.cost = cost;
					node.value = value;
					node.step = data.step+1;
					var parent:Node,np:int;
					v = node.value;
					while(n > 1){
						np = int(n/2);
						parent = OPEN[np];
						if(v >= parent.value)break;
						OPEN[n] = parent;
						parent.index = n;
						n = np;
					}
					OPEN[n] = node;
					node.index = n;
				}
				//估值				
				step = data.step;
			}
			//正常搜索至目标，路径加上终点
			if(mode == ARRIVE)return [];
			var roadLength:int = 0;
			node = data;
			while(node.last != node){
				road[roadLength++] = int((node.x+0.5)*size);
				road[roadLength++] = int((node.y+0.5)*size);
				node = node.last;
			}
			//tracePath(road);
			var end:int,endX:int,endY:int;
			if(near){
				//从终点逐个测试，找离接近目标near距离的点
				near = near*near;
				for(end = 0;end < roadLength;end+=2){
					diffX = road[end] - tx;
					diffY = road[end+1] - ty;
					if((diffX*diffX+diffY*diffY)>near)break;
				}
				//				if(end >= roadLength)return [];
				if(end > 0)end -= 2;
				endX = road[end];
				endY = road[end+1];
			}else{
				end = 0;
				endX = tx;
				endY = ty;
			}

			//路径平滑
			var path:Array = [];
			nowX = x;nowY = y;
			for(i = roadLength - 4;i >= end;i-=2){
				if(pathCheck(nowX,nowY,road[i],road[i+1])){
					nowX = road[i+2];
					nowY = road[i+3];
					path.push(nowX,nowY);
				}
			}
			path.push(endX,endY);
			return path;
		}
		public static  function pathCheck(x1:Number,y1:Number,x2:Number,y2:Number):Boolean{
			//var bottomBorder:int = map.length-1;
			//var rightBorder:int = map[0].length-1;
			//var Xborder:int,Yborder:int;
			var left:int,right:int,left_y:int,right_y:int;
			var order:Boolean;
			var dx:int,dy:int;
			if(((dx=x2-x1)>0?dx:-dx) > ((dy=y2-y1)>0?dy:-dy)){
				order = true;
				//Xborder = rightBorder;
				//Yborder = bottomBorder;
				if(x1 < x2){
					left = x1;
					right = x2;
					left_y = y1;
					right_y = y2;
				}else{
					left = x2;
					right = x1;
					left_y = y2;
					right_y = y1;
				}
			}else{
				order = false;
				//Xborder = bottomBorder;
				//Yborder = rightBorder;
				if(y1 < y2){
					left = y1;
					right = y2;
					left_y = x1;
					right_y = x2;
				}else{
					left = y2;
					right = y1;
					left_y = x2;
					right_y = x1;
				}
			}
			//防止除0错误
			if(right == left)return true;
			var rate:Number = (right_y - left_y)/(right - left);
			var LY:int = int(left_y/size);
			var len:int = int(right/size);
			var m1:Number = rate*size;
			var m2:Number = (size - left)*rate+left_y+0.5;
			for(var X:int = int(left/size);X < len;X++){
				/*
				var nx:int = (X+1)*size;
				var y:int = int((nx - left)*rate+left_y+0.5);
				var Y:int = int(y/size);*/
				var Y:int = int(X*m1+m2)/size;
				//边界保护判断
				//if(X > Xborder)X = Xborder;
				//if(Y > Yborder)Y = Yborder;
				/*
				if((order ? map[LY][X] : map[X][LY]) == 1)return true;
				if(Y != LY){
				if((order ? map[Y][X] : map[X][Y]) == 1)return true;
				LY = Y;
				}*/
				if(order ? isBlock(X,LY) : isBlock(LY,X))return true;
				if(Y != LY){
					if(order ? isBlock(X,Y) : isBlock(Y,X))return true;
					LY = Y;
				}
			}
			return false;
		}
		
		public static function isBlock(x:int,y:int):Boolean{
			if(x >= width || y >= height || x < 0 || y < 0)return false;
			return (map[y][x]&B) == 0;
		}
		public static function isFish(x:int,y:int):Boolean{
			var targetX:int = int(x/size);
			var targetY:int = int(y/size);
			if(targetX >= width || targetY >= height || targetX < 0 || targetY < 0)return false;
			return (map[targetY][targetX]&F) == 0;
		}
		public static function isAlpha(x:int,y:int):Number{
			var targetX:int = int(x/size);
			var targetY:int = int(y/size);
			if(targetX >= width || targetY >= height || targetX < 0 || targetY < 0)return 1;
			if(isBarrier(x, y))return 1;
			return(map[targetY][targetX]&0x2) == 0 ? 0.8 : 1;
		}
		public static function isBarrier(x:int,y:int):Boolean{
			var targetX:int = int(x/size);
			var targetY:int = int(y/size);
			if(targetX >= width || targetY >= height || targetX < 0 || targetY < 0)return true;
			return (map[targetY][targetX]&B) == 0;
		}
		public static function isSafeArea(x:int,y:int):Boolean{
			var targetX:int = int(x/size);      
			var targetY:int = int(y/size);
			if(targetX >= width || targetY >= height || targetX < 0 || targetY < 0)return false;
			return (map[targetY][targetX]&P) == P;
		}
		public static function canArrive(x:Number,y:Number,tx:Number,ty:Number):Boolean{
			mode = ARRIVE;
			var result:Boolean = find(x,y,tx,ty) == null?false:true;
			mode = NORMAL;
			return result;
		}
		//获取目标周围可通行格子
		private static var dir:Array = [-1,  0, 1, 0,
			0, -1, 0, 1];
		private static var targetPos:Point = new Point;
		public static const TYPE_PATH:int=0;
		public static const TYPE_BLOCK:int=1;
		public static const TYPE_MINING:int=2;
		private static function getRoundStartNode(col:int,row:int,type:int=TYPE_PATH):Point{
			var mask:int,res:int;
			if(type==TYPE_PATH){mask=B;res=1;}
			else if(type==TYPE_BLOCK){mask=B;res=0;}
			else if(type==TYPE_MINING){mask=F;res=0;}
			for (var i:int = 2; i < 512; i+=2){
				++row;
				++col;
				for (var j:int = 0; j < 4; ++j){
					for (var k:int = 0; k < i; ++k){
						if(col >= 0 && col < width && row >= 0 && row < height){
							if ((map[row][col]&mask) == res){
								targetPos.x = col;
								targetPos.y = row;
								return targetPos;
							}
						}
						row += dir[j];
						col += dir[j + 4];
					}
				}
			}
			return null;
		}
		public static function getRoundNode(x:int,y:int,type:int):Array{
			var targetX:int = int(x/size);
			var targetY:int = int(y/size);
			var pt:Point =getRoundStartNode(targetX,targetY,type);
			if(pt)return [int((pt.x+0.5)*size),int((pt.y+0.5)*size)];
			else return null;
		}
		
		//设置动态障碍
		public static function setDynamicBlock(x1:int,y1:int,x2:int,y2:int):void{
			var left:int = x1/size;
			var top:int = y1/size;
			var right:int = x2/size;
			var bottom:int = y2/size;
			if(right>=width)right=width-1;
			if(bottom>=height)bottom=height-1;
			for(var j:int = top;j <= bottom;j++){
				for(var i:int = left; i <= right;i++){
					map[j][i] &= ~B;
				}
			}
		}
		//撤销动态障碍，用备份的原始障碍数据恢复
		public static function resumeDynamicBlock(x1:int,y1:int,x2:int,y2:int):void{
			var left:int = x1/size;
			var top:int = y1/size;
			var right:int = x2/size;
			var bottom:int = y2/size;
			if(right>=width)right=width-1;
			if(bottom>=height)bottom=height-1;
			for(var j:int = top;j <= bottom;j++){
				for(var i:int = left; i <= right;i++){
					map[j][i] = raw[j][i];
				}
			}
		}
		
		public static function isStraight(x:Number,y:Number,tx:Number,ty:Number):Boolean {
			return !isBlock(tx,ty) && !pathCheck(x,y,tx,ty);
		}
		
		//获取离目标最近的可达点
		private static function getNearNode(POOL:Vector.<Node>,poolIndex:int):Node{
			var min:int = int.MAX_VALUE;
			var near:Node = null;
			for(var i:int = poolLength-1;i>=poolIndex;i--){
				var data:Node = POOL[i];
				if(data.value - data.cost < min){
					min = data.value - data.cost;
					near = data;
				}
			}
			return near;
		}
		/*
		//检测目标是否可达
		private static function checkReach(targetX:int,targetY:int):Point{
		var pos:Point;
		if(!isBlock(targetX,targetY)){
		pos = checkIsolated2(targetX,targetY);
		if(pos)return pos;
		}
		
		pos = getRoundReachNode(targetX, targetY,maxDistance);
		var radius:int = 1;
		while(true){
		if(pos == null)return null;
		if(!checkIsolated(pos.x,pos.y,targetX,targetY))return pos;
		radius = maxDistance+1;
		pos = getRoundReachNode(targetX, targetY,radius);
		if(pos == null)return null
		radius = maxDistance+1;
		}
		return targetPos;
		}
		//独立区域检测
		private static function checkIsolated2(targetX:int,targetY:int):Point{
		targetPos.x = targetX;
		targetPos.y = targetY;
		for(var count:int = 1;count < 20;count++){
		if((map[targetX+count][targetY]&B)==0){
		if(checkIsolated(targetX+count-1,targetY,targetX,targetY)){
		return null;
		}
		return targetPos;
		}
		}
		return targetPos;
		}
		private static var maxDistance:int;
		private static function checkIsolated(gx:int,gy:int,cx:int,cy:int):Boolean{
		maxDistance = 0;
		var count:int = 0;
		var mapCol:int = height;
		var mapRow:int = width;
		var lastDirect:int = 0;
		var lx:int = gx;
		var ly:int = gy;
		var i:int;
		for(i=0;i<4;i++){
		if((map[gx+dx[i]][gy+dy[i]]&B)==0){
		lastDirect = i;
		break;
		}
		}
		while(count < 20){
		for(i = 1;i <= 4;i++){
		var nextDirect:int = (lastDirect+i)%4;
		var nx:int = lx+dx[nextDirect];
		var ny:int = ly+dy[nextDirect];
		if((ny < 0 || ny >= mapCol)||(nx < 0 || nx >= mapRow))continue;
		if((map[ny][nx]&B)==0)continue;
		if(nx == gx && ny == gy)return true;
		lx = nx;
		ly = ny;
		lastDirect = nextDirect;
		
		var dx:int = nx-cx;
		if(dx<0)dx=-dx;
		var dy:int = ny-cy;
		if(dy<0)dy=-dy;
		if(maxDistance<dx)maxDistance = dx;
		if(maxDistance<dy)maxDistance = dy;
		}
		count++;
		}
		return false;
		}
		private static function getRoundReachNode(col:int,row:int,radius:int=1):Point{
		row = row+radius-1;
		col = col+radius-1;
		for (var i:int = radius*2; i < 512; i+=2){
		++row;
		++col;
		for (var j:int = 0; j < 4; ++j){
		for (var k:int = 0; k < i; ++k){
		if(col >= 0 && col < width && row >= 0 && row < height){
		if ((map[row][col]&B) == 1){
		targetPos.x = col;
		targetPos.y = row;
		maxDistance = col;
		if(maxDistance < row)maxDistance = row;
		return targetPos;
		}
		}
		row += dir[j];
		col += dir[j + 4];
		}
		}
		}
		return null;
		}*/
		/*
		private static function traceFind(DATA:Object):void{
		var arr:Array = [];
		for(var j:int = 0;j < height;j++){
		var row:Array = [];
		arr[j] = row;
		for(var i:int = 0;i < width;i++){
		var symbol:String;
		if(isBlock(i,j)){
		symbol = "O";
		}else{
		symbol = " ";
		}
		row[i] = symbol;
		}
		}
		for each(var node:Node in DATA){
		arr[node.y][node.x] = "*";
		}
		
		for(j = 0;j < height;j++){
		trace((arr[j] as Array).join(""));
		}
		}
		private static function tracePath(path:Array):void{
		var arr:Array = [];
		for(var j:int = 0;j < height;j++){
		var row:Array = [];
		arr[j] = row;
		for(var i:int = 0;i < width;i++){
		var symbol:String;
		if(isBlock(i,j)){
		symbol = "O";
		}else{
		symbol = " ";
		}
		row[i] = symbol;
		}
		}
		for(i = 0;i < path.length;i+=2){
		arr[path[i+1]][path[i]] = "*";
		}
		for(j = 0;j < height;j++){
		trace((arr[j] as Array).join(""));
		}
		}*/
	}
}
final class Node{
	public var index:int;
	public var cost:Number;
	public var value:Number;
	public var last:Node;
	public var x:int;
	public var y:int;
	public var close:Boolean;
	public var step:int;
	public var age:int;
}