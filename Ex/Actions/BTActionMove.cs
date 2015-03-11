using UnityEngine;
using System.Collections;

namespace BT.Ex {

	public class BTActionMove : BTAction {

		private string _readDataName;
		private int _readDataId;
		private float _speed;
		private float _tolerance;
		private DataOpt _dataOpt;
		private UsageOpt _usageOpt;
		private BTDataReadOpt _dataReadOpt;
		private Vector3 _vec3Data;
		private Transform _targetTrans;
		private Transform _trans;

		public BTActionMove (Transform trans, float speed, float tolerance, string readDataName, DataOpt dataOpt, UsageOpt usageOpt, BTDataReadOpt dataReadOpt) {
			_trans = trans;
			_speed = speed;
			_tolerance = tolerance;
			_readDataName = readDataName;
			_dataOpt = dataOpt;
			_usageOpt = usageOpt;
			_dataReadOpt = dataReadOpt;
		}

		public BTActionMove (Transform trans, float speed, float tolerance, Transform targetTrans, BTDataReadOpt dataReadOpt) : 
		this (trans, speed, tolerance, null, DataOpt.ProvidedTrans, UsageOpt.Position, dataReadOpt) {
			_targetTrans = targetTrans;
		}

		public override void Activate (BTDatabase database) {
			base.Activate (database);

			if (_dataOpt != DataOpt.ProvidedTrans) {
				_readDataId = _database.GetDataId(_readDataName);
			}
		}

		protected override void Enter () {
			base.Enter ();

			if (_dataReadOpt == BTDataReadOpt.ReadAtBeginning) {
				ReadVec3Data();
			}
		}

		protected override BTResult Execute () {
			if (_dataReadOpt == BTDataReadOpt.ReadEveryTick) {
				ReadVec3Data();
			}

			Vector3 direction = Vector3.zero;

			switch (_usageOpt) {
			case UsageOpt.Direction:
				direction = _vec3Data;
				break;
			case UsageOpt.Position:
				direction = _vec3Data - _trans.position;
				break;
			}

			if (direction.sqrMagnitude <= _tolerance * _tolerance) {
				return BTResult.Success;
			}
			else {
				Vector3 position = _trans.position;
				position += direction.normalized * _speed * Time.deltaTime;
				_trans.position = position;
			}
			return BTResult.Running;
		}

		private void ReadVec3Data () {
			_vec3Data = Vector3.zero;
			
			switch (_dataOpt) {
			case DataOpt.Vec3:
				_vec3Data = _database.GetData<Vector3>(_readDataId);
				break;
			case DataOpt.Trans:
				_vec3Data = _database.GetData<Transform>(_readDataId).position;
				break;
			case DataOpt.ProvidedTrans:
				_vec3Data = _targetTrans.position;
				break;
			}
		}

		public enum DataOpt {
			Vec3,
			Trans,
			ProvidedTrans,
		}

		public enum UsageOpt {
			Position,
			Direction,
		}
	}

}