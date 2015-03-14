using UnityEngine;
using System.Collections;

namespace BT {

	public class BTCheckDataChanged<T> : BTConditional {
		private string _readDataName;
		private int _readDataId;
		private T _previousData;


		public BTCheckDataChanged (string readDataName) {
			_readDataName = readDataName;
		}

		public override void Activate (BTDatabase database) {
			base.Activate (database);

			_readDataId = database.GetDataId(_readDataName);
			_previousData = database.GetData<T>(_readDataId);
		}

		public override bool Check () {
			T data = _database.GetData<T>(_readDataId);
			if (!_previousData.Equals(data)) {
				_previousData = data;
				return true;
			}
			return false;
		}
	}

}