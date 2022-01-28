using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VoidEvent", menuName = "Envoke/Events/Void Event Channel")]
public class VoidEventChannelSO : ScriptableObject
{
	public UnityAction OnEventRaised;

	public void RaiseEvent()
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke();
	}
}