using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum Effect
{
    Rotate,
    Wave,
    RotateWave,
}
public class ObjectEffect : MonoBehaviour
{
    [SerializeField]
    private float MotionSpeed;
    
    [SerializeField]
    private Effect effect;

    private void Awake()
    {
        StartCoroutine(PlayerEffect(effect));
    }
    private IEnumerator PlayerEffect(Effect Value)
    {
        float WavePos = 0.3f;
        switch (Value)
        {
            case Effect.Rotate:
                {
                    while (this.gameObject != null)
                    {
                        transform.Rotate(0, MotionSpeed * Time.deltaTime, 0);
                        yield return null;
                    }
                }
                break;
            case Effect.Wave:
                {
                    float Y = transform.position.y;
                    while (this.gameObject != null)
                    {
                        transform.DOMoveY(Y + WavePos / 2, 100 / MotionSpeed);
                        yield return new WaitForSeconds(100 / MotionSpeed);

                        WavePos *= -1;
                    }
                }
                break;
            case Effect.RotateWave:
                {
                    StartCoroutine(PlayerEffect(Effect.Rotate));
                    StartCoroutine(PlayerEffect(Effect.Wave));
                }
                break;
            default:
                break;
        }
    }
}
