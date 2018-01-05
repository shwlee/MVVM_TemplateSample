using System;
using System.Collections.Generic;
using System.Linq;
using Define.Enums;
using Define.Interfaces.WindowServices;

namespace Services.Window
{
	public class WindowService : IWindowService
	{
		#region Fields

		private Type _commonWindowType;

		private readonly List<Type> _customWindowTypes = new List<Type>();

		private readonly Dictionary<IWindowContent, WindowViewStore> _windowCaches = new Dictionary<IWindowContent, WindowViewStore>();

		#endregion

		#region Constructors

		public WindowService()
		{
			this.InitializeTypeCache();
		}

		#endregion

		#region Methods

		public IWindowView GetOrCreateWindow(IWindowContent windowContent, IWindowContent ownerViewModel = null, bool isCreate = false)
		{
			var ownerWindow = this.GetWindowView(ownerViewModel);
			return this.GetOrCreateWindowView(windowContent, null, ownerWindow, isCreate);
		}

		public IWindowView GetOrCreateWindow(IWindowContent windowContent, IWindowSettings setting, IWindowContent ownerViewModel = null, bool isCreate = false)
		{
			var ownerWindow = this.GetWindowView(ownerViewModel);
			return this.GetOrCreateWindowView(windowContent, setting, ownerWindow, isCreate);
		}

		public void SetOwner(IWindowContent targetViewModel, IWindowContent ownerViewModel)
		{
			var targetWindowView = this.GetWindowView(targetViewModel);
			if (targetWindowView == null)
			{
				// NOTE : 예외를 발생시켜야 하나?
				// TODO : need logging.
				return;
			}

			var ownerWindowView = this.GetWindowView(ownerViewModel);
			if (ownerWindowView == null)
			{
				// NOTE : 예외를 발생시켜야 하나?
				// TODO : need logging.
				return;
			}

			targetWindowView.SetOwner(ownerWindowView);
		}

		public void SetOwner(IWindowView targetView, IWindowView ownerView)
		{
			targetView.SetOwner(ownerView);
		}

		public void ChangeContent(IWindowContent from, IWindowContent to)
		{
			if (this._windowCaches.ContainsKey(from) == false)
			{
				return;
			}

			var window = this._windowCaches[from];
			window.Context.ContentContext = to;
		}

		public void ChangeWindowState(IWindowContent windowViewModel, WindowViewStates states)
		{
			var windowView = this.GetWindowView(windowViewModel);

			windowView?.ChangeWindowState(states);
		}

		public void ChangeWindowState(IWindowContext windowContext, WindowViewStates states)
		{
			var windowView = this.GetWindowView(windowContext);

			windowView?.ChangeWindowState(states);
		}

		public void ChangeWindowState(IWindowView windowView, WindowViewStates states)
		{
			windowView.ChangeWindowState(states);
		}

		public void ChangeWindowSizeToContent(IWindowContext windowContext, WindowSizeToContents sizeToContent)
		{
			var windowView = this.GetWindowView(windowContext);
			windowView?.ChangeSizeToContent(sizeToContent);
		}

		public void DragMove(IWindowContent windowViewModel)
		{
			var windowView = this.GetWindowView(windowViewModel);

			windowView?.MoveWindow();
		}

		public void DragMove(IWindowContext windowContext)
		{
			var windowView = this.GetWindowView(windowContext);

			windowView?.MoveWindow();
		}

		public void DragMove(IWindowView windowView)
		{
			windowView.MoveWindow();
		}

		public void CloseWindow(IWindowContent windowViewModel)
		{
			var windowView = this.GetWindowView(windowViewModel);
			if (windowView == null)
			{
				// TODO : need logging.
				return;
			}

			windowView.Close();

			this._windowCaches.Remove(windowViewModel);

			this.CheckAndRemoveInvalidCache();
		}

		public void CloseWindow(IWindowContext windowContext)
		{
			var windowView = this.GetWindowView(windowContext);
			if (windowView == null)
			{
				// TODO : need logging.
				return;
			}

			windowView.Close();

			var targetView = this._windowCaches.FirstOrDefault(w => w.Value.Context == windowContext);
			if (Equals(targetView, default(KeyValuePair<IWindowContent, WindowViewStore>)))
			{
				return;
			}

			this._windowCaches.Remove(targetView.Key);

			this.CheckAndRemoveInvalidCache();
		}

		public void CloseWindow(IWindowView windowView)
		{
			windowView.Close();

			var targetPair = this._windowCaches.FirstOrDefault(w => w.Value.View.Equals(windowView));
			if (Equals(targetPair, default(KeyValuePair<IWindowContent, WindowViewStore>)))
			{
				// TODO : need logging.
				return;
			}

			this._windowCaches.Remove(targetPair.Key);

			this.CheckAndRemoveInvalidCache();
		}

		public void Release()
		{
			foreach (var windowViewPair in this._windowCaches)
			{
				var windowView = windowViewPair.Value.View;
				windowView.Close();
			}

			this._windowCaches.Clear();

			this._customWindowTypes.Clear();
		}

		private void InitializeTypeCache()
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();

			foreach (var assembly in assemblies)
			{
				// Jackpot.View 에 포함된 Window만 캐쉬한다.
				if (string.CompareOrdinal(assembly.ManifestModule.Name, "Views.dll") != 0)
				{
					continue;
				}

				var assemblyTypes = assembly.GetTypes();
				foreach (var type in assemblyTypes)
				{
					if (type.IsInterface)
					{
						continue;
					}

					if (type.Name.EndsWith("Window") == false) // Window name은 항상 Window로 끝난다.
					{
						continue;
					}

					if (type.Name.StartsWith("Base")) // 추상화된 base 개체는 제외한다.
					{
						continue;
					}

					if (type.Name.Contains("CommonWindow"))
					{
						this._commonWindowType = type;
						continue;
					}

					this._customWindowTypes.Add(type);
				}
			}
		}

		private IWindowView GetOrCreateWindowView(
			IWindowContent windowContent,
			IWindowSettings setting,
			IWindowView ownerWindow,
			bool isCreate,
			Type windowType = null)
		{
			this.CheckAndRemoveInvalidCache();

			var needCreateView = this._windowCaches.ContainsKey(windowContent) == false || isCreate;

			if (needCreateView)
			{
				this.CreateWindowView(windowContent, setting, windowType);
			}

			var window = this._windowCaches[windowContent];

			if (ownerWindow != null)
			{
				window.View.SetOwner(ownerWindow);
			}

			return window.View;
		}

		private IWindowView GetWindowView(IWindowContext windowContext)
		{
			if (windowContext == null)
			{
				return null;
			}

			var windowView = this._windowCaches.Values.FirstOrDefault(w => w.Context == windowContext);
			return windowView?.View;
		}

		private IWindowView GetWindowView(IWindowContent windowContent)
		{
			if (windowContent == null)
			{
				return null;
			}

			return this._windowCaches.ContainsKey(windowContent) == false ? null : this._windowCaches[windowContent].View;
		}

		private IWindowView CreateWindowView(IWindowContent windowContent, IWindowSettings setting, Type targeType = null)
		{
			var modelType = windowContent.GetType();
			var windowTypeName = modelType.Name.Replace("ViewModel", string.Empty);

			// 1. 특정 Window 타입이 없으면 CommonWindow 사용. 
			// 2. 1번이 null 일 경우 캐쉬되어 있는 this._customWindowTypes 에서 windowContent 의 이름을 기준으로 찾아서 사용.
			// 3. 2번이 null 일 경우 전달 받은 타입 사용.
			var windowType = (targeType == null ?
				this._commonWindowType :
				this._customWindowTypes.FirstOrDefault(w => string.CompareOrdinal(w.Name, windowTypeName) == 0)) ?? targeType;

			if (windowType == null)
			{
				// TODO : need logging
				return null;
			}

			var windowView = Activator.CreateInstance(windowType) as IWindowView;
			if (windowView == null)
			{
				// TODO : need logging
				return null;
			}

			var dataContextInfo = windowType.GetProperty("DataContext");

			// Window의 DataContext는 View에서 함께 생성한다.
			var dataContext = dataContextInfo?.GetValue(windowView);

			// Window의 ViewModel은 반드시 IWindowContext을 상속해야한다.
			var windowViewModel = dataContext as IWindowContext;
			if (windowViewModel == null)
			{
				// TODO : need logging
				return null;
			}

			windowViewModel.Setting = setting;
			windowView.Initialize(setting);

			this._windowCaches.Add(windowContent, new WindowViewStore { View = windowView, Context = windowViewModel });

			windowViewModel.ContentContext = windowContent;
			
			return windowView;
		}

		private void CheckAndRemoveInvalidCache()
		{
			var removeList = this._windowCaches.Where(w => w.Value.View.IsClosed).Select(w => w.Key).ToList();
			foreach (var removeView in removeList)
			{
				this._windowCaches.Remove(removeView);
			}
		}

		#endregion
	}
}
