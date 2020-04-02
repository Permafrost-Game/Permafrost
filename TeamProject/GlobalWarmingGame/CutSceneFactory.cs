using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace GlobalWarmingGame
{
    static class CutSceneFactory
    {
        private static Dictionary<VideoN, Video> cutScenes;
        private static VideoPlayer vp;
        private static float timeSinceStartOfVideo;
        private static Video video;
        public static void LoadContent(ContentManager content)
        {
            vp = new VideoPlayer();
            cutScenes = new Dictionary<VideoN, Video>
            {
                { VideoN.Intro, content.Load<Video>("cutscenes/0000000") }
            };

        }
        public delegate void VideoEvent();
        private static VideoEvent onVideoEnd;


        public static void PlayVideo(VideoN vid, VideoEvent onVideoEnd)
        {
            video = cutScenes[vid];
            CutSceneFactory.onVideoEnd = onVideoEnd;
            timeSinceStartOfVideo = 0f;
            vp.Play(video);
        }


        public static void StopVideo()
        {
            vp.Stop();
        }

        public static void Update(GameTime gameTime)
        {
            timeSinceStartOfVideo += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeSinceStartOfVideo >= video.Duration.TotalMilliseconds)
            {
                onVideoEnd?.Invoke();
            }
        }

        public static void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            Texture2D texture;
            if (vp.State != MediaState.Stopped)
            {
                try
                {
                    texture = vp.GetTexture();
                    spriteBatch.Draw(texture, graphicsDevice.Viewport.Bounds, Color.White);
                }
                catch (System.InvalidOperationException)
                {
                    System.Console.WriteLine("Video failed to process a frame");
                }
            }
        }
    }
}
public enum VideoN
{
    Intro
}
