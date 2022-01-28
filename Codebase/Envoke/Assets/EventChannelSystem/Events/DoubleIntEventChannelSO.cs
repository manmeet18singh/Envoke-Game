using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "DoubleIntEvent", menuName = "Envoke/Events/Double Int Event Channel")]
public class DoubleIntEventChannelSO : ScriptableObject
{
	public UnityAction<int, int> OnEventRaised;
	public void RaiseEvent(int _value, int _value2)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(_value, _value2);
	}
}