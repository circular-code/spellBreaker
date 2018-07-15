﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GestureRecognizer
{

    /// <summary>
    /// Captures player drawing and call the Recognizer to discover which gesture player id.
    /// Calls 'OnRecognize' event when something is recognized.
    /// </summary>
    public class DrawDetector : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {

        public Recognizer recognizer;

        public UILineRenderer line;
        private List<UILineRenderer> lines;

        [Range(0f, 1f)]
        public float scoreToAccept = 0.8f;

        [Range(1, 10)]
        public int minLines = 1;
        public int MinLines { set { minLines = Mathf.Clamp(value, 1, 10); } }

        [Range(1, 10)]
        public int maxLines = 2;
        public int MaxLines { set { maxLines = Mathf.Clamp(value, 1, 10); } }

        public enum RemoveStrategy { RemoveOld, ClearAll }
        public RemoveStrategy removeStrategy;

        public bool clearNotRecognizedLines;

        public bool fixedArea = false;

        GestureData data = new GestureData();

        [System.Serializable]
        public class ResultEvent : UnityEvent<RecognitionResult, GestureData> { }
        public ResultEvent OnRecognize;

        public ResultEvent OnFailedToRecognize;

        RectTransform rectTransform;

        [SerializeField]
        private float recognizeDelay = 0.5f;
        private float recognizeCounter;
        private bool startRecognize = false;
        [SerializeField]
        private List<PointerEventData> recognizeData = new List<PointerEventData>();

        [SerializeField]
        private float clearTimer = 1f;
        private float clearCounter;

        [SerializeField]
        private float clearRecognizedTimer = 3f;
        private bool recognized = false;

        void Start()
        {
            line.relativeSize = true;
            line.LineList = false;
            lines = new List<UILineRenderer>() { line };
            rectTransform = transform as RectTransform;
            UpdateLines();
        }

        private void Update()
        {
            if(startRecognize)
            {
                recognizeCounter += Time.deltaTime;
                if(recognizeCounter >= recognizeDelay)
                {
                    List<PointerEventData> pointerData = new List<PointerEventData>();

                    foreach (PointerEventData data in recognizeData)
                    {
                        pointerData.Add(data);
                    }
                    StartCoroutine(OnEndDragCoroutine(pointerData));
                    startRecognize = false;
                    recognizeCounter = 0;
                    recognizeData.Clear();
                }
            }

            if(Lean.Touch.LeanTouch.Fingers.Count == 0 && !recognized && !Input.GetMouseButton(0))
            {
                clearCounter += Time.deltaTime;
                if(clearCounter >= clearTimer)
                {
                    ClearLines();
                    clearCounter = 0;
                }
            } else if(recognized)
            {
                clearCounter += Time.deltaTime;
                if (clearCounter >= clearRecognizedTimer)
                {
                    ClearLines();
                    clearCounter = 0;
                    recognized = false;
                }
            }
        }

        void OnValidate()
        {
            maxLines = Mathf.Max(minLines, maxLines);
        }

        public void UpdateLines()
        {
            while (lines.Count < data.lines.Count)
            {
                var newLine = Instantiate(line, line.transform.parent);
                lines.Add(newLine);
            }
            for (int i = 0; i < lines.Count; i++)
            {
                lines[i].Points = new Vector2[] { };
                lines[i].SetAllDirty();
            }
            int n = Mathf.Min(lines.Count, data.lines.Count);
            for (int i = 0; i < n; i++)
            {
                lines[i].Points = data.lines[i].points.Select(p => RealToLine(p)).ToArray();
                lines[i].SetAllDirty();
            }
        }

        Vector2 RealToLine(Vector2 position)
        {
            var local = rectTransform.InverseTransformPoint(position);
            var normalized = Rect.PointToNormalized(rectTransform.rect, local);
            return normalized;
        }

        Vector2 FixedPosition(Vector2 position)
        {
            return position;
            //var local = rectTransform.InverseTransformPoint (position);
            //var normalized = Rect.PointToNormalized (rectTransform.rect, local);
            //return normalized;
        }

        public void ClearLines()
        {
            Debug.Log("ClearLines");
            data.lines.Clear();
            UpdateLines();
        }

        public void OnPointerClick(PointerEventData eventData)
        {

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            startRecognize = false;
            recognizeCounter = 0;

            recognized = false;
            clearCounter = 0;
            //Debug.Log("OnBeginDrag");            

            if (Lean.Touch.LeanTouch.Fingers.Count > 0 || Input.GetMouseButton(0))
            {
                if (data.lines.Count >= maxLines)
                {
                    switch (removeStrategy)
                    {
                        case RemoveStrategy.RemoveOld:
                            data.lines.RemoveAt(0);
                            break;
                        case RemoveStrategy.ClearAll:
                            data.lines.Clear();
                            break;
                    }
                }

                data.lines.Add(new GestureLine());

                var fixedPos = FixedPosition(eventData.position);
                if (data.LastLine.points.Count == 0 || data.LastLine.points.Last() != fixedPos)
                {
                    data.LastLine.points.Add(fixedPos);
                    UpdateLines();
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log("OnDrag");
            startRecognize = false;
            recognizeCounter = 0;

            if (Lean.Touch.LeanTouch.Fingers.Count == 1)
            {
                var fixedPos = FixedPosition(eventData.position);
                if (data.LastLine.points.Count == 0 || data.LastLine.points.Last() != fixedPos)
                {
                    data.LastLine.points.Add(fixedPos);
                    UpdateLines();
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            startRecognize = true;
            recognizeCounter = 0;
            recognizeData.Add(eventData);
            //StartCoroutine(OnEndDragCoroutine(eventData));
        }

        IEnumerator OnEndDragCoroutine(List<PointerEventData> eventData)
        {
            //Debug.Log("OnEndDragRoutine");

            // zeichnet eine letzte linie an der mausposition
            // deaktiviert da erkennung verzögert wird
            //data.LastLine.points.Add(FixedPosition(eventData.position));
            //UpdateLines();

            for (int size = data.lines.Count; size >= 1 && size >= minLines; size--)
            {
                //last [size] lines
                var sizedData = new GestureData()
                {
                    lines = data.lines.GetRange(data.lines.Count - size, size)
                };

                var sizedNormalizedData = sizedData;

                if (fixedArea)
                {
                    var rect = this.rectTransform.rect;
                    sizedNormalizedData = new GestureData()
                    {
                        lines = sizedData.lines.Select(line => new GestureLine()
                        {
                            closedLine = line.closedLine,
                            points = line.points.Select(p => Rect.PointToNormalized(rect, this.rectTransform.InverseTransformPoint(p))).ToList()
                        }).ToList()
                    };
                }

                RecognitionResult result = null;
                //run in another thread

                var thread = new System.Threading.Thread(() =>
                {
                    result = recognizer.Recognize(sizedNormalizedData, normalizeScale: !fixedArea);
                });
                thread.Start();
                while (thread.IsAlive)
                {
                    yield return null;
                }

                if (result.gesture != null && result.score.score >= scoreToAccept)
                {
                    recognized = true;
                    clearCounter = 0;
                    OnRecognize.Invoke(result, sizedNormalizedData);
                    if (clearNotRecognizedLines)
                    {
                        data = sizedData;
                        UpdateLines();
                    }
                    break;
                }
                else
                {
                    OnRecognize.Invoke(RecognitionResult.Empty, null);
                }
            }
            yield return null;
        }

    }

}