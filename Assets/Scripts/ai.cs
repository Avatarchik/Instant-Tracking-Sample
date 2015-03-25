using UnityEngine;
using System.Collections;

public class ai : MonoBehaviour
{

    public Transform Target;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(Target);

        Quaternion TargetRotation = Quaternion.LookRotation(Target.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, Time.deltaTime * 2);
    }
}
