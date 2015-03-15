using UnityEngine;
using System;
using System.Collections;

namespace BT {

	public class BTActionCallback : BTAction {

		private Action<BTDatabase> _callback;
		private BTExecuteOpt _executeOpt;
		
		public BTActionCallback (Action<BTDatabase> callback, BTExecuteOpt executeOpt) {
			_callback = callback;
			_executeOpt = executeOpt;
		}

		protected override BTResult Execute () {
			if (_executeOpt != BTExecuteOpt.OnClear) {
				_callback(_database);
			}

			return BTResult.Success;
		}

		public override void Clear () {
			base.Clear ();

			if (_executeOpt != BTExecuteOpt.OnTick) {
				_callback(_database);
			}
		}


	}

}