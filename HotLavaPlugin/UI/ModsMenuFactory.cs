using HotLavaArchipelagoPlugin.Factories;
using Klei.HotLava;
using Klei.HotLava.Intro;
using Klei.HotLava.UI;
using Klei.L10n;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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
            string sceneNameForLog = SceneManager.GetActiveScene().name;
            Plugin.Logger.LogInfo($"[Archipelago] InstallModsButton called. scene='{sceneNameForLog}'");

            if (FindDescendant(pauseMenu.transform, ButtonName) != null)
            {
                Plugin.Logger.LogInfo("[Archipelago] Mods button already present on this PauseMenu instance; skipping install.");
                return;
            }

            Button buttonTemplate = pauseMenu.m_SpectateButton;
            if (buttonTemplate == null)
            {
                Plugin.Logger.LogWarning("[Archipelago] Could not find a button template (m_SpectateButton is null) for the Mods menu; skipping.");
                return;
            }

            Text? buttonTemplateLabel = buttonTemplate.GetComponentInChildren<Text>(true);
            Font font = buttonTemplateLabel != null ? buttonTemplateLabel.font : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            ModsMenu modsMenu = BuildModsPanel(pauseMenu.transform, font);

            Button modsButton = Object.Instantiate(buttonTemplate, buttonTemplate.transform.parent, false);
            modsButton.name = ButtonName;

            // Must happen before the clone is (re)activated below: IntroPauseScreenOption reacts
            // to becoming active by synchronously deactivating the object again in the Intro/
            // ARWorld scenes (its own OnEnable calls SetActive(false)). Object.Destroy is
            // deferred to end of frame, so destroying it *after* SetActive(true) is too late -
            // the harmful OnEnable has already run by then. Stripping it first means the
            // component is gone (and disabled in the interim) before activation ever happens.
            RemoveIntroPauseScreenOptions(modsButton.gameObject);
            RemoveTranslators(modsButton.gameObject);
            RemoveInteractEventTriggers(modsButton.gameObject);

            // Button.onClick.RemoveAllListeners() only clears runtime listeners; it leaves the
            // persistent listener baked into the Spectate button's prefab (StartSpectating())
            // in place, so clicking a clone would also trigger the original Spectate behaviour.
            // Replacing the event object entirely discards that persistent call.
            modsButton.onClick = new Button.ButtonClickedEvent();
            modsButton.interactable = true;
            modsButton.gameObject.SetActive(true);

            Text buttonLabel = modsButton.GetComponentInChildren<Text>(true);
            if (buttonLabel != null)
            {
                buttonLabel.text = "ARCHIPELAGO";
            }

            // The right-hand action list (Daily Challenges .. Restart, Quit To Menu) is one
            // VerticalLayoutGroup, so re-inserting via sibling index (rather than a manual
            // position offset) both places this button right after Ability Help *and* pushes
            // Restart/Quit To Menu down to make room, with no extra bookkeeping needed.
            //
            // That whole action list is hidden outright during the "Intro" prologue and the
            // "ARWorld" tabletop mode - not per-button, but via an IntroPauseScreenOption on the
            // list's container itself - so a clone parented inside it would never actually be
            // visible there, no matter what components we strip off the clone. In those scenes,
            // re-home the button next to Settings instead, which sits in the small RESUME/
            // SETTINGS group that's never hidden.
            string sceneName = SceneManager.GetActiveScene().name;
            bool actionListHidden = sceneName == "Intro" || sceneName == "ARWorld";

            Button? abilityHelpButton = FindButtonByOnClickMethod(pauseMenu.transform, nameof(PauseMenu.OnClick_AbilityHelp));
            Plugin.Logger.LogInfo($"[Archipelago] actionListHidden={actionListHidden} abilityHelpButton={(abilityHelpButton != null ? abilityHelpButton.name : "null")} sameParentAsModsButton={(abilityHelpButton != null && abilityHelpButton.transform.parent == modsButton.transform.parent)}");

            if (!actionListHidden && abilityHelpButton != null && abilityHelpButton.transform.parent == modsButton.transform.parent)
            {
                modsButton.transform.SetSiblingIndex(abilityHelpButton.transform.GetSiblingIndex() + 1);
                Plugin.Logger.LogInfo("[Archipelago] Positioned Mods button after Ability Help.");
            }
            else
            {
                Button? settingsButton = FindButtonByOnClickMethod(pauseMenu.transform, nameof(PauseMenu.OnOptionsClick));
                Plugin.Logger.LogInfo($"[Archipelago] settingsButton={(settingsButton != null ? settingsButton.name : "null")}");
                if (settingsButton != null)
                {
                    modsButton.transform.SetParent(settingsButton.transform.parent, false);
                    modsButton.transform.SetSiblingIndex(settingsButton.transform.GetSiblingIndex() + 1);
                    Plugin.Logger.LogInfo($"[Archipelago] Positioned Mods button after Settings under parent '{settingsButton.transform.parent.name}'.");
                }
                else
                {
                    Plugin.Logger.LogWarning("[Archipelago] Could not find the Ability Help or Settings button to position the Mods button after; leaving it at the end of the list.");
                }
            }

            Plugin.Logger.LogInfo($"[Archipelago] Mods button final state: parent='{modsButton.transform.parent.name}' activeSelf={modsButton.gameObject.activeSelf} activeInHierarchy={modsButton.gameObject.activeInHierarchy} siblingIndex={modsButton.transform.GetSiblingIndex()}");

            modsButton.onClick.AddListener(() => modsMenu.ShowMenu(pauseMenu.m_PauseMenu));
        }

        private const string HamburgerButtonName = "ArchipelagoHamburgerButton";

        /// <summary>
        /// Installs an "ARCHIPELAGO" entry into the title screen's hamburger/settings overlay
        /// (Settings / Profiles / Credits / Exit Game). That overlay's class (HamburgerUI) is
        /// internal to the game's assembly, so it's only ever referenced here through its public
        /// MenuTransition base - we don't need any of its own members, just the transform to
        /// build under and ShowMenu/transition to hook into.
        /// </summary>
        public static void InstallArchipelagoButton(MenuTransition hamburgerMenu)
        {
            if (FindDescendant(hamburgerMenu.transform, HamburgerButtonName) != null)
            {
                // Already installed for this HamburgerUI instance.
                return;
            }

            // "EXIT GAME" is the one entry we can identify with certainty (MainMenu.Quit() is a
            // public method), so it anchors both the button's visual style and its position.
            Button? quitButton = FindButtonByPersistentCall(hamburgerMenu.transform, typeof(MainMenu), nameof(MainMenu.Quit));
            if (quitButton == null)
            {
                Plugin.Logger.LogWarning("Could not find the Exit Game button in the hamburger menu; skipping the Archipelago button there.");
                return;
            }

            // Exit Game is styled as a "danger" action (red text in the vanilla UI), so clone the
            // Settings button instead for a normal-looking entry. Identify it precisely by its
            // persistent onClick target (OptionsMenu.ShowMenu) rather than just picking "some
            // other sibling of Exit Game" - that grabbed an unrelated control (most likely the
            // overlay's own close button) last time, which is why the clone's leftover
            // click-handling closed the whole menu instead of opening ours.
            Button buttonTemplate = FindButtonByPersistentCall(hamburgerMenu.transform, typeof(OptionsMenu), nameof(OptionsMenu.ShowMenu)) ?? quitButton;
            if (buttonTemplate == quitButton)
            {
                Plugin.Logger.LogWarning("Could not find the Settings button to use as a style template; falling back to Exit Game's style.");
            }

            Text? buttonTemplateLabel = buttonTemplate.GetComponentInChildren<Text>(true);
            Font font = buttonTemplateLabel != null ? buttonTemplateLabel.font : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            // Build the panel as a *sibling* of the hamburger menu, not a child of it: ShowMenu
            // deactivates the "from" menu's whole GameObject when transitioning away, and since
            // HamburgerUI (unlike PauseMenu) has no separate controller/panel split, its own
            // GameObject IS the from-menu - a panel nested inside it would get hidden right along
            // with it, which is exactly what made everything disappear on click.
            Transform panelParent = hamburgerMenu.transform.parent != null ? hamburgerMenu.transform.parent : hamburgerMenu.transform;
            ModsMenu modsMenu = BuildModsPanel(panelParent, font);

            Button archipelagoButton = Object.Instantiate(buttonTemplate, buttonTemplate.transform.parent, false);
            archipelagoButton.name = HamburgerButtonName;
            archipelagoButton.onClick = new Button.ButtonClickedEvent();
            archipelagoButton.interactable = true;
            archipelagoButton.gameObject.SetActive(true);

            RemoveTranslators(archipelagoButton.gameObject);
            RemoveInteractEventTriggers(archipelagoButton.gameObject);
            RemoveIntroPauseScreenOptions(archipelagoButton.gameObject);

            Text archipelagoLabel = archipelagoButton.GetComponentInChildren<Text>(true);
            if (archipelagoLabel != null)
            {
                archipelagoLabel.text = "ARCHIPELAGO";
            }

            // Settings' icon (a gear) is a child Image separate from the button's own background
            // Image; swap it for the Archipelago logo. Reset color to white so the logo's own
            // colors render untinted, regardless of whatever tint the gear icon used.
            foreach (Image image in archipelagoButton.GetComponentsInChildren<Image>(true))
            {
                if (image.gameObject != archipelagoButton.gameObject)
                {
                    // rect.size (not sizeDelta) reflects the actual current box size even if
                    // it's driven by stretch anchors rather than an authored sizeDelta.
                    Vector2 originalIconSize = image.rectTransform.rect.size;

                    image.sprite = SpriteFactory.GetArchipelagoSprite();
                    image.color = Color.white;

                    // Without an explicit LayoutElement, Image.preferredWidth/Height (used by the
                    // row's layout group) comes from the sprite's own native pixel size - the
                    // logo's dimensions differ from the gear icon's, so the icon's box widened
                    // and pushed the label away from it. Pin the box back to its original size;
                    // only the picture inside it should have changed.
                    image.rectTransform.sizeDelta = originalIconSize;
                    LayoutElement iconLayoutElement = image.gameObject.GetComponent<LayoutElement>();
                    if (iconLayoutElement == null)
                    {
                        iconLayoutElement = image.gameObject.AddComponent<LayoutElement>();
                    }
                    iconLayoutElement.preferredWidth = originalIconSize.x;
                    iconLayoutElement.preferredHeight = originalIconSize.y;

                    break;
                }
            }

            // Insert right before Exit Game, matching the pause menu's layout (Archipelago
            // grouped with the normal actions, ahead of the destructive one).
            archipelagoButton.transform.SetSiblingIndex(quitButton.transform.GetSiblingIndex());

            archipelagoButton.onClick.AddListener(() => modsMenu.ShowMenu(hamburgerMenu));
        }

        /// <summary>
        /// Transform.Find only searches direct children, not the full descendant tree. Our
        /// installed buttons aren't necessarily direct children of the menu root (they're
        /// siblings of whatever template button they were cloned from, which may be nested a
        /// few levels down), so the idempotency checks need a proper recursive search instead.
        /// </summary>
        private static Transform? FindDescendant(Transform root, string name)
        {
            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
            {
                if (child.name == name)
                {
                    return child;
                }
            }

            return null;
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

        /// <summary>
        /// Same idea as <see cref="FindButtonByOnClickMethod"/>, but also checks the persistent
        /// call's target type. Needed when the method name alone isn't unique (e.g. many buttons
        /// across the game call the inherited MenuTransition.ShowMenu).
        /// </summary>
        private static Button? FindButtonByPersistentCall(Transform root, System.Type targetType, string methodName)
        {
            foreach (Button button in root.GetComponentsInChildren<Button>(true))
            {
                int count = button.onClick.GetPersistentEventCount();
                for (int i = 0; i < count; i++)
                {
                    if (button.onClick.GetPersistentMethodName(i) != methodName)
                    {
                        continue;
                    }

                    Object target = button.onClick.GetPersistentTarget(i);
                    if (target != null && targetType.IsInstanceOfType(target))
                    {
                        return button;
                    }
                }
            }

            return null;
        }

        private static ModsMenu BuildModsPanel(Transform parent, Font font)
        {
            GameObject panelGO = new GameObject("ModsMenuPanel", typeof(RectTransform));
            panelGO.transform.SetParent(parent, false);
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
            buttonRowLayout.childControlWidth = true;
            buttonRowLayout.childControlHeight = true;
            buttonRowLayout.childForceExpandWidth = false;
            buttonRowLayout.childForceExpandHeight = false;
            LayoutElement buttonRowElement = buttonRow.AddComponent<LayoutElement>();
            buttonRowElement.preferredHeight = 60f;

            Button connectButton = CreateButton(buttonRow.transform, "Connect", font);
            Button backButton = CreateButton(buttonRow.transform, "Back", font);

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

        /// <summary>
        /// Builds a plain Connect/Back-style button from scratch rather than cloning one of the
        /// game's own buttons. Cloning kept surfacing hidden behaviour we don't control (ghost
        /// text, a click also triggering the source button's old action, and - as it turns out -
        /// something continually fighting our layout group for position every frame), so this
        /// button only ever reuses the game's font, nothing else.
        /// </summary>
        private static Button CreateButton(Transform parent, string label, Font font)
        {
            GameObject go = new GameObject(label + "Button", typeof(RectTransform));
            go.transform.SetParent(parent, false);

            Image background = go.AddComponent<Image>();
            background.color = new Color(1f, 1f, 1f, 0.15f);

            Button button = go.AddComponent<Button>();
            button.targetGraphic = background;
            ColorBlock colors = button.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color(0.85f, 0.85f, 0.85f, 1f);
            colors.pressedColor = new Color(0.6f, 0.6f, 0.6f, 1f);
            colors.selectedColor = colors.highlightedColor;
            button.colors = colors;

            GameObject textGO = new GameObject("Text", typeof(RectTransform));
            textGO.transform.SetParent(go.transform, false);
            Text text = textGO.AddComponent<Text>();
            text.font = font;
            text.fontSize = 20;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            text.text = label;
            RectTransform textRect = (RectTransform)textGO.transform;
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            LayoutElement layoutElement = go.AddComponent<LayoutElement>();
            layoutElement.preferredWidth = 180f;
            layoutElement.preferredHeight = 50f;

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

            // Belt-and-suspenders: also strip Unity's own stock EventTrigger, in case a source
            // button uses that (with its own persistent listeners) instead of/alongside Klei's.
            foreach (EventTrigger trigger in root.GetComponentsInChildren<EventTrigger>(true))
            {
                Object.Destroy(trigger);
            }
        }

        /// <summary>
        /// Removes IntroPauseScreenOption components from a clone. The Spectate button we clone
        /// the pause-menu button from carries one of these (it hides Spectate/Ability
        /// Help/Restart/Quit during the "Intro" prologue scene); cloning copies it along, and
        /// since we then explicitly SetActive(true) the clone, its OnEnable() fires and
        /// immediately hides our button again whenever the active scene is "Intro". We want the
        /// Archipelago button visible everywhere, including the prologue, so it's stripped here.
        /// </summary>
        private static void RemoveIntroPauseScreenOptions(GameObject root)
        {
            foreach (IntroPauseScreenOption option in root.GetComponentsInChildren<IntroPauseScreenOption>(true))
            {
                // Object.Destroy is deferred to the end of the frame, so the component is still
                // fully live - and its OnEnable would still fire and re-hide the object - if the
                // GameObject gets SetActive(true) before then. Disabling it first makes that
                // OnEnable a no-op immediately, regardless of when the actual destroy lands.
                option.enabled = false;
                Object.Destroy(option);
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
