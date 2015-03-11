using UnityEngine;
using System.Collections;

namespace BT {

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