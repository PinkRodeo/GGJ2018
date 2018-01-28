using UnityEngine;

public class SpriteMouseEvents : MonoBehaviour
{
	public System.Action MouseEnter;
	public System.Action MouseExit;
	public System.Action MouseDown;

	private void OnMouseDown()
	{
		MouseDown();
	}

	private void OnMouseExit()
	{
		MouseExit();
	}

	private void OnMouseEnter()
	{
		MouseEnter();
	}
}
