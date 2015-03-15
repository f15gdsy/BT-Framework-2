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
					foreach (BTConditional conditional in _conditionals) {
						if (!conditional.Check()) {
							return BTResult.Failed;
						}
					}
					break;

				case BTLogic.Or:
					bool anySuccess = false;
					foreach (BTConditional conditional in _conditionals) {
						if (conditional.Check()) {
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

		public void AddConditional (BTConditional conditional) {
			if (!_conditionals.Contains(conditional)) {
				_conditionals.Add(conditional);
			}
		}

		public void RemoveConditional (BTConditional conditional) {
			_conditionals.Remove(conditional);
		}
	}

}