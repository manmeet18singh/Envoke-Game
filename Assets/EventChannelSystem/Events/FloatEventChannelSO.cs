using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "FloatEvent", menuName = "Envoke/Events/Float Event Channel")]
public class FloatEventChannelSO : ScriptableObject
{
	public UnityAction<float> OnEventRaised;
	public void RaiseEvent(float _value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(_value);
	}
}
