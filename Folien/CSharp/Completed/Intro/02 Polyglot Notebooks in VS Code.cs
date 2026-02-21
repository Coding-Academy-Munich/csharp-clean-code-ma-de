// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Polyglot Notebooks in VS Code</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>

// %% [markdown]
//
// # Was sind Polyglot Notebooks?
//
// - Microsoft-Erweiterung für Visual Studio Code
// - Interaktive Notebooks direkt in VS Code
// - Unterstützt C#, F#, PowerShell, SQL und mehr
// - Beste Unterstützung für C# (Code-Vervollständigung, Fehlerdiagnose)
// - Kein Docker erforderlich

// %% [markdown]
//
// # Voraussetzungen
//
// - [.NET SDK](https://dotnet.microsoft.com/download) (Version 8 oder neuer)
//   - Test: `dotnet --version`
// - [Visual Studio Code](https://code.visualstudio.com/)

// %% [markdown]
//
// # Installation der Erweiterung
//
// - Erweiterungen in VS Code öffnen (`Ctrl+Shift+X`)
// - Nach "Polyglot Notebooks" suchen
// - Erweiterung von Microsoft installieren
//   - ID: `ms-dotnettools.dotnet-interactive-vscode`

// %% [markdown]
//
// # Testen der Installation
//
// - Command Palette öffnen: `Ctrl+Shift+P`
// - "Polyglot Notebook: Create new blank notebook" auswählen
// - Als Sprache "C#" (`.NET Interactive`) auswählen
// - In die erste Zelle `1 + 1` eingeben
// - Mit `Ctrl+Enter` ausführen
// - Ergebnis `2` sollte angezeigt werden

// %% [markdown]
//
// # Kurs-Notebooks öffnen
//
// - Kurs-Materialien entpacken
// - In VS Code: `File` > `Open Folder...` > Notebooks-Ordner auswählen
// - `.ipynb`-Dateien im Explorer anklicken
// - Polyglot Notebooks öffnet sie automatisch

// %% [markdown]
//
// # Alternative: Docker
//
// - Falls Probleme mit der lokalen Installation auftreten:
//   - Docker-basierte Notebooks als Alternative
//   - Siehe Video "Docker für Notebooks"
// - Polyglot Notebooks in VS Code ist aber die empfohlene Methode
