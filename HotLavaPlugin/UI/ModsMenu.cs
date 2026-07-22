using BepInEx;
using HotLavaArchipelagoPlugin.Archipelago;
using Klei.HotLava;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace HotLavaArchipelagoPlugin.UI
{
    /// <summary>
    /// The "Mods" panel reachable from the pause menu. Lets the player view/edit the
    /// Archipelago connection details and connect/disconnect without touching the config file.
    /// Built entirely at runtime by <see cref="ModsMenuFactory"/> since the plugin has no
    /// access to the game's Unity prefabs/scenes.
    /// </summary>
    internal class ModsMenu : MenuTransition
    {
        public InputField HostField = null!;
        public InputField PortField = null!;
        public InputField PlayerNameField = null!;
        public InputField PasswordField = null!;
        public Text StatusText = null!;
        public Button ConnectButton = null!;
        public Text ConnectButtonText = null!;
        public Button BackButton = null!;

        private bool _isBusy;

        protected override void OnDisplay()
        {
            base.OnDisplay();
            RefreshFields();
        }

        private void RefreshFields()
        {
            HostField.text = Plugin.ConfigArchipelagoHost.Value;
            PortField.text = Plugin.ConfigArchipelagoPort.Value.ToString();
            PlayerNameField.text = Plugin.ConfigArchipelagoPlayerName.Value;
            PasswordField.text = Plugin.ConfigArchipelagoPassword.Value;
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            if (_isBusy)
            {
                StatusText.text = Multiworld.Connected ? "Disconnecting..." : "Connecting...";
            }
            else if (Multiworld.Connected)
            {
                StatusText.text = $"Connected as {Multiworld.Instance.PlayerName}";
            }
            else
            {
                StatusText.text = "Not connected";
            }

            ConnectButtonText.text = Multiworld.Connected ? "Disconnect" : "Connect";
            ConnectButton.interactable = !_isBusy;
        }

        public void OnConnectClicked()
        {
            if (_isBusy)
            {
                return;
            }

            if (Multiworld.Connected)
            {
                _isBusy = true;
                UpdateStatus();

                Task.Run(async () =>
                {
                    await Multiworld.Disconnect();
                    ThreadingHelper.Instance.StartSyncInvoke(() =>
                    {
                        _isBusy = false;
                        UpdateStatus();
                    });
                });

                return;
            }

            string host = HostField.text.Trim();
            string playerName = PlayerNameField.text.Trim();
            string password = PasswordField.text;

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(playerName))
            {
                StatusText.text = "Host and player name are required";
                return;
            }

            if (!int.TryParse(PortField.text.Trim(), out int port))
            {
                StatusText.text = "Port must be a number";
                return;
            }

            Plugin.ConfigArchipelagoHost.Value = host;
            Plugin.ConfigArchipelagoPort.Value = port;
            Plugin.ConfigArchipelagoPlayerName.Value = playerName;
            Plugin.ConfigArchipelagoPassword.Value = password;
            Plugin.ConfigArchipelagoHost.ConfigFile.Save();

            _isBusy = true;
            UpdateStatus();

            Task.Run(async () =>
            {
                await Multiworld.ConnectDirect(host, port, playerName, string.IsNullOrEmpty(password) ? null : password);
                ThreadingHelper.Instance.StartSyncInvoke(() =>
                {
                    _isBusy = false;
                    UpdateStatus();
                });
            });
        }

        public void OnBackClicked()
        {
            ReturnTranstion();
        }
    }
}
