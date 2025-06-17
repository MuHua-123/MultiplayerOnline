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

	public static DataGameVersion GameVersion;

	public UIScrollViewV scrollView;
	public ModuleUIItems<UIOnline, DataDiscoveryResponse> container;
	public List<DataDiscoveryResponse> discoveredServers = new List<DataDiscoveryResponse>();

	public VisualElement ScrollView => Container.Q<VisualElement>("ScrollView");

	public UIOnlineWindow(VisualElement element, VisualElement canvas, VisualTreeAsset templateAsset) : base(element, canvas) {
		scrollView = new UIScrollViewV(ScrollView, canvas);
		VisualElement svc = scrollView.Container;
		container = new ModuleUIItems<UIOnline, DataDiscoveryResponse>(svc, templateAsset, (data, element) => new UIOnline(data, element, this));

		OnlineDiscovery<DataDiscoveryBroadcast, DataDiscoveryResponse>.OnServerFound += OnlineDiscovery_OnServerFound;
	}
	public override void Update() {
		base.Update();
		scrollView.Update();
	}
	public void Release() => container.Dispose();

	public void OnlineDiscovery_OnServerFound(IPEndPoint sender, DataDiscoveryResponse response) {
		discoveredServers.Add(response);
		container.Create(discoveredServers);
	}

	/// <summary> 设置活动状态 </summary>
	public override void SetActive(bool active) {
		base.SetActive(active);
		if (!active) { OnlineDiscovery<DataDiscoveryBroadcast, DataDiscoveryResponse>.I.StopDiscovery(); return; }
		discoveredServers.Clear();
		container.Create(discoveredServers);
		// 更新版本信息
		GameVersion = ManagerVersion.I.VersionInfo();
		// 发送广播
		OnlineDiscovery<DataDiscoveryBroadcast, DataDiscoveryResponse>.I.StartClient();
		OnlineDiscovery<DataDiscoveryBroadcast, DataDiscoveryResponse>.I.ClientBroadcast(new DataDiscoveryBroadcast());
	}

	#region UI项定义
	/// <summary>
	/// 联机服务器 UI项
	/// </summary>
	public class UIOnline : ModuleUIItem<DataDiscoveryResponse> {
		public readonly UIOnlineWindow parent;

		private DataGameVersion serverVersion;

		public Label Title => element.Q<Label>("Title");
		public Label Count => element.Q<Label>("Count");
		public VisualElement State => Q<VisualElement>("State");

		public UIOnline(DataDiscoveryResponse value, VisualElement element, UIOnlineWindow parent) : base(value, element) {
			this.parent = parent;
			Title.text = $"{value.serverName}[{value.address}]";

			serverVersion = JsonTool.FromJson<DataGameVersion>(value.serverVersion);
			if (GameVersion.Equals(serverVersion)) {
				AllowConnection();
			}
			else {
				VersionInconsistency();
			}
		}
		/// <summary> 允许连接 </summary>
		private void AllowConnection() {
			State.EnableInClassList("ow-template-state-g", true);
			element.RegisterCallback<ClickEvent>(evt => {
				parent.SetActive(false);
				OnlineManager.I.StartClient(value.address.ToString(), value.port.ToString());
			});
		}
		/// <summary> 版本不一致 </summary>
		private void VersionInconsistency() {
			State.EnableInClassList("ow-template-state-y", true);
		}
	}
	#endregion
}
