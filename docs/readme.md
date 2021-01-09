## Migrations
1. Go to the DAL folder
2. Run command
dotnet ef --startup-project ../Playprism.Services.TournamentService.API/ database update
dotnet ef --startup-project ../Playprism.Services.TournamentService.API/ migrations add FirstMigration


## Generate SQLs
dotnet ef --startup-project ./ database update -script -SourceMigration:0

update-database -script -SourceMigration:[name of the migration] -TargetMigration:[name of the migration]
