using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // No KitchenObject here
            if (player.HasKitchenObject())
            {
                // Player carrying something
                if (HasRecipeWithInput((player.GetKitchenObject().GetKitchenObjectSO())))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                }
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

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            // There is KithenObject here && has recipe
            KitchenObjectSO outputKitchenObjectSO = GetOutputFromInput(GetKitchenObject().GetKitchenObjectSO());
            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return true;
            }
        }
        return false;

    }

    private KitchenObjectSO GetOutputFromInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO.output;
            }
        }
        return null;
    }
}
