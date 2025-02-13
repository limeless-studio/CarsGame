using System.Collections.Generic;
using Snowy.Utils;
using UnityEngine;

namespace DefaultNamespace
{
    public class EndlessManager : MonoSingleton<EndlessManager>
    {
        [Title("References")]
        [SerializeField, ReorderableList] private Block[] blocks;
        [SerializeField] private Transform container;
        
        [Title("Settings")]
        [SerializeField, TagSelector] private string playerTag = "Player";
        [SerializeField] private int maxBlocksAtOnce = 10;
        
        [Title("Debug")]
        [SerializeField] private int currentBlockIndex = 0;
        [SerializeField, ReorderableList, Disable] private List<Block> blocksPool = new();
        
        private void Start()
        {
            for (int i = 0; i < maxBlocksAtOnce; i++)
            {
                CreateBlock();
            }
        }
        
        private void CreateBlock()
        {
            Block roadBlock = Instantiate(blocks[Random.Range(0, blocks.Length)], container);
            
            // get the last road block
            Block lastRoadBlock = blocksPool.Count > 0 ? blocksPool[blocksPool.Count - 1] : null;
            
            if (lastRoadBlock != null)
            {
                roadBlock.transform.position = lastRoadBlock.ConnectionPoint.position;
            }
            else
            {
                roadBlock.transform.position = container.position;
            }
            
            roadBlock.Init();
            
            blocksPool.Add(roadBlock);
        }
        
        private void CheckBlock()
        {
            int safety = 20;
            while (blocksPool.Count < maxBlocksAtOnce && safety > 0)
            {
                CreateBlock();
                safety--;
            }
        }
        
        public void OnEnterBlock(Block roadBlock, Collider other)
        {
            if (!other.CompareTag(playerTag)) return;
            
            currentBlockIndex = blocksPool.IndexOf(roadBlock);
            
            // Destroy
            DestroyNext();
            
            // Create a new road block
            CheckBlock();
        }
        
        private void DestroyNext()
        {
            // Check if the currentIndex is less than the destroyBefore
            if (currentBlockIndex <= 1) return;
            
            // Remove the first road block
            Destroy(blocksPool[0].gameObject);
            blocksPool.RemoveAt(0);
            
            // Update the currentRoadBlockIndex
            currentBlockIndex--;
        }
    }
}