using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using BT;

public class BTTreeWindow: EditorWindow {

	private GameObject _go;

	private BTTree _previousTree;
	private BTNodeInfo _info;
	private Vector2 _size = new Vector2(200, 50);
	private Vector2 _offset = new Vector2(250, 100);
	private int _currentWindowId;
	private Dictionary<int, int> _levelToCount;

	private Vector2 _scrollPosition = Vector2.zero;

	
	[MenuItem("Window/BTTree")]
	static void ShowEditor() {
		EditorWindow.GetWindow<BTTreeWindow>();
	}

	void OnInspectorUpdate () {
		Repaint();
	}

	void OnEnable () {
		_previousTree = null;
	}

	void OnGUI() {
		_go = (GameObject) EditorGUI.ObjectField(new Rect(position.xMax - 350, position.yMin-40, 300, 20), "BTTree GameObject", _go, typeof(GameObject), true);

		BTTree tree;
		if (_go == null || (tree = _go.GetComponent<BTTree>()) == null) {
			EditorGUI.LabelField(new Rect(position.xMax - 350, position.y-17, 500, 20), "Find a GameObject with component inherited from BTTree.");
			return ;
		}

		if (_previousTree == null || _previousTree.GetType() != tree.GetType()) {
			BTNode root = tree.root == null ? tree.Init() : tree.root;

			_levelToCount = new Dictionary<int, int>();
			_info = ParseNodeInfo(root, 0, 0);

			_previousTree = tree;
		}

		int maxCount = 0;
		foreach (int count in _levelToCount.Values) {
			if (count > maxCount) {
				maxCount = count;
			}
		}

		_scrollPosition = GUI.BeginScrollView(new Rect(0, 0, position.width - 1, position.height - 1), 
		                                      _scrollPosition,
		                                      new Rect(0, 0, maxCount * _offset.x + 50, _levelToCount.Keys.Count * _offset.y + 50));

		BeginWindows();

		_currentWindowId = 0;
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
		float selfX = info.indexInParent * _offset.x;
		float parentX = 0;

		if (info.indexInParent > 0) {
			selfX += (parentInfo.childrenInfo[info.indexInParent - 1].maxNodeSize - 1) * _offset.x;
		}
		if (parentInfo != null) {
			parentX = parentInfo.positionX;
		}
		else {
			selfX += 50;
		}
		 
		Rect rect = new Rect(selfX + parentX,
		                     info.level * _offset.y,
		                     _size.x, 
		                     _size.y);

		info.positionX = selfX + parentX;

		foreach (BTNodeInfo childInfo in info.childrenInfo) {
			Color color = Color.white;
			if (childInfo.node.isRunning) {
				color = Color.green;
			}
			Rect childRect = DrawNodeInfo(childInfo, info);
			DrawPolygonLine(rect, childRect, color);
		}

		string name = info.node.name != null ? info.node.name : info.node.GetType().ToString();
		string[] nameParts = name.Split('.');

		GUI.Window(_currentWindowId++, rect, DoWindow, nameParts[nameParts.Length-1]);

		return rect;
	}
	

	private static void DrawPolygonLine (Rect rect1, Rect rect2, Color color) {
		float midY = Mathf.Min(rect1.center.y, rect2.center.y) + Mathf.Abs(rect1.center.y - rect2.center.y) / 2;
		Vector3 rect1Point = new Vector3(rect1.center.x, rect1.center.y);
		Vector3 midRect1Point = new Vector3(rect1.center.x, midY);
		Vector3 midRect2Point = new Vector3(rect2.center.x, midY);
		Vector3 rect2Point = new Vector3(rect2.center.x, rect2.center.y);

		Handles.color = color;

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