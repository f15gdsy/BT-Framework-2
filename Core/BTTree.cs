using UnityEngine;
using System.Collections;

namespace BT {

	public abstract class BTTree : MonoBehaviour {
		private BTNode _root;
		protected BTDatabase _database;

		public BTNode root {get {return _root;}}


		void Start () {
			_root = Init();

			_root.Activate(_database);
		}
		

		void Update () {
			_root.Tick();
		}


		public virtual BTNode Init () {
			_database = GetComponent<BTDatabase>();
			if (_database == null) {
				_database = gameObject.AddComponent<BTDatabase>();
			}

			return null;
		}
	}

}