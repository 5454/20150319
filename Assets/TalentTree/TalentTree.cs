using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

    public class TalentTree : MonoBehaviour {

        enum LineType { UpVertical, DownVertical, UpLeftHorizontal, UpRightHorizontal, DownLeftHorizontal, DownRightHorizontal }


        public Vector2 nodeSpace = new Vector2(40f, 40f);
        public bool explainOnLeft = true;
        private Vector2 nodeSize = new Vector2();

        public Transform prefabNode;
        public Transform prefabLine;

        private TalentData treeData;
        private Transform treeContainer;

        private int maxRow = 0;
        private Talents maxWaysRowData;
        private Vector3 maxLeftPos;
        private Vector3 maxRightPos;

        public int currentLevel;

        void Start() {
            UISprite nodeSprite = prefabNode.Find("Background").GetComponent<UISprite>();
            nodeSize = new Vector2(nodeSprite.width, nodeSprite.height);
            maxLeftPos = GetMaxRowXAxis(true);
            maxRightPos = GetMaxRowXAxis(false);

            SuperDebug.Log4ConsoleOneLine(maxLeftPos, maxRightPos);

            BuildTheTree(TreeData, TreeContainer);

            this.GetComponent<DragTree>().dragContainer = treeContainer;
            //CreateTalentDataStruct();

        }

        void Update() {
        }

        void CreateTalentDataStruct() {
            TalentData td = new TalentData();
            td.levelTree = new Talents[10];

            for (int i = 0; i < 10; i++) {
                Talents talents = new Talents();
                talents.ways = new Talent[3];
                for (int j = 0; j < talents.ways.Length; j++) {
                    Talent tt = new Talent();
                    talents.ways.SetValue(tt, j);
                }
                td.levelTree.SetValue(talents, i);
            }
            //JsonUtils.SerializeObjectToJson(td, "TalentData.json");
        }

        public void ReflashNodeState() {
            for (int row = 0; row < treeData.levelTree.Length; row++) {
                for (int column = 0; column < treeData.levelTree[row].ways.Length; column++) {
                    if (int.Parse(treeData.levelTree[row].ways[column].codition) <= currentLevel) {
                        GetNode(row, column).GetComponent<UIButton>().isEnabled = true;
                    } else {
                        GetNode(row, column).GetComponent<UIButton>().isEnabled = false;
                    }
                }
            }
        }

        private Transform GetNode(int row, int column) {
            return treeContainer.Find("node_" + row + "_" + column);
        }


        private TalentData TreeData {
            get {
                if (treeData == null) {
                    //treeData = JsonUtils.JsonResourceToObject<TalentData>("Json/TalentData");
                }
                return treeData;
            }
        }

        private Transform TreeContainer {
            get {
                if (treeContainer == null) {
                    treeContainer = (new GameObject()).transform;
                    treeContainer.name = "TreeContainer";
                    treeContainer.parent = GameObject.Find("UI Root").transform;
                    treeContainer.localScale = new Vector3(1f, 1f, 1f);
                }
                return treeContainer;
            }
        }

        void BuildTheTree(TalentData data, Transform container) {
            for (int i = 0; i < data.levelTree.Length; i++) {
                int ways = data.levelTree[i].ways.Length;
                for (int j = 0; j < ways; j++) {
                    DrawNode(data.levelTree[i], container, nodeSize, i, j, ways);

                    if (data.levelTree[i].ways.Length > 0) {
                        DrawNodeVerticalLine(data, container, nodeSize, i, j, ways);
                        if (i > 0 && data.levelTree[i].ways.Length < data.levelTree[i - 1].ways.Length) {
                            DrawHorizontalLine(data, container, nodeSize, i, j, ways, -1);
                        }
                        if (i < data.levelTree.Length - 1 && data.levelTree[i].ways.Length <= data.levelTree[i + 1].ways.Length) {
                            DrawHorizontalLine(data, container, nodeSize, i, j, ways, 1);
                        }
                    }
                }
            }
        }

        void DrawNode(Talents data, Transform container, Vector2 nodeSize, int currentRow, int currentWay, int totalWays) {
            Transform nodeX = Instantiate(prefabNode) as Transform;
            nodeX.name = "node_" + currentRow + "_" + currentWay;
            nodeX.parent = container;
            nodeX.transform.localScale = Vector3.one;

            nodeX.transform.localPosition = NodePosition(nodeSize, nodeSpace, currentRow, currentWay, totalWays);

            nodeX.GetComponent<UIButton>().isEnabled = false;

            if (currentWay == 0) {
                DrawLevelExplain(data, nodeX.transform.localPosition, container);
            }

        }

        Vector3 NodePosition(Vector2 nodeSize, Vector2 nodeSpace, int currentRow, int currentWay, int totalWays) {
            float posX = -(nodeSize.x / 2 + nodeSpace.x / 2) * (totalWays - 1) + nodeSize.x * currentWay + nodeSpace.x * currentWay;
            float posY = Screen.height / 2 - nodeSpace.y - nodeSize.y / 2 - (nodeSize.y * currentRow + nodeSpace.y * currentRow);
            return new Vector3(posX, posY, 0f);
        }

        string GetPrevRowLinkedSockets(TalentData data, int currentRow, int currentWay) {
            string rt = "";
            if (currentRow > 0) {
                Talent[] ways = data.levelTree[currentRow - 1].ways;
                for (int i = 0; i < ways.Length; i++) {
                    if (ways[i].nextSockets.IndexOf("1", currentWay, 1) > -1) {
                        rt += "1";
                    } else {
                        rt += "0";
                    }
                }
            }
            return rt;
        }

        void DrawHorizontalLine(TalentData data, Transform container, Vector2 nodeSize, int currentRow, int currentWay, int totalWays, int rowOffset) {
            Talent[] drawWays = null;
            string thisWayNextSockets = "";
            //Down
            drawWays = data.levelTree[currentRow + rowOffset].ways;
            if (rowOffset == -1) {
                thisWayNextSockets = GetPrevRowLinkedSockets(data, currentRow, currentWay);
            } else {
                thisWayNextSockets = data.levelTree[currentRow].ways[currentWay].nextSockets;
            }

            if (thisWayNextSockets.IndexOf("1") < 0) { return; }

            float thisNodePos = NodePosition(nodeSize, nodeSpace, currentRow, currentWay, totalWays).x;
            float leftBorder = NodePosition(nodeSize, nodeSpace, currentRow + rowOffset, thisWayNextSockets.IndexOf("1"), drawWays.Length).x;
            float rightBorder = NodePosition(nodeSize, nodeSpace, currentRow + rowOffset, thisWayNextSockets.LastIndexOf("1"), drawWays.Length).x;
            if (thisNodePos > leftBorder && thisNodePos < rightBorder) { } else {
                if (thisNodePos > rightBorder) rightBorder = thisNodePos;
                if (thisNodePos < leftBorder) leftBorder = thisNodePos;
            }

            UISprite lineSprite = null;
            for (int i = 0; i < drawWays.Length; i++) {
                Vector3 nodePos = NodePosition(nodeSize, nodeSpace, currentRow + rowOffset, i, drawWays.Length);
                Transform horizontalLeftLine = null;
                Transform horizontalRightLine = null;
                float posX;
                float posY = nodePos.y + (nodeSize.y / 2 + nodeSpace.y / 2) * rowOffset;

                if (nodePos.x > leftBorder && nodePos.x <= rightBorder) {
                    horizontalLeftLine = DrawBaseLine(container, nodeSize, currentRow, currentWay, rowOffset == 1 ? LineType.DownLeftHorizontal : LineType.UpLeftHorizontal);
                    lineSprite = horizontalLeftLine.GetComponent<UISprite>();
                    lineSprite.width = Mathf.RoundToInt(nodeSize.x / 2 + nodeSpace.x / 2);
                    posX = nodePos.x - (nodeSize.x / 2 + nodeSpace.x / 2) / 2;
                    horizontalLeftLine.localPosition = new Vector3(posX, posY, nodePos.z);
                }
                if (nodePos.x >= leftBorder && nodePos.x < rightBorder) {
                    horizontalRightLine = DrawBaseLine(container, nodeSize, currentRow, currentWay, rowOffset == 1 ? LineType.DownRightHorizontal : LineType.UpRightHorizontal);
                    lineSprite = horizontalRightLine.GetComponent<UISprite>();
                    lineSprite.width = Mathf.RoundToInt(nodeSize.x / 2 + nodeSpace.x / 2);
                    posX = nodePos.x + (nodeSize.x / 2 + nodeSpace.x / 2) / 2;
                    horizontalRightLine.localPosition = new Vector3(posX, posY, nodePos.z);
                }
            }
        }

        void DrawNodeVerticalLine(TalentData data, Transform container, Vector2 nodeSize, int currentRow, int currentWay, int totalWays) {
            Transform node = container.Find("node_" + currentRow + "_" + currentWay);
            UISprite lineSprite = null;
            string prevLinkedSockets = "0";
            if (currentRow > 0) {
                for (int i = 0; i < data.levelTree[currentRow - 1].ways.Length; i++) {
                    prevLinkedSockets = Convert.ToString(Convert.ToInt32(prevLinkedSockets, 2) | Convert.ToInt32(data.levelTree[currentRow - 1].ways[i].nextSockets, 2), 2);
                    for (int j = 0; j < data.levelTree[currentRow].ways.Length - prevLinkedSockets.Length; j++) {
                        prevLinkedSockets = "0" + prevLinkedSockets;
                    }
                }
            }

            #region UpLine
            if (currentRow > 0) {
                if (prevLinkedSockets.IndexOf("1", currentWay, 1) > -1) {
                    Transform lineUp = DrawBaseLine(container, nodeSize, currentRow, currentWay, LineType.UpVertical);
                    lineUp.transform.localPosition = new Vector3(node.localPosition.x, node.localPosition.y + nodeSize.y / 2 + nodeSpace.y / 4, lineUp.localPosition.z);
                    lineSprite = lineUp.GetComponent<UISprite>();
                    lineSprite.height = Mathf.RoundToInt(nodeSpace.y / 2);
                }
            }
            #endregion

            #region DownLine
            if (currentRow < data.levelTree.Length - 1) {
                if (data.levelTree[currentRow].ways[currentWay].nextSockets.IndexOf("1") > -1) {
                    Transform lineDown = DrawBaseLine(container, nodeSize, currentRow, currentWay, LineType.DownVertical);
                    lineSprite = lineDown.GetComponent<UISprite>();
                    lineSprite.height = Mathf.RoundToInt(nodeSpace.y / 2);
                    lineDown.localPosition = new Vector3(node.localPosition.x, node.localPosition.y - nodeSize.y / 2 - nodeSpace.y / 4, lineDown.localPosition.z);
                }
            }
            #endregion

        }

        Transform DrawBaseLine(Transform container, Vector2 nodeSize, int currentRow, int currentWay, LineType lineType) {
            Transform line = Instantiate(prefabLine) as Transform;
            line.name = lineType.ToString() + "Line_" + currentRow + "_" + currentWay;
            line.parent = container;
            line.transform.localScale = Vector3.one;
            return line;
        }

        void DrawLevelExplain(Talents data, Vector3 rowPos, Transform container) {
            Transform explainNode = Instantiate(prefabNode) as Transform;
            explainNode.parent = container;
            explainNode.localPosition = new Vector3(maxLeftPos.x - nodeSize.x - nodeSpace.x, rowPos.y, rowPos.z);
            explainNode.localScale = Vector3.one;
            explainNode.name = "Level_" + data.ways[0].codition;
            Transform label = explainNode.Find("Label");
            label.transform.GetComponent<UILabel>().text = "Level " + data.ways[0].codition;
            label.gameObject.SetActive(true);

            explainNode.GetComponent<UIButton>().isEnabled = false;
        }

        Talents MaxWaysRowData {
            get {
                if (maxWaysRowData == null) {
                    int maxWays = 0;
                    for (int i = 0; i < TreeData.levelTree.Length; i++) {
                        int currentRowWays = TreeData.levelTree[i].ways.Length;
                        if (maxWays < currentRowWays) {
                            maxWays = currentRowWays;
                            maxRow = i;
                            maxWaysRowData = TreeData.levelTree[i];
                        }
                    }
                }
                return maxWaysRowData;
            }
        }

        Vector3 GetMaxRowXAxis(bool left = true) {
            Vector3 rt = new Vector3();
            if (left == true) {
                rt = NodePosition(nodeSize, nodeSpace, maxRow, 0, MaxWaysRowData.ways.Length);
            } else {
                rt = NodePosition(nodeSize, nodeSpace, maxRow, MaxWaysRowData.ways.Length - 1, MaxWaysRowData.ways.Length);
            }
            return rt;
        }

    }
