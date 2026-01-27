using UnityEngine;

namespace HotLavaArchipelagoPlugin.Factories
{
    internal class SpriteFactory
    {
        public static Sprite FromImage(byte[] image)
        {
            Texture2D texture = new Texture2D(256, 256);
            texture.LoadImage(image);

            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        }

        public static Sprite GetArchipelagoSprite()
        {
            return FromImage(Properties.Resources.ArchipelagoLogo);
        }
    }
}
