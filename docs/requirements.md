# Requirements
## Tournament Service
To handle by tournament service
- Create Tournament
- Configure tournament settings
- Configure matches

## Pages Content
### Browse_Tournaments
Filter by:
- Name
- Status (past, incoming, ongoing)
- `Format`
### View_Tournament
Informations presented on a page:
- Name
- Max and assigned number of teams/players eg. 12/32
- Platform (eg. PS4, PC)
- Registration ends Date
- Checkin start Date 
- Checkin Time (in minutes / hours)
- Tournament Start Date (after Checkin Date)
- `Link to Official Website`
- Description
    - Location
        - offline tournament, place where tournament will be played,
        - might be just "Online"
- Prizes (text description)
- Rules
    - below rules as a text or image region/country allowed to play in tournament
- Contact data 
    - email
    - phone number
- `Stream urls (name, language, URL)`
- Tournament type/format
    - Single Elimination
    - Groups
    - League
- `Sponsors`
    - `link`
    - `image/logo`
- Bracket view (separate tab)
- Matches view (not for Single Elimination)

Functions on page:
- Sign up to tournament
    - as player
    - as team (with additional popup)
- Present info that user is already registered

### Tournaments_Manager
Lists of created tournament 
- with option to
    - delete
    - edit
- with content
    - tournament name
    - discipline 
    - start date
    - number of registered participants / max participants

Functions on page:
- create new tournament

### Create_Tournament
It's a popup to initialize tournament with a stepper in it
1. tournament name
2. discipline, are teams checkbox
3. platform
4. `format`
5. max number of players 
    - max is 64
    - registration needs approval checkbox

### Configure_Tournament
- Start Date
- Registration ends Date (if null it's not used) then register time ends on checkin/start date
- Checkin Date (if null then it's not used)
- Checkin Time (in minutes / hours)
- `Location of offline tournament`
- Official Website
- `Logo(?)`
- Description
- Prizes (text description)
- Rules (string of path file / URL - Rules as file)
- Contact data 
    - email
    - phone number
- `Custom player fields`
- `Stream urls (name, language, URL)` (hstore)
- `Region/country allowed to play in tournament`
- Tournament type/format
    - Single Elimination
    - Groups
        - `number of the same matches`
    - League
        - `number of the same matches`
- `Sponsors`
    - link
    - image/logo(?)
-`Invite only`
- Is acceptance needed of admin to join tournament
- Matches settings:
    - Name of settings
    - Type of match (not represented in a DB)
        - Single game
        - Best-of (max number of matches)
    - Is confirmation needed (referee)
    - Date of match
    - Settings are set per round
        - Single Elimination eg. semi-final, final
        - Group: eg. 1st match, 2nd match
        - League: eg. 1st fixture, 2nd fixture

Functions on page:
- save changes made to the tournament
- publish option to allow users to join

### My_Matches
List of incoming matches from all tournaments
List should contain:
- name of the tournament with link to it
- name of the oponent
- score and who win 
- match date
- information that the score is confirmed

Functions on page:
- add score and submit (popup or new site)