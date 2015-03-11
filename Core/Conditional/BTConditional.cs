using UnityEngine;
using System.Collections;

namespace BT {

	public abstract class BTConditional : BTNode {

		sealed public override BTResult Tick () {
			if (Check()) {
				return BTResult.Success;
			}
			else {
				return BTResult.Failed;
			}
		}

		public virtual bool Check () {
			return false;
		}
	}

}