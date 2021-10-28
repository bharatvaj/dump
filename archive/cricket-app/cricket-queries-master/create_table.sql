CREATE TABLE Series(
	SeriesID SERIAL PRIMARY KEY,
	SeriesSeason date,
	SeriesName text,
	SeriesFormat integer, /*
		0 - ODI
		1 - T20I
		2 - First-Class
		3 - List A
		4 - T20
	*/
	SDate date,
	EDate date NULL
);

CREATE TABLE Team(
	TeamID SERIAL PRIMARY KEY,
	TeamName text
);

CREATE TABLE MatchTable(
	MatchID SERIAL PRIMARY KEY,
	SeriesID SERIAL REFERENCES Series(SeriesID),
	MatchIndex integer,
	TeamA SERIAL REFERENCES Team(TeamID),
	TeamB SERIAL REFERENCES Team(TeamID),
	MatchLocation text,
	MatchToss integer,
	MatchFirstBatting integer,
	SDate date,
	EDate date,
	UNIQUE (MatchIndex)
);

CREATE TABLE Player(
	PlayerID SERIAL PRIMARY KEY,
	PlayerName text,
	PlayerType integer
);

CREATE TABLE BR( /*BattingRecord*/
	PlayerID SERIAL REFERENCES Player(PlayerID),
	TeamID SERIAL REFERENCES Team(TeamID),
	BR_R integer,
	BR_B integer,
	BR_4 integer,
	BR_6 integer,
	BR_SR integer
);

CREATE TABLE BOR ( /*BOwlingRecord*/
	PlayerID SERIAL REFERENCES Player(PlayerID),
	TeamID SERIAL REFERENCES Team(TeamID),
	BOR_O integer,
	BOR_M integer,
	BOR_R integer,
	BOR_W integer,
	BOR_ECON integer,
	BOR_0 integer,
	BOR_4 integer,
	BOR_6 integer,
	BOR_WD integer,
	BOR_NB integer
);

