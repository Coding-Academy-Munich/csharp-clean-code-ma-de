// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Einführung in Refactoring</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>

// %% [markdown]
//
// ## Was ist Refactoring?
//
// - Ändern eines Software-Systems
// - In kleinen Schritten
// - Ohne dessen externes Verhalten zu ändern
// - Um dessen interne Struktur zu verbessern
//
// *Im Wesentlichen verbessern Sie beim Refactoring das Design des Codes, nachdem er
// geschrieben wurde.*
//
// (Martin Fowler)

// %% [markdown]
//
// ## Was ist Refactoring nicht?
//
// - Große Änderungen am Code in einem Schritt
// - Hinzufügen von neuen Features
// - Beheben von Bugs
// - **Ändern des externen APIs**

// %% [markdown]
//
// ## Wann Refactoring?
//
// - Wenn Code verstanden werden muss
// - Wenn Code erweitert werden muss
// - Wenn wir schlechten Code finden, den wir ändern müssen
//
// **Refactoring ist keine Aktivität, die vom Programmieren getrennt ist -
// genauso wenig, wie Sie Zeit zum Schreiben von if-Anweisungen einplanen.**
//
// (Martin Fowler)

// %% [markdown]
//
// ## Tests als Sicherheitsnetz
//
// - Refactoring ohne Tests ist gefährlich
// - Tests nach jedem kleinen Schritt laufen lassen
// - Wenn Tests fehlschlagen: Verhalten hat sich geändert!
// - Deshalb: Erst testen lernen, dann refaktorisieren

// %% [markdown]
//
// ## Werkzeuge für Refactoring
//
// - **IDE-Refactoring-Tools** (Rider, Visual Studio, VS Code)
//   - Sicher, schnell, automatisch
// - **KI-Assistenten** (Copilot, Claude, ChatGPT)
//   - Erkennen Code-Smells, schlagen Refactorings vor
// - **Manuelles Refactoring**
//   - Als letzter Ausweg, wenn IDE/KI nicht helfen
