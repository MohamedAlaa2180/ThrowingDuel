using UnityEngine;
using DG.Tweening;

public class SphereSkyBox : MonoBehaviour
{
    [Header("Floating")]
    [SerializeField] private Vector2 amplitudeRange = new Vector2(0.2f, 0.6f);
    [SerializeField] private Vector2 durationRange = new Vector2(1.5f, 3.5f);
    [SerializeField] private Vector3 axisWeight = new Vector3(0.15f, 1f, 0.15f);
    [SerializeField] private float startDelayMax = 0.5f;

    [Header("Rotation (optional)")]
    [SerializeField] private bool rotate = true;
    [SerializeField] private Vector3 rotationAxis = new Vector3(0f, 1f, 0f);
    [SerializeField] private Vector2 rotationSpeedRangeDegPerSec = new Vector2(5f, 20f);

    private Vector3 _initialLocalPos;
    private Tweener _floatTween;
    private Tweener _rotateTween;

    private void Awake()
    {
        _initialLocalPos = transform.localPosition;
    }

    private void OnEnable()
    {
        StartFloating();
        StartRotating();
    }

    private void OnDisable()
    {
        KillTweens();
        // Restore base position to avoid drift if needed
        transform.localPosition = _initialLocalPos;
    }

    private void StartFloating()
    {
        KillFloat();

        float amplitude = Random.Range(amplitudeRange.x, amplitudeRange.y);
        float duration = Random.Range(durationRange.x, durationRange.y);

        // Randomized offset, weighted by axisWeight so Y dominates
        Vector3 offset = new Vector3(
            Random.Range(-amplitude, amplitude) * axisWeight.x,
            Random.Range(-amplitude, amplitude) * axisWeight.y,
            Random.Range(-amplitude, amplitude) * axisWeight.z
        );

        float delay = startDelayMax > 0f ? Random.Range(0f, startDelayMax) : 0f;

        _floatTween = transform
            .DOLocalMove(_initialLocalPos + offset, duration)
            .SetDelay(delay)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetAutoKill(false);
    }

    private void StartRotating()
    {
        KillRotate();
        if (!rotate)
        {
            return;
        }

        float speed = Mathf.Max(0.01f, Random.Range(rotationSpeedRangeDegPerSec.x, rotationSpeedRangeDegPerSec.y));
        float period = 360f / speed; // seconds per full rotation

        // Use local rotation and loop continuously
        _rotateTween = transform
            .DOLocalRotate(rotationAxis.normalized * 360f, period, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart)
            .SetAutoKill(false);
    }

    private void KillTweens()
    {
        KillFloat();
        KillRotate();
    }

    private void KillFloat()
    {
        if (_floatTween != null && _floatTween.IsActive())
        {
            _floatTween.Kill();
            _floatTween = null;
        }
    }

    private void KillRotate()
    {
        if (_rotateTween != null && _rotateTween.IsActive())
        {
            _rotateTween.Kill();
            _rotateTween = null;
        }
    }
}
