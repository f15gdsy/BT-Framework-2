using UnityEngine;
using System.Collections;

namespace BT {

	public enum BTResult {
		Success,
		Failed,
		Running,
	}

	public enum BTAbortOpt {
		None,
		Self,
		LowerPriority,
		Both,
	}

	public enum BTClearOpt {
		Default,
		Selected,
		DefaultAndSelected,
		All,
	}

	public enum BTLogic {
		And,
		Or,
	}

	public enum BTDataReadOpt {
		ReadAtBeginning,
		ReadEveryTick,
	}
}