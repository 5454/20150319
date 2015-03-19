using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Colr {
    public class CameraControl : MonoBehaviour {

        public bool DEBUG = true;
        public Camera controlCamera;
        public Transform manualShell;

        #region InitCamera

        public float fieldOfView = 50;
        public float initVerticalAngle = 45f;
        public float initHorizontalAngle = 0f;

        public float initVerticalPos = 5.5f;
        public float initHorizontalPos = 0f;
        public float initForwardBackwardPos = -6.5f;

        void InitCamera() {
            controlCamera.transform.localPosition = new Vector3(initHorizontalPos, initVerticalPos, initForwardBackwardPos);
            controlCamera.transform.localEulerAngles = new Vector3(initVerticalAngle, initHorizontalAngle, 0f);
            controlCamera.fieldOfView = fieldOfView;
        }
        #endregion

        #region InitGround
        public Transform controlGround;
        public bool groundSizeIsX = true;
        public bool groundSizeIsY = false;
        public bool groundSizeIsZ = false;
        public Vector3 groundSize = new Vector3(30f, 30f, 0f);
        public Vector3 groundCenter;

        private float topBorderLineZPos;
        private float bottomBorderLineZPos;

        private void InitGround() {
            if (controlGround != null) {
                Vector3 groundBoundSize = controlGround.GetComponent<MeshRenderer>().bounds.size;
                if (groundSizeIsX) groundSize = groundBoundSize;
                if (groundSizeIsY) groundSize = new Vector3(groundBoundSize.y, groundBoundSize.x, groundBoundSize.z);
                if (groundSizeIsZ) groundSize = new Vector3(groundBoundSize.z, groundBoundSize.y, groundBoundSize.x);
                groundCenter = controlGround.position;
            }
        }
        #endregion

        private bool startFollow = false;

        public float minAngle = 15f;
        public float maxAngel = 45f;

        public float minPolygonLimitSize = 1f;
        public float maxPolygonLimitSize = 6F;

        public float minDistance = 3.5f;
        public float maxDistance = 9f;

        public float moveSpeed = 3f;
        public float rotateSpeed = 3f;

        public float manualHorizontalAngle = 40f;
        public float manualCenterPointYOffset = 0f;



        //******************************************************************

        public List<GameObject> targets;

        private GameObject minX;
        private GameObject minY;
        private GameObject minZ;

        private GameObject maxX;
        private GameObject maxY;
        private GameObject maxZ;

        private Vector3 taregetCenterPoint;

        private bool startDrag = false;
        private Vector3 startDragPoint;
        private float lastDragAngle;

        void Start() {
            InitCamera();
            InitGround();
        }

        void Awake() {
            //XAI_GlobalEvent.CharDeadListener += OnCharDead;
        }

        void OnCharDead(GameObject ai) {
            targets.Remove(ai);
        }

        public bool StartFollow {
            set {
                startFollow = value;
                controlCamera.fieldOfView = fieldOfView;
            }
        }

        void Update() {
            #region IOList
            /*
            if (Input.GetMouseButtonUp(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                Physics.Raycast(ray, out hitInfo);
                start = true;
                if (hitInfo.collider != null) {
                    if (hitInfo.collider.tag == "Target") {
                        Debug.Log(hitInfo.collider.name);
                        if (!targets.Contains(hitInfo.collider.gameObject)) {
                            targets.Add(hitInfo.collider.gameObject);
                        } else {
                            targets.Remove(hitInfo.collider.gameObject);
                        }

                        lookAtPoint = GetCenterPoint();
                    }
                }
            }
            #endregion

            #region ScrollWhell
            if (Input.GetAxis("Mouse ScrollWheel") < 0) {
                if (lockAngleOnMove) {
                    Camera.main.transform.Translate(Vector3.back);
                } else {
                    Camera.main.transform.position += Vector3.back;
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0) {
                if (lockAngleOnMove) {
                    Camera.main.transform.Translate(Vector3.forward);
                } else {
                    Camera.main.transform.position += Vector3.forward;
                }
            }
            **/
            #endregion
            if (startFollow == true) {
                CameraFollow();
                if (Input.GetMouseButtonDown(0)) {
                    startDrag = true;
                    startDragPoint = Input.mousePosition;
                }
                if (Input.GetMouseButton(0) && startDrag == true) {
                    float angle = Input.mousePosition.x - startDragPoint.x;

                    if (angle > 0) {
                        manualShell.Rotate(Vector3.up);
                        if (manualShell.eulerAngles.y > 40f && manualShell.eulerAngles.y < 320f) {
                            manualShell.eulerAngles = new Vector3(manualShell.eulerAngles.x, 40f, manualShell.eulerAngles.z);
                        }
                    } else {
                        manualShell.Rotate(Vector3.down);
                        if (manualShell.eulerAngles.y < 320f && manualShell.eulerAngles.y > 40f) {
                            manualShell.eulerAngles = new Vector3(manualShell.eulerAngles.x, 320f, manualShell.eulerAngles.z);
                        }
                    }
                }
                if (Input.GetMouseButtonUp(0)) {
                    startDrag = false;
                }
            }
        }


        void FixedUpdate() {
            taregetCenterPoint = GetCenterPoint();
        }

        void OnGUI() {
            if (DEBUG && targets.Count > 0) {
                GUIStyle gs = new GUIStyle("Box");
                gs.alignment = TextAnchor.UpperLeft;
                GUI.Box(new Rect(0, 0, 300, Screen.height),
                    "MinZPosition : " + minZ.transform.position.z.ToString()
                    + "\n" +
                    "MaxZPosition : " + maxZ.transform.position.z.ToString()
                    + "\n" +
                    "MinXPosition : " + minX.transform.position.x.ToString()
                    + "\n" +
                    "MaxXPosition : " + maxX.transform.position.x.ToString()
                    + "\n" +
                    "CameraVerticalAngle : " + controlCamera.transform.localEulerAngles.x.ToString()
                    + "\n" +
                    "-----------------------------------------------------------------------------------------------------"
                    + "\n" +
                    "MaxZName : " + maxZ.name
                    + "\n" +
                   "-----------------------------------------------------------------------------------------------------"
                    + "\n" +
                    "CurrentAngle : " + CurrentAngle.ToString()
                    + "\n" +
                    "CurrentPolygonSize : " + CurrentPolygonSize.ToString()
                    + "\n" +
                     "CurrentDistance : " + Vector3.Distance(taregetCenterPoint, controlCamera.transform.localPosition).ToString()
                    + "\n" +
                   "WantToDistance : " + CurrentDistance.ToString()
                    , gs);
            }
        }

        private void CameraFollow() {
            if (DEBUG) {
                Debug.DrawLine(controlCamera.transform.position, taregetCenterPoint, Color.blue);
            }

            AutoAdjustAngle();
            AutoAdjustPosition();
            AutoResetWorldAngle();
        }

        void LateUpdate() {
        }

        private void AutoResetWorldAngle() {
            if (manualShell.eulerAngles.y != 0f && (manualShell.eulerAngles.y < 10f || manualShell.eulerAngles.y > 350f)) {
                RotateFromAToB(manualShell, manualShell.eulerAngles.y, 0f, Vector3.up, rotateSpeed);
            }
        }

        private void AutoAdjustPosition() {
            if (targets.Count <= 0) return;
            float currentPolygonSize = CurrentPolygonSize;
            Vector3 targetPosition = new Vector3();

            Vector3 tempTargetPosition = new Vector3(taregetCenterPoint.x, taregetCenterPoint.y + CurrentDistance, taregetCenterPoint.z);

            Vector3 vvv = taregetCenterPoint - controlCamera.transform.position;
            vvv.Normalize();

            targetPosition = Quaternion.AngleAxis(90 - CurrentAngle, Vector3.left) * tempTargetPosition;

            //Debug.Log(90 - AreaAngle + "      " + taregetCenterPoint + "       " + tempTargetPosition + "       " + targetPosition);

            if (targetPosition.y < 2.5f) {
                targetPosition = new Vector3(targetPosition.x, 2.5f, targetPosition.z);
            }
            controlCamera.transform.localPosition = Vector3.Slerp(controlCamera.transform.localPosition, targetPosition, Time.deltaTime * moveSpeed);
            //controlCamera.transform.position = targetPosition;

        }

        #region AutoAngle
        private void RotateFromAToB(Transform obj, float angleA, float angleB, Vector3 direction, float speed) {
            obj.localRotation = Quaternion.Slerp(Quaternion.AngleAxis(angleA, direction), Quaternion.AngleAxis(angleB, direction), Time.deltaTime * speed);
        }

        private void AutoAdjustAngle() {
            if (targets.Count <= 0) return;
            float cameraAngle = CurrentAngle;
            if (cameraAngle != controlCamera.transform.localEulerAngles.x) {
                RotateFromAToB(controlCamera.transform, controlCamera.transform.localEulerAngles.x, cameraAngle, Vector3.right, rotateSpeed);
            }
        }

        private float CurrentAngle {
            get {
                float currentPolygonSize = CurrentPolygonSize;
                float rt = 0f;
                if (currentPolygonSize >= maxPolygonLimitSize) {
                    rt = maxAngel;
                } else if (currentPolygonSize <= minPolygonLimitSize) {
                    rt = minAngle;
                } else {
                    float polygonDistance = maxPolygonLimitSize - minPolygonLimitSize;
                    float polygonPercent = currentPolygonSize / polygonDistance;
                    rt = minAngle + (maxAngel - minAngle) * polygonPercent;
                }
                return rt;
            }
        }
        #endregion



        private float CurrentDistance {
            get {
                float currentPolygonSize = CurrentPolygonSize;
                float rt = 0;
                if (currentPolygonSize >= maxPolygonLimitSize) {
                    rt = maxDistance;
                } else if (currentPolygonSize <= minPolygonLimitSize) {
                    rt = minDistance;
                } else {
                    float polygonDistance = maxPolygonLimitSize - minPolygonLimitSize;
                    float polygonPercent = currentPolygonSize / polygonDistance;
                    rt = minDistance + (maxDistance - minDistance) * polygonPercent;
                }
                return rt;
            }
        }

        private float CurrentPolygonSize {
            get {
                float rt = 0f;
                if (targets.Count > 1) {
                    float xSize = 0f;
                    float zSize = 0f;
                    if (minX.transform.position.x < 0 && maxX.transform.position.x < 0 || minX.transform.position.x > 0 && maxX.transform.position.x > 0) {
                        xSize = Mathf.Abs(minX.transform.position.x - maxX.transform.position.x);
                    } else {
                        xSize = Mathf.Abs(Mathf.Abs(minX.transform.position.x) + Mathf.Abs(maxX.transform.position.x));
                    }
                    if (minZ.transform.position.z < 0 && maxZ.transform.position.z < 0 || minZ.transform.position.z > 0 && maxZ.transform.position.z > 0) {
                        zSize = Mathf.Abs(minZ.transform.position.z - maxZ.transform.position.z);
                    } else {
                        zSize = Mathf.Abs(Mathf.Abs(minZ.transform.position.z) + Mathf.Abs(maxZ.transform.position.z));
                    }
                    rt = Mathf.Max(xSize, zSize);
                }
                return rt;
            }
        }

        private float CameraAndCenterDistance {
            get {
                return Vector3.Distance(controlCamera.transform.localPosition, taregetCenterPoint);
            }
        }

        void GetLimitPoint() {
            if (targets.Count <= 0) return;
            Dictionary<float, GameObject> pointListX = new Dictionary<float, GameObject>();
            Dictionary<float, GameObject> pointListY = new Dictionary<float, GameObject>();
            Dictionary<float, GameObject> pointListZ = new Dictionary<float, GameObject>();
            float[] xList = new float[targets.Count];
            float[] yList = new float[targets.Count];
            float[] zList = new float[targets.Count];
            for (int i = 0; i < targets.Count; i++) {
                pointListX[targets[i].transform.position.x] = targets[i];
                pointListY[targets[i].transform.position.y] = targets[i];
                pointListZ[targets[i].transform.position.z] = targets[i];
                xList.SetValue(targets[i].transform.position.x, i);
                yList.SetValue(targets[i].transform.position.y, i);
                zList.SetValue(targets[i].transform.position.z, i);
            }


            float minXValue = Mathf.Min(xList);
            float minYValue = Mathf.Min(yList);
            float minZValue = Mathf.Min(zList);

            float maxXValue = Mathf.Max(xList);
            float maxYValue = Mathf.Max(yList);
            float maxZValue = Mathf.Max(zList);

            if (pointListX.ContainsKey(minXValue)) {
                minX = pointListX[minXValue];
            } else { minX = null; }

            if (pointListY.ContainsKey(minYValue)) {
                minY = pointListY[minYValue];
            } else { minY = null; }

            if (pointListZ.ContainsKey(minZValue)) {
                minZ = pointListZ[minZValue];
            } else { minZ = null; }

            if (pointListX.ContainsKey(maxXValue)) {
                maxX = pointListX[maxXValue];
            } else { maxX = null; }

            if (pointListY.ContainsKey(maxYValue)) {
                maxY = pointListY[maxYValue];
            } else { maxY = null; }

            if (pointListZ.ContainsKey(maxZValue)) {
                maxZ = pointListZ[maxZValue];
            } else { maxZ = null; }

        }

        Vector3 GetCenterPoint() {
            GetLimitPoint();
            if (minX == null || maxX == null || minY == null || maxY == null || minZ == null || maxZ == null) { return Vector3.zero; }
            float xCenter = Vector3.Lerp(minX.transform.position, maxX.transform.position, 0.5f).x;
            float yCenter = Vector3.Lerp(minY.transform.position, maxY.transform.position, 0.5f).y;
            float zCenter = Vector3.Lerp(minZ.transform.position, maxZ.transform.position, 0.5f).z;
            return new Vector3(xCenter, yCenter + manualCenterPointYOffset, zCenter);
        }















    }
}