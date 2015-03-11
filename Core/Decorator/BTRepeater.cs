using UnityEngine;
using System.Collections;

namespace BT {

	public class BTRepeater : BTDecorator {
		public int count {get; set;}
		public bool repeatForever {get; set;}
		public bool endOnFailure {get; set;}

		private int _currentCount;


		private BTRepeater (int count, bool repeatForever, bool endOnFailure, BTNode child = null) : base (child) {
			this.count = count;
			this.repeatForever = repeatForever;
			this.endOnFailure = endOnFailure;
		}

		public BTRepeater (int count, bool endOnFailure, BTNode child = null) : this (count, false, endOnFailure, child) {}
		public BTRepeater (bool endOnFailure, BTNode child = null) : this (0, true, endOnFailure, child) {}

		public override BTResult Tick () {
			if (repeatForever) {
				BTResult result = child.Tick();

				if (endOnFailure && result == BTResult.Failed) {
					return BTResult.Failed;
				}
				return BTResult.Running;
			}
			else if (_currentCount < count) {
				BTResult result = child.Tick();
				_currentCount++;

				if (_currentCount >= count) {
					if (result == BTResult.Running) {
						result = BTResult.Success;
					}
					return result;
				}

				if (endOnFailure && result == BTResult.Failed) {
					return BTResult.Failed;
				}
				return BTResult.Running;
			}

			return BTResult.Failed;
		}

		public override void Clear () {
			base.Clear ();

			_currentCount = 0;
		}
	}

}