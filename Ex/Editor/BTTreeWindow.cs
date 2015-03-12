using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using BT;

public class BTTreeWindow: EditorWindow {

	BTTree tree;
	BTNode _root;
	private BTNodeInfo _info;
	public Vector2 size = new Vector2(100, 100);
	public Vector2 offset = new Vector2(150, 150);
	private int _currentWindowId;
	private Dictionary<int, int> _levelToCount;

	private Vector2 _scrollPosition = Vector2.zero;

	
	[MenuItem("Window/BTTree")]
	static void ShowEditor() {
		BTTreeWindow editor = EditorWindow.GetWindow<BTTreeWindow>();
		editor.Init();
	}
	
	public void Init() {
		tree = GameObject.FindObjectOfType<TestBTWindow>();
		if (_root == null) {
			Debug.Log("Init");
			_root = tree.Init();
			_levelToCount = new Dictionary<int, int>();
			_info = ParseNodeInfo(_root, 0, 0);
		}
	}
	
	void OnGUI() {
		_currentWindowId = 0;

		int maxCount = 0;
		foreach (int count in _levelToCount.Values) {
			if (count > maxCount) {
				maxCount = count;
			}
		}

		EditorWindow editor = EditorWindow.GetWindow<BTTreeWindow>();
		_scrollPosition = GUI.BeginScrollView(new Rect(0, 0, editor.position.width - 1, editor.position.height - 1), 
		                                      _scrollPosition,
		                                      new Rect(0, 0, maxCount * offset.x + 50, _levelToCount.Keys.Count * offset.y + 50));

		BeginWindows();

		DrawNodeInfo(_info, null);

		EndWindows();

		GUI.EndScrollView();
	}

	private BTNodeInfo ParseNodeInfo (BTNode node, int level, int indexInParent) {
		int maxNodeSize = 1;
		List<BTNodeInfo> infos = new List<BTNodeInfo>();

		if (node is BTSimpleParallel) {		// simple parallel has a primary child
			BTSimpleParallel simpleParallel = (BTSimpleParallel) node;
			List<BTNode> children = simpleParallel.children;
			children.Insert(0, simpleParallel.primaryChild);

			if (children.Count > 0) {
				maxNodeSize = 0;
			}
			
			int i=0;
			foreach (BTNode child in children) {
				BTNodeInfo info = ParseNodeInfo(child, level+1, i++);
				maxNodeSize += info.maxNodeSize;
				infos.Add(info);
			}
		}
		else if (node is BTComposite) {
			BTComposite composite = (BTComposite) node;
			List<BTNode> children = composite.children;

			if (children.Count > 0) {
				maxNodeSize = 0;
			}

			int i=0;
			foreach (BTNode child in children) {
				BTNodeInfo info = ParseNodeInfo(child, level+1, i++);
				maxNodeSize += info.maxNodeSize;
				infos.Add(info);
			}
		}
		else if (node is BTDecorator) {
			BTDecorator decorator = (BTDecorator) node;

			if (decorator.child != null) {
				BTNodeInfo info = ParseNodeInfo(decorator.child, level+1, 0);
				if (info.maxNodeSize > maxNodeSize) {
					maxNodeSize = info.maxNodeSize;
				}
				infos.Add(info);
			}
		}

		int countInLevel;
		_levelToCount.TryGetValue(level, out countInLevel);

		_levelToCount[level] = countInLevel + maxNodeSize;

		return new BTNodeInfo(node, infos, maxNodeSize, level, countInLevel, indexInParent);
	}

	private Rect DrawNodeInfo (BTNodeInfo info, BTNodeInfo parentInfo) {
		float selfX = info.indexInParent * offset.x;
		float parentX = 0;

		if (info.indexInParent > 0) {
			selfX += (parentInfo.childrenInfo[info.indexInParent - 1].maxNodeSize - 1) * offset.x;
		}
		if (parentInfo != null) {
			parentX = parentInfo.positionX;
		}
		else {
			selfX += 50;
		}
		 
		Rect rect = new Rect(selfX + parentX,
		                     info.level * offset.y,
		                     size.x, 
		                     size.y);

		info.positionX = selfX + parentX;

		foreach (BTNodeInfo childInfo in info.childrenInfo) {
			Rect childRect = DrawNodeInfo(childInfo, info);
			DrawPolygonLine(rect, childRect);
		}

		GUI.Window(_currentWindowId++, rect, DoWindow, info.node.GetType().ToString());

		return rect;
	}
	

	private static void DrawPolygonLine (Rect rect1, Rect rect2) {
		float midY = Mathf.Min(rect1.center.y, rect2.center.y) + Mathf.Abs(rect1.center.y - rect2.center.y) / 2;
		Vector3 rect1Point = new Vector3(rect1.center.x, rect1.center.y);
		Vector3 midRect1Point = new Vector3(rect1.center.x, midY);
		Vector3 midRect2Point = new Vector3(rect2.center.x, midY);
		Vector3 rect2Point = new Vector3(rect2.center.x, rect2.center.y);

		Handles.DrawPolyLine(new Vector3[] {
			rect1Point, 
			midRect1Point,
			midRect2Point,
			rect2Point
		});
	}

	private void DoWindow (int id) {
				
	}


	public class BTNodeInfo {
		public BTNode node;
		public List<BTNodeInfo> childrenInfo;
		public int maxNodeSize;
		public int level;
		public int countInLevel;
		public float positionX;
		public int indexInParent;

		public BTNodeInfo (BTNode node, List<BTNodeInfo> childrenInfo, int maxNodeSize, int level, int countInLevel, int indexInParent) {
			this.node = node;
			this.childrenInfo = childrenInfo;
			this.maxNodeSize = maxNodeSize;
			this.level = level;
			this.countInLevel = countInLevel;
			this.indexInParent = indexInParent;
		}
	}

	
	private enum NodeType {
		Invalid, 
		
		Sequence,
		Selector,
		SimpleParallel,
		
		Inverter,
		Repeater,
		ConditionalEvaluator,
		Timer,
		
		Conditional,
		
		Action,
	}
}