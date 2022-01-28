using UnityEngine;
using UnityEngine.Events;

public class IntEventListener : MonoBehaviour
{
	[SerializeField] private IntEventChannelSO _channel = default;

	public UnityEvent<int> OnEventRaised;

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

	private void Respond(int value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}
}