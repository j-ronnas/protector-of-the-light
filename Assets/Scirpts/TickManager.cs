using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickManager : MonoBehaviour
{

    Action tickActions;
    // Start is called before the first frame update
    

    public void TriggerTick()
    {
        tickActions();
    }


    public void AddTickAction(Action tickAction)
    {
        tickActions += tickAction;
    }

    public void RemoveTickAction(Action tickAction)
    {
        tickActions -= tickAction;
    }
    

    public void ClearTickActions()
    {
        tickActions = null;
    }
}
