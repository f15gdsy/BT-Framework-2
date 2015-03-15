using UnityEngine;
using System.Collections;

namespace BT {

	/// <summary>
	/// BTInverter is a decorator node that inverts what its child returns.
	/// </summary>
	public class BTInverter : BTDecorator {

		public BTInverter (BTNode child = null) : base (child) {}

		public override BTResult Tick () {
			switch (child.Tick()) {
			case BTResult.Running:
				return BTResult.Running;
			case BTResult.Success:
				return BTResult.Failed;
			case BTResult.Failed:
				return BTResult.Success;
			}
			return BTResult.Failed;
		}
	}

}