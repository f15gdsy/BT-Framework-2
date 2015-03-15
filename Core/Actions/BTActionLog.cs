using UnityEngine;
using System.Collections;

namespace BT {

	/// <summary>
	/// BTActionLog is an action node that performs a Unity log.
	/// It returns success after logging once.
	/// </summary>
	public class BTActionLog : BTAction {

		private string _text;
		private bool _isError;

		public BTActionLog (string text, bool isError = false) {
			_text = text;
			_isError = isError;
		}

		protected override BTResult Execute () {
			if (_isError) {
				Debug.LogError(_text);
			}
			else {
				Debug.Log(_text);
			}

			return BTResult.Success;
		}
	}

}