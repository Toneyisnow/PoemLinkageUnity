using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseActivity
{
    // Start is called before the first frame update
    public abstract void OnBeginning();

    public abstract void OnFinished();

    public abstract bool HasFinished();

    // Update is called once per frame
    public abstract void Update();



}
