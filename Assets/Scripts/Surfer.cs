using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Surfer : MonoBehaviour {
	public float fallSpeed = 0f;
	public float maxFallSpeed = -6f;
	public float turnRateLine = 90f;
	private Quaternion targetRotation;
	private int grounded = 0;
	private bool turning = false;
	private bool jumping = false;
	private bool canjump = false;
	public Train train;
	
    private Button pauseButton;
    private bool paused;
    private UIManager uimanager;

	public float[] speeds = new float[6] { 4f, 2f, 3f, 4f, 5f, 6f };
	public float[] gravities = new float[6] { 9.81f, 9.81f, 9.81f, 9.81f, 9.81f, 9.81f };
	public float[] turnRatesJump = new float[6] { 360.0f, 360.0f, 360.0f, 360.0f, 360.0f, 360.0f };

	private float startX;

	private float trickRotation = 0f;

	void Awake () {
		startX = transform.position.x;
	}

	void Update () {
		if (Input.GetButtonDown ("Jump") && canjump && !jumping) {
			GetComponent<Animator> ().Play ("jump");
			fallSpeed = speeds[Train.level];
			grounded = 0;
			jumping = true;
			turning = false;
			canjump = false;
		}

        /* Inputs mobile*/
        MobileJump();
		if (jumping) {
			if (Input.GetButton ("Jump") || Input.touchCount > 0) {
				float angle = -turnRatesJump[Train.level] * Time.deltaTime;
				transform.RotateAround (GetComponent<Collider2D> ().bounds.center, Vector3.back, angle);
				trickRotation += angle;
			} else {
				float angle = Quaternion.Angle (transform.rotation, Quaternion.identity);
				if (angle > 2.0f) {
					angle = (transform.right.y > 0 ? 1f : -1f) * turnRatesJump[Train.level] * Time.deltaTime;
					transform.RotateAround (GetComponent<Collider2D> ().bounds.center, Vector3.back, angle);
					trickRotation += angle;
				}
			}
		}
		if (grounded > 0) {
			fallSpeed = Mathf.Max (0f, fallSpeed);
		} else {
			fallSpeed -= gravities[Train.level] * Time.deltaTime;
			fallSpeed = Mathf.Max (maxFallSpeed, fallSpeed);
		}
		transform.position += new Vector3 (0f, grounded > 0 ? 0f : fallSpeed * Time.deltaTime);
		transform.position = new Vector3 (startX, transform.position.y);
		if (grounded > 0 || turning) {
			float angle = Quaternion.Angle (transform.rotation, targetRotation);
			if (angle <= 5.0f) {
				turning = false;
				canjump = true;
			} else {
				transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, turnRateLine / angle * Time.deltaTime);
			}
		}
	}

	void OnCollisionEnter2D (Collision2D coll) {
		if (coll.gameObject.tag == "Collector") {
			Destroy (coll.gameObject);
			Train.AddScore (5);
			return;
		}
		if (fallSpeed < 0 || grounded <= 0) {
			if (jumping && !turning) {
				bool success = Vector3.Angle (transform.right, coll.transform.up) < 90;
				int score = 0;
				if (success) {
					int rotations = (int) ((trickRotation - 90f) / -360f);
					for (int i = 1, j = 1; i <= rotations; i++, j += i) {
						score += j * 10;
					}
				} else {
					GetComponent<Animator> ().Play ("ouch");
				}
				train.Trick (success, score);
			}
			turning = true;
			jumping = false;
			trickRotation = 0f;
		}
		if (grounded > 0) {
			targetRotation = Quaternion.Lerp (coll.gameObject.transform.rotation * Quaternion.Euler (0f, 0f, 90f), targetRotation, 0.5f);
		} else {
			targetRotation = coll.gameObject.transform.rotation * Quaternion.Euler (0f, 0f, 90f);
		}
		grounded++;
	}

	void OnCollisionExit2D (Collision2D coll) {
		if (grounded > 0) {
			grounded--;
		}
	}

    public void MobileJump()
    {
        if (Input.touchCount > 0 && canjump && !jumping)
        {
            if (EventSystem.current.lastSelectedGameObject == GameObject.Find("PauseRestartInput") ||
                EventSystem.current.currentSelectedGameObject == GameObject.Find("PauseRestartInput"))
            {
                return;
            }
            GetComponent<Animator>().Play("jump");
            fallSpeed = speeds[Train.level];
            grounded = 0;
            jumping = true;
            turning = false;
            canjump = false;
        }
    }
}
