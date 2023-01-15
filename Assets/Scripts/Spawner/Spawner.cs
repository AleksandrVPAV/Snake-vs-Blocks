using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Transform _container;
    [SerializeField] private int _repeatCount;
    [SerializeField] private int _distanceBetweenFullLines;
    [SerializeField] private int _distanceBetweenRandomLines;
    [Header("Block")]
    [SerializeField] private Block _blockTemplate;
    [SerializeField] private int _blockSpawnChance;
    [Header("Wall")]
    [SerializeField] private Wall _wallTemplate;
    [SerializeField] private int _wallSpawnChance;
    [Header("Frontier")]
    [SerializeField] private Frontier _frontier;
    [Header("Bonus")]
    [SerializeField] private Bonus _bonus;
    [SerializeField] private int _bonusSpawnChance;

    private BonusSpawnPoint[] _bonusSpawnPoints;
    private BlockSpawnPoint[] _blockSpawnPoints;
    private WallSpawnPoint[] _wallSpawnPoints;
    private FrontierSpawnPoint[] _frontierSpawnPoints;

    private int _frontierSpawnChance = 100;

    private void Start()
    {
        SpawnLevel();
    }

    private void SpawnLevel()
    {
        _bonusSpawnPoints = GetComponentsInChildren<BonusSpawnPoint>();
        _frontierSpawnPoints = GetComponentsInChildren<FrontierSpawnPoint>();
        _blockSpawnPoints = GetComponentsInChildren<BlockSpawnPoint>();      // заполняем массив списком дочерних SpawnPoint 
        _wallSpawnPoints = GetComponentsInChildren<WallSpawnPoint>();

        for (int i = 0; i < _repeatCount; i++)
        {
            MoveSpawner(_distanceBetweenFullLines);
            GenerateRandomElements(_wallSpawnPoints, _wallTemplate.gameObject, _wallSpawnChance, _distanceBetweenFullLines / 2);
            GenerateFullLine(_blockSpawnPoints, _blockTemplate.gameObject);
            GenerateRandomElements(_frontierSpawnPoints, _frontier.gameObject, _frontierSpawnChance);
            MoveSpawner(_distanceBetweenRandomLines);
            GenerateRandomElements(_wallSpawnPoints, _wallTemplate.gameObject, _wallSpawnChance, _distanceBetweenRandomLines / 2);
            GenerateRandomElements(_blockSpawnPoints, _blockTemplate.gameObject, _blockSpawnChance);
            GenerateRandomElements(_bonusSpawnPoints, _bonus.gameObject, _bonusSpawnChance);
        }
    }

    private void GenerateFullLine(SpawnPoint[] spawnPoints, GameObject generatedElement)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GenerateElement(spawnPoints[i].transform.position, generatedElement);
        }
    }

    private void GenerateRandomElements(SpawnPoint[] spawnPoints, GameObject generatedElement, int spawnChance, int scaleY = 1)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (Random.Range(0, 100) < spawnChance)
            {
                GameObject element = GenerateElement(spawnPoints[i].transform.position, generatedElement);
                if (element.TryGetComponent(out Wall wall))
                {
                    element.transform.localScale = new Vector3(element.transform.localScale.x, scaleY, element.transform.localScale.x);
                }             
            }
        }
    }

    private GameObject GenerateElement(Vector3 spawnPont, GameObject generatedElement)
    {        
        GameObject TemplateGeneratedElement = Instantiate(generatedElement, spawnPont, Quaternion.identity, _container);
       
        if (TemplateGeneratedElement.TryGetComponent(out Block block))
        {
            TemplateGeneratedElement.transform.Rotate(0, 0, 90);
        }        
        if (TemplateGeneratedElement.TryGetComponent(out Wall wall))
        {
            TemplateGeneratedElement.transform.position -= TemplateGeneratedElement.transform.localScale;
        }
        if (TemplateGeneratedElement.TryGetComponent(out Frontier frontier))
        {
            TemplateGeneratedElement.transform.localScale = new Vector3(TemplateGeneratedElement.transform.localScale.x, _distanceBetweenRandomLines*3, TemplateGeneratedElement.transform.localScale.z);
        }
        return TemplateGeneratedElement;
    }

    private void MoveSpawner(int distanceY)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + distanceY, transform.position.z); 
    }   
}
