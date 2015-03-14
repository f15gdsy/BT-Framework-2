using UnityEngine;
using System.Collections;

namespace BT.Ex {

	public class BTActionPlayAnimation : BTAction {
		private string stateName;
		private Animator _animator;
		private int _stateHash;
		private bool _justEntered;



		public BTActionPlayAnimation (Animator animator, string stateName, string layerName = "Base Layer") {
			this.stateName = stateName;
			_animator = animator;
			_stateHash = Animator.StringToHash(layerName + "." + stateName);
		}

		protected override void Enter () {
			base.Enter ();

			_animator.Play(stateName);
			_justEntered = true;
		}

		protected override BTResult Execute () {
			// If an animation is not loop, and to make it return success after one play, 
			// you needs to set the transition in animator controller.
			if (_justEntered || _animator.GetCurrentAnimatorStateInfo(0).IsName(stateName)) {
				if (_justEntered) {
					_justEntered = false;
				}

				return BTResult.Running;
			}
			return BTResult.Success;
		}
	}

}