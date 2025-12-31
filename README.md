# Multiplayer Console Bomberman

A real-time multiplayer Bomberman game developed in C# using a custom Client-Server architecture. The project runs entirely in the console, utilizing ANSI escape codes for rendering and a hybrid TCP/UDP networking model for low-latency gameplay.

## Architecture

The solution is divided into two main projects: **Game (Client)** and **Server (Backend)**.

### 1. Server (`src/Server`)
The server acts as the authoritative manager for user accounts and matchmaking, while acting as a relay for real-time game state.
*   **Hybrid Networking:**
    *   **TCP:** Used for reliable, transactional operations like Login, Registration, Leaderboard, and Matchmaking.
    *   **UDP:** Used for high-frequency gameplay events like Player Movement and Bomb Placement where speed is critical.
*   **Request Pipeline:** Incoming requests are processed through a chain of handlers, allowing for modular message processing.
*   **Database:** Uses PostgreSQL with Dapper for persisting player data (wins, games played, themes).

### 2. Client (`src/Game`)
The client handles user input, rendering, and local game logic.
*   **Game Loop:** A central loop manages input processing, network synchronization, and frame rendering.
*   **Rendering Engine:** A custom console-based rendering pipeline that draws the map in layers (Background -> Items -> Entities) to handle overlapping objects correctly.

## Design Patterns

The project extensively uses Object-Oriented Design Patterns to ensure modularity and maintainability:

### Client-Side Patterns
*   **Singleton:** Used for `GameManager`, `NetworkManager`, and `MenuManager` to ensure a single shared instance manages global state (Game Loop, Network Connection, UI Navigation).
*   **Observer:** Implemented in the Bomb mechanics.
    *   **Subject:** `Bomb`
    *   **Observers:** `Player`, `Monster`, `Wall`
    *   *Usage:* When a bomb explodes, it notifies all subscribed observers in its range to `takeDamage()`, decoupling the explosion logic from the entities it affects.
*   **Abstract Factory:** Used for Map Themes.
    *   **Factory:** `ThemeFactory` (Concrete: `CityFactory`, `DesertFactory`, `ForestFactory`)
    *   *Usage:* Creates families of related objects (Walls, Ground) ensuring the map has a consistent visual theme without coupling the game logic to specific assets.
*   **Builder:** Used in `MapRenderer`.
    *   *Usage:* `MapRendererBuilder` constructs the complex rendering pipeline step-by-step, allowing different configurations of map layers.
*   **Chain of Responsibility (Rendering):**
    *   *Usage:* The `RendererHandler` chain (`RenderBomb` -> `RenderEnemy` -> `RenderPlayer`...) determines the drawing order, ensuring dynamic entities appear on top of the background.
*   **State:** Used for Network Connection handling.
    *   *Usage:* The `ConnectionContext` switches between `Connected` and `NotConnected` states to manage the initial server connection loop.

### Server-Side Patterns
*   **Facade:**
    *   *Usage:* The `Server` class provides a simplified interface to start and manage the complex `TcpHandler` and `UdpHandler` subsystems.
*   **Chain of Responsibility (Network):**
    *   *Usage:* `RequestHandler` classes (`LoginHandle`, `MatchHandle`, `MoveHandle`) form a pipeline. Each handler checks if it can process the incoming message type; if not, it passes it to the next handler.
*   **Repository:**
    *   *Usage:* `PlayerRepository` abstracts the Data Access Layer (DAL), separating raw SQL queries (Dapper/PostgreSQL) from the business logic.

## Requirements

*   **.NET SDK:** Version 6.0, 7.0, or 8.0.
*   **PostgreSQL:** A running instance of PostgreSQL.
*   **Terminal:** A console that supports ANSI escape codes (e.g., Windows Terminal, VS Code Integrated Terminal, PowerShell 7+).

## Setup & Installation

### 1. Database Configuration
1.  Install and start PostgreSQL.
2.  Create a database named `bomberman`.
3.  Run the following SQL script to create the required table:

```sql
CREATE TABLE tbl_players (
    fld_id SERIAL PRIMARY KEY,
    fld_username VARCHAR(50) NOT NULL UNIQUE,
    fld_password VARCHAR(50) NOT NULL,
    fld_theme INT DEFAULT 1,
    fld_win INT DEFAULT 0,
    fld_games INT DEFAULT 0
);
```

4.  **Important:** The server is currently configured to connect with the following credentials. If your setup differs, update `src/Backend/Network/Requests/Abstractions/RequestHandler.cs`:
    *   **Host:** localhost
    *   **Port:** 5432
    *   **User:** postgres
    *   **Password:** 1234

### 2. Running the Server
1.  Open a terminal in the `Server` directory.
2.  Run the project:
    ```bash
    dotnet run --project src/Server.csproj
    ```
    *Or use the provided `server.bat` file.*
3.  The server will start listening on Port **15000**.

### 3. Running the Client
1.  Open a terminal in the `Game` directory.
2.  Run the project:
    ```bash
    dotnet run --project src/Game.csproj
    ```
    *Or use the provided `Bomberman.bat` file.*
3.  **Multiplayer:** To play with yourself or others locally, open a second terminal and run the Client project again.

## How to Play

1.  **Login/Register:** Create an account or log in.
2.  **Main Menu:**
    *   **Start Game:** Enters the matchmaking queue. When another player joins, the game starts.
    *   **Choose Theme:** Select the visual style of the map (Forest, Desert, City).
    *   **Leaderboard:** View top players.
3.  **Controls:**

| Key | Action |
| :--- | :--- |
| **W** | Move Up |
| **A** | Move Left |
| **S** | Move Down |
| **D** | Move Right |
| **Space** | Place Bomb |

## Project Structure

```text
Bomberman/
├── Game/ (Client Application)
│   ├── src/Game/
│   │   ├── Bombs/          # Observer Pattern Implementation
│   │   ├── Enemy/          # Enemy Logic (Strategy/State)
│   │   ├── Map/            # Abstract Factory & Rendering Chain
│   │   ├── Menu/           # Menu System
│   │   ├── Players/        # Player Logic
│   │   ├── GameManager.cs  # Main Game Loop (Singleton)
│   │   └── NetworkManager.cs
│
├── Server/ (Backend Application)
│   ├── src/Backend/
│   │   ├── Database/       # Repository Pattern
│   │   ├── Entities/       # Data Models
│   │   ├── Network/        # TCP/UDP Sockets & Facade
│   │   │   └── Requests/   # Chain of Responsibility Pipeline
```

## Troubleshooting

*   **"Connection Refused":** Ensure the Server is running before starting the Client.
*   **"Player Not Found":** Ensure you have Registered before trying to Login.
*   **Database Errors:** Check `RequestHandler.cs` to ensure the connection string matches your PostgreSQL credentials.
*   **Weird Characters in Console:** Your terminal does not support ANSI colors. Try using **Windows Terminal** or enabling "Virtual Terminal Processing" in CMD.

## License

This project is created for educational purposes to demonstrate Software Architecture and Design Patterns.