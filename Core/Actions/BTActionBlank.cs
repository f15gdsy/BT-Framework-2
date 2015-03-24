using UnityEngine;
using System.Collections;

namespace BT {

	public class BTActionBlank : BTAction {

		protected override BTResult Execute () {
			return BTResult.Running;
		}
	}

}