using UnityEngine;

public class BurnTree : MonoBehaviour
{
    // 木に「火（FireSource）」が触れた時に実行される
    private void OnTriggerEnter(Collider other)
    {
        // 触れてきた相手のタグが "FireSource" かどうかをチェック
        if (!other.CompareTag("FireSource")) return;
        StartBurning();
    }

    void StartBurning()
    {
        // 燃焼エフェクトを再生
        GetComponent<WoodView>().PlayFireEffect();

        AudioController.Instance.PlaySE(Audio_Data.SEType.DeathByFire);
        
        // 2秒後に木を完全に消去する
        Destroy(gameObject, 2.0f);
    }
}