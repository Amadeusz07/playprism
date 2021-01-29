# UI Flow
`Home_Page` is `Browse_Games` for now
## Not logged in
```mermaid
graph LR
    Home_Page --> Browse_Games
    Browse_Games --> Browse_Tournaments
    Browse_Tournaments -- Filter --> View_Tournament
    View_Tournament --> Sign_Up
```
`Sign_Up` - sign up to tournament as competitor. It redirects to login page and then back to `View_Tournament`
## Logged In
```mermaid
graph LR
    Home_Page --> Tournaments_Manager 
    Tournaments_Manager --> Create_Tournament --> Configure_Tournament --> Start_Tournament
    Tournaments_Manager --> My_Tournaments_List --> View_Tournament

    Home_Page --> Browse_Games
    Browse_Games --> Browse_Tournaments
    Browse_Tournaments -- Filter --> View_Tournament
    View_Tournament --> Sign_Up
    
    Home_Page --> My_Teams
    My_Teams --> Create_Team
    My_Teams --> Edit_Team --> Invite_By_Username
    My_Teams --> Join_Team

    Home_Page --> My_Matches --> Add_Score
```
`My_Teams` list of teams I'm owner of, part of, invitations