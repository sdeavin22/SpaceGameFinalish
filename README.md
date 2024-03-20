# SpaceShooter
A multiplayer game for a Advanced 3D Game Design course using 

Steps to test game:
1. Build and Run it first in the Network Bootstrap scene. The client/user window will appear but do nothing else at this point
2. Network Bootstrap Scene in Scenes on the host side (so not in the build window, but in the unity app itself), and then press play
3. When the main menu appers in unity (again, not in the build window at this point), click on the host button and the game will begin on host side
4. In order to join the game as a client and utilize multiplayer functionality, check the console for the very last debug.log message. It will have a join code used for cients/users to the join game
5. Navigate to the build window (as a client), and enter that code where prompted to in main menu and click on client 
6. Now both screens will be able to interact with one another, demonstrating the use of multiplayer functionality
