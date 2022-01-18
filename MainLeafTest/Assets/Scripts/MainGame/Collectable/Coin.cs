using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : CollectableItem
{
    public int i_coinValor;
    public override void Action(Collider player)
    {
        player.GetComponent<PlayerController>().ChangeCoins(i_coinValor);
        Destroy(gameObject);
    }
}
