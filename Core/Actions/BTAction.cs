using UnityEngine;
using System.Collections;

namespace BT {

	/// <summary>
	/// BTAction is the base class for action nodes.
	/// It is where the actual gameplay logic happens.
	/// </summary>
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

		/// <summary>
		/// Called when the action node is about to execute.
		/// </summary>
		protected virtual void Enter () {}

		/// <summary>
		/// Called every frame if the action node is active.
		/// </summary>
		protected virtual BTResult Execute () {return BTResult.Failed;}

		/// <summary>
		/// Called when the action node finishes.
		/// </summary>
		protected virtual void Exit () {}



		private enum BTActionStatus {
			Ready,
			Running,
		}
	}
}