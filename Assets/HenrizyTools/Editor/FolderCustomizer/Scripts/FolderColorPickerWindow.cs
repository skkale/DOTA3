using UnityEditor;
using UnityEngine;

namespace Henrizy.Editor.ColoredFolder
{
    public class FolderCustomizeWindow : EditorWindow
    {
        private static string _folderPath;
        private Color _selectedColor;
        private Color _selectedTextColor;
        private string _tooltip;
        private int _chosenStyle = 0;
        private string[] _restrictStringInTooltip = new string[]
        {
            "-", ";", ":"
        };

        // Open the window and pass the selected folder path
        public static void Open(string path)
        {
            _folderPath = path;
            FolderCustomizeWindow window = GetWindow<FolderCustomizeWindow>("Customize Folder");
            window._selectedColor = FolderCustomizerMeta.LoadFolderColor(path);
            window._selectedTextColor = FolderCustomizerMeta.LoadFolderTextColor(path);
            window._tooltip = FolderCustomizerMeta.LoadFolderTooltip(path);
            if (!int.TryParse(FolderCustomizerMeta.LoadStyle(path), out window._chosenStyle))
            {
                window._chosenStyle = 0;
            }
            window.minSize = new Vector2(300, 300);
            window.Show();
        }

        void OnGUI()
        {
            // Display the color picker
            this._selectedColor = EditorGUILayout.ColorField("Folder Color:", _selectedColor);
            this._selectedTextColor = EditorGUILayout.ColorField("Text Color:", this._selectedTextColor);

            EditorGUILayout.LabelField("Tooltip:");
            this._tooltip = EditorGUILayout.TextArea(this._tooltip, GUILayout.Height(60));
            this._tooltip = this._tooltip.ReplaceAny(_restrictStringInTooltip, "");

            var alignLeftIcon = EditorGUIUtility.IconContent("align_horizontally_left");
            var alignCenterIcon = EditorGUIUtility.IconContent("align_horizontally_center");
            var alignRightIcon = EditorGUIUtility.IconContent("align_horizontally_right");

            this._chosenStyle = EditorGUILayout.Popup("Style", this._chosenStyle, new string[]
            {
                "Normal", "Bold", "Italic", "Bold & Italic"
            });

            // Apply and save the color
            if (GUILayout.Button("Apply"))
            {
                if (Mathf.Approximately(this._selectedColor.a, 0f))
                {
                    bool proceed = EditorUtility.DisplayDialog(
                        "Transparent Color",
                        "The selected folder color is fully transparent. Do you want to proceed?",
                        "Yes", "No"
                    );
                    if (!proceed)
                        return;
                }

                FolderCustomizerMeta.SetFolderMetadata(_folderPath, FolderCustomizerMeta.CustomColorKey, $"{ColorUtility.ToHtmlStringRGBA(this._selectedColor)}-{this._selectedColor.a}");
                FolderCustomizerMeta.SetFolderMetadata(_folderPath, FolderCustomizerMeta.CustomTextColorKey, $"{ColorUtility.ToHtmlStringRGBA(this._selectedTextColor)}-{this._selectedTextColor.a}");
                FolderCustomizerMeta.SetFolderMetadata(_folderPath, FolderCustomizerMeta.CustomTooltipKey, this._tooltip);
                FolderCustomizerMeta.SetFolderMetadata(_folderPath, FolderCustomizerMeta.CustomStyleKey, this._chosenStyle.ToString());
                AssetDatabase.Refresh(); // Refresh the Project window to apply changes
                Close();
            }
        }
    }
}