using UnityEngine;
using System.Collections;
namespace Cosmos
{
	public enum JobState
	{
		Unassigned,
		Waiting,
		Working,
		Failed,
		Success,
	}
}