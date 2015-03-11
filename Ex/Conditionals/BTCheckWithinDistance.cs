using UnityEngine;
using System.Collections;

namespace BT.Ex {

	public class BTCheckWithinDistance : BTConditional {
		private string _readDataName;
		private int _readDataId;
		private DataOpt _dataOpt;
		private float _distance;
		private Transform _targetTrans;
		private Transform _trans;


		public BTCheckWithinDistance (Transform trans, float distance, string readDataName, DataOpt dataOpt) {
			_trans = trans;
			_distance = distance;
			_readDataName = readDataName;
			_dataOpt = dataOpt;
		}

		public BTCheckWithinDistance (Transform trans, float distance, Transform targetTrans) : this ( trans, distance, null, DataOpt.ProvidedTrans) {
			_targetTrans = targetTrans;
		}

		public override void Activate (BTDatabase database) {
			base.Activate (database);

			if (_dataOpt != DataOpt.ProvidedTrans) {
				_readDataId = _database.GetDataId(_readDataName);
			}
		}

		public override bool Check () {
			Vector3 dataPosition = Vector3.zero;

			switch (_dataOpt) {
			case DataOpt.Vec3:
				dataPosition = _database.GetData<Vector3>(_readDataId);
				break;
			case DataOpt.Trans:
				Transform trans = _database.GetData<Transform>(_readDataId);
				dataPosition = trans.position;
				break;
			case DataOpt.ProvidedTrans:
				dataPosition = _targetTrans.position;
				break;
			}

			Vector3 offset = dataPosition - _trans.position;
			return offset.sqrMagnitude <= _distance * _distance;
		}

		public enum DataOpt {
			Vec3,
			Trans,
			ProvidedTrans,
		}
	}
}