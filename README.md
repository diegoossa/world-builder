# Pod Racer - World builder
Trial Task

<img width="337" alt="Unity_KZcX4aZocq" src="https://user-images.githubusercontent.com/3436237/189285942-65f5210c-8ca1-4eda-a0b6-6838397dc920.png">

I wanted to create a world creator based on the Star Wars Tatooine environment, specifically the pod race.
I also wanted to develop some gameplay, but I didn't have much time left.

I decided to use an architecture based on Scriptable Objects, like the one used in [this project](https://www.youtube.com/watch?v=WLDgtRNK2VE). Since it is an easy to use and very flexible architecture.

Some of the decisions I made were:
* I decided to use the new input system, as I had never used it with Touch before and found it interesting to learn. 
* I wanted to use UIElements, but it doesn't support WorldSpace, nor is it very friendly for animations, so I stuck with uGUI.
* I used a [plugin](https://assetstore.unity.com/packages/tools/utilities/easy-save-the-complete-save-data-serialization-system-768) to save the state of the Prefabs that are instantiated, for lack of time and ease.

Challenges:
* I think one of the biggest problems I had was that I spent a lot of time on some things that were not so important, instead of working on the main requirements first.
* I still have to learn how to use the Input System better.
* There were some things missing in the commands for the undo system. As the creation of elements and when they are all deleted.
* Anxiety.

There is still a lot of polishing to be done on the example, but I didn't want to take too much more time.

[Link to the APK] (https://drive.google.com/file/d/1mUl9axuZxylW9r8Ok7nsivWlyYoCuT4e/view?usp=sharing)
