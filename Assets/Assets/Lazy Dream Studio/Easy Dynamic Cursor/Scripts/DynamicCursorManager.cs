using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DynamicCursor
{
    [AddComponentMenu("Easy Dynamic Cursor/Dynamic Cursor Manager")]
    public class DynamicCursorManager : MonoBehaviour
    {
        public static DynamicCursorManager instance;

        [Header("Cursor Textures")]
        [Tooltip("The default cursor texture")]
        public Texture2D defaultCursor;
        [Tooltip("The cursor texture when the mouse pointer is over some object or UI element")]
        public Texture2D selectionCursor;
        [Tooltip("The cursor texture when clicking with the left mouse button on some object or UI element")]
        public Texture2D leftClickCursor;
        [Tooltip("The cursor texture when clicking with the right mouse button on some object or UI element")]
        public Texture2D rightClickCursor;
        [Tooltip("The cursor texture when scrolling the mouse wheel Up on some object or UI element")]
        public Texture2D scrollUpCursor;
        [Tooltip("The cursor texture when scrolling the mouse wheel Down on some object or UI element")]
        public Texture2D scrollDownCursor;
        [Tooltip("The cursor texture when the mouse wheel is pressed on some object or UI element")]
        public Texture2D scrollHoldCursor;

        [Header("Cursor Settings")]
        [Tooltip("If true, enable a 'hotspot value' field to customize the cursor textures hotspot. Leave it as false then the default hotspot will be (0,0) i.e top left corner")]
        [HideInInspector] public bool customHotspot;
        [HideInInspector] public Vector2 hotspotValue;
        private Vector2 defaultHotspot;
        private readonly CursorMode cursorMode = CursorMode.ForceSoftware;


        // Awake is called before the start (first frame update)
        private void Awake()
        {
            //Check if a instace already exist and destroy it, otherwise don't destroy this object on load
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            // Set default parameters
            if (customHotspot)
                defaultHotspot = hotspotValue;
            else
                defaultHotspot = Vector2.zero;

            SetCustomCursor(defaultCursor);
        }

        //Set desired custom texture and hotspot
        public void SetCustomCursor(Texture2D cursorTexture)
        { 
            if(cursorTexture != null) Cursor.SetCursor(cursorTexture, defaultHotspot, cursorMode);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(DynamicCursorManager))]
    public class DynamicCursorManagerEditor : Editor
    {
        SerializedProperty customHotspotProp;
        SerializedProperty hotspotValueProp;

        private const string componentDescription = "Add the 'Dynamic Cursor Trigger' component on each GameObject you would like to change the cursor";

        void OnEnable()
        {
            customHotspotProp = serializedObject.FindProperty("customHotspot");
            hotspotValueProp = serializedObject.FindProperty("hotspotValue");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox(componentDescription, MessageType.Info);
            DrawDefaultInspector();

            serializedObject.Update();

            EditorGUILayout.PropertyField(customHotspotProp);

            if (customHotspotProp.boolValue)
            {
                EditorGUILayout.PropertyField(hotspotValueProp);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
