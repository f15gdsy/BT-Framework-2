using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BT {

	/// <summary>
	/// BT node is the base class for any other nodes in BT framework.
	/// Usually, you don't need to override this node to utilize the framework, unless you know what you are doing.
	/// </summary>
	public abstract class BTNode {

		protected BTDatabase _database;

		public string name {get; set;}
		public string details {get; set;}
		public bool isRunning {get; set;}

		/// <summary>
		/// Activate the node, which is called on Start of a BTTree.
		/// Can be seen as initialization method.
		/// </summary>
		/// <param name="database">Database.</param>
		public virtual void Activate (BTDatabase database) {
			_database = database;
		}

		/// <summary>
		/// Tick is where logics happens every frame (not all nodes will be ticked in a frame).
		/// </summary>
		public virtual BTResult Tick () {return BTResult.Failed;}

		/// <summary>
		/// Clear the node.
		/// </summary>
		public virtual void Clear () {
			isRunning = false;
		}
	}

}