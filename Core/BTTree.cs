using UnityEngine;
using System.Collections;

namespace BT {

	public abstract class BTTree : MonoBehaviour {
		private BTNode _root;
		protected BTDatabase _database;


		void Start () {
			Init();

			_root.Activate(_database);
		}
		

		void Update () {
			_root.Tick();
		}


		protected void SetRoot (BTNode node) {
			_root = node;
		}

		protected virtual void Init () {
			_database = GetComponent<BTDatabase>();
			if (_database == null) {
				_database = gameObject.AddComponent<BTDatabase>();
			}
		}
	}

}