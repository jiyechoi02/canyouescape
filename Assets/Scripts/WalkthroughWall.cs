using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkthroughWall : MonoBehaviour
{
    void OnEnable()
    {
        gameObject.layer = LayerMask.NameToLayer("Ghost");
    }
    void OnDisable()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
