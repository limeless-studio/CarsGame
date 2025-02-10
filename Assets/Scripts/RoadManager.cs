using System.Collections.Generic;
using Snowy.Utils;
using UnityEngine;

namespace DefaultNamespace
{
    public class RoadManager : MonoSingleton<RoadManager>
    {
        [Title("References")]
        [SerializeField, ReorderableList] private RoadBlock[] roadBlocks;
        [SerializeField] private Transform roadContainer;
        
        [Title("Settings")]
        [SerializeField, TagSelector] private string playerTag = "Player";
        [SerializeField] private int maxRoadBlocks = 10;
        
        [Title("Debug")]
        [SerializeField] private int currentRoadBlockIndex = 0;
        [SerializeField, ReorderableList, Disable] private List<RoadBlock> roadBlockPool = new();
        
        private void Start()
        {
            for (int i = 0; i < maxRoadBlocks; i++)
            {
                CreateRoadBlock();
            }
        }
        
        private void CreateRoadBlock()
        {
            RoadBlock roadBlock = Instantiate(roadBlocks[Random.Range(0, roadBlocks.Length)], roadContainer);
            
            // get the last road block
            RoadBlock lastRoadBlock = roadBlockPool.Count > 0 ? roadBlockPool[roadBlockPool.Count - 1] : null;
            
            if (lastRoadBlock != null)
            {
                roadBlock.transform.position = lastRoadBlock.ConnectionPoint.position;
            }
            else
            {
                roadBlock.transform.position = roadContainer.position;
            }
            
            roadBlock.Init();
            
            roadBlockPool.Add(roadBlock);
        }
        
        private void CheckRoad()
        {
            int safety = 20;
            while (roadBlockPool.Count < maxRoadBlocks && safety > 0)
            {
                CreateRoadBlock();
                safety--;
            }
        }
        
        public void OnEnterRoadBlock(RoadBlock roadBlock, Collider other)
        {
            if (!other.CompareTag(playerTag)) return;
            
            currentRoadBlockIndex = roadBlockPool.IndexOf(roadBlock);
            
            // Destroy
            DestroyNext();
            
            // Create a new road block
            CheckRoad();
        }
        
        private void DestroyNext()
        {
            // Check if the currentIndex is less than the destroyBefore
            if (currentRoadBlockIndex <= 1) return;
            
            // Remove the first road block
            Destroy(roadBlockPool[0].gameObject);
            roadBlockPool.RemoveAt(0);
            
            // Update the currentRoadBlockIndex
            currentRoadBlockIndex--;
        }
    }
}