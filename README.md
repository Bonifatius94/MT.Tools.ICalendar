# MT.Tools.ICalendar
An implementation of the iCalendar standard (RFC 5545) for C# .NET Core

# Disclaimer
This lib does currently not work. It's still WIP. Issues are mainly the component classes and generic date rules (e.g. 'every first monday of the month until the end of next year').

# About
The main purpose of this project is trying out some useful programming patterns and hopefully creating a good iCalendar lib. But the learning is really the main focus.

# Building the Project
Just standard .NET Core build stuff, nothing too special. On Linux / VSCode, restore the solution (dotnet restore ...) and then build (dotnet build ...). It's just a class library project at the moment.

If you're using Visual Studio (e.g. Community Edition 2019), then just open the solution file and hit "Build". Sounds simple, right ^^

# Testing
There are currently no unit tests, but it would be useful to add plenty of them in the future. For the moment there is just a console app that launches the parser logic with an iCal file. If my library is able to parse this iCal file (created by Microsoft Outlook), I may add some better tests instead and also look at the serialization logic for writing iCal files. Ideally writing the parsed content out again will produce the same output file again (or at least the same object tree when reading both files).

# Roadmap
- finish implementation of the iCal standard and make the lib work properly
- add plenty of unit tests (and tests with real files)
- add operations for better usability of the lib
- improve parser performance (make sure that the parser has linear complexity, minimize effective resources like CPU/RAM)
- add support for proprietary attributes / components
