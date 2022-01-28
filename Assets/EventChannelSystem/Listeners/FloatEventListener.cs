using UnityEngine;
using UnityEngine.Events;

public class FloatEventListener : MonoBehaviour
{
	[SerializeField] private FloatEventChannelSO _channel = default;

	public UnityEvent<float> OnEventRaised;

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

	private void Respond(float _value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(_value);
	}
}