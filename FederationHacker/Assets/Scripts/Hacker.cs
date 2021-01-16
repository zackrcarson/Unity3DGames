using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hacker : MonoBehaviour
{
    // Enums
    enum Screen { Name, MainMenu, Password, WinScreen, FailScreen, Exited };

    // Config Params
    [Header("Name Input String Configurations")]
    [SerializeField] string nameAsk = "Greetings, hacker. What is your name?";
    [SerializeField] string hackerLocation = "@hacker:~$ ";
    [SerializeField] string initialHackerName = "anon";
    string hackerName = "anon";

    [Header("Main Menu String Configurations")]
    [SerializeField] string mainMenuInitialMessage = "Hello hacker. What would you like to hack into today?";
    [SerializeField] string mainMenuFinalMessage = "Enter your selection, or type menu to start over: ";

    [SerializeField] string invalidInputMessage = "Please pick a valid target!";

    [Header("Password Guessing Screen String Configurations")]
    [SerializeField] int initialAttemptsRemaining = 5;
    [SerializeField] int bonusAttemptFactorPerDifficulty = 1;
    int attemptsRemaining = 5;

    [SerializeField] string hackingIntoMessage = "Hacking into the ";
    [SerializeField] string passwordInitialMessage = "Please provide the password: ";
    [SerializeField] string encryptionIntro = "Password encryption key:        ";
    [SerializeField] string wrongGuess = "Warning! This attempt has failed. You may not have many attempts remaining...";
    [SerializeField] string correctGuess = "This password attempt has succeeded! Proceeding to hack into the ";

    [Header("Hacking Successful")]
    [SerializeField] float hackingTimeDelay = 1f;
    [SerializeField] int hackingNumberOfDelays = 10;
    [SerializeField] string[] winStrings = new string[] { "Access to personnel records granted.", "Space ship access granted. Awaiting destination.", "Solar Federation internal access granted." };
    [SerializeField] string returnToMainStringOnWin = "You have succeeded! Enter 'menu' to select a new target.";

    [Header("Hacking Failed")]
    [SerializeField] string failedString = "You have failed to hack into the ";
    [SerializeField] string[] failedStringUnique = new string[] { "Colony robotic security initiated. Engaging CaptureFelony() order.", "Automated security activated, engaging ShootToKill() order.", "Solar Federation Militia en route. You will not escape. Surrender now for lifetime imprisonment, or we will shoot to kill." };
    [SerializeField] string returnToMainStringOnFail = "You attempted to resist. You were killed.. Enter 'menu' to attempt a new target.";

    [Header("Hacking Options and Passwords")]
    [SerializeField] string[] hackOptions = new string[] { "Local colony personnel records", "Colony space ship access", "Solar Federation database" };
    [SerializeField] string[] level1passwords = { "profiles", "affairs", "admin", "hiring", "jobs", "files" };
    [SerializeField] string[] level2passwords = { "trajectory", "suborbital", "astronaut", "freefall", "galactic", "vacuum" };
    [SerializeField] string[] level3passwords = { "astrodynamics", "firstcontact", "spectroscopy", "penumbral", "interferometry", "tidaldisruption" };
    List<string[]> passwords;

    List<string> winImages;
    string winImage0 = @"
                       .,,,,,,,,,,.
                     ,;;;;;;;;;;;;;;,
                   ,;;;;;;;;;;;)));;(((,,;;;,,_
                  ,;;;;;;;;;;'      |)))))))))))\\
                  ;;;;;;/ )''    - /,)))((((((((((\
                  ;;;;' \        ~|\  ))))))))))))))
                  /     /         |   ((((((((((((((
                /'      \      _/~'    ')|()))))))))
              /'         `\   />     o_/)))((((((((
             /          /' `~~(____ /  ()))))))))))
            |     ---,   \        \     ((((((((((
                      `\   \~-_____|      ))))))))
                        `\  |      |_.---.  \       
";
    string winImage1 = @"
                        `. ___
                    __,' __`.                _..----....____
        __...--.'``;.   ,.   ;``--..__     .'    ,-._    _.-'
    _..-''-------'   `'   `'   `'     O ``-''._   (,;') _,'
,'________________                          \`-._`-','
    `._              ```````````------...___   '-.._'-:
    ```--.._      ,.                     ````--...__\-.
            `.--. `-`                       ____    |  |`
                `. `.                       ,'`````.  ;  ;`
                `._`.        __________   `.      \'__/`
                    `-:._____/______/___/____`.     \  `
                                |       `._    `.    \
                                `._________`-.   `.   `.___
";
    string winImage2 = @"
                              .:.
                             .:::.
                            .:::::.
                        ***.:::::::.***
                   *******.:::::::::.*******
                 ********.:::::::::::.********
                ********.:::::::::::::.********
                *******.::::::'***`::::.*******
                ******.::::'*********`::.******
                 ****.:::'*************`:.****
                   *.::'*****************`.*
                   .:'  ***************    .
                  .
";

    string loseImage = @"
                              .___.
                    /)     ,-^     ^-.
                    //     /           \
            .-----| |----/  __     __  \--.__
            |WMWMW| |>>> | />>\   />>\ |>>>>>:>
            `-----| |----| \__/   \__/ |--'^^
                    \\     \    /|\    /
                    \)     \   \_/   /
                            |       |
                            |+H+H+H+|
                            \       /
                             ^-----^
";

    // State Variables
    int level = 0;
    Screen currentScreen = Screen.Name;

    string currentPassword = "";

    // Use this for initialization
    void Start()
    {
        attemptsRemaining = initialAttemptsRemaining;
        hackerName = initialHackerName;

        passwords = new List<string[]>();
        passwords.Add(level1passwords);
        passwords.Add(level2passwords);
        passwords.Add(level3passwords);

        winImages = new List<string>();
        winImages.Add(winImage0);
        winImages.Add(winImage1);
        winImages.Add(winImage2);

        DisplayNameSelect();
    }

    private void OnUserInput(string input)
    {
        if (currentScreen == Screen.Exited)
        {
            hackerName = initialHackerName;

            Terminal.ClearScreen();
            Terminal.WriteLine("Terminal Access Lost. Reload Page or type 'sudo restart' to reconnect.");

            if (input == "sudo restart")
            {
                Terminal.ClearScreen();
                currentScreen = Screen.Name;

                Terminal.WriteLine(nameAsk);

                return;
            }
        }

        if (input == "menu" || input == "Menu" || input == "restart" || input == "restart" 
            || input == "Restart" || input == "again" || input == "Again"
            )
        {
            DisplayMainMenu();
        }
        else if (input == "quit" || input == "Quit" || input == "close" || input == "Close" || 
            input == "exit" || input == "Exit" || input == "done" || input == "Done" ||
            input == "finish" || input == "Finish"
            )
        {
            Terminal.WriteLine("Exiting terminal application...");

            currentScreen = Screen.Exited;
            //Application.Quit();
        }
        else if (currentScreen == Screen.Name)
        {
            hackerName = input;

            DisplayMainMenu();
        }
        else if (currentScreen == Screen.MainMenu)
        {
            HandleMainMenuInput(input);
        }
        else if (currentScreen == Screen.Password)
        {
            HandlePasswordScreenInput(input);
        }
    }


    private void DisplayNameSelect()
    {
        currentScreen = Screen.Name;

        Terminal.ClearScreen();

        Terminal.WriteLine(nameAsk);
    }


    private void DisplayMainMenu()
    {
        currentScreen = Screen.MainMenu;
        attemptsRemaining = initialAttemptsRemaining;

        Terminal.WriteLine(mainMenuInitialMessage);
        Terminal.WriteBlankLine();

        int i = 0;
        foreach (string option in hackOptions)
        {
            Terminal.WriteLine(i.ToString() + ". " + option);
            i++;
        }

        Terminal.WriteBlankLine();
        Terminal.WriteLine(mainMenuFinalMessage);
    }

    private void HandleMainMenuInput(string input)
    {
        bool isValidLevelNumber = false;
        for (int i = 0; i < hackOptions.Length; i++)
        {
            if (input == i.ToString())
            {
                isValidLevelNumber = true;
                level = i;
                attemptsRemaining += bonusAttemptFactorPerDifficulty * level;

                DisplayPasswordScreen();

                break;
            }
        }

        if (!isValidLevelNumber)
        {
            Terminal.WriteLine(invalidInputMessage);
        }
    }


    private void DisplayPasswordScreen()
    {
        currentScreen = Screen.Password;

        Terminal.WriteLine(hackingIntoMessage + hackOptions[level].ToLower() + " (difficulty " + level + ").");

        currentPassword = passwords[level][Random.Range(0, passwords[level].Length)];

        Terminal.WriteBlankLine();

        Terminal.WriteLine(encryptionIntro + currentPassword.Anagram());

        Terminal.WriteBlankLine();

        Terminal.WriteLine(passwordInitialMessage + attemptsRemaining);
    }

    private void HandlePasswordScreenInput(string input)
    {
        if (attemptsRemaining <= 1)
        {
            DisplayFailScreen();
        }
        else if (input != currentPassword)
        {
            attemptsRemaining--;

            Terminal.WriteLine(wrongGuess + attemptsRemaining);
        }
        else
        {
            DisplayWinScreen();
        }
    }


    private void DisplayFailScreen()
    {
        currentScreen = Screen.FailScreen;

        Terminal.WriteLine(failedString + hackOptions[level].ToLower() +"! " + failedStringUnique[level]);

        Terminal.WriteLine(loseImage);
        Terminal.WriteLine(returnToMainStringOnFail);
    }

    private void DisplayWinScreen()
    {
        currentScreen = Screen.WinScreen;

        Terminal.WriteLine(correctGuess + hackOptions[level].ToLower() + "...");
        StartCoroutine(TimeDelayAndDisplayReward());
        
    }


    private IEnumerator TimeDelayAndDisplayReward()
    {
        string currentDotBuffer = "";
        for (int i = 0; i < hackingNumberOfDelays; i++)
        {
            Terminal.ClearScreen();

            Terminal.WriteLine(correctGuess + hackOptions[level].ToLower() + "...");
            Terminal.WriteLine(currentDotBuffer);

            currentDotBuffer += '.';

            yield return new WaitForSeconds(hackingTimeDelay);
        }

        Terminal.WriteBlankLine();
        
        Terminal.WriteLine(winStrings[level]);

        Terminal.WriteLine(winImages[level]);

        Terminal.WriteLine(returnToMainStringOnWin);
    }

    public string GetName()
    {
        return hackerName + hackerLocation;
    }
}
