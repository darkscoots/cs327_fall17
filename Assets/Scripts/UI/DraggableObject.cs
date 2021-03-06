﻿// Author(s): Joel Esquilin, Paul Calande, Yixiang Xu

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// This class should be inheritable to create other new blocks.
public class DraggableObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    [Tooltip("Whether the GameObject is currently draggable.")]
    protected bool isDraggable;
    [SerializeField]
    [Tooltip("A List of locations that this GameObject can snap to.")]
    protected List<SnapLocation> snapToAreas;
    [SerializeField]
    [Tooltip("Reference to the UI canvas' RectTransform.")]
    protected RectTransform canvasTransform;
    [SerializeField]
    [Tooltip("Reference to the draggable GameObject's RectTransform.")]
    protected RectTransform rectTransform;
    [SerializeField]
    [Tooltip("The position to return to if dragging ceases and no SnapLocation is snapped to.")]
    protected Vector2 defaultPosition;
    [SerializeField]
    [Tooltip("The current position.")]
    protected Vector2 currentPosition;
    [SerializeField]
    [Tooltip("The size of the draggable block while being dragged.")]
    Vector3 draggingScale;
    [SerializeField]
    [Tooltip("The size of the draggable block while not being dragged.")]
    Vector3 nonDraggingScale;
    [SerializeField]
    [Tooltip("The time when drag event begins.")]
    float startTime;
    [SerializeField]
    [Tooltip("The spped of scale lerping.")]
    float lerpSpeed;
    [SerializeField]
    [Tooltip("Reference to ScreenTapping.")]
    ScreenTapping screenTapping;

    protected static Vector2 piecePlacementOffset = new Vector2(80, 80);

    public Action<DraggableObject> BeginDragEvent;
    public Action<DraggableObject> EndDragEvent;

    protected Vector2 _pointerOffset = Vector2.zero;
    // Whether the object is currently being dragged or not.
    protected bool isDragging = false;

    // Snap detection offset for detecting snap locations.
    protected Vector2 snapDetectionOffset;

    SnapLocation lastLocation = null;

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (isDraggable)
        {
            startTime = Time.time;
            isDragging = true;
            screenTapping.TappingEffect(eventData); //Play screenTapping animation

            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent.parent.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out _pointerOffset);

            if (BeginDragEvent != null)
            {
                AudioController.Instance.PlaySFX("Tiles_Pickup_1");
                BeginDragEvent(this);
            }
        }
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (isDraggable)
        {
            screenTapping.TappingEffect(eventData); //Play screenTapping animation
            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, eventData.position, eventData.pressEventCamera, out localPointerPosition))
            {
                float xPos = localPointerPosition.x - _pointerOffset.x;
                float yPos = localPointerPosition.y - _pointerOffset.y;

                transform.localPosition = new Vector2(xPos, yPos);
            }

            SnapLocation locationToGoTo = GetClosestSnapLocation();

            if (locationToGoTo != null)
            {
                if (lastLocation != locationToGoTo)
                {
                    lastLocation = locationToGoTo;
                    locationToGoTo.Hover(gameObject, false); // Clear all highlights
                    locationToGoTo.Hover(gameObject, true); // Set on highlight for current tile
                    AudioController.Instance.SnapTile();
                }
            }
            else
            {
                lastLocation = null;
                TurnOffHovering();
            }
        }
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (isDraggable)
        {
            startTime = Time.time;
            isDragging = false;
            screenTapping.TappingEffect(eventData); //Play screenTapping animation

            SnapLocation locationToGoTo = GetClosestSnapLocation();

            if (locationToGoTo != null)
            {
                transform.position = locationToGoTo.transform.position;
                locationToGoTo.Snap(gameObject);
                locationToGoTo.Hover(gameObject, false);
            }
            else
            {
                transform.localScale = nonDraggingScale; //Make the block samller
                //transform.localScale = new Vector3(1f, 1f, 1f);
                TurnOffHovering();
                currentPosition = transform.localPosition;
                //transform.localPosition = defaultPosition;
            }

            if (EndDragEvent != null)
            {
                EndDragEvent(this);
            }
        }
    }

    private void Start()
    {
        //transform.localScale = nonDraggingScale;
    }

    private void Update()
    {
        //Lerping between dragging scale and nondragging scale
        if (isDragging)
        {
            transform.localScale = Vector3.Lerp(nonDraggingScale, draggingScale, (Time.time - startTime) * lerpSpeed);
        }
        else
        {
            transform.localScale = Vector3.Lerp(draggingScale, nonDraggingScale,  (Time.time - startTime) * lerpSpeed);
            transform.localPosition = Vector3.Lerp(currentPosition, defaultPosition, (Time.time - startTime) * lerpSpeed);
        }
    }

    // Gets the closest SnapLocation within the range of the DraggableObject.
    // Returns null if no SnapLocations are in range.
    protected virtual SnapLocation GetClosestSnapLocation()
    {
        SnapLocation locationToGoTo = null;

        Vector2 myPos = transform.position;
        myPos += snapDetectionOffset;

        // Used to figure out which location is the closest before snapping.
        float smallestDistance = Mathf.Infinity;

        foreach (SnapLocation solution in snapToAreas)
        {
            Vector2 solutionPos = solution.transform.position;
            float solutionX = solutionPos.x;
            float solutionY = solutionPos.y;

            //Debug.Log("Me: (" + myPos.x + ", " + myPos.y + "), solution: (" + solutionX + ", " + solutionY + ")");

            if ((myPos.x > solutionX - piecePlacementOffset.x) &&
                (myPos.x < solutionX + piecePlacementOffset.x) &&
                (myPos.y > solutionY - piecePlacementOffset.y) &&
                (myPos.y < solutionY + piecePlacementOffset.y))
            {
                float distance = Vector2.Distance(myPos, solutionPos);
                if (distance < smallestDistance)
                {
                    locationToGoTo = solution;
                    smallestDistance = distance;
                }
            }
        }

        return locationToGoTo;
    }

    protected virtual void OnDestroy()
    {
        BeginDragEvent = null;
        EndDragEvent = null;
    }

    public void SetSnapToAreas(List<SnapLocation> snapLocations)
    {
        snapToAreas = snapLocations;
    }

    public void SetIsDraggable(bool draggable)
    {
        isDraggable = draggable;
    }

    public void SetScreenTapping(ScreenTapping tapping)
    {
        screenTapping = tapping;
    }

    public void SetDraggingScale(Vector3 scale)
    {
        draggingScale = scale;
    }

    public void SetNonDraggingScale(Vector3 scale)
    {
        nonDraggingScale = scale;
    }

    public void SetCanvasTransform(RectTransform newCanvasTransform)
    {
        canvasTransform = newCanvasTransform;
    }

    public void SetDefaultPosition(Vector2 position)
    {
        defaultPosition = position;
    }

    public bool GetIsDragging()
    {
        return isDragging;
    }

    public void SetSnapDetectionOffset(Vector2 offset)
    {
        // Account for the canvas' scale when setting the snap detection offset.
        float canvasXScale = canvasTransform.lossyScale.x;
        float canvasYScale = canvasTransform.lossyScale.y;
        offset.x *= canvasXScale;
        offset.y *= canvasYScale;
        snapDetectionOffset = offset;

        //Debug.Log("DraggableObject canvas transform x scale: " + canvasXScale);
    }

    private void TurnOffHovering()
    {
        if (snapToAreas.Count != 0)
        {
            snapToAreas[0].Hover(gameObject, false); // Turn off highlights
        }
    }

    /*
    private void OnDrawGizmos()
    {
        float cubeSize = 25.0f;
        Vector3 cube = new Vector3(cubeSize, cubeSize, cubeSize);

        Vector2 pos = transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawCube(pos, cube);
        pos += snapDetectionOffset;
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(pos, cube);
    }
    */
}