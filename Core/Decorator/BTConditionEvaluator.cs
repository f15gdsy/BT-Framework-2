using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BT {

	/// <summary>
	/// BTConditionEvaluator is a decorator node.
	/// It performs conditional check to determine if the child should be Tick.
	/// 
	/// It allows the child to tick if the conditional check return true, and returns what the child returns.
	/// 
	/// It has two clear options
	/// 	- OnAbortRunning: if Clear is called, it will only clear its child, if it was running.
	/// 	- OnNotRunning: if Clear is called, it will clear its child no matter what.
	/// </summary>
	public class BTConditionEvaluator : BTDecorator {

		private List<BTConditional> _conditionals;
		private List<bool> _conditionalInverts;
		public BTLogic logicOpt;
		public bool reevaludateEveryTick;
		public ClearChildOpt clearOpt;

		private BTResult _previousResult = BTResult.Failed;

		
		
		public BTConditionEvaluator (List<BTConditional> conditionals, BTLogic logicOpt, bool reevaluateEveryTick, ClearChildOpt clearOpt, BTNode child = null) : base (child) {
			this._conditionals = conditionals;
			this.logicOpt = logicOpt;
			this.reevaludateEveryTick = reevaluateEveryTick;
			this.clearOpt = clearOpt;
		}

		public BTConditionEvaluator (BTLogic logicOpt, bool reevaluateEveryTick, ClearChildOpt clearOpt, BTNode child = null) : base (child) {
			this._conditionals = new List<BTConditional>();
			this._conditionalInverts = new List<bool>();
			this.logicOpt = logicOpt;
			this.reevaludateEveryTick = reevaluateEveryTick;
			this.clearOpt = clearOpt;
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
			if (_previousResult == BTResult.Running) {
				isRunning = true;
			}

			return _previousResult;
		}

		public override void Clear () {
			if ((isRunning && clearOpt == ClearChildOpt.OnAbortRunning) || 
			    (_previousResult == BTResult.Success && clearOpt == ClearChildOpt.OnStopRunning) ||
			    clearOpt == ClearChildOpt.OnNotRunning) {
				isRunning = false;
				child.Clear();
			}

			if (clearTick != null) {
				clearTick.Tick();
			}
			_previousResult = BTResult.Failed;
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


		public enum ClearChildOpt {
			OnAbortRunning,
			OnStopRunning,
			OnNotRunning,
		}
	}

}