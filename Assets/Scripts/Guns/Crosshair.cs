using System;
using System.Collections;
using System.Collections.Generic;
using Guns;
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

    public CrosshairData currentCrosshair;
    public CrosshairData fallbackCrosshair;
    public CrosshairData CurrentCrosshair => currentCrosshair != null ? currentCrosshair : fallbackCrosshair;
    
    private RectTransform _rectTransform;
    private float _timer;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();

        
        if (!Application.isPlaying)
        {
            Awake();
        }

        var rect = _rectTransform.rect;
        float halfInsideThickness = CurrentCrosshair.insideThickness / 2f;
        float halfOutsideThickness = CurrentCrosshair.outsideThickness / 2f;
        
        vh.Clear();
        int vertexCounter = 0;
        UIVertex v = UIVertex.simpleVert;
        v.color = CurrentCrosshair.insideColor;
        
        switch (CurrentCrosshair.insideShape)
        {
            case InsideShape.Dot:
                v.position = rect.center + new Vector2(halfInsideThickness, halfInsideThickness);
                v.position = Quaternion.Euler(0, 0, CurrentCrosshair.runtimeInsideRotation) * v.position;
                vh.AddVert(v);
                v.position = rect.center + new Vector2(halfInsideThickness, -halfInsideThickness);
                v.position = Quaternion.Euler(0, 0, CurrentCrosshair.runtimeInsideRotation) * v.position;
                vh.AddVert(v);
                v.position = rect.center + new Vector2(-halfInsideThickness, -halfInsideThickness);
                v.position = Quaternion.Euler(0, 0, CurrentCrosshair.runtimeInsideRotation) * v.position;
                vh.AddVert(v);
                v.position = rect.center + new Vector2(-halfInsideThickness, halfInsideThickness);
                v.position = Quaternion.Euler(0, 0, CurrentCrosshair.runtimeInsideRotation) * v.position;
                vh.AddVert(v);
                vh.AddTriangle(vertexCounter, vertexCounter + 2, vertexCounter + 1);
                vh.AddTriangle(vertexCounter + 2, vertexCounter, vertexCounter + 3);
                vertexCounter += 4;
                break;
            case InsideShape.Cross:
                //Top part
                v.position = rect.center + new Vector2(halfInsideThickness, halfInsideThickness + CurrentCrosshair.runtimeInsideOffset);
                v.position = Quaternion.Euler(0, 0, CurrentCrosshair.runtimeInsideRotation) * v.position;
                vh.AddVert(v);
                v.position = rect.center + new Vector2(halfInsideThickness, halfInsideThickness);
                v.position = Quaternion.Euler(0, 0, CurrentCrosshair.runtimeInsideRotation) * v.position;
                vh.AddVert(v);
                v.position = rect.center + new Vector2(-halfInsideThickness, halfInsideThickness);
                v.position = Quaternion.Euler(0, 0, CurrentCrosshair.runtimeInsideRotation) * v.position;
                vh.AddVert(v);
                v.position = rect.center + new Vector2(-halfInsideThickness, halfInsideThickness + CurrentCrosshair.runtimeInsideOffset);
                v.position = Quaternion.Euler(0, 0, CurrentCrosshair.runtimeInsideRotation) * v.position;
                vh.AddVert(v);
                vh.AddTriangle(vertexCounter, vertexCounter + 2, vertexCounter + 1);
                vh.AddTriangle(vertexCounter + 2, vertexCounter, vertexCounter + 3);
                vertexCounter += 4;
                //Bottom part
                v.position = rect.center + new Vector2(halfInsideThickness, -halfInsideThickness - CurrentCrosshair.runtimeInsideOffset);
                v.position = Quaternion.Euler(0, 0, CurrentCrosshair.runtimeInsideRotation) * v.position;
                vh.AddVert(v);
                v.position = rect.center + new Vector2(halfInsideThickness, -halfInsideThickness);
                v.position = Quaternion.Euler(0, 0, CurrentCrosshair.runtimeInsideRotation) * v.position;
                vh.AddVert(v);
                v.position = rect.center + new Vector2(-halfInsideThickness, -halfInsideThickness);
                v.position = Quaternion.Euler(0, 0, CurrentCrosshair.runtimeInsideRotation) * v.position;
                vh.AddVert(v);
                v.position = rect.center + new Vector2(-halfInsideThickness, -halfInsideThickness - CurrentCrosshair.runtimeInsideOffset);
                v.position = Quaternion.Euler(0, 0, CurrentCrosshair.runtimeInsideRotation) * v.position;
                vh.AddVert(v);
                vh.AddTriangle(vertexCounter, vertexCounter + 2, vertexCounter + 1);
                vh.AddTriangle(vertexCounter + 2, vertexCounter, vertexCounter + 3);
                vertexCounter += 4;
                //Center part
                v.position = rect.center + new Vector2(halfInsideThickness + CurrentCrosshair.runtimeInsideOffset, halfInsideThickness);
                v.position = Quaternion.Euler(0, 0, CurrentCrosshair.runtimeInsideRotation) * v.position;
                vh.AddVert(v);
                v.position = rect.center + new Vector2(halfInsideThickness + CurrentCrosshair.runtimeInsideOffset, -halfInsideThickness);
                v.position = Quaternion.Euler(0, 0, CurrentCrosshair.runtimeInsideRotation) * v.position;
                vh.AddVert(v);
                v.position = rect.center + new Vector2(-halfInsideThickness - CurrentCrosshair.runtimeInsideOffset, -halfInsideThickness);
                v.position = Quaternion.Euler(0, 0, CurrentCrosshair.runtimeInsideRotation) * v.position;
                vh.AddVert(v);
                v.position = rect.center + new Vector2(-halfInsideThickness - CurrentCrosshair.runtimeInsideOffset, halfInsideThickness);
                v.position = Quaternion.Euler(0, 0, CurrentCrosshair.runtimeInsideRotation) * v.position;
                vh.AddVert(v);
                vh.AddTriangle(vertexCounter, vertexCounter + 2, vertexCounter + 1);
                vh.AddTriangle(vertexCounter + 2, vertexCounter, vertexCounter + 3);
                vertexCounter += 4;
                break;
            case InsideShape.NGon:
                //Inside verts
                for (int i = 0; i < CurrentCrosshair.insideNgon; i++)
                {
                    float progress = ((float) i / CurrentCrosshair.insideNgon) * 2f * Mathf.PI;
                    Vector2 insideNPointOffset = new Vector2(Mathf.Sin(progress), Mathf.Cos(progress));
                    v.position = rect.center + insideNPointOffset * CurrentCrosshair.runtimeInsideOffset;
                    v.position = Quaternion.Euler(0, 0, CurrentCrosshair.runtimeInsideRotation) * v.position;
                    vh.AddVert(v);
                    v.position = rect.center + insideNPointOffset * (CurrentCrosshair.runtimeInsideOffset + CurrentCrosshair.insideThickness);
                    v.position = Quaternion.Euler(0, 0, CurrentCrosshair.runtimeInsideRotation) * v.position;
                    vh.AddVert(v);
                    if (i != 0)
                    {
                        vh.AddTriangle(vertexCounter, vertexCounter - 1, vertexCounter - 2);
                        vh.AddTriangle(vertexCounter, vertexCounter - 1, vertexCounter + 1);
                    }
                    else
                    {
                        vh.AddTriangle(vertexCounter + 1, vertexCounter + ((CurrentCrosshair.insideNgon - 1) * 2), vertexCounter + ((CurrentCrosshair.insideNgon - 1) * 2) + 1);
                        vh.AddTriangle(vertexCounter, vertexCounter + ((CurrentCrosshair.insideNgon - 1) * 2), vertexCounter + 1);
                    }
                    vertexCounter += 2;
                }
                break;
            case InsideShape.Nothing:
                break;
        }

        v.color = CurrentCrosshair.outsideColor;
        switch (CurrentCrosshair.outsideShape)
        {
            case OutsideShape.Lines:
                for (int i = 0; i < CurrentCrosshair.outsideNgon; i++)
                {
                    v.position = rect.center + new Vector2(halfOutsideThickness, halfOutsideThickness + CurrentCrosshair.runtimeOutsideOffset + CurrentCrosshair.outsideLength);
                    v.position = Quaternion.Euler(0, 0, CurrentCrosshair.runtimeOutsideRotation) * v.position;
                    vh.AddVert(v);
                    v.position = rect.center + new Vector2(halfOutsideThickness, halfOutsideThickness + CurrentCrosshair.runtimeOutsideOffset);
                    v.position = Quaternion.Euler(0, 0, CurrentCrosshair.runtimeOutsideRotation) * v.position;
                    vh.AddVert(v);
                    v.position = rect.center + new Vector2(-halfOutsideThickness, halfOutsideThickness + CurrentCrosshair.runtimeOutsideOffset);
                    v.position = Quaternion.Euler(0, 0, CurrentCrosshair.runtimeOutsideRotation) * v.position;
                    vh.AddVert(v);
                    v.position = rect.center + new Vector2(-halfOutsideThickness, halfOutsideThickness + CurrentCrosshair.runtimeOutsideOffset + CurrentCrosshair.outsideLength);
                    v.position = Quaternion.Euler(0, 0, CurrentCrosshair.runtimeOutsideRotation) * v.position;
                    vh.AddVert(v);
                    vh.AddTriangle(vertexCounter, vertexCounter + 2, vertexCounter + 1);
                    vh.AddTriangle(vertexCounter + 2, vertexCounter, vertexCounter + 3);
                    vertexCounter += 4;
                    CurrentCrosshair.runtimeOutsideRotation += 360f / CurrentCrosshair.outsideNgon;
                }
                CurrentCrosshair.runtimeOutsideRotation -= 360;
                break;
        }
    }

    private void Update()
    {
        switch (CurrentCrosshair.outsideBehaviour)
        {
            case CrosshairBehaviour.Breathe:
                CurrentCrosshair.runtimeOutsideOffset = (-Mathf.Cos(_timer) + 2) * CurrentCrosshair.outsideBehaviourStrength;
                SetVerticesDirty();
                break;
            case CrosshairBehaviour.Offset:
                CurrentCrosshair.runtimeOutsideOffset = CurrentCrosshair.runtimeOutsideBehaviourAmount * CurrentCrosshair.outsideBehaviourStrength;
                SetVerticesDirty();
                break;
            case CrosshairBehaviour.InverseOffset:
                CurrentCrosshair.runtimeOutsideOffset = (1 - CurrentCrosshair.runtimeOutsideBehaviourAmount) * CurrentCrosshair.outsideBehaviourStrength;
                SetVerticesDirty();
                break;
            case CrosshairBehaviour.Spin:
                CurrentCrosshair.runtimeOutsideRotation += Time.deltaTime * 360f * CurrentCrosshair.runtimeOutsideBehaviourAmount * CurrentCrosshair.outsideBehaviourStrength;
                SetVerticesDirty();
                break;
        }
        
        switch (CurrentCrosshair.insideBehaviour)
        {
            case CrosshairBehaviour.Breathe:
                CurrentCrosshair.runtimeInsideOffset = (-Mathf.Cos(_timer) + 2) * CurrentCrosshair.outsideBehaviourStrength;
                SetVerticesDirty();
                break;
            case CrosshairBehaviour.Offset:
                CurrentCrosshair.runtimeInsideOffset = CurrentCrosshair.runtimeOutsideBehaviourAmount * CurrentCrosshair.outsideBehaviourStrength;
                SetVerticesDirty();
                break;
            case CrosshairBehaviour.InverseOffset:
                CurrentCrosshair.runtimeInsideOffset = (1 - CurrentCrosshair.runtimeOutsideBehaviourAmount) * CurrentCrosshair.outsideBehaviourStrength;
                SetVerticesDirty();
                break;
            case CrosshairBehaviour.Spin:
                CurrentCrosshair.runtimeInsideRotation += Time.deltaTime * 360f * CurrentCrosshair.runtimeOutsideBehaviourAmount * CurrentCrosshair.outsideBehaviourStrength;
                SetVerticesDirty();
                break;
        }
        

        _timer += Time.deltaTime * CurrentCrosshair.runtimeOutsideBehaviourAmount;
        if (CurrentCrosshair.outsideBehaviourDecay > 0 && CurrentCrosshair.runtimeOutsideBehaviourAmount != 0)
        {
            CurrentCrosshair.runtimeOutsideBehaviourAmount = Mathf.Lerp(CurrentCrosshair.runtimeOutsideBehaviourAmount, 0, (1f / CurrentCrosshair.outsideBehaviourDecay) * Time.deltaTime);
        }
    }
}
