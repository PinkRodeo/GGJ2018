using UnityEngine;

public class ClickScript : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo))
			{
				iClickObject clickObject = hitInfo.transform.GetComponent<iClickObject>();
				if (clickObject != null)
				{
					clickObject.OnClicked();
				}
			}
		}
	}
}
