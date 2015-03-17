using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace BT {

	/// <summary>
	/// BTDatabase is the blackboard in a classic blackboard system. 
	/// (I found the name "blackboard" a bit hard to understand so I call it database ;p)
	/// 
	/// It is the place to store data from local nodes, cross-tree nodes, and even other scripts.

	/// Nodes can read the data inside a database by the use of a string, or an int id of the data.
	/// The latter one is prefered for efficiency's sake.
	/// </summary>
	public class BTDatabase : MonoBehaviour {
		
		// _database & _dataNames are 1 to 1 relationship
		private List<object> _dataList = new List<object>();
		private List<string> _dataNames = new List<string>();
		
		
		// Should use dataId as parameter to get data instead of this
		public T GetData<T> (string dataName) {
			int dataId = IndexOfDataId(dataName);
			if (dataId == -1) Debug.LogError("BTDatabase: Data for " + dataName + " does not exist!");
			
			return (T) _dataList[dataId];
		}
		
		// Should use this function to get data!
		public T GetData<T> (int dataId) {
			if (BT.BTConfiguration.ENABLE_DATABASE_LOG) {
				Debug.Log("BTDatabase: getting data for " + _dataNames[dataId]);
			}
			return (T) _dataList[dataId];
		}
		
		public void SetData<T> (string dataName, T data) {
			int dataId = GetDataId(dataName);
			_dataList[dataId] = (object) data;
		}
		
		public void SetData<T> (int dataId, T data) {
			_dataList[dataId] = (object) data;
		}

		public bool CheckDataNull (string dataName) {
			int dataId = IndexOfDataId(dataName);
			if (dataId == -1) return true;

			return CheckDataNull(dataId);
		}

		public bool CheckDataNull (int dataId) {
			// Despite == test, Equal test helps the case that the reference is Monobahvior and is destroyed.
			return _dataList[dataId] == null || _dataList[dataId].Equals(null);
		}
		
		public int GetDataId (string dataName) {
			int dataId = IndexOfDataId(dataName);
			if (dataId == -1) {
				_dataNames.Add(dataName);
				_dataList.Add(null);
				dataId = _dataNames.Count - 1;
			}
			
			return dataId;
		}
		
		private int IndexOfDataId (string dataName) {
			for (int i=0; i<_dataNames.Count; i++) {
				if (_dataNames[i].Equals(dataName)) return i;
			}
			
			return -1;
		}
		
		public bool ContainsData (string dataName) {
			return IndexOfDataId(dataName) != -1;
		}
	}

}