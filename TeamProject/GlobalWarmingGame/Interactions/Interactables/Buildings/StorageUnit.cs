using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    /// <summary>
    /// This class encapsulates a storage unit object<br/>
    /// This is an <see cref="IInteractable"/> object that will store a single <see cref="ResourceItem"/><br/>
    /// </summary>
    class StorageUnit : Sprite, IInteractable, IBuildable, IReconstructable
    {
        public List<ResourceItem> CraftingCosts { get; } = new List<ResourceItem>() { new ResourceItem(Resource.Stone, 4),
                                                                                      new ResourceItem(Resource.Wood,  8)};

        public List<InstructionType> InstructionTypes { get; private set; }

        public readonly static ResourceItem EMPTY_RESOURCE_ITEM = new ResourceItem(new ResourceType());

        public ResourceItem ResourceItem { get; set; }

        [PFSerializable]
        public ResourceItem PFSResourceItem
        {
            get { return ResourceItem != null ? ResourceItem : EMPTY_RESOURCE_ITEM; }
            set { ResourceItem = value; }
        }

        public InstructionType StoreInstruction { get; private set; }

        [PFSerializable]
        public Vector2 PFSPosition
        {
            get { return Position; }
            set { Position = value; }
        }

        public StorageUnit() : base(Vector2.Zero, Textures.Map[TextureTypes.StorageUnit])
        {

        }

        public StorageUnit(Vector2 position, ResourceItem resourceItem = null) : base
        (
            position: position,
            texture: Textures.Map[TextureTypes.StorageUnit]
        )
        {
            ResetState();

            if (resourceItem != null &&
                resourceItem.ResourceType.displayName != ResourceItemEmpty.ResourceType.displayName)
            {
                this.ResourceItem = resourceItem;

                InstructionTypes.Clear();

                StoreInstruction = new InstructionType(
                id: "storeItems",
                name: $"Store {ResourceItem.ResourceType.displayName}",
                description: "",
                //requiredResources: new List<ResourceItem> { new ResourceItem(ResourceItem.ResourceType, 1) },
                //checkValidity: (Instruction i) => i.ActiveMember.Inventory.ContainsType(ResourceItem.ResourceType.ResourceID),
                onStart: StoreItem
                );

                InstructionTypes.Add(new InstructionType(
                                id: "takeItems",
                                name: $"Take All {ResourceItem.ResourceType.displayName}",
                                description: "",
                                onStart: TakeAll
                                )
                    );
            }
        }

        /// <summary>
        /// Creates the TakeItem <see cref="InstructionType"/>
        /// </summary>
        /// <param name="amount">The amount of resource to be taken</param>
        /// <returns>the take item <see cref="InstructionType"/></returns>
        public InstructionType TakeItemInstruction(int amount)
        {
            return new InstructionType(
                id: "takeItem",
                name: $"Take {amount}{ResourceItem}",
                onComplete: (Instruction i) => TakeItem(i.ActiveMember.Inventory, amount)
                );
        }

        /// <summary>
        /// Instruction callback for the take all of the <see cref="ResourceItem"/> and reset the storage unit, so no resource is set.
        /// </summary>
        /// <param name="instruction"></param>
        private void TakeAll(Instruction instruction)
        {
            Inventory inventory = instruction.ActiveMember.Inventory;
            if (inventory.AddItem(ResourceItem))
            {
                ResetState();
            }

        }

        /// <summary>
        /// Instruction callback for the take item instruction
        /// </summary>
        /// <param name="destination">destination inventoy for the items tobe given</param>
        /// <param name="amount">the amount of the resource item to be given</param>
        private void TakeItem(Inventory destination, int amount)
        {
            if (ResourceItem?.Weight >= amount
                && destination.AddItem(new ResourceItem(ResourceItem.ResourceType, amount)))
            {
                ResourceItem.Weight -= amount;
            }
        }

        /// <summary>
        /// Creates the Set Resource <see cref="InstructionType"/>
        /// </summary>
        /// <returns></returns>
        private List<InstructionType> CreateSetResourceInstructionTypes()
        {
            return Enum.GetValues(typeof(Resource)).Cast<Resource>()
                .Select(r => new InstructionType(
                    id: r.ToString(),
                    name: $"Set {r.ToString()}",
                    description: $"Set {r.ToString()} as the active resource",
                    //requiredResources: new List<ResourceItem> { new ResourceItem(ResourceTypeFactory.GetResource(r)) },
                    checkValidity: (Instruction i) => ResourceItem == null,
                    onComplete: SetResource
                )).ToList();
        }


        /// <summary>
        /// Instruction onComplete callback to set the <see cref="InstructionType"/> of this <see cref="StorageUnit"/>
        /// </summary>
        /// <param name="instruction"></param>
        private void SetResource(Instruction instruction)
        {
            Resource r = (Resource)Enum.Parse(typeof(Resource), instruction.Type.ID);
            ResourceItem = new ResourceItem(r);
            InstructionTypes.Clear();

            StoreInstruction = new InstructionType(
                            id: "storeItems",
                            name: $"Store {ResourceItem.ResourceType.displayName}",
                            description: "",
                            onStart: StoreItem
                            );

            InstructionTypes.Add(new InstructionType(
                            id: "takeItems",
                            name: $"Take All {ResourceItem.ResourceType.displayName}",
                            description: "",
                            onStart: TakeAll
                            )
                );
        }

        /// <summary>
        /// Sets this <see cref="StorageUnit"/>'s state so there is no resource type set.
        /// </summary>
        private void ResetState()
        {
            this.ResourceItem = null;
            InstructionTypes = CreateSetResourceInstructionTypes();
        }

        /// <summary>
        /// Instruction callback to store an item in the storage unit
        /// </summary>
        /// <param name="instruction"></param>
        private static void StoreItem(Instruction instruction)
        {
            StorageUnit storage = (StorageUnit)instruction.PassiveMember;
            IInstructionFollower activeMember = instruction.ActiveMember;

            if (storage.ResourceItem != null)
            {
                Inventory inventory = activeMember.Inventory;
                Resource resourceType = storage.ResourceItem.ResourceType.ResourceID;

                if (inventory.Resources.ContainsKey(resourceType))
                {
                    int reserve = activeMember.InventoryRules.ContainsKey(resourceType) ? activeMember.InventoryRules[resourceType] : 0;
                    ResourceItem item = inventory.Resources[resourceType].Clone();
                    item.Weight -= reserve;
                    {
                        storage.ResourceItem.Weight += item.Weight;
                        inventory.RemoveItem(item);
                    }
                    
                }
            }

        }

        public void Build()
        {
            GameObjectManager.Add(this);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (ResourceItem != null)
            {
                Texture2D resourceTexture = ResourceItem.ResourceType.Texture;
                spriteBatch.Draw(
                    texture: resourceTexture,
                    position: Position,
                    sourceRectangle: null,
                    color: Color.White,
                    rotation: Rotation,
                    origin: CalculateOrigin(new Vector2(resourceTexture.Width, resourceTexture.Height)),
                    scale: 1f,
                    effects: SpriteEffect,
                    layerDepth: CalculateDepth(Position, 0.1f)
                );
            }
        }

        public object Reconstruct()
        {
            return new StorageUnit(PFSPosition, PFSResourceItem);
        }
    }
}