using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SettingMaterial : MonoBehaviour {

	public Vector4 offset;
	public float brightness = 2.78f;
	public float distance = 100;

	public Material[] materials;

	#if UNITY_EDITOR

	void Update () {
		for(int i = 0; i < materials.Length; i++){
			materials[i].shader = Shader.Find("Custom/Curved");;
			materials[i].SetVector("_QOffset", offset);
			materials[i].SetFloat("_Brightness", brightness);
			materials[i].SetFloat("_Dist", distance);
		}
	}
	#endif
}
