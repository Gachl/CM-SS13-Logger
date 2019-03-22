# CM-SS13-Logger
Log file parser and highlighter for CM-SS13. This application will parse CM-SS13 log files on-the-fly (meaning you will instantly see log output as it appears in the BYOND client), display them by category and apply highlighting rules for a better overview of the game. Never miss your SLs instructions in a flood of emotes and actions again!

# Installation
Download and extract the latest [release](https://github.com/Gachl/CM-SS13-Logger/releases) into a folder of your choice. If you are upgrading, do not overwrite the `windowsettings.json` file as this would overwrite your configuration.

# Usage
To use the logging feature you first have to export the log from the BYOND client by right-clicking into the text output field, selecting `Log` and saving the `*.htm` log file into a folder of your choosing. This file will be constantly written to, it is recommended but not required to put it on a (spinning) hard drive or RAM disk to save SSD cycles.

When starting the application you will be prompted to select a log file, navigate and open the log file that you have saved in the first step.

Next, you will be prompted to skip to the end of the log file. Choose `Yes` if you want to skip to the end and only parse new log entries or `No` if you would like to parse the whole file from start to finish (note that this may take a while depending on the size of the log file).

The application will now start, load your previous (or default) settings and parse the log.

It is recommended to use this application on a second screen as it requires a lot of space.

You may find other user created settings (parse, sorting and highlighting rules) in the CM-SS13 forum.

# Poweruser
The application is very flexible and can easily be adjusted to fit your needs. If you're a just a marine or the commander, set up the windows and highlights as you require.

Any visible log window is being saved into the `windowsettings.json` file, changes made to these windows will also be instantly saved. You can manually save by clicking `File` and `Save`.

To create new log windows click on `Windows` and `New`.

To edit the settings of a window, right-click into the table and select an option.

## Columns
Here you will setup the columns that should be shown in this window. Make sure to use safe names (eg. lower/camelcase, no special characters/spaces). You can choose any label you like.

Note that editing columns will truncate the current table and discard all previously shown messages.

## Parse rules
These rules will control which log message will be parsed and shown. They are based on regular expressions and use named capture groups to assign match groups to columns.

Simply write a regular expressing matching the log line that you want to display and make sure to use the same name for the named capture groups that you have entered in the `Columns Editor`. You can find examples of this in the default settings.

Successive matching regular expressions will be ignored, the first matching expressing will be used to parse the line. It will not generate multiple entries per log line if multiple expressions would match.

## Highlight rules
The same as with the `Parse rules`, the highlight rules use regular expressions. If the expression matches a line then the highlight rule will be applied to the whole row. Only the first matching rule will be applied.

Valid color names can be found on the [KnownColors enum documentation page](https://docs.microsoft.com/en-us/dotnet/api/system.drawing.knowncolor).
