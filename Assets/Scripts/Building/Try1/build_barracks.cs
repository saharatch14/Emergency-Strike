using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class build_barracks : MonoBehaviour
{
    public GameObject barracks_blueprint;

    public void spawn_barracks_blueprint()
    {
        Instantiate(barracks_blueprint);
    }
}
