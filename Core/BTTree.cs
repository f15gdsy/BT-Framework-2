using UnityEngine;
using System.Collections;

namespace BT {

	/// <summary>
	/// BTTree is where the behavior tree should be constructed.
	/// </summary>
	public abstract class BTTree : MonoBehaviour {
		private BTNode _root;
		protected BTDatabase _database;

		public BTNode root {get {return _root;}}


		void Start () {
			_root = Init();

			if (_root.name == null) {
				_root.name = "Root";
			}
			_root.Activate(_database);
		}
		

		void Update () {
			_root.Tick();
		}

		/// <summary>
		/// Init this tree by constructing the behavior tree.
		/// Root node should be returned.
		/// </summary>
		public virtual BTNode Init () {
			_database = GetComponent<BTDatabase>();
			if (_database == null) {
				_database = gameObject.AddComponent<BTDatabase>();
			}

			return null;
		}
	}

}