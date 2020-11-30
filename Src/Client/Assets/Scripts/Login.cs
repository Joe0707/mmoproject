using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Network.NetClient.Instance.Connect();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
