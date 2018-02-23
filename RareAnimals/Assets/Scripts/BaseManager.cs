using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    public virtual void DestroyView()
    {
        Destroy(this.gameObject);
    }

    public virtual void InitView()
    {
        Debug.Log(" virtual Init View!");
    }
}
