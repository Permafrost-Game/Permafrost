using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentPipeline
{
    [ContentImporter(".tmx", DefaultProcessor = "TiledMapProcessor", DisplayName = "TileMap Importer - ContentPipeline.TileMap")]
    public class TiledMapImporter : ContentImporter<SerialisedTileMap>
    {
    }
}
