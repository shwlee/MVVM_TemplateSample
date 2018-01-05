
namespace Define.EventAggregators
{
	public interface IEventAggregator
	{
		TEventType GetEvent<TEventType>() where TEventType : EventBase, new();
	}
}
