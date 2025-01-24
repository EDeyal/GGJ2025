using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WaveUpgrade : MonoBehaviour
{
    public abstract void Activate();
    public abstract bool CheckIsAvailable();
    public abstract string Description();
}
