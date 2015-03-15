using UnityEngine;
using System.Collections;

namespace BT {

	/// <summary>
	/// BTCompareData is a conditional inheriting from BTConditional.
	/// 
	/// It performs comparison between the provided data with what's found in BTDatabase.
	/// It returns true if they are equal, false otherwise.
	/// </summary>
	public class BTCompareData<T> : BTConditional {
		private string _readDataName;
		private int _readDataId;
		private T _rhs;


		public BTCompareData (string readDataName, T rhs) {
			_readDataName = readDataName;
			_rhs = rhs;
		}

		public override void Activate (BTDatabase database) {
			base.Activate (database);

			_readDataId = database.GetDataId(_readDataName);
		}

		public override bool Check () {
			if (_rhs == null) {
				return _database.CheckDataNull(_readDataId);
			}
			return _database.GetData<T>(_readDataId).Equals(_rhs);
		}
	}

}