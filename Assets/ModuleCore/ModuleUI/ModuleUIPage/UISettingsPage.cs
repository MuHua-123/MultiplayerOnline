using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

/// <summary>
/// 设置页面
/// </summary>
public class UISettingsPage : ModuleUIPage {
	/// <summary>
	/// 设置类型
	/// </summary>
	public enum DataSettingsType { 图形, 其他 }

	public VisualTreeAsset TitleTemplate;

	public ModuleUIItems<UISettingsTypeItem, DataSettingsType> SettingsTypes;

	public override VisualElement Element => root.Q<VisualElement>("SettingsPage");

	public VisualElement Top => Q<VisualElement>("Top");

	public VisualElement Middle => Q<VisualElement>("Middle");

	public VisualElement Bottom => Q<VisualElement>("Bottom");
	public Button Button1 => Bottom.Q<Button>("Button1");// 返回
	public Button Button2 => Bottom.Q<Button>("Button2");// ???
	public Button Button3 => Bottom.Q<Button>("Button3");// ???

	private void Awake() {
		SettingsTypes = new ModuleUIItems<UISettingsTypeItem, DataSettingsType>(Top, TitleTemplate,
			(data, element) => new UISettingsTypeItem(data, element, this));

		Button1.clicked += () => ModuleUI.Settings(EnumPage.Menu);

		ModuleUI.OnJumpPage += ModuleUI_OnJumpPage;
	}
	private void ModuleUI_OnJumpPage(EnumPage type) {
		Element.EnableInClassList("document-page-hide", type != EnumPage.Settings);
		if (type != EnumPage.Settings) { return; }
		List<DataSettingsType> types = new List<DataSettingsType>{
			DataSettingsType.图形,
			DataSettingsType.其他
		};
		SettingsTypes.Create(types);
		SettingsTypes[0].Select();
	}

	/// <summary> 设置类型 </summary>
	public void SetSettingsType(DataSettingsType type) {

	}

	#region UI项定义
	/// <summary>
	/// 设置标题 UI项
	/// </summary>
	public class UISettingsTypeItem : ModuleUIItem<DataSettingsType> {
		public readonly UISettingsPage parent;

		public Label Title => Q<Label>();

		public UISettingsTypeItem(DataSettingsType value, VisualElement element, UISettingsPage parent) : base(value, element) {
			this.parent = parent;
			Title.text = value.ToString();
			Title.RegisterCallback<ClickEvent>(evt => Select());
		}
		public override void DefaultState() {
			Title.text = value.ToString();
			Title.EnableInClassList("template-title-s", false);
		}
		public override void SelectState() {
			parent.SetSettingsType(value);
			Title.text = $"<u>{value}";
			Title.EnableInClassList("template-title-s", true);
		}
	}
	#endregion
}
