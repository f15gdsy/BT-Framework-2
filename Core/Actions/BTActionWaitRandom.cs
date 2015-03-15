using UnityEngine;
using System.Collections;

namespace BT {

	/// <summary>
	/// BTActionWaitRandom is almost the same as BTActionWait, 
	/// except every time it is about to execute, the waiting time is randomly generated in a specified range.
	/// </summary>
	public class BTActionWaitRandom : BTAction {
		private float _startTime;
		private float _interval;

		public float secondsMin {get; set;}
		public float secondsMax {get; set;}

		public BTActionWaitRandom (float secondsMin, float secondsMax) {
			this.secondsMin = secondsMin;
			this.secondsMax = secondsMax;
		}

		protected override void Enter () {
			base.Enter ();

			_startTime = Time.time;
			_interval = Random.Range(secondsMin, secondsMax);
		}

		protected override BTResult Execute () {
			if (Time.time - _startTime >= _interval) {
				return BTResult.Success;
			}
			return BTResult.Running;
		}
	}

}