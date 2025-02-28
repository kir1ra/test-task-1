using System;
using UnityEngine;

namespace UniRx.Triggers
{
    // for UI
    public static class ObservableTriggerUIExtensions
    {
        #region For GameObject

        /// <summary>OnMouseDown is called when the user has pressed the mouse button while over the UIElement.</summary>
        public static IObservable<Unit> OnMouseDownOnUIAsObservable(this GameObject gameObject)
        {
            if (gameObject == null) return Observable.Empty<Unit>();
            return GetOrAddComponent<ObservableMouseUITrigger>(gameObject).OnMouseDownAsObservable();
        }

        /// <summary>OnMouseDrag is called when the user has clicked on a UIElement and is still holding down the mouse.</summary>
        public static IObservable<Unit> OnMouseDragOnUIAsObservable(this GameObject gameObject)
        {
            if (gameObject == null) return Observable.Empty<Unit>();
            return GetOrAddComponent<ObservableMouseUITrigger>(gameObject).OnMouseDragAsObservable();
        }

        /// <summary>OnMouseEnter is called when the mouse entered the UIElement.</summary>
        public static IObservable<Unit> OnMouseEnterOnUIAsObservable(this GameObject gameObject)
        {
            if (gameObject == null) return Observable.Empty<Unit>();
            return GetOrAddComponent<ObservableMouseUITrigger>(gameObject).OnMouseEnterAsObservable();
        }

        /// <summary>OnMouseExit is called when the mouse is not any longer over the UIElement.</summary>
        public static IObservable<Unit> OnMouseExitOnUIAsObservable(this GameObject gameObject)
        {
            if (gameObject == null) return Observable.Empty<Unit>();
            return GetOrAddComponent<ObservableMouseUITrigger>(gameObject).OnMouseExitAsObservable();
        }

        /// <summary>OnMouseUp is called when the user has released the mouse button.</summary>
        public static IObservable<Unit> OnMouseUpOnUIAsObservable(this GameObject gameObject)
        {
            if (gameObject == null) return Observable.Empty<Unit>();
            return GetOrAddComponent<ObservableMouseUITrigger>(gameObject).OnMouseUpAsObservable();
        }

        #endregion

        #region For Component

        /// <summary>OnMouseDown is called when the user has pressed the mouse button while over the UIElement.</summary>
        public static IObservable<Unit> OnMouseDownOnUIAsObservable(this Component component)
        {
            if (component == null || component.gameObject == null) return Observable.Empty<Unit>();
            return GetOrAddComponent<ObservableMouseUITrigger>(component.gameObject).OnMouseDownAsObservable();
        }

        /// <summary>OnMouseDrag is called when the user has clicked on a UIElement and is still holding down the mouse.</summary>
        public static IObservable<Unit> OnMouseDragOnUIAsObservable(this Component component)
        {
            if (component == null || component.gameObject == null) return Observable.Empty<Unit>();
            return GetOrAddComponent<ObservableMouseUITrigger>(component.gameObject).OnMouseDragAsObservable();
        }

        /// <summary>OnMouseEnter is called when the mouse entered the UIElement.</summary>
        public static IObservable<Unit> OnMouseEnterOnUIAsObservable(this Component component)
        {
            if (component == null || component.gameObject == null) return Observable.Empty<Unit>();
            return GetOrAddComponent<ObservableMouseUITrigger>(component.gameObject).OnMouseEnterAsObservable();
        }

        /// <summary>OnMouseExit is called when the mouse is not any longer over the UIElement.</summary>
        public static IObservable<Unit> OnMouseExitOnUIAsObservable(this Component component)
        {
            if (component == null || component.gameObject == null) return Observable.Empty<Unit>();
            return GetOrAddComponent<ObservableMouseUITrigger>(component.gameObject).OnMouseExitAsObservable();
        }

        /// <summary>OnMouseUp is called when the user has released the mouse button.</summary>
        public static IObservable<Unit> OnMouseUpOnUIAsObservable(this Component component)
        {
            if (component == null || component.gameObject == null) return Observable.Empty<Unit>();
            return GetOrAddComponent<ObservableMouseUITrigger>(component.gameObject).OnMouseUpAsObservable();
        }

        #endregion

        static T GetOrAddComponent<T>(GameObject gameObject)
            where T : Component
        {
            return gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
        }
    }
}
