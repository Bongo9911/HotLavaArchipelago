using BepInEx;
using Klei.HotLava;
using Klei.HotLava.UI;

namespace HotLavaArchipelagoPlugin.Helpers
{
    internal static class UIHelper
    {
        public static void SendNotificationMessage(string message)
        {
            //Needs to be ran on main thread or the game crashes
            ThreadingHelper.Instance.StartSyncInvoke(() =>
            {
                CharacterCanvas characterCanvas = Singleton<LevelSingleton>.Instance.m_CharacterCanvas;
                characterCanvas?.m_PushMessages.QueueMessage(message);
            });
        }

        public static void SendChatMessage(string message)
        {
            //Needs to be ran on main thread or the game crashes
            ThreadingHelper.Instance.StartSyncInvoke(() =>
            {
                CharacterCanvas characterCanvas = Singleton<LevelSingleton>.Instance.m_CharacterCanvas;
                characterCanvas?.m_NetworkChat.QueueMessage(message);
            });
        }

        public static void ShowPopup(string message, float width = 502, float height = 142)
        {
            //Needs to be ran on main thread or the game crashes
            ThreadingHelper.Instance.StartSyncInvoke(() =>
            {
                CharacterCanvas characterCanvas = Singleton<LevelSingleton>.Instance.m_CharacterCanvas;
                UnityEngine.Vector2 vector = new UnityEngine.Vector2()
                {
                    x = width,
                    y = height
                };
                characterCanvas?.m_Popup.ShowPopup(message, null, UnityEngine.Color.white, UnityEngine.TextAnchor.MiddleCenter, 2.6f, vector);
            });
        }
    }
}
