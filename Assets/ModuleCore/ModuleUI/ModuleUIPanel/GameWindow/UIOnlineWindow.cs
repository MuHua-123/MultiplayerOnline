using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

/// <summary>
/// 联机列表窗口
/// </summary>
public class UIOnlineWindow : UIWindow {

	public UIScrollView scrollView;
	public ModuleUIItems<UIOnline, DataDiscoveryResponse> container;
	public List<DataDiscoveryResponse> discoveredServers = new List<DataDiscoveryResponse>();

	public VisualElement ScrollView => Container.Q<VisualElement>("ScrollView");

	public UIOnlineWindow(VisualElement element, VisualElement canvas, VisualTreeAsset templateAsset) : base(element, canvas) {
		scrollView = new UIScrollView(ScrollView, canvas, UIDirection.Vertical);
		VisualElement svc = scrollView.Container;
		container = new ModuleUIItems<UIOnline, DataDiscoveryResponse>(svc, templateAsset, (data, element) => new UIOnline(data, element, this));

		OnlineManager.OnServerFound += OnlineManager_OnServerFound;
	}
	public override void Update() {
		base.Update();
		scrollView.Update();
	}
	public void Release() => container.Release();

	public void OnlineManager_OnServerFound(IPEndPoint sender, DataDiscoveryResponse response) {
		discoveredServers.Add(response);
		container.Create(discoveredServers);
	}

	/// <summary> 设置活动状态 </summary>
	public override void SetActive(bool active) {
		base.SetActive(active);
		if (!active) { OnlineManager.I.discovery.StopDiscovery(); return; }
		OnlineManager.I.discovery.StartClient();
		discoveredServers.Clear();
		OnlineManager.I.discovery.ClientBroadcast(new DataDiscoveryBroadcast());
		container.Create(discoveredServers);
	}

	#region UI项定义
	/// <summary>
	/// 联机服务器 UI项
	/// </summary>
	public class UIOnline : ModuleUIItem<DataDiscoveryResponse> {
		public readonly UIOnlineWindow parent;

		public Label Title => element.Q<Label>("Title");
		public VisualElement State => Q<VisualElement>("State");

		public UIOnline(DataDiscoveryResponse value, VisualElement element, UIOnlineWindow parent) : base(value, element) {
			this.parent = parent;
			Title.text = $"{value.ServerName}[{value.address}]";

			element.RegisterCallback<ClickEvent>(evt => Select());
		}
		public override void SelectState() {
			// SingleManager.I.StartClient(value.address.ToString(), value.Port.ToString());
			parent.SetActive(false);
		}
	}
	#endregion
}
