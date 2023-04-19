using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Blade : MonoBehaviour
{
	private Camera mainCamera;
	private Collider bladeCollider;
	private TrailRenderer bladeTrail;
	private bool slicing;
	private int numberOfSliced = 0;
	// private float elapsedTime = 0f;
	

	public Vector3 direction {get; private set; }
	public float sliceForce = 5f;
	public float minSliceVelocity = 0.01f;
	public VoidEventChannel slicedFruit;
	public TextMeshPro textPrefab;
	// public float actionDelay = 0.5f;
	

	private void Awake()
	{
		mainCamera = Camera.main;
		bladeCollider = GetComponent<Collider>();
		bladeTrail = GetComponentInChildren<TrailRenderer>();
		slicedFruit.OnEventRaised += increaseNumberOfSliced;
	}
	
	private void OnEnable()
	{
		StopSlicing();
	}

	private void OnDisable()
	{
		StopSlicing();
	}

	private void Update()
	{
		if(Input.GetMouseButtonDown(0)){
			StartSlicing();
		}else if (Input.GetMouseButtonUp(0)){
			StopSlicing();
		}else if (slicing){
			ContinueSlicing();
		}

		// elapsedTime += Time.deltaTime;

        // if (elapsedTime >= actionDelay)
        // {
        //     PerformAction();
        //     elapsedTime = 0f; // Reset the timer if you want the action to be performed repeatedly
        // }


	}

	private void StartSlicing()
	{
		Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
		newPosition.z = 0f;

		transform.position = newPosition;

		slicing = true;
		bladeCollider.enabled = true;
		bladeTrail.enabled = true;
		bladeTrail.Clear();
	}

	private void StopSlicing()
	{
		slicing = false;
		bladeCollider.enabled = false;
		bladeTrail.enabled = false;
		numberOfSliced = 0;
	}

	private void ContinueSlicing()
	{
		Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
		newPosition.z = 0f;

		direction = newPosition - transform.position;

		float velocity = direction.magnitude / Time.deltaTime;
		bladeCollider.enabled = velocity > minSliceVelocity;

		transform.position = newPosition;
	}

	 private void increaseNumberOfSliced()
    {
      	numberOfSliced += 1;

		Vector3 mousePosition = Input.mousePosition;
		Ray ray = mainCamera.ScreenPointToRay(mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			TextMeshPro textInstance = Instantiate(textPrefab);
			textInstance.transform.position = hit.point;
			textInstance.gameObject.SetActive(true);
        	textInstance.text = "X" + numberOfSliced.ToString() + "!!";
			Destroy(textInstance, 0.7f);
		}
    }

}