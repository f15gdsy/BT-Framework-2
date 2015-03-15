using UnityEngine;
using System.Collections;

namespace BT {

	/// <summary>
	/// BTActionWait is an action node that waits for a specified time.
	/// It returns running during the wait, and returns success after that.
	/// </summary>
	public class BTActionWait : BTAction {
		private float _startTime;

		public float seconds {get; set;}


		public BTActionWait (float seconds) {
			this.seconds = seconds;
		}

		protected override void Enter () {
			base.Enter ();

			_startTime = Time.time;
		}

		protected override BTResult Execute () {
			if (Time.time - _startTime >= seconds) {
				return BTResult.Success;
			}
			return BTResult.Running;
		}
	}

}