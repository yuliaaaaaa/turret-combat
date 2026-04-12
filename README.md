<img width="1327" height="1080" alt="image" src="https://github.com/user-attachments/assets/4b749b4d-d68a-4094-8b74-89e35c681bb9" />

# Unity Runner Shooter
A small Unity project where the player controls a car that moves forward and automatically shoots enemies.  
The main goal is to complete the level, destroy enemies, and reach the finish line.

## Core Architecture
The project is structured with a clear separation of responsibilities:

- game state management
- level flow (start, lose, win)
- UI
- gameplay (car, enemies, shooting)

Dependency injection is handled using Zenject.

## Main Classes

### GameStateService
Stores the current game state (Playing, Paused, Win, Lose) and notifies other systems about changes.

### LevelFlowCoordinator
Handles the overall level logic:

- game start
- player (car) death handling
- reaching the finish line
- switching to win/lose states

### SceneReloadService
Reloads the scene and allows restarting the level immediately.

### CarController
Controls forward movement and horizontal movement (X axis), and handles collision reactions.

### Enemy (and its types)
Base enemy logic (movement, attack, death).  
Specific types (Runner, Ambush) define unique behaviors.

### EnemySpawner
Spawns enemies for each road chunk.

### RoadGenerator
Generates the road using chunks and determines the finish point.

### AutoShooter
Automatically fires projectiles when enemies are nearby.

### Projectile
Moves forward, deals damage to enemies, and plays hit effects.

## UI
The UI is divided into several views:

- HudView — main in-game interface
- PauseMenuView — pause menu
- GameOverlayView — start / win / lose screens
- SettingsPanelView — settings menu

GameUiCoordinator connects UI with the game state.

## Additional Systems
- Health — reusable health system
- EnemyEncounterService — tracks active enemies
- LevelProgressService — calculates level completion progress

---
## Summary
The project is built with a focus on clean architecture and separation of concerns, making the code easy to maintain and extend.
Author: Zahorodnia Yulia

-----------------------------------------------------------------------------------------------------------------------------

# Unity Runner Shooter (Test Project)
Невеликий проєкт на Unity, де гравець керує машиною, яка рухається вперед і автоматично стріляє по ворогах. Основна ідея - проїхати рівень, знищуючи ворогів і дійти до фінішу.

## Основна логіка
Архітектура побудована так, щоб розділити відповідальності:

- окремо стан гри
- окремо flow рівня (старт, програш, перемога)
- окремо UI
- окремо gameplay (машина, вороги, стрільба)

Використовується Zenject для залежностей.

## Основні класи

### GameStateService
Зберігає стан гри (Playing, Paused, Win, Lose) і повідомляє інші системи про зміни.

### LevelFlowCoordinator
Керує логікою рівня:

- старт гри
- обробка смерті машини
- досягнення фінішу
- перехід у win/lose

### SceneReloadService
Перезавантажує сцену і дозволяє стартувати рівень одразу після рестарту.

### CarController
Рухає машину вперед і по осі X, а також обробляє реакцію на удари.

### Enemy (і його типи)
Базова логіка ворогів (рух, атака, смерть).
Конкретні типи (Runner, Ambush) визначають поведінку.

### EnemySpawner
Спавнить ворогів для кожного чанку дороги.

### RoadGenerator
Генерує дорогу з чанків і визначає точку фінішу.

### AutoShooter
Автоматично створює снаряди, коли є вороги поруч.

### Projectile
Рухається вперед, наносить шкоду ворогам і відтворює ефекти влучання.

## UI
UI розділений на кілька view:

- HudView — основний інтерфейс під час гри
- PauseMenuView — меню паузи
- GameOverlayView — старт / перемога / програш
- SettingsPanelView — налаштування

GameUiCoordinator зв’язує UI зі станом гри.

## Додатково
- Health — універсальна система здоров’я
- EnemyEncounterService — відслідковує активних ворогів
- LevelProgressService — рахує прогрес проходження рівня

---
Проєкт зроблений з фокусом на чисту архітектуру і розділення відповідальностей, щоб код було легко розширювати і підтримувати.
Автор: Загородня Юлія

