/*#region Licence
 *     Copyright (C) 2011 by Catalin Zima-Zegreanu

    Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”),
    to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
    and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
    WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Engine.Lighting
{
    public enum ShadowmapSize
    {
        Size128 = 6,
        Size256 = 7,
        Size512 = 8,
        Size1024 = 9,
    }
    /// <remarks>
    /// This class was created by Catalin ZZ 
    /// Source http://www.catalinzima.com/xna/samples/shader-based-dynamic-2d-smooth-shadows/
    /// </remarks>
    public class ShadowmapResolver
    {
        private GraphicsDevice graphicsDevice;

        private int reductionChainCount;
        private int baseSize;
        private int depthBufferSize;

        Effect resolveShadowsEffect;
        Effect reductionEffect;

        RenderTarget2D distortRT;
        RenderTarget2D shadowMap;
        RenderTarget2D shadowsRT;
        RenderTarget2D processedShadowsRT;

        QuadRenderComponent quadRender;
        RenderTarget2D distancesRT;
        RenderTarget2D[] reductionRT;


        /// <summary>
        /// Creates a new shadowmap resolver
        /// </summary>
        /// <param name="graphicsDevice">The Graphics Device used by the XNA game</param>
        /// <param name="quadRender"></param>
        /// <param name="baseSize">The size of the light regions </param>
        public ShadowmapResolver(GraphicsDevice graphicsDevice, QuadRenderComponent quadRender, ShadowmapSize maxShadowmapSize, ShadowmapSize maxDepthBufferSize)
        {
            this.graphicsDevice = graphicsDevice;
            this.quadRender = quadRender;

            reductionChainCount = (int)maxShadowmapSize;
            baseSize = 2 << reductionChainCount;
            depthBufferSize = 2 << (int)maxDepthBufferSize;
        }

        public void LoadContent(ContentManager content)
        {
            reductionEffect = content.Load<Effect>(@"shaders/reductionEffect");
            resolveShadowsEffect = content.Load<Effect>(@"shaders/resolveShadowsEffect");

            distortRT = new RenderTarget2D(graphicsDevice, baseSize, baseSize, false, SurfaceFormat.HalfVector2,DepthFormat.None);
            distancesRT = new RenderTarget2D(graphicsDevice, baseSize, baseSize, false, SurfaceFormat.HalfVector2,DepthFormat.None);
            shadowMap = new RenderTarget2D(graphicsDevice, 2, baseSize, false, SurfaceFormat.HalfVector2,DepthFormat.None);
            reductionRT = new RenderTarget2D[reductionChainCount];
            for (int i = 0; i < reductionChainCount; i++)
            {
                reductionRT[i] = new RenderTarget2D(graphicsDevice, 2 << i, baseSize, false, SurfaceFormat.HalfVector2,DepthFormat.None);
            }


            shadowsRT = new RenderTarget2D(graphicsDevice, baseSize, baseSize);
            processedShadowsRT = new RenderTarget2D(graphicsDevice, baseSize, baseSize);
        }

        public void ResolveShadows(Texture2D shadowCastersTexture, RenderTarget2D result, Vector2 lightPosition)
        {
            graphicsDevice.BlendState = BlendState.Opaque;

            ExecuteTechnique(shadowCastersTexture, distancesRT, "ComputeDistances");
            ExecuteTechnique(distancesRT, distortRT, "Distort");
            ApplyHorizontalReduction(distortRT, shadowMap);
            ExecuteTechnique(null, shadowsRT, "DrawShadows", shadowMap);
            ExecuteTechnique(shadowsRT, processedShadowsRT, "BlurHorizontally");
            ExecuteTechnique(processedShadowsRT, result, "BlurVerticallyAndAttenuate");
        }

        private void ExecuteTechnique(Texture2D source, RenderTarget2D destination, string techniqueName)
        {
            ExecuteTechnique(source, destination, techniqueName, null);
        }

        private void ExecuteTechnique(Texture2D source, RenderTarget2D destination, string techniqueName, Texture2D shadowMap)
        {
            Vector2 renderTargetSize;
            renderTargetSize = new Vector2((float)baseSize, (float)baseSize);
            graphicsDevice.SetRenderTarget(destination);
            graphicsDevice.Clear(Color.White);
            resolveShadowsEffect.Parameters["renderTargetSize"].SetValue(renderTargetSize);

            if (source != null)
                resolveShadowsEffect.Parameters["InputTexture"].SetValue(source);
            if (shadowMap !=null)
                resolveShadowsEffect.Parameters["ShadowMapTexture"].SetValue(shadowMap);

            resolveShadowsEffect.CurrentTechnique = resolveShadowsEffect.Techniques[techniqueName];
            
            foreach (EffectPass pass in resolveShadowsEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                quadRender.Render(Vector2.One * -1, Vector2.One);
            }
            graphicsDevice.SetRenderTarget(null);
        }


        private void ApplyHorizontalReduction(RenderTarget2D source, RenderTarget2D destination)
        {
            int step = reductionChainCount-1;
            RenderTarget2D s = source;
            RenderTarget2D d = reductionRT[step];
            reductionEffect.CurrentTechnique = reductionEffect.Techniques["HorizontalReduction"];

            while (step >= 0)
            {
                d = reductionRT[step];

                graphicsDevice.SetRenderTarget(d);
                graphicsDevice.Clear(Color.White);

                reductionEffect.Parameters["SourceTexture"].SetValue(s);
                Vector2 textureDim = new Vector2(1.0f / (float)s.Width, 1.0f / (float)s.Height);
                reductionEffect.Parameters["TextureDimensions"].SetValue(textureDim);

                foreach (EffectPass pass in reductionEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    quadRender.Render(Vector2.One * -1, new Vector2(1, 1));
                }

                graphicsDevice.SetRenderTarget(null);

                s = d;
                step--;
            }

            //copy to destination
            graphicsDevice.SetRenderTarget(destination);
            reductionEffect.CurrentTechnique = reductionEffect.Techniques["Copy"];
            reductionEffect.Parameters["SourceTexture"].SetValue(d);

            foreach (EffectPass pass in reductionEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                quadRender.Render(Vector2.One * -1, new Vector2(1, 1));
            }

            reductionEffect.Parameters["SourceTexture"].SetValue(reductionRT[reductionChainCount - 1]);
            graphicsDevice.SetRenderTarget(null);
        }

        
    }
}
