using System; // require keep for Windows Universal App
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniRx.Triggers
{
    [DisallowMultipleComponent]
    public class ObservableMouseUITrigger : ObservableTriggerBase, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        Subject<Unit> onMouseDown;

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (onMouseDown != null) onMouseDown.OnNext(Unit.Default);
        }

        /// <summary>OnMouseDown is called when the user has pressed the mouse button while over the UIElement.</summary>
        public IObservable<Unit> OnMouseDownAsObservable()
        {
            return onMouseDown ?? (onMouseDown = new Subject<Unit>());
        }

        Subject<Unit> onMouseDrag;

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (onMouseDrag != null) onMouseDrag.OnNext(Unit.Default);
        }

        /// <summary>OnMouseDrag is called when the user has clicked on a UIElement and is still holding down the mouse.</summary>
        public IObservable<Unit> OnMouseDragAsObservable()
        {
            return onMouseDrag ?? (onMouseDrag = new Subject<Unit>());
        }

        Subject<Unit> onMouseEnter;

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (onMouseEnter != null) onMouseEnter.OnNext(Unit.Default);
        }

        /// <summary>OnMouseEnter is called when the mouse entered the UIElement.</summary>
        public IObservable<Unit> OnMouseEnterAsObservable()
        {
            return onMouseEnter ?? (onMouseEnter = new Subject<Unit>());
        }

        Subject<Unit> onMouseExit;

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (onMouseExit != null) onMouseExit.OnNext(Unit.Default);
        }

        /// <summary>OnMouseExit is called when the mouse is not any longer over the UIElement.</summary>
        public IObservable<Unit> OnMouseExitAsObservable()
        {
            return onMouseExit ?? (onMouseExit = new Subject<Unit>());
        }

        Subject<Unit> onMouseUp;

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (onMouseUp != null) onMouseUp.OnNext(Unit.Default);
        }

        /// <summary>OnMouseUp is called when the user has released the mouse button.</summary>
        public IObservable<Unit> OnMouseUpAsObservable()
        {
            return onMouseUp ?? (onMouseUp = new Subject<Unit>());
        }

        protected override void RaiseOnCompletedOnDestroy()
        {
            if (onMouseDown != null)
            {
                onMouseDown.OnCompleted();
            }
            if (onMouseDrag != null)
            {
                onMouseDrag.OnCompleted();
            }
            if (onMouseEnter != null)
            {
                onMouseEnter.OnCompleted();
            }
            if (onMouseExit != null)
            {
                onMouseExit.OnCompleted();
            }
            if (onMouseUp != null)
            {
                onMouseUp.OnCompleted();
            }
        }
    }
}