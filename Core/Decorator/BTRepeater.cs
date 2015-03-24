using UnityEngine;
using System.Collections;

namespace BT {

	/// <summary>
	/// BTRepeater is a decorator node that keeps the child tick for a number of times, or forever.
	/// </summary>
	public class BTRepeater : BTDecorator {
		/// <summary>
		/// How many times can the child tick.
		/// </summary>
		public int count {get; set;}

		/// <summary>
		/// Tick the child forever.
		/// </summary>
		public bool repeatForever {get; set;}

		/// <summary>
		/// Should end the repetition if the child return failure.
		/// </summary>
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

				isRunning = true;
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
				isRunning = true;
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