using Klei.HotLava;
using Klei.HotLava.UI;

namespace HotLavaArchipelagoPlugin.Helpers
{
    internal static class UIHelper
    {
        public static void SendNotificationMessage(string message)
        {
            CharacterCanvas characterCanvas = Singleton<LevelSingleton>.Instance.m_CharacterCanvas;
            characterCanvas?.m_PushMessages.QueueMessage(message);
        }

        public static void SendChatMessage(string message)
        {
            CharacterCanvas characterCanvas = Singleton<LevelSingleton>.Instance.m_CharacterCanvas;
            characterCanvas?.m_NetworkChat.QueueMessage(message);
        }

        public static void ShowPopup(string message)
        {
            CharacterCanvas characterCanvas = Singleton<LevelSingleton>.Instance.m_CharacterCanvas;
            UnityEngine.Vector2 vector = new UnityEngine.Vector2();
            vector.x = 502;
            vector.y = 142;
            characterCanvas?.m_Popup.ShowPopup(message, null, UnityEngine.Color.white, UnityEngine.TextAnchor.MiddleCenter, 2.6f, vector);
        }
    }
}
