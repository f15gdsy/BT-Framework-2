using UnityEngine;
using System.Collections;

namespace BT {

	public class BTSequence : BTComposite {

		private int _activeChildIndex;


		public int activeChildIndex {get {return _activeChildIndex;}}


		public override BTResult Tick () {
			return TickFromActiveChild();
		}

		public override void Clear () {
			switch (clearOpt) {
			case BTClearOpt.Default:
				if (_activeChildIndex != -1) {
					children[_activeChildIndex].Clear();
				}
				break;

			case BTClearOpt.Selected:
				foreach (BTNode child in selectedChildrenForClear) {
					int index = children.IndexOf(child);
					// greater than active child index means they are not cleared yet
					// also if _activeChildIndex is -1, then all selected children will be cleared
					if (index >= _activeChildIndex) {	
						child.Clear();
					}
				}
				break;

			case BTClearOpt.DefaultAndSelected:
				if (_activeChildIndex != -1) {
					BTNode activeChild = children[_activeChildIndex];
					if (!selectedChildrenForClear.Contains(activeChild)) {
						activeChild.Clear();
					}
				}
				foreach (BTNode child in selectedChildrenForClear) {
					int index = children.IndexOf(child);
					if (index >= _activeChildIndex) {
						child.Clear();
					}
				}
				break;

			case BTClearOpt.All:
				if (_activeChildIndex > -1) {
					for (int i=_activeChildIndex; i<children.Count; i++) {
						if (i < 0) continue;
						children[i].Clear();
					}
				}
				break;
			}

			_activeChildIndex = -1;
		}

		private BTResult TickFromActiveChild () {
			if (_activeChildIndex == -1) {
				_activeChildIndex = 0;
			}

			for (; _activeChildIndex<children.Count; _activeChildIndex++) {
				BTNode activeChild = children[_activeChildIndex];
				
				switch (activeChild.Tick()) {
				case BTResult.Running:
					return BTResult.Running;
				case BTResult.Success:
					activeChild.Clear();
					continue;
				case BTResult.Failed:
					activeChild.Clear();
					_activeChildIndex = -1;
					return BTResult.Failed;
				}
			}

			_activeChildIndex = -1;
			return BTResult.Success;
		}

	}

}