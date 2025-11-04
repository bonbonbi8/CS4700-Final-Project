# Quick Setup Guide

This guide will help you set up all three game systems in Unity.

## Step 1: Tag and Layer Setup

1. **Create "Player" Tag** (if not exists):
   - Edit → Project Settings → Tags and Layers
   - Add "Player" tag

2. **Create "Enemy" Layer** (if not exists):
   - Edit → Project Settings → Tags and Layers
   - Add "Enemy" layer (use an empty layer slot, e.g., Layer 8)

## Step 2: Player Setup

1. Select your Player GameObject
2. Add/verify these components (they should auto-add):
   - `PlayerController2D`
   - `Inventory`
   - `PlayerStats`
   - `ItemConsumer`
   - `EquipmentSystem`
   - `PlayerCombat`
3. Set Player tag to "Player"
4. Configure `PlayerStats`:
   - Max Health: 100
   - Max Hunger: 100
   - Max Hydration: 100
   - Hunger Decay Rate: 0.5
   - Hydration Decay Rate: 0.8

## Step 3: Create Items (ScriptableObjects)

Create these items in your Project:

1. **Wood** (Material)
   - Item Type: Material
   - Max Stack: 50

2. **Stone** (Material)
   - Item Type: Material
   - Max Stack: 50

3. **Iron** (Material)
   - Item Type: Material
   - Max Stack: 50

4. **Axe** (Weapon)
   - Item Type: Weapon
   - Damage: 15
   - Attack Range: 1.5
   - Attack Cooldown: 0.8

5. **Sword** (Weapon)
   - Item Type: Weapon
   - Damage: 25
   - Attack Range: 1.5
   - Attack Cooldown: 0.6

6. **Apple** (Consumable)
   - Item Type: Consumable
   - Hunger Restore: 20
   - Health Restore: 5

7. **Water Bottle** (Consumable)
   - Item Type: Consumable
   - Hydration Restore: 30

8. **Health Potion** (Consumable)
   - Item Type: Consumable
   - Health Restore: 25

## Step 4: Create Recipes (ScriptableObjects)

Create these recipes:

1. **Wooden Axe Recipe**
   - Required: 3x Wood, 2x Stone
   - Output: 1x Axe

2. **Iron Sword Recipe**
   - Required: 3x Iron, 2x Stone
   - Output: 1x Sword

3. **Health Potion Recipe**
   - Required: 2x Wood, 1x Water Bottle
   - Output: 1x Health Potion

4. **Food Recipe** (example)
   - Required: 2x Wood, 1x Apple
   - Output: 1x Apple (or create a new food item)

5. **Water Recipe** (example)
   - Required: 1x Wood
   - Output: 1x Water Bottle

## Step 5: UI Setup

### Stats UI

1. Create Canvas (if not exists)
2. Create empty GameObject "StatsUI", add `StatsUI` component
3. Create UI elements:
   - Health Slider (set to fill from left, min 0, max 1)
   - Health Text (TMP)
   - Hunger Slider
   - Hunger Text (TMP)
   - Hydration Slider
   - Hydration Text (TMP)
4. Assign all to `StatsUI` component fields

### Inventory UI

1. Create empty GameObject "InventoryUI", add `InventoryUI` component
2. Create Panel "InventoryPanel" (initially disabled)
3. Create empty GameObject "InventorySlotParent" inside panel
4. Create InventorySlot prefab:
   - Image (for icon)
   - Text (TMP, for quantity)
   - Button component
5. Assign to `InventoryUI` component

### Crafting UI

1. Create empty GameObject "CraftingUI", add `CraftingUI` component
2. Create Panel "CraftingPanel" (initially disabled)
3. Create empty GameObject "RecipeListParent" inside panel
4. Create RecipeButton prefab:
   - Button
   - Text (TMP, child of button)
5. Create:
   - Craft Button
   - Recipe Description Text (TMP)
   - Materials Text (TMP)
6. Assign all to `CraftingUI` component
7. **Drag your 5 recipes into the `availableRecipes` list**

## Step 6: Enemy Setup

1. Create Enemy GameObject (or prefab)
2. Add:
   - `EnemyHealth` component (set Max Health: 50)
   - `SimpleEnemy` component
   - `SpriteRenderer` (for visual)
   - `Collider2D` (Circle or Box)
   - `Rigidbody2D` (optional, set to Kinematic if you don't want physics)
3. Set GameObject layer to "Enemy"
4. Position enemy in scene

## Step 7: Testing Setup

1. Create empty GameObject "ItemSpawner", add `ItemSpawner` component
2. Drag your test items into the `testItems` array
3. Set quantities (e.g., 5, 3, 2...)
4. Enable "Auto-spawn on Start" or use "Spawn All Items" context menu

## Step 8: Testing Checklist

- [ ] Player moves with WASD
- [ ] Player rotates to face mouse
- [ ] Stats UI shows health, hunger, hydration
- [ ] Stats decrease over time
- [ ] Press 'I' to open inventory
- [ ] Press 'C' to open crafting menu
- [ ] Spawn items using ItemSpawner
- [ ] Craft items (check materials are consumed, output added)
- [ ] Consume items from inventory (click them)
- [ ] Equip weapon (can be done via code or UI button)
- [ ] Left click to attack
- [ ] Enemies take damage and die
- [ ] Enemy attacks player and reduces health

## Troubleshooting

**"Player not found" errors:**
- Make sure Player GameObject has "Player" tag

**Enemies not taking damage:**
- Check enemy layer is set to "Enemy"
- Check PlayerCombat's enemyLayer includes Enemy layer
- Ensure enemies have Collider2D

**UI not updating:**
- Check that UI components are properly assigned
- Verify PlayerStats/Inventory components are on Player
- Check console for errors

**Crafting not working:**
- Ensure recipes are assigned to CraftingUI
- Check materials are in inventory
- Verify recipe CanCraft() logic

**Stats not decaying:**
- Check PlayerStats coroutines are starting
- Verify coroutine intervals are set correctly
- Check console for errors stopping coroutines

