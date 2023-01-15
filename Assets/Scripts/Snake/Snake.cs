using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TailGenerator))]
[RequireComponent(typeof(SnakeInput))]
public class Snake : MonoBehaviour
{
    [SerializeField] private SnakeHead _head;
    [SerializeField] private float _speed;
    [SerializeField] private int _tailSize;
    [SerializeField] private float _tailSpingness;

    private SnakeInput _snakeInput;
    private List<Segment> _tail;
    private TailGenerator _tailGenerator;

    public event UnityAction<int> SnakeSizeUpdated;

    private void Start()
    {
        _snakeInput = GetComponent<SnakeInput>();
        _tailGenerator = GetComponent<TailGenerator>();

        _tail = _tailGenerator.Generate(_tailSize);      
        SnakeSizeUpdated?.Invoke(_tail.Count);  
    }

    private void OnEnable()
    {
        _head.BlockCollided += OnBlockCollided;
        _head.BonusCollected += OnBonusColled;
    }
       

    private void FixedUpdate()          
    {
        Move(_head.transform.position + _head.transform.up * _speed * Time.fixedDeltaTime);

        _head.transform.up = _snakeInput.GetDirectionToClick(_head.transform.position);
    }

    private void OnDisable()
    {
        _head.BlockCollided -= OnBlockCollided;
        _head.BonusCollected -= OnBonusColled;
    }

    private void Move(Vector3 nextPosition)
    {
        Vector3 previousPoint = _head.transform.position;

        foreach (var segment in _tail)
        {
            Vector3 tempPosition = segment.transform.position;     
            segment.transform.position = Vector2.Lerp(segment.transform.position, previousPoint, _tailSpingness * Time.deltaTime);
            previousPoint = tempPosition;
        }
        _head.Move(nextPosition);
    }

    private void OnBlockCollided()
    {
        Segment deletedSegment = _tail[_tail.Count - 1];   
        _tail.Remove(deletedSegment);
        Destroy(deletedSegment.gameObject);

        SnakeSizeUpdated?.Invoke(_tail.Count);
    }

    private void OnBonusColled(int bonusSize)
    {
       _tail.AddRange(_tailGenerator.Generate(bonusSize));        
       SnakeSizeUpdated?.Invoke(_tail.Count);
    }
}
