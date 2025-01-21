using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private PlayerController player;
    public int treeCollisionCountForDestroy = 0;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnAxeSlashAnimationEnd()
    {
        if (player.addItemAfterAnimation)
        {
            player.AddItemToInventory(1); 
            treeCollisionCountForDestroy++;
            player.grabItem = false;
            player.addItemAfterAnimation = false; // Reset flag
        }
    }
}
