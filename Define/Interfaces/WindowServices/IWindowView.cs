using Define.Enums;

namespace Define.Interfaces.WindowServices
{
	/// <summary>
	/// Window 대신 반환하는 추상화 형식.
	/// </summary>
	public interface IWindowView
	{
		void SetOwner(IWindowView windowView);

		void Initialize(IWindowSettings settings);

		void Show();

		bool? ShowDialog();

		bool Activate();

		void MoveWindow();

		void ChangeWindowState(WindowViewStates states);

		void ChangeSizeToContent(WindowSizeToContents sizeToContent);

		void Close();

		bool IsClosed { get; }
	}
}
