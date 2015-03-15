using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BT {

	/// <summary>
	/// BTComposite is the base class for composite nodes.
	/// </summary>
	public class BTComposite : BTNode {

		private List<BTNode> _children;
		private List<BTNode> _selectedChildrenForClear;	// Used for BTClearOpt.Selected & BTClearOpt.DefaultAndSelected

		/// <summary>
		/// There are different options for Clear method.
		/// The actually implementation depends on different types of composite nodes.
		/// </summary>
		public BTClearOpt clearOpt {get; set;}
//		public BTAbortOpt abortOpt {get; set;}
		public List<BTNode> children {
			get {
				if (_children == null) {
					_children = new List<BTNode>();
				}
				return _children;
			}
		}
		protected List<BTNode> selectedChildrenForClear {
			get {
				if (_selectedChildrenForClear == null) {
					_selectedChildrenForClear = new List<BTNode>();
				}
				return _selectedChildrenForClear;
			}
		}


		public override void Activate (BTDatabase database) {
			base.Activate (database);

			foreach (BTNode child in children) {
				child.Activate(database);
			}
		}

		public void AddChild (BTNode node) {
			if (node != null) {
				children.Add(node);
			}
		}

		public void AddChild (BTNode node, bool selectForClear) {
			AddChild(node);
			if (selectForClear) {
				selectedChildrenForClear.Add(node);
			}
		}

		public void RemoveChild (BTNode node) {
			children.Remove(node);
			selectedChildrenForClear.Remove(node);
		}
	}

}