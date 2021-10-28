


var id;

function print(str){
  return document.getElementById('texts').innerHTML = str;
}

function command(command, callback)
{
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.onreadystatechange = function() { 
        if (xmlHttp.readyState == 4 && xmlHttp.status == 200)
            callback(xmlHttp.responseText);
    }
    xmlHttp.open("GET", "http://localhost:3000/forwarder/"+command, true); // true for asynchronous 
    xmlHttp.send(null);
}


function add(){
  command('add', (msg)=>{
    print(msg);
  });
}

function ls(){
  command('ls', (msg)=>{
    print(msg);
  });
}

function cg(){
  command('config', (msg)=>{
    print(msg);
  });
}

function bd(){
  command('bind', (msg)=>{
    print(msg);
  });
}

function st(){
  command('start', (msg)=>{
    print(msg);
  });
}



