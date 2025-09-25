using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorM : MonoBehaviour
{
    public static bool Controller = false;

    public void OperatorChange()
    {
        Controller = !Controller;
        FindObjectOfType<Ondisplay>()?.OperationChange();
    }
}
