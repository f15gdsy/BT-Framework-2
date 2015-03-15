using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BT {

	/// <summary>
	/// BTConditionEvaluator is a decorator node.
	/// It performs conditional check to determine if the child should be Tick.
	/// 
	/// It allows the child to tick if the conditional check return true, and returns what the child returns.
	/// </summary>
	public class BTConditionEvaluator : BTDecorator {

		private List<BTConditional> _conditionals;
		private List<bool> _conditionalInverts;
		public BTLogic logicOpt;
		public bool reevaludateEveryTick;

		private BTResult _previousResult = BTResult.Success;

		
		
		public BTConditionEvaluator (List<BTConditional> conditionals, BTLogic logicOpt, bool reevaluateEveryTick, BTNode child = null) : base (child) {
			this._conditionals = conditionals;
			this.logicOpt = logicOpt;
			this.reevaludateEveryTick = reevaluateEveryTick;
		}

		public BTConditionEvaluator (BTLogic logicOpt, bool reevaluateEveryTick, BTNode child = null) : base (child) {
			this._conditionals = new List<BTConditional>();
			this._conditionalInverts = new List<bool>();
			this.logicOpt = logicOpt;
			this.reevaludateEveryTick = reevaluateEveryTick;
		}

		public override void Activate (BTDatabase database) {
			base.Activate (database);

			foreach (BTConditional conditional in _conditionals) {
				conditional.Activate(database);
			}
		}

		public override BTResult Tick () {
			if (_previousResult != BTResult.Running || reevaludateEveryTick) {
				switch (logicOpt) {
				case BTLogic.And:
					int i = 0;
					foreach (BTConditional conditional in _conditionals) {
						bool invert = _conditionalInverts[i++];
						if ((invert && conditional.Check()) ||
						    (!invert && !conditional.Check())) {
							return BTResult.Failed;
						}
					}
					break;

				case BTLogic.Or:
					bool anySuccess = false;
					i = 0;
					foreach (BTConditional conditional in _conditionals) {
						bool invert = _conditionalInverts[i++];
						if ((invert && !conditional.Check()) ||
						    (!invert && conditional.Check())) {
							anySuccess = true;
							break;
						}
					}
					if (!anySuccess) {
						return BTResult.Failed;
					}
					break;
				}
			}

			_previousResult = child.Tick();

			return _previousResult;
		}

		public override void Clear () {
			base.Clear();
			_previousResult = BTResult.Success;
		}

		public void AddConditional (BTConditional conditional, bool invertResult = false) {
			if (!_conditionals.Contains(conditional)) {
				_conditionals.Add(conditional);
				_conditionalInverts.Add(invertResult);
			}
		}

		public void RemoveConditional (BTConditional conditional) {
 			int index = _conditionals.IndexOf(conditional);
			_conditionals.Remove(conditional);
			_conditionalInverts.RemoveAt(index);
		}
	}

}