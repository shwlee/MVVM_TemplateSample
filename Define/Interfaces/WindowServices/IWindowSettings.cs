using Define.Enums;

namespace Define.Interfaces.WindowServices
{
	/// <summary>
	/// Window 최초 생성 시 필요한 기본 설정들. Window 의 DataContext 에서 Binding 으로 사용한다.
	/// </summary>
	public interface IWindowSettings
	{
		string BaseTitle { get; }

		WindowStyles Style { get; }

		WindowResizeModes ResizeMode { get; }

		WindowSizeToContents SizeToContent { get; }

		WindowViewStates State { get; }
	}
}
