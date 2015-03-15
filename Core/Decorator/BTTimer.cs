using UnityEngine;
using System.Collections;

namespace BT {

	/// <summary>
	/// BTTimer is a child node that ticks the child with interval.
	/// During interval, it returns running.
	/// If ticking the child, it returns what the child returns.
	/// </summary>
	public class BTTimer : BTDecorator {

		private float _timer;

		public float interval {get; set;}


		public BTTimer (float interval, BTNode child = null) : base (child) {
			this.interval = interval;
		}

		public override BTResult Tick () {
			_timer += Time.deltaTime;

			if (_timer >= interval) {
				_timer = 0;
				BTResult result = child.Tick();
				return result;
			}
			else {
				return BTResult.Running;
			}
		}

		public override void Clear () {
			base.Clear ();
			_timer = 0;
		}
	}

}