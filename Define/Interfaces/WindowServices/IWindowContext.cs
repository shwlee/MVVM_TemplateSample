
namespace Define.Interfaces.WindowServices
{
	/// <summary>
	/// Window가 사용할 DataContext.
	/// </summary>
	public interface IWindowContext
	{
		IWindowContent ContentContext { get; set; }

		IWindowSettings Setting { get; set; }
	}
}
