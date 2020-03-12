using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace GlobalWarmingGame
{
    static class CutSceneFactory  
    {
        private static Dictionary<VideoN, Video> cutScenes;
        private static Texture2D videoTexture;
        private static VideoPlayer vp;
        private static Video video; 
        public static void LoadContent(ContentManager content)
        {
            vp = new VideoPlayer();
            cutScenes = new Dictionary<VideoN, Video>
            {
                { VideoN.Intro, content.Load<Video>("cutscenes/0000000") }
            };

        }
        public static void PlayVideo(VideoN vid)
        {
            video = cutScenes[vid];
            try
            {
                vp.Play(video);
            }
            catch (Exception e)
            {

            }
        }
        public static void StopVideo()
        {
            vp.Stop();
        }
        public static void Update(GameTime gameTime)
        {
            if (vp.State == MediaState.Playing)
            {
                videoTexture = vp.GetTexture();
            }
        }
        public static void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            Texture2D texture = null;
            if (vp.State != MediaState.Stopped)
            {
                texture = vp.GetTexture();
                if (texture != null)
                {
                    spriteBatch.Draw(texture, graphicsDevice.Viewport.Bounds, Color.White);
                }
            }
        }
    }
}
public enum VideoN
{
    Intro
}
