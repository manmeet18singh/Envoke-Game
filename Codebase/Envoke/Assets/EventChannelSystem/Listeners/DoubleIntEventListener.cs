using UnityEngine;
using UnityEngine.Events;

public class DoubleIntEventListener : MonoBehaviour
{
	[SerializeField] private DoubleIntEventChannelSO _channel = default;

	public UnityEvent<int, int> OnEventRaised;

	private void OnEnable()
	{
		if (_channel != null)
			_channel.OnEventRaised += Respond;
	}

	private void OnDisable()
	{
		if (_channel != null)
			_channel.OnEventRaised -= Respond;
	}

	private void Respond(int _value, int _value2)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(_value, _value2);
	}
}