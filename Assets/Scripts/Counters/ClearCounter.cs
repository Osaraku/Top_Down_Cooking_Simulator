using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // No KitchenObject here
            if (player.HasKitchenObject())
            {
                // Player carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // player is not carrying something
            }
        }
        else
        {
            // Has KitchenObject here
            if (player.HasKitchenObject())
            {
                // Player carrying something
            }
            else
            {
                // player is not carrying something
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
