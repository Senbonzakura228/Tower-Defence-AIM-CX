using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
    [SerializeField] private float speed = 0.05f;
    [SerializeField] private int maxHP = 30;
    const float REACH_DISTANCE = 0.3f;
    private Transform _targetDestination;
    private Coroutine _moveRoutine;
    private float _currentHP;
    private Vector3 _targetMoveDirection;

    public Vector3 TargetMoveDirection => _targetMoveDirection;

    void Start()
    {
        _currentHP = maxHP;
    }

    public void Init(Transform targetDestination)
    {
        transform.SetParent(null);
        _targetDestination = targetDestination;
        _moveRoutine = StartCoroutine(MoveToDestination());
    }

    public void TakeDamage(float damage)
    {
        _currentHP -= damage;
        if (_currentHP <= 0) Destroy(gameObject);
    }

    private IEnumerator MoveToDestination()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            CheckDestinationReaching();
            transform.Translate(PrepareMoveTranslation());
        }
    }

    private void CheckDestinationReaching()
    {
        if (!(Vector3.Distance(transform.position, _targetDestination.transform.position) <= REACH_DISTANCE)) return;
        StopCoroutine(_moveRoutine);
        Destroy(gameObject);
    }

    private Vector3 PrepareMoveTranslation()
    {
        var translation = _targetDestination.transform.position - transform.position;
        _targetMoveDirection = translation.normalized * (speed * Time.deltaTime);
        return _targetMoveDirection;
    }
}