using UnityEngine;
using System.Collections;

[RequireComponent (typeof (LineRenderer))]
public class ElectricalLine : MonoBehaviour {
	public Transform leftPole;
	public Transform rightPole;
	private BezierSpline spline;
	private LineRenderer lineRenderer;

	private int curveCount = 0;
	private int layerOrder = 0;
	private int SEGMENT_COUNT = 50;
	
	void Awake () {
		if (!spline) {
			spline = GetComponent<BezierSpline> ();
        }
    }

    private void Start()
    {
        GetComponent<SplineDecorator>().enabled = true;
        ResetLine();
    }

    void Update () {
		DrawCurve ();
	}

	public void ResetLine () {
		float d = rightPole.position.x - leftPole.position.x;
		transform.position = Vector3.zero;
		spline.points[0] = leftPole.position;
		spline.points[3] = rightPole.position;

		Vector3 cpDirection = new Vector3 (d, -(d /= 3.0f) * 2.0f);
		Vector3 verticalDifference = new Vector3 (0f, (leftPole.position.y - rightPole.position.y) / 3f);

		spline.points[1] = leftPole.position + Vector3.ClampMagnitude (cpDirection, d) - verticalDifference;
		cpDirection.x = -cpDirection.x;
		spline.points[2] = rightPole.position + Vector3.ClampMagnitude (cpDirection, d) + verticalDifference;

		if (!lineRenderer) {
			lineRenderer = GetComponent<LineRenderer> ();
		}
		lineRenderer.sortingLayerID = layerOrder;
		curveCount = (int) spline.points.Length / 3;

		GetComponent<SplineDecorator> ().Decorate ();
		//transform.position = leftPole.position;
	}

	void DrawCurve () {
		for (int j = 0; j < curveCount; j++) {
			for (int i = 1; i <= SEGMENT_COUNT; i++) {
				float t = i / (float) SEGMENT_COUNT;
				int nodeIndex = j * 3;
				Vector3 pixel = CalculateCubicBezierPoint (t, spline.points[nodeIndex], spline.points[nodeIndex + 1], spline.points[nodeIndex + 2], spline.points[nodeIndex + 3]);
				lineRenderer.SetVertexCount (((j * SEGMENT_COUNT) + i));
				lineRenderer.SetPosition ((j * SEGMENT_COUNT) + (i - 1), pixel);
			}
		}
	}

	Vector3 CalculateCubicBezierPoint (float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
		float u = 1 - t;
		float tt = t * t;
		float uu = u * u;
		float uuu = uu * u;
		float ttt = tt * t;

		Vector3 p = uuu * p0;
		p += 3 * uu * t * p1;
		p += 3 * u * tt * p2;
		p += ttt * p3;

		return p;
	}
}
