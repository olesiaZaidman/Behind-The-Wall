using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DynamicCursor 
{
    [AddComponentMenu("Easy Dynamic Cursor/Dynamic Cursor Trigger")]
    public class DynamicCursorTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IScrollHandler
    {
        //Scroll References
        private bool mouseScrolling;
        private bool scrollDelaying;

        #region Custom Editor Properties
        [Tooltip("If true, allow overriding custom cursor textures for this specific GameObject")]
        [HideInInspector] public bool overrideCursorTextures;
        [HideInInspector] public Texture2D overrideSelectionCursor;
        [HideInInspector] public Texture2D overrideLeftClickCursor;
        [HideInInspector] public Texture2D overrideRightClickCursor;
        [HideInInspector] public Texture2D overrideScrollUpCursor;
        [HideInInspector] public Texture2D overrideScrollDownCursor;
        [HideInInspector] public Texture2D overrideScrollHoldCursor;

        [Tooltip("If true, allow to invoke mouse events when cursor changes")]
        [HideInInspector] public bool enableMouseEvents;
        [HideInInspector] public UnityEvent OnEnter;
        [HideInInspector] public UnityEvent OnLeftClick;
        [HideInInspector] public UnityEvent OnRightClick;
        [HideInInspector] public UnityEvent OnScrollUp;
        [HideInInspector] public UnityEvent OnScrollDown;
        [HideInInspector] public UnityEvent OnScrollHold;
        #endregion

        #region GameObjects Methods

        // It is called when mouse ENTER this object collider
        void OnMouseEnter()
        {
            //Set custom texture if 'override cursor textures' option is enabled
            if(overrideCursorTextures && overrideSelectionCursor != null)
                DynamicCursorManager.instance.SetCustomCursor(overrideSelectionCursor);
            else
                DynamicCursorManager.instance.SetCustomCursor(DynamicCursorManager.instance.selectionCursor);

            //Call OnEnter event
            if (enableMouseEvents)
                OnEnter.Invoke();
        }

        // It is called when mouse is OVER this object collider
        void OnMouseOver()
        {
            #region LeftButton
            // Checks if the left mouse button is pressed and sets the cursor texture accordingly
            if (Input.GetMouseButtonDown(0))
            {
                //Set custom texture if 'override cursor textures' option is enabled
                if (overrideCursorTextures && overrideLeftClickCursor != null)
                    DynamicCursorManager.instance.SetCustomCursor(overrideLeftClickCursor);
                else
                    DynamicCursorManager.instance.SetCustomCursor(DynamicCursorManager.instance.leftClickCursor);

                //Call OnLeftClick event
                if (enableMouseEvents)
                    OnLeftClick.Invoke();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (overrideCursorTextures && overrideSelectionCursor != null)
                    DynamicCursorManager.instance.SetCustomCursor(overrideSelectionCursor);
                else
                    DynamicCursorManager.instance.SetCustomCursor(DynamicCursorManager.instance.selectionCursor);
            }
            #endregion

            #region RightButton
            // Checks if the right mouse button is pressed and sets the cursor texture accordingly
            if (Input.GetMouseButtonDown(1))
            {
                //Set custom texture if 'override cursor textures' option is enabled
                if (overrideCursorTextures && overrideRightClickCursor != null)
                    DynamicCursorManager.instance.SetCustomCursor(overrideRightClickCursor);
                else
                    DynamicCursorManager.instance.SetCustomCursor(DynamicCursorManager.instance.rightClickCursor);

                //Call OnRightClick event
                if (enableMouseEvents)
                    OnRightClick.Invoke();
            }
            else if (Input.GetMouseButtonUp(1))
            {
                if (overrideCursorTextures && overrideSelectionCursor != null)
                    DynamicCursorManager.instance.SetCustomCursor(overrideSelectionCursor);
                else
                    DynamicCursorManager.instance.SetCustomCursor(DynamicCursorManager.instance.selectionCursor);
            }
            #endregion

            #region Mouse Scroll
            if (DynamicCursorManager.instance.scrollUpCursor == null)
                return;

            /// Check if mouse wheel button is being pressed
            /// if it's not, set cursor texture to default
            /// otherwise set it to 'scrollHoldCursor' texture
            if (Input.GetMouseButtonUp(2))
            {
                if (overrideCursorTextures && overrideSelectionCursor != null)
                    DynamicCursorManager.instance.SetCustomCursor(overrideSelectionCursor);
                else
                    DynamicCursorManager.instance.SetCustomCursor(DynamicCursorManager.instance.selectionCursor);
            }
            else if (Input.GetMouseButton(2))
            {
                //Set custom texture if 'override cursor textures' option is enabled
                if (overrideCursorTextures && overrideScrollHoldCursor != null)
                    DynamicCursorManager.instance.SetCustomCursor(overrideScrollHoldCursor);
                else
                    DynamicCursorManager.instance.SetCustomCursor(DynamicCursorManager.instance.scrollHoldCursor);

                //Call OnScrollHold event
                if (enableMouseEvents)
                    OnScrollHold.Invoke();
            }

            // Check if mouse wheel is scrolling and set a custom texture
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                //Set custom texture if 'override cursor textures' option is enabled
                if (overrideCursorTextures && overrideScrollUpCursor != null)
                    DynamicCursorManager.instance.SetCustomCursor(overrideScrollUpCursor);
                else
                    DynamicCursorManager.instance.SetCustomCursor(DynamicCursorManager.instance.scrollUpCursor);

                mouseScrolling = true;

                //Call OnScrollUp event
                if (enableMouseEvents)
                    OnScrollUp.Invoke();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                //Set custom texture if 'override cursor textures' option is enabled
                if (overrideCursorTextures && overrideScrollDownCursor != null)
                    DynamicCursorManager.instance.SetCustomCursor(overrideScrollDownCursor);
                else
                    DynamicCursorManager.instance.SetCustomCursor(DynamicCursorManager.instance.scrollDownCursor);

                mouseScrolling = true;

                //Call OnScrollDown event
                if (enableMouseEvents)
                    OnScrollDown.Invoke();
            }
            else
            {
                // Check if mouse wheel stopped scrolling and star a coroutine to reset cursor texture
                if (mouseScrolling)
                {
                    mouseScrolling = false;

                    if (!scrollDelaying)
                    {
                        scrollDelaying = true;
                        StartCoroutine(DelayToSelectionCursor());
                    }
                }
            }
            #endregion
        }

        // It is called when mouse EXITS this object collider
        void OnMouseExit()
        {
            DynamicCursorManager.instance.SetCustomCursor(DynamicCursorManager.instance.defaultCursor);
        }
        #endregion

        #region UI Methods
        // It is called when mouse ENTERS this UI element
        public void OnPointerEnter(PointerEventData eventData)
        {
            //Set custom texture if 'override cursor textures' option is enabled
            if (overrideCursorTextures && overrideSelectionCursor != null)
                DynamicCursorManager.instance.SetCustomCursor(overrideSelectionCursor);
            else
                DynamicCursorManager.instance.SetCustomCursor(DynamicCursorManager.instance.selectionCursor);

            //Call OnEnter event
            if (enableMouseEvents)
                OnEnter.Invoke();
        }

        // It is called when mouse EXIT this UI element
        public void OnPointerExit(PointerEventData eventData)
        {
            DynamicCursorManager.instance.SetCustomCursor(DynamicCursorManager.instance.defaultCursor);
        }

        // It is called when any mouse button clicks DOWN this UI element
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                //Set custom texture if 'override cursor textures' option is enabled
                if (overrideCursorTextures && overrideLeftClickCursor != null)
                    DynamicCursorManager.instance.SetCustomCursor(overrideLeftClickCursor);
                else
                    DynamicCursorManager.instance.SetCustomCursor(DynamicCursorManager.instance.leftClickCursor);

                //Call OnLeftClick event
                if (enableMouseEvents)
                    OnLeftClick.Invoke();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                //Set custom texture if 'override cursor textures' option is enabled
                if (overrideCursorTextures && overrideRightClickCursor != null)
                    DynamicCursorManager.instance.SetCustomCursor(overrideRightClickCursor);
                else
                    DynamicCursorManager.instance.SetCustomCursor(DynamicCursorManager.instance.rightClickCursor);

                //Call OnRightClick event
                if (enableMouseEvents)
                    OnRightClick.Invoke();
            }
            else if (eventData.button == PointerEventData.InputButton.Middle)
            {
                //Set custom texture if 'override cursor textures' option is enabled
                if (overrideCursorTextures && overrideScrollHoldCursor != null)
                    DynamicCursorManager.instance.SetCustomCursor(overrideScrollHoldCursor);
                else
                    DynamicCursorManager.instance.SetCustomCursor(DynamicCursorManager.instance.scrollHoldCursor);

                //Call OnScrollHold event
                if (enableMouseEvents)
                    OnScrollHold.Invoke();
            }
        }

        // It is called when any mouse button clicks UP this UI element
        public void OnPointerUp(PointerEventData eventData)
        {
            if (overrideCursorTextures && overrideSelectionCursor != null)
                DynamicCursorManager.instance.SetCustomCursor(overrideSelectionCursor);
            else
                DynamicCursorManager.instance.SetCustomCursor(DynamicCursorManager.instance.selectionCursor);
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (DynamicCursorManager.instance.scrollUpCursor == null)
                return;

            // Check if mouse wheel is scrolling and set a custom texture
            if (eventData.scrollDelta.y > 0)
            {
                //Set custom texture if 'override cursor textures' option is enabled
                if (overrideCursorTextures && overrideScrollUpCursor != null)
                    DynamicCursorManager.instance.SetCustomCursor(overrideScrollUpCursor);
                else
                    DynamicCursorManager.instance.SetCustomCursor(DynamicCursorManager.instance.scrollUpCursor);

                mouseScrolling = true;

                //Call OnScrollUp event
                if (enableMouseEvents)
                    OnScrollUp.Invoke();
            }
            else if (eventData.scrollDelta.y < 0)
            {
                //Set custom texture if 'override cursor textures' option is enabled
                if (overrideCursorTextures && overrideScrollDownCursor != null)
                    DynamicCursorManager.instance.SetCustomCursor(overrideScrollDownCursor);
                else
                    DynamicCursorManager.instance.SetCustomCursor(DynamicCursorManager.instance.scrollDownCursor);

                mouseScrolling = true;

                //Call OnScrollDown event
                if (enableMouseEvents)
                    OnScrollDown.Invoke();
            }
            else
            {
                // Check if mouse wheel stopped scrolling and star a coroutine to reset cursor texture
                if (mouseScrolling)
                {
                    mouseScrolling = false;

                    if (!scrollDelaying)
                    {
                        scrollDelaying = true;
                        StartCoroutine(DelayToSelectionCursor());
                    }
                }
            }
        }
        #endregion

        // Wait a few seconds to set cursor back to default texture
        IEnumerator DelayToSelectionCursor()
        {
            yield return new WaitForSeconds(1.5f);

            if (Input.GetAxis("Mouse ScrollWheel") == 0)
            {
                if (overrideCursorTextures && overrideSelectionCursor != null)
                    DynamicCursorManager.instance.SetCustomCursor(overrideSelectionCursor);
                else
                    DynamicCursorManager.instance.SetCustomCursor(DynamicCursorManager.instance.selectionCursor);
            }

            scrollDelaying = false;
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(DynamicCursorTrigger))]
    public class DynamicCursorTriggerEditor : Editor 
    {
        private const string componentDescription = "If this GameObject is not a UI element, it requires some kind of 'Collider' attached to it to work properly";

        SerializedProperty overrideCursorProp;
        SerializedProperty overrideSelectionProp;
        SerializedProperty overrideLeftClickProp;
        SerializedProperty overrideRightClickProp;
        SerializedProperty overrideScrollUpnProp;
        SerializedProperty overrideScrollDownProp;
        SerializedProperty overrideScrollHoldProp;

        SerializedProperty enableMouseEventsProp;
        SerializedProperty onEnterProp;
        SerializedProperty onLeftClickProp;
        SerializedProperty onRightClickProp;
        SerializedProperty onScrollUpProp;
        SerializedProperty onScrollDownProp;
        SerializedProperty onScrollHoldProp;


        void OnEnable()
        {
            overrideCursorProp = serializedObject.FindProperty("overrideCursorTextures");
            overrideSelectionProp = serializedObject.FindProperty("overrideSelectionCursor");
            overrideLeftClickProp = serializedObject.FindProperty("overrideLeftClickCursor");
            overrideRightClickProp = serializedObject.FindProperty("overrideRightClickCursor");
            overrideScrollUpnProp = serializedObject.FindProperty("overrideScrollUpCursor");
            overrideScrollDownProp = serializedObject.FindProperty("overrideScrollDownCursor");
            overrideScrollHoldProp = serializedObject.FindProperty("overrideScrollHoldCursor");

            enableMouseEventsProp = serializedObject.FindProperty("enableMouseEvents");
            onEnterProp = serializedObject.FindProperty("OnEnter");
            onLeftClickProp = serializedObject.FindProperty("OnLeftClick");
            onRightClickProp = serializedObject.FindProperty("OnRightClick");
            onScrollUpProp = serializedObject.FindProperty("OnScrollUp");
            onScrollDownProp = serializedObject.FindProperty("OnScrollDown");
            onScrollHoldProp = serializedObject.FindProperty("OnScrollHold");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox(componentDescription, MessageType.Info);
            DrawDefaultInspector();

            serializedObject.Update();

            //Enable cursor textures override
            EditorGUILayout.PropertyField(overrideCursorProp);
            if (overrideCursorProp.boolValue)
            {
                EditorGUILayout.PropertyField(overrideSelectionProp);
                EditorGUILayout.PropertyField(overrideLeftClickProp);
                EditorGUILayout.PropertyField(overrideRightClickProp);
                EditorGUILayout.PropertyField(overrideScrollUpnProp);
                EditorGUILayout.PropertyField(overrideScrollDownProp);
                EditorGUILayout.PropertyField(overrideScrollHoldProp);

                EditorGUILayout.Space();
            }

            //Enable mouse events
            EditorGUILayout.PropertyField(enableMouseEventsProp);
            if (enableMouseEventsProp.boolValue)
            {
                EditorGUILayout.PropertyField(onEnterProp);
                EditorGUILayout.PropertyField(onLeftClickProp);
                EditorGUILayout.PropertyField(onRightClickProp);
                EditorGUILayout.PropertyField(onScrollUpProp);
                EditorGUILayout.PropertyField(onScrollDownProp);
                EditorGUILayout.PropertyField(onScrollHoldProp);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
    #endif
}
