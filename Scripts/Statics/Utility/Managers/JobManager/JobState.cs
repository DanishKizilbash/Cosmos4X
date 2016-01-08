using UnityEngine;
using System.Collections;
namespace Eniso {
	public enum JobState {
		Unassigned,
		Waiting,
		Working,
		Failed,
		Success,
	}
}