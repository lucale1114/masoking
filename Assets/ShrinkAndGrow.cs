using UnityEngine;

public class ShrinkAndGrow : MonoBehaviour
{
    private float _totalTime;
    private float _minimumScale;
    private AnimationCurve _animationCurve;

    private float _maxHeight;
    private float _currentTime;
    private Vector3 _originalScale;

    public void SetData(AnimationCurve animationCurve, float totalTime, float minimumScale)
    {
        _animationCurve = animationCurve;
        _totalTime = totalTime;
        _minimumScale = minimumScale;

        _originalScale = transform.localScale;

        _maxHeight = 0f;
        foreach (var animationCurveKey in _animationCurve.keys)
        {
            if (animationCurveKey.value > _maxHeight)
            {
                _maxHeight = animationCurveKey.value;
            }
        }
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;

        var currentScale = (1 - _minimumScale * _animationCurve.Evaluate(_currentTime / _totalTime));
        Debug.Log(currentScale);

        transform.localScale =
            new Vector3(_originalScale.x, _originalScale.y, _originalScale.z) * currentScale;
    }
}