﻿// Include the namespaces (code libraries) you need below.
using System.Numerics;


// The namespace your code is in.
namespace MohawkGame2D;
/// <summary>
///     Your game code goes inside this class!
/// </summary>
public class Game
{
    float playerX = 50; // Player X 
    float playerY = 350; // Player Y 
    float playerSpeed = 5f; // Player Speed 
    float DeltaTime = 2.5f; // Delta Time 
    int currentFrame = 0; // Current Frame 
    float frameTime = 5f; // Time per Frame 
    float frameTimer = 0f; // Timer for Frame
    int windowWidth = 500; // Window Width 
    int windowHeight = 500; // Window Height 

    // Gravity
    float gravity = 1.5f; // Gravity 
    float jumpStrength = 16f; // Jump Strength 
    float yVelocity = 1f; // Y Velocity 
    bool isJumping = false; // Is Jumping 
    bool isGrounded = false; // Is Grounded
    bool isMoving = false; // Is Moving
    

    //collision
    float playerWidth = 32; // Player Width
    float playerHeight = 80; // Player Height
    float platformWidth = 32; // Platform Width
    float platformHeight = 25; // Platform Height

    // Define platforms array 
    Platform[] platforms;

    // Setup method 
    public void Setup()
    {
        Window.SetTitle("Blair's 2D Game"); // Set Title 
        Window.SetSize(500, 500); // Set Window Size 
        Window.TargetFPS = 60; // Set Target FPS 
        // Music 
        Music music = Audio.LoadMusic("../../../Assets/2D Game music fin.WAV"); // Load Music 
        // Play Music 
        Audio.Play(music); // Play music 


        //  Initialize platforms here and load texture
        Texture2D platformTexture = Graphics.LoadTexture("../../../Assets/platform-long.png"); // Load the platform texture 


        platforms = new Platform[] // Initialize platforms array 
        {
                new Platform(250, 400,  platformTexture), // Platform 1
                new Platform(125, 340, platformTexture), // Platform 2
                new Platform(55, 300, platformTexture), // Platform 3
                new Platform(245, 275, platformTexture), // Platform 4
                new Platform(350, 235, platformTexture), // Platform 5
                new Platform(455, 275, platformTexture), // Platform 6
                new Platform(15, 200, platformTexture), // Platform 7
                new Platform(200, 150, platformTexture), // Platform 8
                new Platform(250, 50, platformTexture), // Platform 9
                new Platform(450,125, platformTexture), // Platform 10 
                new Platform(350, 50, platformTexture), // Platform 11
               
                //
               


        };

        
    }

    

   
    public void Update()
    {
        // Background Image 
        Texture2D bg = Graphics.LoadTexture("../../../Assets/MountainBG.png");
        Graphics.Scale = 1.3f; // Scale Background Image 
        Graphics.Draw(bg, 0, 0);

        //Load player textures only once, outside the Update loop
        Texture2D image1 = Graphics.LoadTexture("../../../Assets/bear-walk1.png"); // Load Player Image 1 
        Texture2D image2 = Graphics.LoadTexture("../../../Assets/bear-walk2.png"); // Load Player Image 2 
        Texture2D image3 = Graphics.LoadTexture("../../../Assets/bear-walk3.png"); // Load Player Image 3 
        Texture2D image4 = Graphics.LoadTexture("../../../Assets/bear-walk4.png"); // Load Player Image 4 

       
        bool isMoving = false; // Track if player is moving 

        // Player Movement 
        if (Input.IsKeyboardKeyDown(KeyboardInput.Left)) // If Left Arrow is Pressed 
        {
            playerX -= playerSpeed; // Move Player Left 
            
            isMoving = true; // Player is moving 
            
        }
        else if (Input.IsKeyboardKeyDown(KeyboardInput.Right)) // If Right Arrow is Pressed 
        {
            playerX += playerSpeed; // Move Player Right 
            isMoving = true; // Player is moving 

        }

        // Jumping and Gravity 
        if (isGrounded && Input.IsKeyboardKeyDown(KeyboardInput.Space)) // If Space is Pressed and Grounded 
        {
            yVelocity = -jumpStrength; // Jump Strength applied to Y Velocity
            isJumping = true; // Set isJumping to True 
            isGrounded = false; // Player is no longer grounded 
        }

        if (!isGrounded) // If Player is not Grounded 
        {
            playerY += yVelocity; // Apply Y Velocity 
            yVelocity += gravity; // Apply Gravity to Y Velocity 


        }
        foreach( var platform in platforms)
        {
            if (playerY + playerHeight <= platform.Y && playerY + playerHeight + yVelocity >= platform.Y &&
            playerX + playerWidth > platform.X && playerX < platform.X + platformWidth)
            {
                // Player has landed on the platform, stop vertical velocity (gravity)
                playerY = platform.Y - playerHeight; // Place the player just on top of the platform
                yVelocity = 0; // Stop the downward velocity
                isGrounded = true; // Player is grounded now
                break; // Exit the loop after collision
            }


        }

        // Handle Ground Collision 
        if (playerY >= 350) // If Player Y is greater than or equal to 350 
        {
            playerY = 350; // Reset Player Y to ground level 
            isJumping = false; // Set isJumping to False 
            isGrounded = true; // Set isGrounded to True 
            yVelocity = 0; // Reset Y Velocity 
        }
        
        

        // Frame Animation Logic 
        frameTimer += DeltaTime; // Add DeltaTime to FrameTimer 
        if (frameTimer >= frameTime) // If FrameTimer exceeds FrameTime 
        {
            frameTimer = 0; // Reset FrameTimer 
            currentFrame = (currentFrame + 1) % 4; // Cycle through the frames 
        }


        // Player Wrapping 
        if (playerX > Window.Width) // If Player X exceeds Window Width 
        {
            playerX = 0; // Reset Player X to left side of screen 
        }
        else if (playerX < 0) // If Player X is less than 0 
        {
            playerX = Window.Width; // Reset Player X to right side of screen 
        }

        // Set current player texture based on frame 
        Texture2D currentPlayerTexture = isMoving switch
        {
            true => currentFrame switch
            {
                0 => image1, // Frame 1 
                1 => image2, // Frame 2 
                2 => image3, // Frame 3 
                3 => image4, // Frame 4 
                _ => image1 // Default 
            },
            false => image2 // standing still

        };

        // Draw Player 
        Graphics.Draw(currentPlayerTexture, playerX, playerY);

        // Draw all platforms here by looping through the array
        foreach (var platform in platforms) // Iterate over each platform 
        {
            Graphics.Draw(platform.Texture, platform.X, platform.Y); // Draw platform 
        }

        

        // Clear Background 
        Window.ClearBackground(Color.Clear); // Clear Background 
    }
}
