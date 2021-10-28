const express = require('express')
const app = express()
const { Client } = require('pg')

const q = require('./query')

const client = new Client({
    user: 'laz3r',
    host: 'localhost',
    database: 'cricket',
    password: 'admin',
    port: 5432,
})
client.connect()


function response(req, res, squery) {
    const results = client.query(squery, (error, results) => {
        if (error) {
            res.status(404).json([])
            return
        }
        res.status(200).json(results.rows)
    })
}

app.get('/', (req, res) => {
    res.sendFile("api.html", { root: __dirname })
})


// Series
app.get('/series/:seriesName', (req, res) => {
    response(req, res, q.getSeriesByName(req.params.seriesName))
})

app.get('/season/:seasonYear', (req, res) => {
    response(req, res, q.getSeriesBySeasonYear(req.params.seasonYear))
})

app.get('/series/:seriesName/season/:seasonYear', (req, res) => {
    response(req, res, q.getSeriesBySeason(req.params.seriesName, req.params.seasonYear))
})

//Match
app.get('/series/:seriesName/season/:seasonYear/match/', (req, res) => {
    response(req, res, q.getMatchesBySeries(req.params.seriesName, req.params.seasonYear))
})

app.get('/match/date/:matchDate', (req, res) => {
    response(req, res, q.getMatchesByDate(req.params.matchDate))
})

app.get('/series/:seriesName/season/:seasonYear/match/number/:matchNo', (req, res) => {
    response(req, res, q.getMatchByNumber(req.params.seriesName, req.params.seasonYear, req.params.matchNo))
})

//Team
app.get('/series/:seriesName/season/:seasonYear/match/number/:matchNo/teams', (req, res) => {
    response(req, res, q.getTeamsByMatch(req.params.seriesName, req.params.seasonYear, req.params.matchNo))
})

app.get('/series/:seriesName/season/:seasonYear/match/number/:matchNo/team/:teamId', (req, res) => {
    response(req, res, q.getTeamByMatch(req.params.seriesName, req.params.seasonYear, req.params.matchNo, req.params.teamId))
})

app.get('/team/:teamId', (req, res) => {
    response(req, res, q.getTeamById(req.params.teamId))
})

//Player
app.get('/series/:seriesName/season/:seasonYear/match/number/:matchNo/team/:teamId/players', (req, res) => {
    response(req, res, q.getPlayersByTeam(req.params.seriesName, req.params.seasonYear, req.params.matchNo, req.params.teamId))
})

app.get('/player/:playerId', (req, res) => {
    response(req, res, q.getPlayerById(req.params.playerId))
})

//Player Batting
app.get('/series/:seriesId/season/:seasonYear/match/number/:matchNo/team/:teamId/player/:playerId/br', (req, res) => {
    response(req, res, q.getPlayersByTeam(req.params.seriesName, req.params.seasonYear, req.params.matchNo, req.params.teamId, req.params.playerId))
})

app.get('/series/:seriesName/player/:playerId/br', (req, res) => {
    response(req, res, q.getPlayerBattingBySeries(req.params.seriesName, req.params.playerId))
})

app.get('/series/:seriesName/season/:seasonYear/player/:playerId/br', (req, res) => {
    response(req, res, q.getPlayerBattingBySeriesSeason(req.params.seriesName, req.params.seasonYear, req.params.playerId))
})

//Player Bowling
app.get('/series/:seriesId/season/:seasonYear/match/number/:matchNo/team/:teamId/player/:playerId/bor', (req, res) => {
    response(req, res, q.getPlayersByTeam(req.params.seriesName, req.params.seasonYear, req.params.matchNo, req.params.teamId, req.params.playerId))
})

app.get('/series/:seriesName/player/:playerId/bor', (req, res) => {
    response(req, res, q.getPlayerBowlingBySeries(req.params.seriesName, req.params.playerId))
})

app.get('/series/:seriesName/season/:seasonYear/player/:playerId/bor', (req, res) => {
    response(req, res, q.getPlayerBowlingBySeriesSeason(req.params.seriesName, req.params.seasonYear, req.params.playerId))
})


app.listen(3000)