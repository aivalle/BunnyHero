using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSetValues : MonoBehaviour {


	Animator anim;

	void Start(){
		anim = GetComponent<Animator> ();
	}

	public void ToggleBool(string name)
	{
		anim.SetBool(name, !anim.GetBool(name));
	}

	public void ToggleBoolLogo(bool opc)
	{
		if (opc) {
			anim.SetBool("normal", true);
		} else {
			anim.SetBool("normal", false);
		}
	}
}
