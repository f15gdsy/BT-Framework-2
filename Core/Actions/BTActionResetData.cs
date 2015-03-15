using UnityEngine;
using System.Collections;

namespace BT {

	/// <summary>
	/// BTActionResetData is an action node that reset a data in BTDatabase.
	/// It provides three options:
	/// 	- OnTick: reset the data on tick;
	/// 	- OnClear: reset the data on clear;
	/// 	- Both: reset the data on tick and on clear.
	/// It is useful with the use of different decorators.
	/// </summary>
	public class BTActionResetData<T> : BTAction {
		private string _setDataName;
		private int _setDataId;
		private T _rhs;
		private BTExecuteOpt _resetOpt;


		public BTActionResetData (string setDataName, T rhs, BTExecuteOpt resetOpt) {
			_setDataName = setDataName;
			_rhs = rhs;
			_resetOpt = resetOpt;
		}

		public override void Activate (BTDatabase database) {
			base.Activate (database);

			_setDataId = _database.GetDataId(_setDataName);
		}

		protected override BTResult Execute () {
			if (_resetOpt != BTExecuteOpt.OnClear) {
				_database.SetData<T>(_setDataId, _rhs);
			}
			return BTResult.Success;
		}

		public override void Clear () {
			base.Clear ();

			if (_resetOpt != BTExecuteOpt.OnTick) {
				_database.SetData<T>(_setDataId, _rhs);
			}
		}
	}

}