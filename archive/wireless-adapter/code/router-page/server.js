var netlinkwrapper = require('netlinkwrapper');
var readline = require('readline');
var express = require('express');
var path = require('path');
var app = express();
var log = '';

var client = new netlinkwrapper();
client.connect(50000, 'localhost');

client.blocking(true);
app.get('/', function(req, res) {
  res.sendFile(path.join(__dirname + '/index.html'));
  log += client.read(1024);
  console.log(log);
});
app.get('/console', function(req, res) {
  res.send(log);
});
app.get('/js/index.js', function(req, res) {
  res.sendFile(path.join(
      __dirname + '/' +
      'js/index.js'))
});

app.get('/forwarder/:cmd', function(req, res) {
  client.write(req.params.cmd);
  var str = '';
  str += client.read(1024);
  log += str;
  if (str !== 'forwarder> ') {
    log += '\n';
  }
  res.send('' + str);
});

var rl = readline.createInterface(
    {input: process.stdin, output: process.stdout, terminal: false});

rl.on('line', function(line) {
  log += line;
  log += '\n';
  client.write(line);
});

app.listen(process.env.PORT || 3000, function() {});
