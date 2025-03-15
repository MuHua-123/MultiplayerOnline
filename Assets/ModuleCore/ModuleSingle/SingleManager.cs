using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MuHua;

public class SingleManager : ModuleSingle<SingleManager> {
	protected override void Awake() => NoReplace();

	private void Start() {
		SceneManager.LoadScene("MenuScene");
	}
}
