using Klei.HotLava.UI;
using Klei.L10n;
using UnityEngine;
using UnityEngine.UI;

namespace HotLavaArchipelagoPlugin.UI
{
    /// <summary>
    /// Builds the "Mods" pause-menu button and its <see cref="ModsMenu"/> panel entirely at
    /// runtime, since a plugin has no access to the game's Unity prefabs/scenes. The pause
    /// menu button is cloned from an existing button (for click/hover behaviour and styling),
    /// but the panel's own text and input fields are built from scratch rather than cloned from
    /// other game UI: cloning brought along hidden state we don't control (stray child
    /// captions, inherited interactable/layout flags, event wiring pointing back at the
    /// original component), which caused ghost text and uneditable fields. Only the font asset
    /// is reused from the game, for visual consistency.
    /// </summary>
    internal static class ModsMenuFactory
    {
        private const string ButtonName = "ModsButton";

        public static void InstallModsButton(PauseMenu pauseMenu)
        {
            if (pauseMenu.transform.Find(ButtonName) != null)
            {
                // Already installed for this PauseMenu instance.
                return;
            }

            Button buttonTemplate = pauseMenu.m_SpectateButton;
            if (buttonTemplate == null)
            {
                Plugin.Logger.LogWarning("Could not find a button template for the Mods menu; skipping.");
                return;
            }

            Text? buttonTemplateLabel = buttonTemplate.GetComponentInChildren<Text>(true);
            Font font = buttonTemplateLabel != null ? buttonTemplateLabel.font : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            ModsMenu modsMenu = BuildModsPanel(pauseMenu, buttonTemplate, font);

            Button modsButton = Object.Instantiate(buttonTemplate, buttonTemplate.transform.parent, false);
            modsButton.name = ButtonName;
            // Button.onClick.RemoveAllListeners() only clears runtime listeners; it leaves the
            // persistent listener baked into the Spectate button's prefab (StartSpectating())
            // in place, so clicking a clone would also trigger the original Spectate behaviour.
            // Replacing the event object entirely discards that persistent call.
            modsButton.onClick = new Button.ButtonClickedEvent();
            modsButton.interactable = true;
            modsButton.gameObject.SetActive(true);

            RemoveTranslators(modsButton.gameObject);
            RemoveInteractEventTriggers(modsButton.gameObject);

            Text buttonLabel = modsButton.GetComponentInChildren<Text>(true);
            if (buttonLabel != null)
            {
                buttonLabel.text = "ARCHIPELAGO";
            }

            // The right-hand action list (Daily Challenges .. Restart, Quit To Menu) is one
            // VerticalLayoutGroup, so re-inserting via sibling index (rather than a manual
            // position offset) both places this button right after Ability Help *and* pushes
            // Restart/Quit To Menu down to make room, with no extra bookkeeping needed.
            Button? abilityHelpButton = FindButtonByOnClickMethod(pauseMenu.transform, nameof(PauseMenu.OnClick_AbilityHelp));
            if (abilityHelpButton != null && abilityHelpButton.transform.parent == modsButton.transform.parent)
            {
                modsButton.transform.SetSiblingIndex(abilityHelpButton.transform.GetSiblingIndex() + 1);
            }
            else
            {
                Plugin.Logger.LogWarning("Could not find the Ability Help button to position the Mods button after; leaving it at the end of the list.");
            }

            modsButton.onClick.AddListener(() => modsMenu.ShowMenu(pauseMenu.m_PauseMenu));
        }

        /// <summary>
        /// Finds a Button under root whose onClick has a persistent (Inspector-wired) call to
        /// the given method name. Used to locate vanilla buttons that PauseMenu doesn't expose
        /// as a field (e.g. Ability Help), without depending on GameObject names or on text that
        /// may not be resolved yet (TextTranslator applies its localized string in Start(),
        /// which may not have run yet when this is called from an Awake postfix).
        /// </summary>
        private static Button? FindButtonByOnClickMethod(Transform root, string methodName)
        {
            foreach (Button button in root.GetComponentsInChildren<Button>(true))
            {
                int count = button.onClick.GetPersistentEventCount();
                for (int i = 0; i < count; i++)
                {
                    if (button.onClick.GetPersistentMethodName(i) == methodName)
                    {
                        return button;
                    }
                }
            }

            return null;
        }

        private static ModsMenu BuildModsPanel(PauseMenu pauseMenu, Button buttonTemplate, Font font)
        {
            GameObject panelGO = new GameObject("ModsMenuPanel", typeof(RectTransform));
            panelGO.transform.SetParent(pauseMenu.transform, false);
            RectTransform panelRect = (RectTransform)panelGO.transform;
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;

            Image dim = panelGO.AddComponent<Image>();
            dim.color = new Color(0f, 0f, 0f, 0.75f);

            ModsMenu modsMenu = panelGO.AddComponent<ModsMenu>();

            GameObject boxGO = new GameObject("ContentBox", typeof(RectTransform));
            boxGO.transform.SetParent(panelGO.transform, false);
            RectTransform boxRect = (RectTransform)boxGO.transform;
            boxRect.anchorMin = new Vector2(0.5f, 0.5f);
            boxRect.anchorMax = new Vector2(0.5f, 0.5f);
            boxRect.pivot = new Vector2(0.5f, 0.5f);
            boxRect.sizeDelta = new Vector2(640f, 420f);

            Image boxImage = boxGO.AddComponent<Image>();
            boxImage.color = new Color(0.08f, 0.08f, 0.1f, 0.96f);

            VerticalLayoutGroup layout = boxGO.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(32, 32, 32, 32);
            layout.spacing = 14f;
            layout.childAlignment = TextAnchor.UpperCenter;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            Text title = CreateText(boxGO.transform, "Archipelago", font, 28);
            title.alignment = TextAnchor.MiddleCenter;
            SetPreferredHeight(title, 40f);

            modsMenu.HostField = CreateFieldRow(boxGO.transform, "Host", font);
            modsMenu.PortField = CreateFieldRow(boxGO.transform, "Port", font);
            modsMenu.PortField.contentType = InputField.ContentType.IntegerNumber;
            modsMenu.PlayerNameField = CreateFieldRow(boxGO.transform, "Player Name", font);
            modsMenu.PasswordField = CreateFieldRow(boxGO.transform, "Password", font);
            modsMenu.PasswordField.contentType = InputField.ContentType.Password;

            Text status = CreateText(boxGO.transform, "Not connected", font, 20);
            status.alignment = TextAnchor.MiddleCenter;
            SetPreferredHeight(status, 30f);
            modsMenu.StatusText = status;

            GameObject buttonRow = new GameObject("ButtonRow", typeof(RectTransform));
            buttonRow.transform.SetParent(boxGO.transform, false);
            HorizontalLayoutGroup buttonRowLayout = buttonRow.AddComponent<HorizontalLayoutGroup>();
            buttonRowLayout.spacing = 60f;
            buttonRowLayout.childAlignment = TextAnchor.MiddleCenter;
            buttonRowLayout.childForceExpandWidth = false;
            buttonRowLayout.childForceExpandHeight = false;
            LayoutElement buttonRowElement = buttonRow.AddComponent<LayoutElement>();
            buttonRowElement.preferredHeight = 60f;

            Button connectButton = CreateButton(buttonTemplate, buttonRow.transform, "Connect");
            Button backButton = CreateButton(buttonTemplate, buttonRow.transform, "Back");

            modsMenu.ConnectButton = connectButton;
            modsMenu.ConnectButtonText = connectButton.GetComponentInChildren<Text>(true);
            modsMenu.BackButton = backButton;

            connectButton.onClick.AddListener(modsMenu.OnConnectClicked);
            backButton.onClick.AddListener(modsMenu.OnBackClicked);

            MenuScreen screen = panelGO.GetComponent<MenuScreen>();
            if (screen != null)
            {
                screen.SetDefaultButton(backButton);
                screen.m_GamepadActions[(int)MenuScreen.StandardButtons.Cancel] = backButton;
            }

            panelGO.SetActive(false);

            return modsMenu;
        }

        private static InputField CreateFieldRow(Transform parent, string labelText, Font font)
        {
            GameObject row = new GameObject(labelText + "Row", typeof(RectTransform));
            row.transform.SetParent(parent, false);

            HorizontalLayoutGroup rowLayout = row.AddComponent<HorizontalLayoutGroup>();
            rowLayout.spacing = 12f;
            rowLayout.childAlignment = TextAnchor.MiddleLeft;
            rowLayout.childControlWidth = true;
            rowLayout.childControlHeight = true;
            rowLayout.childForceExpandWidth = false;
            rowLayout.childForceExpandHeight = true;

            LayoutElement rowElement = row.AddComponent<LayoutElement>();
            rowElement.preferredHeight = 28f;

            Text label = CreateText(row.transform, labelText, font, 20);
            LayoutElement labelElement = label.gameObject.AddComponent<LayoutElement>();
            labelElement.preferredWidth = 160f;
            labelElement.flexibleWidth = 0f;
            label.alignment = TextAnchor.MiddleLeft;

            InputField field = CreateInputField(row.transform, labelText + "Field", font);
            LayoutElement fieldElement = field.gameObject.AddComponent<LayoutElement>();
            fieldElement.flexibleWidth = 1f;
            fieldElement.preferredHeight = 16f;

            return field;
        }

        private static Text CreateText(Transform parent, string content, Font font, int fontSize)
        {
            GameObject go = new GameObject("Text", typeof(RectTransform));
            go.transform.SetParent(parent, false);
            Text text = go.AddComponent<Text>();
            text.font = font;
            text.fontSize = fontSize;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleLeft;
            text.text = content;
            return text;
        }

        private static InputField CreateInputField(Transform parent, string name, Font font)
        {
            GameObject go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);

            Image bg = go.AddComponent<Image>();
            bg.color = new Color(1f, 1f, 1f, 0.15f);

            InputField field = go.AddComponent<InputField>();
            field.targetGraphic = bg;
            field.interactable = true;

            GameObject textGO = new GameObject("Text", typeof(RectTransform));
            textGO.transform.SetParent(go.transform, false);
            Text text = textGO.AddComponent<Text>();
            text.font = font;
            text.fontSize = 18;
            text.color = Color.white;
            text.supportRichText = false;
            RectTransform textRect = (RectTransform)textGO.transform;
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(8, 4);
            textRect.offsetMax = new Vector2(-8, -4);
            field.textComponent = text;

            field.text = string.Empty;
            return field;
        }

        private static Button CreateButton(Button template, Transform parent, string label)
        {
            Button button = Object.Instantiate(template, parent, false);
            button.name = label + "Button";
            // See the comment in InstallModsButton: RemoveAllListeners() would not remove the
            // persistent Spectate listener baked into the source button's prefab.
            button.onClick = new Button.ButtonClickedEvent();
            button.gameObject.SetActive(true);

            RemoveTranslators(button.gameObject);
            RemoveInteractEventTriggers(button.gameObject);

            Text text = button.GetComponentInChildren<Text>(true);
            if (text != null)
            {
                text.text = label;
            }

            return button;
        }

        /// <summary>
        /// Removes Klei's TextTranslator components from a clone. TextTranslator re-applies its
        /// localized m_Key string to its sibling Text.text in Start(), which would otherwise
        /// stomp the custom label text we just set (e.g. every button cloned from the Spectate
        /// button would keep showing "Spectate").
        /// </summary>
        private static void RemoveTranslators(GameObject root)
        {
            foreach (TextTranslator translator in root.GetComponentsInChildren<TextTranslator>(true))
            {
                Object.Destroy(translator);
            }
        }

        /// <summary>
        /// Removes Klei's ButtonInteractEventTrigger components from a clone. This component
        /// fires its own m_OnActivate UnityEvent on click/submit, independent of Button.onClick,
        /// and can carry the same kind of persistent (Inspector-wired) listener pointing back at
        /// the source button's original behaviour (e.g. StartSpectating()).
        /// </summary>
        private static void RemoveInteractEventTriggers(GameObject root)
        {
            foreach (ButtonInteractEventTrigger trigger in root.GetComponentsInChildren<ButtonInteractEventTrigger>(true))
            {
                Object.Destroy(trigger);
            }
        }

        private static void SetPreferredHeight(Text text, float height)
        {
            LayoutElement element = text.gameObject.GetComponent<LayoutElement>();
            if (element == null)
            {
                element = text.gameObject.AddComponent<LayoutElement>();
            }
            element.preferredHeight = height;
        }
    }
}
