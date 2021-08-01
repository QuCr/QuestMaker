# QuestMaker

A program that is an editor and is able to generate scripts for each class and class objects. An use case for this program is  to make a quest in a game by creating a Dialog object where Person objects can interact with each other. The generated script can be used to have that dialog in-game.

You can generate any script by writing C# code for it and compiling it. (an built-in editor is not yet implemented)

Current use case: Makes quests for Minecraft by retrieving data files and compiling those to a Minecraft datapack.

---

![](./docs/main.png)

---

The projects is divided into  projects: the main project is QuestMakerConsole and the other 2 reference from it.

[![](https://mermaid.ink/img/eyJjb2RlIjoiZ3JhcGggVERcbiAgICBRdWVzdE1ha2VyQ29uc29sZSAtLT4gUXVlc3RNYWtlclRlc3RzXG4gICAgUXVlc3RNYWtlclVJIC0tPiBRdWVzdE1ha2VyVGVzdHNcbiAgICBRdWVzdE1ha2VyQ29uc29sZSAtLT4gUXVlc3RNYWtlclVJXG4gICIsIm1lcm1haWQiOnsidGhlbWUiOiJmb3Jlc3QifSwidXBkYXRlRWRpdG9yIjpmYWxzZSwiYXV0b1N5bmMiOnRydWUsInVwZGF0ZURpYWdyYW0iOmZhbHNlfQ)]()

## QuestMakerConsole ##
Backend of the program that contains all the logic for the entities and database.

## QuestMakerUI ##
Frontend of the program that contains all the logic for the forms. Sends packets/command to the backend and gets a response back from the backend if needed (updates of UI).

## QuestMakerTests ##
Unit Tests for the program.
