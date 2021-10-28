var net = new WebTCP('localhost', 9999)

//Now you can create sockets like this
var socket = net.createSocket("localhost", 50000)

// On connection callback
socket.on('connect', function(){
  console.log('connected');
})
socket.write("add");

function myFunction() {
  document.getElementById("demo").innerHTML = "YOU CLICKED ME!";
}

var id;
function add(){
  document.getElementById("texts").innerHTML="Add is clicked";
  id=getElementById('add');
}

function ls(){
  document.getElementById("texts").innerHTML="List is clicked";
  id=getElementById('list');
}

function cg(){
  document.getElementById("texts").innerHTML="Configure is clicked";
  id=getElementById('config');
}

function bd(){
  document.getElementById("texts").innerHTML="Bind is clicked";
  id=getElementById('bind');
}

function st(){
  document.getElementById("texts").innerHTML="Start is clicked";
  id=getElementById('start');
}

// To send data to socket
socket.write(id)

// This gets called every time new data for this socket is received
socket.on('data', function(data) {
  console.log("received: " + data);
});

socket.on('end', function(data) {
  console.log("socket is closed ");
});


