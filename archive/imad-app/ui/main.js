//Counter node
var counterBtn = document.getElementById("counterBtn");
counterBtn.onclick = function(){
    var request = new XMLHttpRequest();
    request.onreadystatechange = function() {
        if(request.readyState === XMLHttpRequest.DONE){
            if(request.status === 200){
                var counter = request.responseText;
                var count = document.getElementById("count");
                count.innerHTML = counter.toString();
            }
        }
    };
    request.open("GET", "http://localhost/counter", true);
    request.send(null);
};