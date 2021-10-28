const http = require('http');
const path = require('path');
const fs = require('fs');
const express = require('express');
const app = express()

app.get('/', function (req, res) {
    res.send("Welcome to Health Care server");
});

app.get('/api/:platform', function (req, res) {
    var modelPath = path.join(__dirname, "pretrained", req.params.platform, "latest");
    if (fs.existsSync(modelPath)) {
        res.sendFile(modelPath);
    } else {
        res.send("Invalid Request");
    }
});

app.listen(8080);
