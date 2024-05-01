## Note
This is a snapshot of an academic solo project currently under development.

# SyncRide
A prototype for a ride-sharing platform. It allows users to find others who have similar daily commuting routes and connect with them.

## Installation
```bash
# Run this in the main directory

# Docker compose
docker compose docker-compose.yml -p syncride up -d

# Build
dotnet build SyncRide.sln

# Run
cd .\WebApp\bin\Debug\net8.0
.\WebApp.exe
```

access client on http://localhost:5000

default admin account is 
```yaml
username: admin@syncride.ee
password: House.Master2
```

## Under development
* Documentation
* REST API
* React client
* Tests

## Ideas
A collection of ideas to implement

* Messages
  * Mentions
  * Reactions
  * Edit
  * Reply
* Locations
  * Map based choosing
  * Better calculations (or use OSRM?)
* Block users
* Decline Matches
* Channels
  * Add automatic route creation for a group
  * Add route creation option for a group
  * DM note (personal description for DM)
  * Replace user in channel names with `You` or remove completely
  * Auto refresh!

## Chores/Bugs

* Translations
* do not delete user messages when user is kicked from the server
* take into account if route is active when matching
* admin controller censor password
* sort messages
* translate sent at to local
* some BLL checks are too loose (mostly POST methods)