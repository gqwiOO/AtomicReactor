# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**AtomicReactor** is a tile-based building/management game (Unity 6, URP) similar in style to Factorio. Players place buildings on a grid map, manage electricity/fluid/item resource chains, and automate production via crafting recipes.

**Unity Version:** 6000.4.3f1  
**Main Scene:** `Assets/_Project/Scenes/Gameplay.unity`

## Build & Development

This is a Unity project — build and run via the Unity Editor (no CLI build scripts exist). Open `D:\Projects\AtomicReactor` as a Unity project with Unity 6000.4.3f1.

There are no automated test suites configured. Testing is done by running the game in Play Mode.

## Code Architecture

### Directory Layout

| Path | Purpose |
|---|---|
| `Assets/_Project/_Scripts/` | All game-specific code |
| `Assets/Core/Scripts/` | Reusable infrastructure (UpdateService, StateMachine, Pools, etc.) |
| `Assets/Core/Scripts/Raccoons.Core/` | Custom utility library (files, math, storage, networking, scores) |

### Assembly Definitions

- **GameAssembly** — main game logic, depends on Core modules
- **CoreDefinition** — shared infrastructure
- **Raccoons.Core.\*** — independent utility modules (math, serialization, storage, etc.)

### Key Frameworks & Libraries

- **Zenject** — dependency injection (MonoInstaller pattern throughout)
- **UniTask** — all async/await; never use `Task` or coroutines for async work
- **Sirenix OdinInspector** — inspector tooling; game MonoBehaviours extend `SerializedMonoBehaviour`
- **DOTween** — animations/tweens
- **Flexalon** — UI layout
- **New Input System** — all input handling

### Core Systems

**UpdateService** (`Assets/Core/Scripts/Services/UpdateService/`)  
Central tick dispatcher. All tickable objects implement `IUpdatable` and register with `UpdateService` — never use `MonoBehaviour.Update()` directly.

**Dependency Injection**  
Zenject installers are composed via `MonoAdapter`. Each major system has its own `MonoInstaller`. Bindings are per-scene; look for `*Installer.cs` files near the system they install.

**Building System** (`Assets/_Project/_Scripts/Gameplay/Map/Building/`)  
Buildings are `BuildingMapObject` subclasses with an `IBuildingCore` behavior component. Configuration lives in `BuildingSettingsDataAsset` ScriptableObjects. Key building types: crafting, electricity generation/consumption, fluid pumps, wire poles, chests.

**Map & Grid** (`Assets/_Project/_Scripts/Gameplay/Map/`)  
Square grid using `Vector2Int` positions. `MapCellsService` handles BFS pathfinding and neighbor queries (4-directional). Cells expose an `ICellVisitor` interface so buildings, units, and nature objects can occupy the same cell system.

**Electricity** — `IElectricityProvider` / `IElectricityContainer` interfaces; buildings declare consumption, generators produce supply; wire poles connect networks.

**Inventory** — Generic `Inventory<T>` with `InventoryCell` slots. `OnChanged` event drives UI. `SingleCellInventory` variant for simple containers.

**Crafting** — Recipe-driven via `ItemDataAsset` ScriptableObjects. `CraftService` resolves recipes by output item.

**Unit AI** (`Assets/_Project/_Scripts/Gameplay/Units/`)  
Async state machine (`BaseStateMachine` in Core) with UniTask-based state transitions. States are serialized in the inspector.

**Map Generation** — Uses `FastNoise` (Perlin/Voronoi) and `SharpVoronoiLib` for biome/terrain layout. Entry point is `MapCreator`.

**Timer System** (`Assets/Core/Scripts/Mechanics/Timer/`)  
Use `TimerService` / `TimerObject` rather than `Invoke` or coroutine delays.

### Patterns to Follow

- **New systems need a Zenject installer** — bind services in a `MonoInstaller`, inject via constructor or `[Inject]`.
- **Async code uses UniTask** — `async UniTask`, `UniTaskCompletionSource`, `UniTask.Delay`.
- **ScriptableObjects for data** — settings, item definitions, recipes, and building configs are all SOs.
- **Visitor pattern on cells** — to add a new cell occupant type, implement `ICellVisitor`.
- **Events over polling** — expose `event Action OnChanged` / `OnUpdated` rather than polling state each frame.
