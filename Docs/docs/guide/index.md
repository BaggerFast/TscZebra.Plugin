# Getting Started

## Overview
The TscZebra.Plugin project is divided into two parts
- [TscZebra.Plugin](https://www.nuget.org/packages/TscZebra.Plugin)
- [TscZebra.Plugin.Abstractions](https://www.nuget.org/packages/TscZebra.Plugin.Abstractions)

### TscZebra.Plugin
Contains all the main logic and functionality of the project.  
It's designed to be used exclusively in contexts where direct interaction with printers is required

```bash
dotnet add package TscZebra.Plugin
```

### TscZebra.Plugin.Abstractions
Contains only interfaces without any concrete implementations.  
Having interfaces in a separate project makes it easier to import printer-related functionality 
into other projects without adding unnecessary dependencies.

```bash
dotnet add package TscZebra.Plugin.Abstractions
```