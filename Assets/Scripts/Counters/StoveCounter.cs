using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter
{
    [SerializeField] FryingRecipeSO[] fryingRecipeSOArray;

    private float fryingTimer;

    private void Update()
    {
        if (HasKitchenObject())
        {
            fryingTimer += Time.deltaTime;
            FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            if (fryingTimer >= fryingRecipeSO.fryingTimerMax)
            {
                // Fried
                GetKitchenObject().DestroySelf();
                fryingTimer = 0f;

                Debug.Log("Fried");

                KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
            }
            Debug.Log(fryingTimer);
        }
    }

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
                    // Player carrying something that can be fried
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

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;

    }

    private KitchenObjectSO GetOutputFromInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }
}
