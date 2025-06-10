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
	public ModuleUIItems<UIOnlineItem, DataDiscoveryResponse> container;
	public List<DataDiscoveryResponse> discoveredServers = new List<DataDiscoveryResponse>();

	public UIOnlineWindow(VisualElement element, VisualElement canvas, VisualTreeAsset templateAsset) : base(element, canvas) {
		VisualElement ScrollViewVisualElement = Container.Q<VisualElement>("ScrollView");
		scrollView = new UIScrollView(ScrollViewVisualElement, canvas, UIDirection.Vertical);
		VisualElement svc = scrollView.Container;
		container = new ModuleUIItems<UIOnlineItem, DataDiscoveryResponse>(svc, templateAsset, (data, element) => new UIOnlineItem(data, element, this));

		OnlineManager.I.discovery.OnServerFound += OnServerFound;
	}
	public void Release() => container.Release();

	public void OnServerFound(IPEndPoint sender, DataDiscoveryResponse response) {
		response.address = sender.Address;
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
	public class UIOnlineItem : ModuleUIItem<DataDiscoveryResponse> {
		public readonly UIOnlineWindow parent;

		public Label Column => element.Q<Label>("Column");

		public UIOnlineItem(DataDiscoveryResponse value, VisualElement element, UIOnlineWindow parent) : base(value, element) {
			this.parent = parent;
			Column.text = $"{value.ServerName}[{value.address}]";
			Column.RegisterCallback<ClickEvent>(evt => Select());
		}
		public override void SelectState() {
			// SingleManager.I.StartClient(value.address.ToString(), value.Port.ToString());
			parent.SetActive(false);
		}
	}
	#endregion
}
