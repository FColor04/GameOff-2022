using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : Graphic
{
    public enum InsideShape
    {
        Nothing,
        Dot,
        Cross,
        NGon
    }
    public enum OutsideShape
    {
        Nothing,
        Lines,
        HLines,
        VLines,
        NGon,
    }
    public enum CrosshairBehaviour
    {
        Nothing,
        Offset,
        InverseOffset,
        Breathe,
        Spin
    }
    
    [Header("Inside")]
    public InsideShape insideShape;
    [Range(3, 30)]
    public int insideNgon = 3;
    public Color insideColor = Color.white;
    private float _insideOffset;
    public float insideOffset;
    public float insideThickness;
    private float _insideRotation;
    public float insideRotation;
    
    [Header("Outside")]
    public OutsideShape outsideShape;
    [Range(3, 30)] 
    public int outsideNgon = 3;
    public Color outsideColor = Color.white;
    private float _outsideOffset;
    public float outsideOffset;
    private float _outsideRotation;
    public float outsideRotation;
    public float outsideThickness;
    public float outsideLength;

    public CrosshairBehaviour insideBehaviour;
    public CrosshairBehaviour outsideBehaviour;
    public float outsideBehaviourAmount;
    public float outsideBehaviourDecay;
    private RectTransform _rectTransform;
    private float _timer;

    protected override void Awake()
    {
        _insideOffset = insideOffset;
        _insideRotation = insideRotation;
        
        _outsideOffset = outsideOffset;
        _outsideRotation = outsideRotation;
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();

        if (!Application.isPlaying)
        {
            Awake();
        }

        var rect = _rectTransform.rect;
        float halfInsideThickness = insideThickness / 2f;
        float halfOutsideThickness = outsideThickness / 2f;
        
        vh.Clear();
        int vertexCounter = 0;
        UIVertex v = UIVertex.simpleVert;
        v.color = insideColor;
        
        switch (insideShape)
        {
            case InsideShape.Dot:
                v.position = rect.center + new Vector2(halfInsideThickness, halfInsideThickness);
                v.position = Quaternion.Euler(0, 0, _insideRotation) * v.position;
                vh.AddVert(v);
                v.position = rect.center + new Vector2(halfInsideThickness, -halfInsideThickness);
                v.position = Quaternion.Euler(0, 0, _insideRotation) * v.position;
                vh.AddVert(v);
                v.position = rect.center + new Vector2(-halfInsideThickness, -halfInsideThickness);
                v.position = Quaternion.Euler(0, 0, _insideRotation) * v.position;
                vh.AddVert(v);
                v.position = rect.center + new Vector2(-halfInsideThickness, halfInsideThickness);
                v.position = Quaternion.Euler(0, 0, _insideRotation) * v.position;
                vh.AddVert(v);
                vh.AddTriangle(vertexCounter, vertexCounter + 2, vertexCounter + 1);
                vh.AddTriangle(vertexCounter + 2, vertexCounter, vertexCounter + 3);
                vertexCounter += 4;
                break;
            case InsideShape.Cross:
                //Top part
                v.position = rect.center + new Vector2(halfInsideThickness, halfInsideThickness + _insideOffset);
                v.position = Quaternion.Euler(0, 0, _insideRotation) * v.position;
                vh.AddVert(v);
                v.position = rect.center + new Vector2(halfInsideThickness, halfInsideThickness);
                v.position = Quaternion.Euler(0, 0, _insideRotation) * v.position;
                vh.AddVert(v);
                v.position = rect.center + new Vector2(-halfInsideThickness, halfInsideThickness);
                v.position = Quaternion.Euler(0, 0, _insideRotation) * v.position;
                vh.AddVert(v);
                v.position = rect.center + new Vector2(-halfInsideThickness, halfInsideThickness + _insideOffset);
                v.position = Quaternion.Euler(0, 0, _insideRotation) * v.position;
                vh.AddVert(v);
                vh.AddTriangle(vertexCounter, vertexCounter + 2, vertexCounter + 1);
                vh.AddTriangle(vertexCounter + 2, vertexCounter, vertexCounter + 3);
                vertexCounter += 4;
                //Bottom part
                v.position = rect.center + new Vector2(halfInsideThickness, -halfInsideThickness - _insideOffset);
                v.position = Quaternion.Euler(0, 0, _insideRotation) * v.position;
                vh.AddVert(v);
                v.position = rect.center + new Vector2(halfInsideThickness, -halfInsideThickness);
                v.position = Quaternion.Euler(0, 0, _insideRotation) * v.position;
                vh.AddVert(v);
                v.position = rect.center + new Vector2(-halfInsideThickness, -halfInsideThickness);
                v.position = Quaternion.Euler(0, 0, _insideRotation) * v.position;
                vh.AddVert(v);
                v.position = rect.center + new Vector2(-halfInsideThickness, -halfInsideThickness - _insideOffset);
                v.position = Quaternion.Euler(0, 0, _insideRotation) * v.position;
                vh.AddVert(v);
                vh.AddTriangle(vertexCounter, vertexCounter + 2, vertexCounter + 1);
                vh.AddTriangle(vertexCounter + 2, vertexCounter, vertexCounter + 3);
                vertexCounter += 4;
                //Center part
                v.position = rect.center + new Vector2(halfInsideThickness + _insideOffset, halfInsideThickness);
                v.position = Quaternion.Euler(0, 0, _insideRotation) * v.position;
                vh.AddVert(v);
                v.position = rect.center + new Vector2(halfInsideThickness + _insideOffset, -halfInsideThickness);
                v.position = Quaternion.Euler(0, 0, _insideRotation) * v.position;
                vh.AddVert(v);
                v.position = rect.center + new Vector2(-halfInsideThickness - _insideOffset, -halfInsideThickness);
                v.position = Quaternion.Euler(0, 0, _insideRotation) * v.position;
                vh.AddVert(v);
                v.position = rect.center + new Vector2(-halfInsideThickness - _insideOffset, halfInsideThickness);
                v.position = Quaternion.Euler(0, 0, _insideRotation) * v.position;
                vh.AddVert(v);
                vh.AddTriangle(vertexCounter, vertexCounter + 2, vertexCounter + 1);
                vh.AddTriangle(vertexCounter + 2, vertexCounter, vertexCounter + 3);
                vertexCounter += 4;
                break;
            case InsideShape.NGon:
                //Inside verts
                for (int i = 0; i < insideNgon; i++)
                {
                    float progress = ((float) i / insideNgon) * 2f * Mathf.PI;
                    Vector2 insideNPointOffset = new Vector2(Mathf.Sin(progress), Mathf.Cos(progress));
                    v.position = rect.center + insideNPointOffset * _insideOffset;
                    v.position = Quaternion.Euler(0, 0, _insideRotation) * v.position;
                    vh.AddVert(v);
                    v.position = rect.center + insideNPointOffset * (_insideOffset + insideThickness);
                    v.position = Quaternion.Euler(0, 0, _insideRotation) * v.position;
                    vh.AddVert(v);
                    if (i != 0)
                    {
                        vh.AddTriangle(vertexCounter, vertexCounter - 1, vertexCounter - 2);
                        vh.AddTriangle(vertexCounter, vertexCounter - 1, vertexCounter + 1);
                    }
                    else
                    {
                        vh.AddTriangle(vertexCounter + 1, vertexCounter + ((insideNgon - 1) * 2), vertexCounter + ((insideNgon - 1) * 2) + 1);
                        vh.AddTriangle(vertexCounter, vertexCounter + ((insideNgon - 1) * 2), vertexCounter + 1);
                    }
                    vertexCounter += 2;
                }
                break;
            case InsideShape.Nothing:
                break;
        }

        v.color = outsideColor;
        switch (outsideShape)
        {
            case OutsideShape.Lines:
                for (int i = 0; i < outsideNgon; i++)
                {
                    v.position = rect.center + new Vector2(halfOutsideThickness, halfOutsideThickness + _outsideOffset + outsideLength);
                    v.position = Quaternion.Euler(0, 0, _outsideRotation) * v.position;
                    vh.AddVert(v);
                    v.position = rect.center + new Vector2(halfOutsideThickness, halfOutsideThickness + _outsideOffset);
                    v.position = Quaternion.Euler(0, 0, _outsideRotation) * v.position;
                    vh.AddVert(v);
                    v.position = rect.center + new Vector2(-halfOutsideThickness, halfOutsideThickness + _outsideOffset);
                    v.position = Quaternion.Euler(0, 0, _outsideRotation) * v.position;
                    vh.AddVert(v);
                    v.position = rect.center + new Vector2(-halfOutsideThickness, halfOutsideThickness + _outsideOffset + outsideLength);
                    v.position = Quaternion.Euler(0, 0, _outsideRotation) * v.position;
                    vh.AddVert(v);
                    vh.AddTriangle(vertexCounter, vertexCounter + 2, vertexCounter + 1);
                    vh.AddTriangle(vertexCounter + 2, vertexCounter, vertexCounter + 3);
                    vertexCounter += 4;
                    _outsideRotation += 360f / outsideNgon;
                }
                _outsideRotation -= 360;
                break;
        }
    }

    private void Update()
    {
        switch (outsideBehaviour)
        {
            case CrosshairBehaviour.Breathe:
                _outsideOffset = (-Mathf.Cos(_timer) + 2) * outsideOffset;
                SetVerticesDirty();
                break;
            case CrosshairBehaviour.Offset:
                _outsideOffset = (outsideBehaviourAmount + 1) * outsideOffset;
                SetVerticesDirty();
                break;
            case CrosshairBehaviour.InverseOffset:
                _outsideOffset = (1 - outsideBehaviourAmount) * outsideOffset;
                SetVerticesDirty();
                break;
            case CrosshairBehaviour.Spin:
                _outsideRotation += Time.deltaTime * 360f * outsideBehaviourAmount;
                SetVerticesDirty();
                break;
        }
        
        switch (insideBehaviour)
        {
            case CrosshairBehaviour.Breathe:
                _insideOffset = (-Mathf.Cos(_timer) + 2) * outsideOffset;
                SetVerticesDirty();
                break;
            case CrosshairBehaviour.Offset:
                _insideOffset = (outsideBehaviourAmount + 1) * outsideOffset;
                SetVerticesDirty();
                break;
            case CrosshairBehaviour.InverseOffset:
                _insideOffset = (1 - outsideBehaviourAmount) * outsideOffset;
                SetVerticesDirty();
                break;
            case CrosshairBehaviour.Spin:
                _insideRotation += Time.deltaTime * 360f * outsideBehaviourAmount;
                SetVerticesDirty();
                break;
        }
        

        _timer += Time.deltaTime * outsideBehaviourAmount;
        if (outsideBehaviourDecay > 0 && outsideBehaviourAmount != 0)
        {
            outsideBehaviourAmount = Mathf.Lerp(outsideBehaviourAmount, 0, (1f / outsideBehaviourDecay) * Time.deltaTime);
        }
    }
}
