## Step 5: UI Setup

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