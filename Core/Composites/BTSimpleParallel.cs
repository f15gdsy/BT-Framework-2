using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BT {

	/// <summary>
	/// BTSimpleParallel is a composite node that has a primary child and other children (background).
	/// 
	/// If primary child returns running, the background children nodes will run, and it returns running.
	/// If primary child returns failure or success, the background children will not run and it returns failure or success accordingly.
	/// 
	/// Default clear option is to clear the primary child & any running background children.
	/// </summary>
	public class BTSimpleParallel : BTComposite {

		private BTNode _primaryChild;
		private List<BTNode> _runningChildren;
		private bool _shouldClearPrimaryChild = true;

		public BTNode primaryChild {get {return _primaryChild;}}


		public void SetPrimaryChild (BTNode node, bool selectForClear = false) {
			if (_primaryChild != null) {
				selectedChildrenForClear.Remove(_primaryChild);
			}

			_primaryChild = node;
			if (selectForClear) {
				selectedChildrenForClear.Add(_primaryChild);
			}
		}

		public override void Activate (BTDatabase database) {
			base.Activate (database);

			_primaryChild.Activate(database);
			ResetRuningChildren();
		}

		public override BTResult Tick () {
			if (_primaryChild == null) {
				Debug.LogError("Primary Child not set!");
			}

			BTResult primaryChildResult = _primaryChild.Tick();

			if (primaryChildResult == BTResult.Running) {
				RunBackground();
				isRunning = true;
				return BTResult.Running;
			}
			else {
				_shouldClearPrimaryChild = false;
				_primaryChild.Clear();
				isRunning = false;
				return primaryChildResult;
			}
		}

		public override void Clear () {
			base.Clear ();

			switch (clearOpt) {
			case BTClearOpt.Default:
			case BTClearOpt.DefaultAndSelected:
			case BTClearOpt.All:
				if (_shouldClearPrimaryChild) {
					_primaryChild.Clear();
				}
				foreach (BTNode child in _runningChildren) {
					child.Clear();
				}
				break;

			case BTClearOpt.Selected:
				foreach (BTNode child in selectedChildrenForClear) {
					if ((_shouldClearPrimaryChild && child == _primaryChild) || _runningChildren.Contains(child)) {
						child.Clear();
					}
				}
				break;
			}

			_shouldClearPrimaryChild = true;
			ResetRuningChildren();
		}

		private void ResetRuningChildren () {
			_runningChildren = new List<BTNode>();
			foreach (BTNode child in children) {
				_runningChildren.Add(child);
			}
		}

		private void RunBackground () {
			for (int i=_runningChildren.Count-1; i>=0; i--) {
				BTNode child = _runningChildren[i];
				BTResult result = child.Tick();

				if (result != BTResult.Running) {
					child.Clear();
					_runningChildren.RemoveAt(i);
				}
			}
		}


//		private FinishOpt _finishOpt;

//		public enum FinishOpt {
//			AbortBackground,
//			WaitForBackground,
//		}
	}

}