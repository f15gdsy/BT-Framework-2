using UnityEngine;
using System.Collections;

namespace BT {

	public class BTAction : BTNode {

		private BTActionStatus _status = BTActionStatus.Ready;


		sealed public override BTResult Tick () {
			BTResult result = BTResult.Success;

			if (_status == BTActionStatus.Ready) {
				Enter();
				_status = BTActionStatus.Running;
			}
			if (_status == BTActionStatus.Running) {
				result = Execute();
				if (result != BTResult.Running) {
					Exit();
					_status = BTActionStatus.Ready;
				}
			}
			return result;
		}

		public override void Clear () {
			if (_status != BTActionStatus.Ready) {	// not cleared yet
				Exit();
				_status = BTActionStatus.Ready;
			}
		}

		protected virtual void Enter () {}
		protected virtual BTResult Execute () {return BTResult.Failed;}
		protected virtual void Exit () {}



		private enum BTActionStatus {
			Ready,
			Running,
		}
	}
}