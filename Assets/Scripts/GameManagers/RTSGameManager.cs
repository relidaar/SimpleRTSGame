using System;
using Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameManagers
{
    public class RTSGameManager : MonoBehaviour
    {
        [SerializeField] private Text unitsText;
        [SerializeField] private Text coinsText;
        [SerializeField] private GameObject gameMenuPanel;
        [SerializeField] private Text gameMenuTitleText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button buyUnitButton;
        
        [SerializeField] private GameObject playerUnitPrefab;
        [SerializeField] private GameObject playerUnitSpawn;
        [SerializeField] private int coinPerUnit;
        [SerializeField] private int unitCost;
        
        [SerializeField] private int coinsSpawnCount;
        [SerializeField] private float coinsSpawnTimeout;
        private float _coinsSpawnTimer;

        [SerializeField] private GameObject enemyUnitPrefab;
        [SerializeField] private GameObject enemyUnitSpawn;
        [SerializeField] private int enemyUnitSpawnCount;
        [SerializeField] private float enemyUnitSpawnTimeout;
        [SerializeField] private float enemyAttackTimeout;
        private float _enemySpawnTimer;
        private float _enemyAttackTimer;
        
        private GameObject _playerCastle;

        public int PlayerCoins { get; private set; }

        private float fixedDeltaTime;

        void Awake()
        {
            fixedDeltaTime = Time.fixedDeltaTime;
        }
        
        private void Start()
        {
            restartButton.onClick.AddListener(RestartScene);
            mainMenuButton.onClick.AddListener(ToMainMenu);
            gameMenuPanel.gameObject.SetActive(false);
            
            buyUnitButton.onClick.AddListener(BuyUnit);
            buyUnitButton.gameObject.SetActive(false);

            var music = GetComponent<Music>();

            _playerCastle = GameObject.FindGameObjectWithTag("PlayerCastle"); 
            _playerCastle.GetComponent<CastleController>().CastleDestroyed += () =>
            {
                gameMenuTitleText.text = "Defeat";
                music.PlayDefeatMusic();
                OpenGameMenu();
            };
            
            GameObject.FindGameObjectWithTag("EnemyCastle").GetComponent<CastleController>().CastleDestroyed += () =>
            {
                gameMenuTitleText.text = "Victory";
                music.PlayVictoryMusic();
                OpenGameMenu();
            };

            foreach (var go in GameObject.FindGameObjectsWithTag("EnemyUnit"))
            {
                go.GetComponent<UnitController>().UnitDie += () => PlayerCoins += coinPerUnit;
            }
        }

        private void Update()
        {
            _enemyAttackTimer += Time.deltaTime;
            if (_enemyAttackTimer >= enemyAttackTimeout)
            {
                foreach (var enemy in GameObject.FindGameObjectsWithTag("EnemyUnit"))
                {
                    enemy.GetComponent<UnitController>().Attack(_playerCastle.transform);
                }                   
                _enemyAttackTimer = 0;
            }
            
            _enemySpawnTimer += Time.deltaTime;
            if (_enemySpawnTimer >= enemyUnitSpawnTimeout)
            {
                for (int i = 0; i < enemyUnitSpawnCount; i++)
                {
                    Instantiate(enemyUnitPrefab, enemyUnitSpawn.transform.position, enemyUnitPrefab.transform.rotation);
                    _enemySpawnTimer = 0;
                }
            }

            _coinsSpawnTimer += Time.deltaTime;
            if (_coinsSpawnTimer >= coinsSpawnTimeout)
            {
                PlayerCoins += coinsSpawnCount;
                _coinsSpawnTimer = 0;
            }
            
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (gameMenuPanel.activeSelf)
                {
                    CloseGameMenu();
                }
                else
                {
                    OpenGameMenu();
                }
            }
        }
        
        private void BuyUnit()
        {
            PlayerCoins -= unitCost;
            Instantiate(playerUnitPrefab, playerUnitSpawn.transform.position, playerUnitPrefab.transform.rotation);
        }

        private void OpenGameMenu()
        {
            StopTime();
            gameMenuPanel.gameObject.SetActive(true);
        }

        private void CloseGameMenu()
        {
            StartTime();
            gameMenuPanel.gameObject.SetActive(false);
        }

        private void ToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
            StartTime();
        }

        private void RestartScene()
        {      
            var scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
            StartTime();
        }

        private void StartTime()
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
        }

        private void StopTime()
        {
            Time.timeScale = 0;
        }

        public static void UnitTakeDamage(UnitController attacking, UnitController attacked)
        {
            if (attacked.Health <= 0) return;
            var damage = attacking.Stats.attackDamage;
            attacked.TakeDamage(attacking, damage);
        }

        public static void CastleTakeDamage(UnitController attacking, CastleController attacked)
        {
            if (attacked.Health <= 0) return;
            var damage = attacking.Stats.attackDamage;
            attacked.TakeDamage(attacking, damage);
        }

        private void OnGUI()
        {
            buyUnitButton.gameObject.SetActive(PlayerCoins >= unitCost);
            coinsText.text = $"Coins: {PlayerCoins}";
            unitsText.text = $"Units: {GameObject.FindGameObjectsWithTag("PlayerUnit").Length}";
        }
    }
}
