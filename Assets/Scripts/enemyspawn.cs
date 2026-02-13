using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemyspawn : MonoBehaviour
{
    [Header("スポーン設定")]
    [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>(); // 敵プレハブのリスト
    [SerializeField] private float spawnInterval = 2f; // スポーン間隔（秒）
    [SerializeField] private float spawnDistance = 1f; // カメラ外からのスポーン距離
    [SerializeField] private bool autoSpawn = true; // 自動スポーンを有効にするか
    
    [Header("スポーン位置制限")]
    [SerializeField] private bool spawnFromTop = true;
    [SerializeField] private bool spawnFromBottom = true;
    [SerializeField] private bool spawnFromLeft = true;
    [SerializeField] private bool spawnFromRight = true;
    
    private Camera mainCamera;
    private float timer;
    
    void Start()
    {
        mainCamera = Camera.main;
        
        if (mainCamera == null)
        {
            Debug.LogError("メインカメラが見つかりません！");
            enabled = false;
            return;
        }
        
        if (enemyPrefabs.Count == 0)
        {
            Debug.LogWarning("敵プレハブが登録されていません！");
        }
    }
    
    void Update()
    {
        if (autoSpawn && enemyPrefabs.Count > 0)
        {
            timer += Time.deltaTime;
            
            if (timer >= spawnInterval)
            {
                SpawnEnemy();
                timer = 0f;
            }
        }
    }
    
    /// <summary>
    /// ランダムな敵をカメラ外にスポーンさせる
    /// </summary>
    public void SpawnEnemy()
    {
        if (enemyPrefabs.Count == 0) return;
        
        // ランダムな敵プレハブを選択
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        
        // カメラ外のランダムな位置を取得
        Vector3 spawnPosition = GetRandomSpawnPositionOutsideCamera();
        
        // 敵を生成
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
    
    /// <summary>
    /// カメラ外のランダムな位置を取得
    /// </summary>
    private Vector3 GetRandomSpawnPositionOutsideCamera()
    {
        // 有効なスポーン方向のリストを作成
        List<int> validDirections = new List<int>();
        if (spawnFromTop) validDirections.Add(0);
        if (spawnFromBottom) validDirections.Add(1);
        if (spawnFromLeft) validDirections.Add(2);
        if (spawnFromRight) validDirections.Add(3);
        
        if (validDirections.Count == 0)
        {
            Debug.LogWarning("スポーン方向が設定されていません！デフォルトで上からスポーンします。");
            validDirections.Add(0);
        }
        
        // ランダムな方向を選択
        int direction = validDirections[Random.Range(0, validDirections.Count)];
        
        Vector3 spawnPosition = Vector3.zero;
        float randomX, randomY;
        
        switch (direction)
        {
            case 0: // 上
                randomX = Random.Range(0f, 1f);
                spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(randomX, 1f + spawnDistance, mainCamera.nearClipPlane));
                break;
                
            case 1: // 下
                randomX = Random.Range(0f, 1f);
                spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(randomX, -spawnDistance, mainCamera.nearClipPlane));
                break;
                
            case 2: // 左
                randomY = Random.Range(0f, 1f);
                spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(-spawnDistance, randomY, mainCamera.nearClipPlane));
                break;
                
            case 3: // 右
                randomY = Random.Range(0f, 1f);
                spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(1f + spawnDistance, randomY, mainCamera.nearClipPlane));
                break;
        }
        
        // Z座標を0に設定（2Dゲームの場合）
        spawnPosition.z = 0f;
        
        return spawnPosition;
    }
    
    /// <summary>
    /// 特定の敵プレハブをスポーンさせる
    /// </summary>
    public void SpawnSpecificEnemy(int index)
    {
        if (index < 0 || index >= enemyPrefabs.Count)
        {
            Debug.LogError($"無効なインデックス: {index}");
            return;
        }
        
        Vector3 spawnPosition = GetRandomSpawnPositionOutsideCamera();
        Instantiate(enemyPrefabs[index], spawnPosition, Quaternion.identity);
    }
    
    /// <summary>
    /// 指定された位置に敵をスポーンさせる
    /// </summary>
    public void SpawnEnemyAtPosition(Vector3 position)
    {
        if (enemyPrefabs.Count == 0) return;
        
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        Instantiate(enemyPrefab, position, Quaternion.identity);
    }
}
