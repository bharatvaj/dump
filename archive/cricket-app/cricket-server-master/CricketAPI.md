Cricket API
=
### Series

- series array
```
/series/:seriesName
/season/:seasonYear
```
- series
```
/series/:seriesName/season/:seasonYear
```

### Match
- match array
```
/series/:seriesName/season/:seasonYear/match/
/match/date/:DATE
```
- match
```
/series/:seriesId/season/:seasonYear/match/number/:matchNo
```
### Team
- team A & B
```
/series/:seriesId/season/:seasonYear/match/number/:matchNo/teams/
```
- team
```
/series/:seriesId/season/:seasonYear/match/number/:matchNo/team/:teamId
/team/:teamId
```

### Player
- playerId array
```
/series/:seriesId/season/:seasonYear/match/number/:matchNo/team/:teamId/players
```
- player
```
/player/:playerId
```
### Player Batting
- match stats
```
/series/:seriesId/season/:seasonYear/match/number/:matchNo/team/:teamId/player/:playerId/br
```
- series stats
```
/series/:seriesName/player/:playerId/br
```
- per season stats
```
/series/:seriesName/season/:seasonYear/player/:playerId/br
```

### Player Bowling
- match stats
```
/series/:seriesId/season/:seasonYear/match/number/:matchNo/team/:teamId/player/:playerId/bor
```
- series stats
```
/series/:seriesName/player/:playerId/bor
```
- per season stats
```
/series/:seriesName/season/:seasonYear/player/:playerId/bor
```