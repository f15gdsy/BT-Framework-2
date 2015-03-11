using UnityEngine;
using System.Collections;

namespace BT {
	
	public class BTDecorator : BTNode {

		public BTNode child {get; set;}


		public BTDecorator (BTNode node) {
			this.child = node;
		}

		public override void Activate (BTDatabase database) {
			base.Activate (database);

			child.Activate(database);
		}

		public override void Clear () {
			base.Clear ();
			child.Clear();
		}
	}

}