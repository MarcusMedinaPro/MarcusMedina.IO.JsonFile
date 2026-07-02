# MarcusMedina.IO.JsonFile

[![NuGet](https://img.shields.io/nuget/v/MarcusMedina.IO.JsonFile.svg?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/MarcusMedina.IO.JsonFile/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/MarcusMedina.IO.JsonFile.svg?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/MarcusMedina.IO.JsonFile/)
[![C#](https://img.shields.io/badge/C%23-14.0-239120?style=for-the-badge&logo=csharp&logoColor=white)](#)
[![.NET](https://img.shields.io/badge/.NET-10.0+-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](https://opensource.org/licenses/MIT)
![Open Source](https://raw.githubusercontent.com/MarcusMedinaPro/MarcusMedina.IO.JsonFile/main/assets/open-source.svg)
[![Build](https://img.shields.io/github/actions/workflow/status/MarcusMedinaPro/MarcusMedina.IO.JsonFile/release.yml?branch=main&label=Build&style=for-the-badge&logo=github)](https://github.com/MarcusMedinaPro/MarcusMedina.IO.JsonFile/actions)
[![Signed](https://img.shields.io/badge/Signed-Sigstore-green?style=for-the-badge&logo=linux)](https://docs.sigstore.dev)

**Save and load any object as a JSON file — one class, zero boilerplate.**

> **Story:** _TODO — ask Marcus for the real background behind this package._

```csharp
using MarcusMedina.IO.JsonFile;

var settings = new JsonFile<AppSettings>("settings");
settings.Data.Theme = "dark";
settings.Save();

// Next run — loads automatically in constructor
var settings = new JsonFile<AppSettings>("settings");
Console.WriteLine(settings.Data.Theme); // "dark"
```

---

## Features

- Generic `JsonFile<T>` — works with any serializable type
- Auto-loads on construction, auto-saves on `Dispose()`
- Creates `.bak` backup before overwriting
- Configurable suffix (default `.json`)
- Implicit conversion to `T` for clean usage
- Graceful handling of missing or corrupt files

---

## Installation

```bash
dotnet add package MarcusMedina.IO.JsonFile
```

---

## Quick Start

```csharp
using MarcusMedina.IO.JsonFile;

// Works with any type — list, record, class
var log = new JsonFile<List<string>>("app-log");
log.Data ??= [];
log.Data.Add("Started");
log.Save();

// Implicit conversion
List<string> entries = log;
```

**With `using` block (auto-save on exit):**

```csharp
using var config = new JsonFile<Config>("config");
config.Data ??= new Config();
config.Data.Debug = true;
// Save() called automatically when leaving the block
```

---

## API

| Member | Description |
|--------|-------------|
| `JsonFile<T>(string filename)` | Creates and loads from `{filename}.json` |
| `Data` | The deserialized object |
| `Filename` | File name without suffix |
| `Suffix` | File extension (default `"json"`) |
| `Format` | `JsonSerializerOptions` for customizing serialization |
| `Load()` | Reloads from disk |
| `Save()` | Writes to disk, creates `.bak` backup |
| `Dispose()` | Calls `Save()` — use with `using` |

---

## Migrating from MarcusMedina.JsonFileWrapper

Replace the using directive:

```csharp
// Before
using MarcusMedina.JsonFileWrapper;

// After
using MarcusMedina.IO.JsonFile;
```

The `JsonFile<T>` class is identical — no other changes needed.

---

## Package Integrity

All releases are signed with [cosign](https://docs.sigstore.dev) (Sigstore keyless signing).

To verify a downloaded package, download both the `.nupkg` and its `.sigstore.json` bundle from the [GitHub Release](https://github.com/MarcusMedinaPro/MarcusMedina.IO.JsonFile/releases), then run:

```bash
cosign verify-blob <package.nupkg> \
  --bundle <package.nupkg.sigstore.json> \
  --certificate-identity-regexp "https://github.com/MarcusMedinaPro/.*/release.yml" \
  --certificate-oidc-issuer https://token.actions.githubusercontent.com
```

Expected output: `Verified OK`

---

## Built with Human + AI Collaboration

This library was written by **Marcus Medina** together with **Claude Code** (Anthropic) — not through "vibe coding" where you just describe and accept, but through genuine collaboration: planning together, reviewing each other's decisions, pushing back when something felt wrong, and iterating until the result felt right.

The goal was always to write code worth reading — the kind a student can open, understand, and learn from. AI was a partner in that process, not a shortcut around it.

If you're curious about this way of working, the source code and git history are open. Every decision has a reason behind it.

## Made for Curious Minds

This library was built with students in mind — not as a black box to copy and paste, but as a real-world example of how clean, purposeful code is written and shared.

Whether you're discovering C# for the first time, need a reliable helper for your school project, or are simply trying to fall in love with writing code — you're exactly who this was made for.

The source is open. Read it, fork it, break it, improve it. That's the whole point.

And if this library saved you an afternoon, or made something click that didn't before — that's everything.

*Non-students are equally welcome. Good code doesn't care about your diploma.*

⭐ If this helped you, consider starring the project on GitHub — it helps other students find it too.

💬 Have an idea, a feature request, or just want to say hi? Open an issue on GitHub — I'd love to hear from you.

## License

MIT — see [LICENSE](https://github.com/MarcusMedinaPro/MarcusMedina.IO.JsonFile/blob/main/LICENSE)
