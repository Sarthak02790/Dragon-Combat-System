# Dragon-Combat-System (DCS)

## Overview
A technical prototype featuring a CharacterController-based dragon with three distinct combat abilities, custom animations, and scene management.

## How to Play
- **WASD / Arrows:** Move Dragon
- **Fire Button:** Fire Breath attack
- **Tail Button:** Tail Slam (Melee)
- **Fly Button:** Aerial Fire Attack
- **Quit:** Returns to Main Menu

## AI Usage Note
AI Usage Note
For this technical assessment, I integrated AI tools into my development workflow to ensure the project was completed with high code quality and optimal game feel within the given timeframe.
Tools Used
•	Google Gemini / ChatGPT: Used as a pair-programming partner for architectural logic, coroutine management, and debugging.
•	Unity Documentation & Manual: Used for cross-referencing API changes (specifically for Character Controller and Particle System modules).
How AI Enhanced My Workflow
The use of AI was instrumental in three key areas of this project:
1.	Iterative Logic Refinement: While developing the Dragon's "Fly Attack," I encountered a common issue where the fire particle spawn was misaligned due to the head bone's animation rotation. I used AI to brainstorm architectural solutions, eventually deciding to implement a secondary "Static Spawn Point" tied to the root transform. This allowed the fire to remain consistent and "level" without battling the animators keyframes.
2.	Sound & UI Synchronization: AI helped me quickly structure a decoupled Sound Manager and Game Manager using the Singleton pattern. This ensured that game-state events (like the "FIGHT!" text appearing) were perfectly synced with audio triggers via Coroutines, reducing the manual trial-and-error time significantly.
3.	Code Optimization & Debugging: I used AI to review my Update loops to ensure I wasn't performing expensive operations every frame. It assisted in refining the Check Damage logic to be more performant by using distance-based checks rather than constant trigger collisions, which is more efficient for a 1v1 battle scenario.
Conclusion
By leveraging AI, I was able to focus more on the "Game Feel" the weight of the dragon's movement, the timing of the attacks, and the UI feedback while ensuring the underlying C# architecture followed solid programming principles. I view AI as a powerful collaborative tool that allows a Junior Developer to produce professional-grade, bug-free results efficiently.



## Technical Details
- **Unity Version:** 6000.0.63f1
- **Language:** C#
- **Key Systems:** Coroutine-based attack states, Singleton SoundManager, and BuildIndex Scene Management.
