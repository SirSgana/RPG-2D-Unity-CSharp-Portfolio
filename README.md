# 2D Action / Metroidvania Prototype — Unity & C# Portfolio

This repository contains C# source code samples from a 2D game prototype currently in development in Unity.

To see the systems in action and test the mechanics, you can play the demo here:  
**[Play Prototype on Itch.io](https://sirsgana.itch.io/prototype-2d)**

> **Note on Repository Scope:** This repository currently showcases how UI scripts and selected systems are structured. Other gameplay systems featured in the playable demo are not publicly included due to privacy/project constraints. If you would like to discuss specific implementations or see additional code examples, feel free to reach out (leaving a comment on the Itch.io page works too!).

---

## Architecture, Tools & Workflow

A core principle of my workflow is **designer-friendly configuration and separation of concerns**. Whenever possible, key gameplay parameters and tuning values are exposed to the Unity Inspector so game designers—or whoever is tuning the experience—can iterate on **game feel** and **game balance** directly in the editor without modifying core logic.

### 1. Player State & Game Feel Configuration
The player setup exposes tuning variables so designers can adjust movement, responsiveness, and combat feel during playtesting.

<img width="389" height="755" alt="Screenshot 2026-11-19 135517" src="https://github.com/user-attachments/assets/42bfbccf-d3a3-410f-a060-bee6aebe1382" />

In the code, initialization and state references are wired up cleanly in `Awake()` to connect the player with its available states:

<img width="1920" height="1024" alt="Screenshot 2026-11-19 135813" src="https://github.com/user-attachments/assets/2fe446aa-985c-4e3f-9b6d-1634db360729" />

Animator transitions and states are organized to match gameplay states clearly:

<img width="1421" height="632" alt="Screenshot 2026-11-19 142156" src="https://github.com/user-attachments/assets/2fefb532-d0f9-46a8-8ac8-57857cd5f15d" />

### 2. Modular Component Structure
Rather than packing everything into a single monolithic script, player responsibilities are split into focused components (such as stats, movement/states, and related systems) for better readability, debugging, and maintainability.

<img width="386" height="572" alt="Screenshot 2026-11-19 140054" src="https://github.com/user-attachments/assets/09ce3684-556f-4434-8af7-70a2143c0057" />

Expanding `Player_Stats` shows how granular parameters are organized so values can be fine-tuned during testing and balancing passes:

<img width="1920" height="1024" alt="Screenshot 2026-11-19 140920" src="https://github.com/user-attachments/assets/db0ddb99-38ed-486d-8eed-9bf8618113ce" />

<img width="1920" height="1024" alt="Screenshot 2026-11-19 141119" src="https://github.com/user-attachments/assets/4aa09401-29dc-4aeb-b129-5a63f8c4f72b" />

### 3. Data-Driven Items, Equipment & Skills
Items, equipment pieces, and skills are configured with their own dedicated data setups, making it straightforward to add content, adjust stats, and iterate on progression and combat mechanics.

<img width="387" height="834" alt="Screenshot 2026-11-19 141441" src="https://github.com/user-attachments/assets/52ce9736-37e9-40fe-9a9c-8fc54ac1f85a" />

<img width="920" height="1024" alt="Screenshot 2026-11-19 141637" src="https://github.com/user-attachments/assets/49631877-37db-451a-9734-0b931927ba38" />
