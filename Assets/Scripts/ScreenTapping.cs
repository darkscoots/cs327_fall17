﻿// Author(s): Yixiang Xu
/* This class is working for screen tapping
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ScreenTapping : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    [Tooltip("The prefab to instantiate for ScreenTapping.")]
    GameObject prefabScreenTapping;

    public void OnBeginDrag(PointerEventData eventData)
    {
        tappingEffect(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        tappingEffect(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        tappingEffect(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        tappingEffect(eventData);
    }

    public void tappingEffect(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPoint);
        GameObject tappingAnim = Instantiate(prefabScreenTapping, transform, false);
        tappingAnim.transform.localPosition = localPoint;
    }
}
