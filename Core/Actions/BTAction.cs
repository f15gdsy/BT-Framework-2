using UnityEngine;
using System.Collections;

namespace BT {

	public class BTAction : BTNode {

		private BTActionStatus _status = BTActionStatus.Ready;


		sealed public override BTResult Tick () {
			BTResult tickResult = BTResult.Success;

			if (_status == BTActionStatus.Ready) {
				Enter();
				_status = BTActionStatus.Running;
			}
			if (_status == BTActionStatus.Running) {
				tickResult = Execute();
				if (tickResult != BTResult.Running) {
					Exit();
					_status = BTActionStatus.Ready;
					isRunning = false;
				}
				else {
					isRunning = true;
				}
			}
			return tickResult;
		}

		public override void Clear () {
			base.Clear();

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