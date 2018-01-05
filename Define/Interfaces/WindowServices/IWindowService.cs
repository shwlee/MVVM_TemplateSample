using Define.Enums;

namespace Define.Interfaces.WindowServices
{
	/// <summary>
	/// Window 를 조작하기 위한 서비스.
	/// </summary>
	public interface IWindowService
	{
		/// <summary>
		/// CommonWindow를 이용한 Window 호출 시 사용.
		/// </summary>
		/// <param name="windowContent">대상 Window 가 사용할 Content 객체.(ViewModel)</param>
		/// <param name="ownerViewModel">Owner로 사용할 Window의 ViewModel.</param>
		/// <param name="isCreate">true 이면 새로 생성; false 이면 WindowManagerService에 캐쉬된 부분 사용.</param>
		/// <returns></returns>
		IWindowView GetOrCreateWindow(IWindowContent windowContent, IWindowContent ownerViewModel = null, bool isCreate = false);

		/// <summary>
		/// CommonWindow를 이용한 Window 호출 시 사용.
		/// </summary>
		/// <param name="windowContent">대상 Window 가 사용할 Content 객체.(ViewModel)</param>
		/// <param name="setting">Window 초기 형태를 결정할 Setting instance.</param>
		/// <param name="ownerViewModel">Owner로 사용할 Window의 ViewModel.</param>
		/// <param name="isCreate">true 이면 새로 생성; false 이면 WindowManagerService에 캐쉬된 부분 사용.</param>
		/// <returns></returns>
		IWindowView GetOrCreateWindow(IWindowContent windowContent, IWindowSettings setting, IWindowContent ownerViewModel = null, bool isCreate = false);

		void SetOwner(IWindowContent targetViewModel, IWindowContent ownerViewModel);

		void SetOwner(IWindowView targetView, IWindowView ownerView);

		void ChangeContent(IWindowContent from, IWindowContent to);

		void ChangeWindowState(IWindowContent windowViewModel, WindowViewStates states);

		void ChangeWindowState(IWindowContext windowContext, WindowViewStates states);

		void ChangeWindowState(IWindowView windowView, WindowViewStates states);

		void ChangeWindowSizeToContent(IWindowContext windowContext, WindowSizeToContents sizeToContent);

		void DragMove(IWindowContent windowViewModel);

		void DragMove(IWindowContext windowContext);

		void DragMove(IWindowView windowView);

		void CloseWindow(IWindowContent windowViewModel);

		void CloseWindow(IWindowContext windowContext);

		void CloseWindow(IWindowView windowView);

		void Release();
	}
}
