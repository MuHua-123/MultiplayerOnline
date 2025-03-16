using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDispel : MonoBehaviour {
    public float dispelTime = 1.0f; // 倒计时时间

    // Start is called before the first frame update
    void Start() {
        // 启动协程进行倒计时
        StartCoroutine(DispelAfterTime(dispelTime));
    }

    private IEnumerator DispelAfterTime(float time) {
        // 等待指定的时间
        yield return new WaitForSeconds(time);

        // 销毁当前游戏对象
        Destroy(gameObject);
    }
}
