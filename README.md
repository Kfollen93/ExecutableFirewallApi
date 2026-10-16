# Executable Firewall API

A small .NET API for scanning folders for executables and managing Windows Firewall rules.

I originally built this as a console utility for blocking network access for downloaded game development projects during testing. I moved it into an API to separate file discovery from firewall operations, and added support for removing the rules after I was done testing. This is primarily just an internal tool that I use before doing game testing to give myself a little extra security - it is NOT intended to be a sandbox virus environment, but rather just block telemetry/communication.

## How it works

- Recursively scans a folder for .exe, .com, and .scr files.
- Creates inbound and outbound block rules using netsh.
- Names rules using the executable’s filename and a shortened SHA-256 hash of its path.
- Checks for existing rules before adding them.
- Returns counts of newly blocked, already blocked, and failed executables, with equivalent results for unblocking.

Controllers handle HTTP requests, ExecutableScanner handles file discovery, and NetshFirewall handles firewall operations. Services are registered through dependency injection.

# Endpoints

`POST` -> `/api/scan` -> List executables in a folder and its subfolders.
`POST` -> `/api/firewall/block` -> Add inbound and outbound block rules.
`POST` -> `/api/firewall/unblock` -> Remove the application's matching rules.

### Requirements/Backend

- Windows with Windows Firewall and netsh
- .NET 10
- Administrator privileges to add or remove firewall rules
