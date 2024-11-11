using UnityEngine;
/// <summary>
/// 2D Background Scroller
/// </summary>
public class ScrollBackground : MonoBehaviour  {

	/// <summary>
	/// Scrolling speed of the texture.
	/// </summary>
	public float speed = 0.05F;
	private Renderer rend;
	
	void Start ()
	{
		//Renderer component
		rend = GetComponent<Renderer> ();
	}
	void Update ()
	{
		//Moving the offset with time and speed
		Vector2 offset = new Vector2(0,Time.time * speed);	
		//Aplying the offset to the texture
		rend.material.mainTextureOffset = offset;			
	}
}