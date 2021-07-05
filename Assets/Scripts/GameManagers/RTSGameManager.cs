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
        [SerializeField] private Text gameOverText;
        [SerializeField] private Button restartButton;
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
        
        private void Start()
        {
            restartButton.onClick.AddListener(RestartScene);
            restartButton.gameObject.SetActive(false);
            
            buyUnitButton.onClick.AddListener(BuyUnit);
            buyUnitButton.gameObject.SetActive(false);
            
            gameOverText.enabled = false;

            var music = GetComponent<Music>();

            _playerCastle = GameObject.FindGameObjectWithTag("PlayerCastle"); 
            _playerCastle.GetComponent<CastleController>().CastleDestroyed += () =>
            {
                gameOverText.text = "Defeat";
                music.PlayDefeatMusic();
                GameOver();
            };
            
            GameObject.FindGameObjectWithTag("EnemyCastle").GetComponent<CastleController>().CastleDestroyed += () =>
            {
                gameOverText.text = "Victory";
                music.PlayVictoryMusic();
                GameOver();
            };

            foreach (var go in GameObject.FindGameObjectsWithTag("EnemyUnit"))
                go.GetComponent<UnitController>().UnitDie += () => PlayerCoins += coinPerUnit;
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
        }
        
        private void BuyUnit()
        {
            PlayerCoins -= unitCost;
            Instantiate(playerUnitPrefab, playerUnitSpawn.transform.position, playerUnitPrefab.transform.rotation);
        }

        private void GameOver()
        {
            Time.timeScale = 0;
            gameOverText.enabled = true;
            restartButton.gameObject.SetActive(true);
        }
        
        public void RestartScene()
        {      
            var scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
            Time.timeScale = 1;
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
