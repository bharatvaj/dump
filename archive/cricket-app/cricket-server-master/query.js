function _toSpace(_string) {
    return _string.split('_').join(' ');
}
module.exports = {


    //Series
    getSeriesByName: function(seriesName) {
        return `SELECT * FROM Series WHERE LOWER(seriesName)=LOWER('${_toSpace(seriesName)}')`
    },
    getSeriesBySeasonYear: function(seasonYear) {
        return `SELECT * FROM Series WHERE DATE_TRUNC('year', SeriesSeason)=TO_DATE('${seasonYear}', 'YYYY')`
    },
    getSeriesBySeason: function(seriesName, seasonYear) {
        return `SELECT * FROM Series WHERE LOWER(SeriesName)=LOWER('${_toSpace(seriesName)}') AND DATE_TRUNC('year', SeriesSeason)=TO_DATE('${seasonYear}', 'YYYY')`
    },

    //Match
    getMatchesBySeries: function(seriesName, seasonYear) {
        return '' //TODO
    },
    getMatchesByDate: function(matchDate) {
        return '' //TODO
    },
    getMatchByNumber: function(seriesName, seasonYear, matchNo) {
        return '' //TODO
    },

    //Team
    getTeamsByMatch: function(seriesName, seasonYear, matchNo) {
        return '' //TODO
    },
    getTeamByMatch: function(seriesName, seasonYear, matchNo, teamId) {
        return '' //TODO
    },
    getTeamById: function(teamId) {
        return '' //TODO
    },

    //Player
    getPlayersByTeam: function(seriesName, seasonYear, matchNo, teamId) {
        return '' //TODO
    },

    getPlayerById: function(playerId) {
        return `SELECT * FROM Player WHERE PlayerID=${playerId}`
    },

    //PlayerBatting Stats
    getPlayerBattingByMatch: function(seriesName, seasonYear, matchNo, teamId, playerId) {
        return '' //TODO
    },
    getPlayerBattingBySeries: function(seriesName, playerId) {
        return '' //TODO
    },
    getPlayerBattingBySeriesSeason: function(seriesName, seasonYear, playerId) {
        return '' //TODO
    },

    //PlayerBowling Stats
    getPlayerBowlingByMatch: function(seriesName, seasonYear, matchNo, teamId, playerId) {
        return '' //TODO
    },
    getPlayerBowlingBySeries: function(seriesName, playerId) {
        return '' //TODO
    },
    getPlayerBowlingBySeriesSeason: function(seriesName, seasonYear, playerId) {
        return '' //TODO
    }
}