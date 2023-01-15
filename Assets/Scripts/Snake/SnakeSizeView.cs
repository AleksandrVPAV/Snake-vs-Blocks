using TMPro;
using UnityEngine;

[RequireComponent(typeof(Snake))]
public class SnakeSizeView : MonoBehaviour
{
    [SerializeField] private TMP_Text _view;

    private Snake _snake;

    private void Awake()
    {
        _snake = GetComponent<Snake>();
    }

    private void OnEnable()
    {
        _snake.SnakeSizeUpdated += OnSnakeSizeUpdated;
    }

    private void OnDisable()
    {
        _snake.SnakeSizeUpdated -= OnSnakeSizeUpdated;
    }

    private void OnSnakeSizeUpdated(int size)   // принимаем количество обьектов в хвосте
    {
        _view.text = size.ToString();
    }
}
