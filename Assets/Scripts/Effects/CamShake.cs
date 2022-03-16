using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    public float strengthTOShake;
    public IEnumerator Shake(float duration, float magnitude){
        float elapsed = .0f;

        while(elapsed < duration){
            float x = Random.Range(-1f, 1f) * magnitude/strengthTOShake;
            float y = Random.Range(-1f, 1f) * magnitude/strengthTOShake;

            transform.localPosition = new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = Vector3.zero;
    }
}
