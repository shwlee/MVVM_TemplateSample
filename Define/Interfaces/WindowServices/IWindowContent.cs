
namespace Define.Interfaces.WindowServices
{
	/// <summary>
	/// Window의 Content로 채워질 하위 View의 ViewModel.
	/// </summary>
	public interface IWindowContent
	{
		// TODO : 하위 View에서 필요한 기능이 있을 경우 추가 작성 필요.

		/// <summary>
		/// Window close 시 ViewModel의 종료 처리를 위해 호출한다.
		/// </summary>
		void Close();
	}
}
