package game.core.utils {
	import flash.utils.ByteArray;

	public class Dijkstra {
		
		public static var isOpen:Boolean = false;
		private static var g:Graph;
		public static function init(byte:ByteArray=null):void {
			isOpen = false;
			if(byte.position >= byte.length) return;
			if(!g) g = new 	Graph();
			else g.clear();
			var len:int = byte.readShort();
			if(len>0) isOpen = true;
			for(var i:int=0;i<len;i++) g.add(byte.readShort(),byte.readShort(),byte.readShort(),byte.readShort());
		}
		
		public static var SIZE:int = 25;
		public static function find(x:Number,y:Number,tx:Number,ty:Number,near:int = 0):Array {
			
			if(Astar.isStraight(x,y,tx,ty)) {
				Astar.reach = true;				
				return [tx,ty];
			}
			
			var sx1:int = int(x/SIZE);
			var sy1:int = int(y/SIZE);
			var sx2:int = int(tx/SIZE);
			var sy2:int = int(ty/SIZE);
			
			var startVertex:Vertex = g.getStartNearVertex(sx1,sy1,sx2,sy2);
			var endVertex:Vertex = g.getStartNearVertex(sx2,sy2,sx1,sy1);
			if(!startVertex || !endVertex) return Astar.find(x,y,tx,ty,near);
			
			var apath:Array;
			if((startVertex.x == 0 && startVertex.y == 0) || (endVertex.x == 0 && endVertex.y == 0) || (startVertex.x == endVertex.x && startVertex.y == endVertex.y)) {
				return Astar.find(x,y,tx,ty,near);
			}else {
				apath = [];
				var path:Array = g.find(startVertex.x,startVertex.y,endVertex.x,endVertex.y);
				path = g.pathFix(x,y,tx,ty,path);
				if(path.length == 0) return Astar.find(x,y,tx,ty,near);
				for(var i:int=0;i<path.length;i=i+2) {
					if(i==0) apath = apath.concat(Astar.find(x,y,path[i],path[i+1]));
					else apath = apath.concat(Astar.find(apath[apath.length-2],apath[apath.length-1],path[i],path[i+1]));
				}
				apath = apath.concat(Astar.find(apath[apath.length-2],apath[apath.length-1],tx,ty,near));
			}
			return apath;
		}
		
	}
}
import flash.utils.Dictionary;

import game.core.utils.Astar;
import game.core.utils.Dijkstra;
import game.core.utils.Pool;
import game.core.utils.Utils;

class Graph {
	
	private var vertexs:Dictionary = new Dictionary();
	private var edges:Dictionary = new Dictionary();
	
	private var distance:Dictionary = new Dictionary();
	private var nodes:Vector.<Vertex> = new Vector.<Vertex>;
	private var tmpNodes:Vector.<Vertex> = new Vector.<Vertex>;
	private var previous:Dictionary = new Dictionary();
	
	public function find(startX:Number,startY:Number,endX:Number,endY:Number):Array {
		
		nodes.length = 0;
		
		var v:Vertex;
		var smallest:Vertex;
		var n:Vertex;
		
		var startID:String = Vertex.getID(startX,startY);
		var endID:String = Vertex.getID(endX,endY);
		
		var path:Array;
		
		for each(v in vertexs) {
			n = v.clone();
			nodes.push(n);
			tmpNodes.push(n);
			previous[n.id] = null;
			if(n.id == startID) {
				distance[n.id] = 0;
				n.cost = 0;				
			}else {
				distance[n.id] = Number.MAX_VALUE; 
				n.cost = Number.MAX_VALUE;				
			}
		}
		
		while(nodes.length) {
			smallest = getMinCostVertex();
			if(!smallest || smallest.id == endID) {
				path = [];
				if(smallest) {
					while(previous[smallest.id] != null) {
						path.push(smallest.y * Dijkstra.SIZE,smallest.x * Dijkstra.SIZE);
						smallest = previous[smallest.id];
					}
					path.push(startY * Dijkstra.SIZE,startX * Dijkstra.SIZE);
					path.reverse();
				}
				clearTmpNodes();
				return path;
			}
			
			if(distance[smallest.id] == Number.MAX_VALUE) {
				break;
			}
			
			if(edges[smallest.id]) {
				for each(n in edges[smallest.id]) {
					var cost:int = distance[smallest.id] + n.cost;
					if(cost < distance[n.id]) {
						
						distance[n.id] = cost;
						previous[n.id] = smallest;
						
						for each(v in nodes) {
							if(v.id == n.id) {
								v.cost = cost;
								break;
							}
						}
						
					}
				}
			}
		}
		clearTmpNodes();
		return [];
	}
	
	public function add(x:int,y:int,tx:int,ty:int):void {
		var cost:int = Utils.dist(x,y,tx,ty);
		var id1:String = Vertex.getID(x,y);
		var id2:String = Vertex.getID(tx,ty);
		addEdge(id1,tx,ty,cost);
		addEdge(id2,x,y,cost);
		addVertex(id1,x,y);
		addVertex(id2,tx,ty);
	}
	
	public function pathFix(x:Number,y:Number,tx:Number,ty:Number,path:Array):Array {
		var i:int=0;
		var maxIndex:int = -1;
		var minIndex:int = -1;
		var maxCnt:int = 2;
		var minCnt:int = 2;
		var len:int = path.length;
		for(i=0;i<path.length;i+=2) {
			if(maxCnt > 0 && Astar.isStraight(x,y,path[i],path[i+1])) {
				maxCnt--;				
				maxIndex = i;
			}
			if((minIndex == -1 || (len - minIndex)/2  > minCnt) && Astar.isStraight(tx,ty,path[i],path[i+1])) {
				minIndex = i; 
			}
		}
		if(maxIndex!=-1) path = path.slice(maxIndex);
		if(minIndex!=-1) path.splice(minIndex-maxIndex,path.length-minIndex+maxIndex);
		return path;
	}
	
	private static var nearVertexs:Array = [];
	public function getStartNearVertex(x:int,y:int,tx:int,ty:int):Vertex {
		nearVertexs.length = 0;
		var minLen1:Number = Number.MAX_VALUE;
		var minLen2:Number = Number.MAX_VALUE;
		var tmpMinLen:Number;
		var minKey1:String;
		var minKey2:String;
		var tmpVertex:Vertex;
		var key:String;
		var id:String = Vertex.getID(x,y);
		var tid:String = Vertex.getID(tx,ty);
		for(key in vertexs) {
			tmpVertex = vertexs[key];
			if(tmpVertex.id == id || tmpVertex.id == tid || !Astar.isStraight(Dijkstra.SIZE * x,Dijkstra.SIZE * y,Dijkstra.SIZE * tmpVertex.x,Dijkstra.SIZE * tmpVertex.y)) continue;
			tmpMinLen = Utils.dist(x,y,tmpVertex.x,tmpVertex.y);
			if(tmpMinLen < minLen1) {
				minLen2 = minLen1;
				minLen1 = tmpMinLen;
				minKey2 = minKey1;				
				minKey1 = key;
			}else if(tmpMinLen < minLen2) {
				minLen2 = tmpMinLen;
				minKey2 = key;
			}
		}
		
		var vertex1:Vertex = vertexs[minKey1];
		var vertex2:Vertex = vertexs[minKey2];
		if(vertex1) nearVertexs.push(vertex1);
		if(vertex2) nearVertexs.push(vertex2);
		
		var i:int;
		var minIndex:int = -1;
		minLen1 = Number.MAX_VALUE;
		for(i=0;i<nearVertexs.length;i++) {
			tmpVertex = nearVertexs[i];
			tmpMinLen = Utils.dist(tmpVertex.x,tmpVertex.y,tx,ty);
			if(tmpMinLen < minLen1) {
				minLen1 = tmpMinLen;
				minIndex = i;
			}
		}
		return nearVertexs[minIndex];
	}
	
	private function clearTmpNodes():void {
		while(tmpNodes.length) tmpNodes.pop().dispose();
	}
	
	private function getMinCostVertex():Vertex {
		var minCost:Number = Number.MAX_VALUE;
		var minIndex:int = -1;
		var i:int=0;
		var tmpVertex:Vertex;
		for(i=0;i<nodes.length;i++) {
			tmpVertex = nodes[i];
			if(tmpVertex.cost < minCost) {
				minCost = tmpVertex.cost;
				minIndex = i;
			}
		}
		if(minIndex != -1)return nodes.splice(minIndex,1)[0];			
		return null;
	}
	
	private function addEdge(id:String,x:int,y:int,cost:Number):void {
		if(!edges[id]) edges[id] = new Vector.<Vertex>;
		var v:Vertex;
		for each(v in edges[id]) if(v.id == id) return;
		v = Vertex.instance;
		v.x = x;
		v.y = y;
		v.cost = cost;
		edges[id].push(v);
	}
	
	private function addVertex(id:String,x:uint,y:uint):void {
		if(vertexs[id]) return;
		var v:Vertex = Vertex.instance;
		v.x = x;
		v.y = y;
		vertexs[id] = v;
	}
	
	public function clear():void {
		
		var key:String;
		var array:Vector.<Vertex>;
		var v:Vertex;
		
		for(key in vertexs) {
			v = vertexs[key];
			v.dispose();
			delete vertexs[key];
		}
		
		for(key in edges) {
			array = edges[key];
			for each(v in array) v.dispose();
			array.length = 0;
		}
		
	}
	
}

class Vertex {
	
	private static var pool:Pool = new Pool(Vertex);
	public static function get instance():Vertex {
		return pool.pop();
	}
	
	public static function getID(x:uint,y:uint):String {
		return x+"-"+y;
	}
	
	public function get id():String {
		return getID(x,y);
	}
	
	private var _x:Number;
	public function set x(value:Number):void {
		_x = value;
	}
	
	public function get x():Number {
		return _x;
	}
	
	private var _y:Number;
	public function set y(value:Number):void {
		_y = value;
	}
	
	public function get y():Number {
		return _y;
	}
	
	public function get sx():Number {
		return _x * Dijkstra.SIZE;
	}
	
	public function get sy():Number {
		return _y * Dijkstra.SIZE;
	}
	
	private var _cost:Number;
	public function set cost(value:Number):void {
		_cost = value;
	}
	
	public function get cost():Number {
		return _cost;
	}
	
	public function clone():Vertex {
		var v:Vertex = Vertex.instance;
		v.x = x;
		v.y = y;
		v.cost = cost;
		return v;
	}
	
	public function dispose():void {
		x = 0;
		y = 0;
		_cost = 0;
		pool.push(this);
	}
}
