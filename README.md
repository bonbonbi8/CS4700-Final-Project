# CS4700 Final Project - 2D Game Systems Implementation

This Unity project implements three core game systems: **Crafting**, **Stats & Resources**, and **Tools/Equipment/Combat**.

## Features

### System 1: Crafting System
- **ScriptableObjects**: Items and recipes are defined as ScriptableObjects for easy design and iteration
- **Crafting Recipes**: 5+ unique recipes with material requirements and outputs
- **Crafting UI**: Full crafting menu with recipe display, material checking, and visual feedback
- **Inventory Integration**: Seamlessly integrates with the inventory system

### System 2: Stats & Resources
- **Health System**: Player health with regeneration and damage mechanics
- **Hunger System**: Hunger decreases over time using coroutines
- **Hydration System**: Hydration decreases over time using coroutines
- **Real-time UI**: Stats displayed on UI with sliders and text updates
- **Item Consumption**: Consumable items restore stats when used

### System 3: Tools, Equipment & Combat
- **Equipment System**: Equip and switch between different weapons/tools
- **Combat System**: Attack mechanics with hit detection and damage
- **Enemy System**: Enemies with health, AI, and death mechanics
- **Input Handling**: Mouse click to attack, proper input system integration

## Setup Instructions

### 1. Player Setup
1. Select the Player GameObject in your scene
2. Ensure the Player has the **"Player"** tag
3. Add the following components (they should auto-add via RequireComponent):
   - `Inventory`
   - `PlayerStats`
   - `ItemConsumer`
   - `EquipmentSystem`
   - `PlayerCombat`
4. Configure `PlayerStats`:
   - Set max health, hunger, and hydration values
   - Adjust decay rates and regeneration settings

### 2. Enemy Setup
1. Create an Enemy GameObject (or prefab)
2. Add the following components:
   - `EnemyHealth` (set max health)
   - `SimpleEnemy` (for AI behavior)
   - `Collider2D` (for hit detection)
   - `Rigidbody2D` (optional, for physics)
3. Set the GameObject's layer to **"Enemy"** (create the layer if it doesn't exist)
4. Tag the GameObject appropriately

### 3. UI Setup

#### Stats UI
1. Create a Canvas if you don't have one
2. Create a GameObject for Stats UI and add the `StatsUI` component
3. Create UI elements:
   - 3 Sliders for Health, Hunger, Hydration
   - 3 Text (TMP) elements for displaying values
4. Assign these to the `StatsUI` component's fields

#### Inventory UI
1. Create a GameObject for Inventory UI and add the `InventoryUI` component
2. Create:
   - A Panel (inventoryPanel) that can be toggled
   - A parent GameObject for inventory slots (inventorySlotParent)
   - A prefab for inventory slots (inventorySlotPrefab) with:
     - Image component for item icon
     - Text (TMP) component for quantity
     - Button component for clicking

#### Crafting UI
1. Create a GameObject for Crafting UI and add the `CraftingUI` component
2. Create:
   - A Panel (craftingPanel) that can be toggled
   - A parent GameObject for recipe buttons (recipeListParent)
   - A prefab for recipe buttons (recipeButtonPrefab) with Button and Text (TMP)
   - A Button for crafting (craftButton)
   - Text (TMP) elements for recipe description and materials
3. Assign your crafting recipes to the `availableRecipes` list

### 4. Creating Items and Recipes

#### Creating Items
1. Right-click in Project window → Create → Game → Item Data
2. Configure:
   - Item name and description
   - Icon sprite
   - Item type (Material, Consumable, Tool, Weapon)
   - Stats (health/hunger/hydration restore for consumables, damage for weapons)

#### Creating Recipes
1. Right-click in Project window → Create → Game → Crafting Recipe
2. Configure:
   - Recipe name and description
   - Required materials (drag ItemData assets, set quantities)
   - Output item and quantity

### 5. Input System
The input actions file has been updated with:
- **Move**: WASD
- **Look**: Mouse position
- **Attack**: Left Mouse Button
- **Crafting Menu**: Press 'C' key
- **Inventory**: Press 'I' key

## Controls

- **WASD**: Move player
- **Mouse**: Look/rotate player
- **Left Click**: Attack (when weapon equipped)
- **C**: Open/Close crafting menu
- **I**: Open/Close inventory
- **Click consumables in inventory**: Consume item

## Example Recipe Setup

Here are 5 example recipes you can create:

1. **Wooden Axe**
   - Materials: 3x Wood, 2x Stone
   - Output: 1x Axe (Weapon, damage: 15)

2. **Food Ration**
   - Materials: 2x Raw Meat, 1x Salt
   - Output: 1x Cooked Meat (Consumable, hunger: +30)

3. **Health Potion**
   - Materials: 2x Herbs, 1x Water
   - Output: 1x Health Potion (Consumable, health: +25)

4. **Water Bottle**
   - Materials: 1x Empty Bottle, 1x Water Source
   - Output: 1x Water Bottle (Consumable, hydration: +40)

5. **Steel Sword**
   - Materials: 3x Iron, 2x Coal, 1x Wood
   - Output: 1x Steel Sword (Weapon, damage: 25)

## Testing Tips

1. **Test Crafting**: Add some materials to player inventory via code or inspector, then try crafting
2. **Test Stats**: Watch stats decrease over time, consume items to restore them
3. **Test Combat**: Equip a weapon, spawn enemies, and attack them
4. **Test Equipment**: Equip different weapons and verify visual changes and damage values

## Troubleshooting

- **Enemies not taking damage**: Ensure enemies are on the "Enemy" layer and the layer is added to the PlayerCombat's enemyLayer mask
- **UI not updating**: Check that event subscriptions are set up correctly in Start() methods
- **Crafting not working**: Verify recipes are assigned to CraftingUI and materials are in inventory
- **Stats not decaying**: Check that coroutines are starting in PlayerStats.Start()

## Code Structure

- **ItemData.cs**: ScriptableObject for item definitions
- **CraftingRecipe.cs**: ScriptableObject for recipe definitions
- **Inventory.cs**: Manages player inventory with events
- **PlayerStats.cs**: Manages health, hunger, hydration with coroutines
- **ItemConsumer.cs**: Handles consuming items
- **EquipmentSystem.cs**: Manages weapon/tool equipping
- **PlayerCombat.cs**: Handles attack mechanics
- **EnemyHealth.cs**: Enemy health and death system
- **SimpleEnemy.cs**: Basic enemy AI
- **CraftingUI.cs**: Crafting menu UI controller
- **StatsUI.cs**: Stats display UI controller
- **InventoryUI.cs**: Inventory display UI controller

## Assignment Checklist

All required features are implemented:
- ✅ ScriptableObjects for items and recipes
- ✅ 5+ crafting recipes
- ✅ Crafting UI with all features
- ✅ Health, Hunger, Hydration systems
- ✅ Stats decay via coroutines
- ✅ Item consumption system
- ✅ Equipment/weapon system
- ✅ Combat with hit detection
- ✅ Enemy health system
- ✅ Input handling
- ✅ UI integration

## Video Demonstration

https://youtu.be/_LE_IHNQ1Vo