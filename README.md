# QuestMaker

A program that is an editor and is able to generate scripts for each class and class objects. An use case for this program is  to make a quest in a game by creating a Dialog object where Person objects can interact with each other. The generated script can be used to have that dialog in-game.

You can generate any script by writing c# code for it and compiling it. (not yet  implemented!)

Current use case: Makes quests for Minecraft by retrieving data files and compiling those to a Minecraft datapack.

---

![ef](./docs/main.png)

---

The projects is divided into 4 projects: the main project is QuestMakerConsole and all other 3 reference from it.

[![](https://mermaid.ink/img/eyJjb2RlIjoiZ3JhcGggVERcbiAgICBRdWVzdE1ha2VyQ29uc29sZSAtLT4gUXVlc3RNYWtlclRlc3RzXG4gICAgUXVlc3RNYWtlclVJIC0tPiBRdWVzdE1ha2VyVGVzdHNcbiAgICBRdWVzdE1ha2VyQ29uc29sZSAtLT4gUXVlc3RNYWtlclVJXG4gICAgUXV0aWxpdGllcyAtLT4gUXVlc3RNYWtlclVJXG4gICAgUXVlc3RNYWtlckNvbnNvbGUgLS0-IFF1dGlsaXRpZXNcbiAgIiwibWVybWFpZCI6eyJ0aGVtZSI6IiJ9LCJ1cGRhdGVFZGl0b3IiOmZhbHNlLCJhdXRvU3luYyI6dHJ1ZSwidXBkYXRlRGlhZ3JhbSI6ZmFsc2V9)](https://mermaid-js.github.io/mermaid-live-editor/edit/##eyJjb2RlIjoiZ3JhcGggVERcbiAgICBRdWVzdE1ha2VyQ29uc29sZSAtLT4gUXVlc3RNYWtlclRlc3RzXG4gICAgUXVlc3RNYWtlclVJIC0tPiBRdWVzdE1ha2VyVGVzdHNcbiAgICBRdWVzdE1ha2VyQ29uc29sZSAtLT4gUXVlc3RNYWtlclVJXG4gICAgUXVlc3RNYWtlckNvbnNvbGUgLS0-IFF1ZXN0TWFrZXJVSVxuICAgIFF1ZXN0TWFrZXJDb25zb2xlIC0tPiBRdXRpbGl0aWVzXG4gICIsIm1lcm1haWQiOiJ7XG4gIFwidGhlbWVcIjogXCJcIlxufSIsInVwZGF0ZUVkaXRvciI6ZmFsc2UsImF1dG9TeW5jIjp0cnVlLCJ1cGRhdGVEaWFncmFtIjpmYWxzZX0)

## QuestMakerConsole ##
Backend side of the program that contains all the logic for the entities and database.

## QuestMakerUI ##
Frontend side of the program that contains all the logic for the forms. Sends packets/command to the backend and gets a response back from the backend if needed (updates of UI).

## QuestMakerTests ##
Unit Tests for the program.

## Qutilities ##
A combination of the letter 'Q' and the word "utilities". Used for functions that are needed over the whole program. 
An example is getting the generic type of a list

