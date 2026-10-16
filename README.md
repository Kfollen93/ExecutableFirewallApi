# Executable Firewall API

A small .NET API for scanning folders for executables and managing Windows Firewall rules.

I built this for testing game projects shared in Discord servers. I usually check downloads with VirusTotal, but also wanted a quick way to block their network access while testing.

It started as a console app that added firewall rules. I later moved it into an API, separated file discovery from firewall operations, and added support for removing the rules afterward.

This is a personal tool for blocking network communication, not a sandbox or a guarantee that an executable is safe to run.

## How it works

- Recursively scans a folder for .exe, .com, and .scr files.
- Creates inbound and outbound block rules using netsh.
- Names rules using the executable’s filename and a shortened SHA-256 hash of its path.
- Checks for existing rules before adding them.
- Returns counts of newly blocked, already blocked, and failed executables, with equivalent results for unblocking.

Controllers handle HTTP requests, ExecutableScanner handles file discovery, and NetshFirewall handles firewall operations. Services are registered through dependency injection.

## Endpoints

- `POST` -> `/api/scan` -> List executables in a folder and its subfolders.
- `POST` -> `/api/firewall/block` -> Add inbound and outbound block rules.
- `POST` -> `/api/firewall/unblock` -> Remove the application's matching rules.

## Requirements

- Windows with Windows Firewall and netsh
- .NET 10
- Administrator privileges to add or remove firewall rules

## Known Limitations

This project currently does what I need for my own testing, but there are a couple of things I would improve:

- Unblocking relies on files still being in their original locations. Moving or deleting them can leave firewall rules behind.
- Invalid folder paths return empty results, and failed firewall lookups aren't distinguished from missing rules.
