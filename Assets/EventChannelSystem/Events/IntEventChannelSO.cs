using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "IntEvent", menuName = "Envoke/Events/Int Event Channel")]
public class IntEventChannelSO : ScriptableObject
{
	public UnityAction<int> OnEventRaised;
	public void RaiseEvent(int value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}
}
