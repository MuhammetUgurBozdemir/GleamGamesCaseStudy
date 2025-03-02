using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemView : MonoBehaviour
{
    [SerializeField] private int itemIndex;
    public int ItemIndex => itemIndex;
    public void Init()
    {
       transform.localPosition=Vector3.zero;
    }
}
