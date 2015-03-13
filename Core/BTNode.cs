using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BT {

	public abstract class BTNode {

		protected BTDatabase _database;

		public string name {get; set;}
		public string details {get; set;}


		public virtual void Activate (BTDatabase database) {
			_database = database;
		}
		public virtual BTResult Tick () {return BTResult.Failed;}
		public virtual void Clear () {}
	}

}