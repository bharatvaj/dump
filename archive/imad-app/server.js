var express = require('express');
var morgan = require('morgan');
var path = require('path');

var app = express();
app.use(morgan('combined'));

var lols = {
  'lol1' : {
  head : "Article One",
  msg: `
  <p>
    Message of the article one
  </p>
  `
  },
  'lol2' : {
  head : "Article Two",
  msg: `
  <p>
    Message of the article one
  </p>
  `
  },
  'lol3' : {
    head : "Article Three",
    msg: `
    <p>
      Message of the article one
    </p>
    `
    }
};

function createTemplate(data){
  var head = data.head;
  var msg = data.msg;
  var htmlTemplate = `
  <h4>${head}</h4>
  <div>
    ${msg}
  </div>
  `;
  return htmlTemplate;
}

app.get('/', function (req, res) {
  res.sendFile(path.join(__dirname, 'ui', 'index.html'));
});


var counter = 0;
app.get("/counter", function(req, res){
  counter = counter + 1;
  res.send(counter.toString());
});

app.get('/ui/:lolName', function(req, res) {
  var lolName = req.params.lolName;
  res.send(createTemplate(lols[lolName]));
});

app.get('/style.css', function (req, res) {
  res.sendFile(path.join(__dirname, 'ui', 'style.css'));
});
app.get('/madi.png', function (req, res) {
  res.sendFile(path.join(__dirname, 'ui', 'madi.png'));
});
app.get('/main.js', function (req, res) {
  res.sendFile(path.join(__dirname, 'ui', 'main.js'));
});


// Do not change port, otherwise your app won't run on IMAD servers
// Use 8080 only for local development if you already have apache running on 80

var port = 80;
app.listen(port, function () {
  console.log(`IMAD course app listening on port ${port}!`);
});
